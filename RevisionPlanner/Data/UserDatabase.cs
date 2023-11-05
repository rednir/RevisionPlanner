using System.Diagnostics;
using RevisionPlanner.Model;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public class UserDatabase
{
    public const int UserId = 0;

    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public const string FileName = "user.db3";

    public static string FilePath => Path.Combine(App.AppDataRoot, FileName);

    private SQLiteAsyncConnection _connection;

    public async Task SetUserQualificationAsync(UserQualification userQualification)
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.SetUserQualification, (int)userQualification, UserId);
        Debug.WriteLine("Set user qualification");
    }

    public async Task SetStudyDayAsync(StudyDay studyDay)
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.SetStudyDay, (int)studyDay, UserId);
        Debug.WriteLine("Set study day");
    }

    public async Task<User> GetUserAsync()
    {
        await Init();

        var result = await _connection.QueryAsync<User>(UserDatabaseStatements.GetUser, UserId);
        return result.FirstOrDefault();
    }

    public async Task AddUserSubjectAsync(UserSubject userSubject)
    {
        await Init();

        // Insert the subject into the database.
        await _connection.ExecuteAsync(UserDatabaseStatements.AddUserSubject,
            userSubject.Id,
            userSubject.Name,
            userSubject.ExamBoard,
            userSubject.Qualification);

        // Insert the subject's topics into the database
        if (userSubject.Topics is not null)
            await AddUserTopicsAsync(userSubject.Id, userSubject.Topics);

        Debug.WriteLine($"Added user subject: {userSubject}");
    }

    public async Task AddUserTopicsAsync(int userSubjectId, IEnumerable<UserTopic> userTopics)
    {
        await Init();

        foreach (UserTopic topic in userTopics)
        {
            await _connection.ExecuteAsync(UserDatabaseStatements.AddUserTopic, topic.Id, userSubjectId, topic.Name);

            // Insert the topic's subtopics into the database
            if (topic.Subtopics is not null)
                await AddUserSubtopicsAsync(topic.Id, topic.Subtopics);

            Debug.WriteLine($"Added user topic {topic.Name} to subject {userSubjectId}");
        }
    }

    public async Task AddUserSubtopicsAsync(int userTopicId, IEnumerable<UserSubtopic> userSubtopics)
    {
        await Init();

        foreach (UserSubtopic subtopic in userSubtopics)
        {
            await _connection.ExecuteAsync(UserDatabaseStatements.AddUserSubtopic, subtopic.Id, userTopicId, subtopic.Name);
            Debug.WriteLine($"Added user subtopic {subtopic} to topic {userTopicId}");
        }
    }

    public async Task<UserSubject> GetUserSubjectAsync(int id)
    {
        await Init();

        var result = await _connection.QueryAsync<UserSubject>(UserDatabaseStatements.GetUserSubject, id);
        var subject = result.FirstOrDefault();

        if (subject != null)
        {
            // Populate the topics for this subject object by making another database query.
            var topics = await GetUserTopicsAsync(subject.Id);
            subject.Topics = topics.ToArray();
        }

        return subject;
    }

    public async Task<IEnumerable<UserSubject>> GetAllUserSubjectsAsync()
    {
        await Init();

        var result = await _connection.QueryAsync<UserSubject>(UserDatabaseStatements.GetAllUserSubjects);

        // Populate each subject object with its topics with extra database queries.
        foreach (UserSubject subject in result)
        {
            var topics = await GetUserTopicsAsync(subject.Id);
            subject.Topics = topics.ToArray();
        }

        return result;
    }

    public async Task<IEnumerable<UserTopic>> GetUserTopicsAsync(int userSubjectId)
    {
        await Init();

        var result = await _connection.QueryAsync<UserTopic>(UserDatabaseStatements.GetUserTopics, userSubjectId);

        // Populate the subtopics for each topic object by making another database query.
        foreach (UserTopic topic in result)
        {
            var subtopics = await GetUserSubtopicsAsync(topic.Id);
            topic.Subtopics = subtopics.ToArray();
        }

        return result;
    }

    public async Task<IEnumerable<UserSubtopic>> GetUserSubtopicsAsync(int userTopicId)
    {
        await Init();

        var result = await _connection.QueryAsync<UserSubtopic>(UserDatabaseStatements.GetUserSubtopics, userTopicId);
        return result;
    }

    public async Task RemoveAllUserSubjectsAsync()
    {
        await Init();

        // Remove all subtopics and topics the subject contains first due to the composition relationship.
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveAllUserSubtopics);
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveAllUserTopics);
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveAllUserSubjects);

        Debug.WriteLine("Removed all user subjects.");
    }

    public async Task AddExamAsync(Exam exam)
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.AddExam,
	        exam.Id,
	        exam.SubjectId,
	        exam.Deadline,
	        exam.CustomName);

        // Iterate through the exam content and add each content to the database.
        foreach (CourseContent content in exam.Content)
        {
            // Polymorphism is used to treat exam topics and subtopics the same under the CourseContent object, however they should be stored seperately at a database level.
            if (content is UserTopic userTopic)
            {
                await _connection.ExecuteAsync(UserDatabaseStatements.AddExamTopic, exam.Id, userTopic.Id);
	        }
            else if (content is UserSubtopic userSubtopic)
            {
                await _connection.ExecuteAsync(UserDatabaseStatements.AddExamSubtopic, exam.Id, userSubtopic.Id);
	        }
	    }

        // Update the user's timetable.
        await PopulateUserTasksFromExams();

        Debug.WriteLine($"Added exam: {exam.Name}");
    }

    public async Task<IEnumerable<Exam>> GetExamsAsync()
    {
        await Init();

        var result = await _connection.QueryAsync<Exam>(UserDatabaseStatements.GetExams);
        foreach (Exam exam in result)
            await PopulateExamContent(exam);

        return result;
    }

    public async Task<Exam> GetExamAsync(int id)
    {
        await Init();

        var result = await _connection.QueryAsync<Exam>(UserDatabaseStatements.GetExam, id);

        Exam exam = result.FirstOrDefault();
        if (exam is not null)
            await PopulateExamContent(exam);

        return exam;
    }

    private async Task PopulateExamContent(Exam exam)
    {
        // Populate the course content of this exam. Use polymorphism to cast both topics and subtopics to the same type, so they can be stored in the same collection.
        IEnumerable<CourseContent> topics = await GetUserTopicsFromExamAsync(exam.Id);
        IEnumerable<CourseContent> subtopics = await GetUserSubtopicsFromExamAsync(exam.Id);

        exam.Content = topics.Concat(subtopics).ToArray();
    }

    public async Task<IEnumerable<UserTopic>> GetUserTopicsFromExamAsync(int examId)
    {
        await Init();

        var result = await _connection.QueryAsync<UserTopic>(UserDatabaseStatements.GetUserTopicsFromExam, examId);

        // Populate the subtopics for each topic object by making another database query.
        foreach (UserTopic topic in result)
        {
            var subtopics = await GetUserSubtopicsAsync(topic.Id);
            topic.Subtopics = subtopics.ToArray();
        }

        return result;
    }

    public async Task<IEnumerable<UserSubtopic>> GetUserSubtopicsFromExamAsync(int examId)
    {
        await Init();

        var result = await _connection.QueryAsync<UserSubtopic>(UserDatabaseStatements.GetUserSubtopicsFromExam, examId);
        return result;
    }

    public async Task RemoveExamAsync(int id)
    {
        await Init();

        // Remove subtopics, then topics, then the exam itself in that order to avoid foreign key dependency issues.
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveExamSubtopic, id);
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveExamTopic, id);
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveExam, id);

        return;
    }

    public async Task AddUserTaskAsync(UserTask userTask)
    {
        await Init();

        // Add the user task, setting UserTopicId or UserSubtopicId to the course content ID depending on the type of content.
        await _connection.ExecuteAsync(UserDatabaseStatements.AddUserTask,
            userTask.Id,
            userTask.CourseContent is UserTopic ? userTask.CourseContent.Id : null,
            userTask.CourseContent is UserSubtopic ? userTask.CourseContent.Id : null,
            userTask.Deadline);

        Debug.WriteLine($"Added user task: {userTask}");
    }

    public async Task<IEnumerable<UserTask>> GetUserTasksForDateAsync(DateTime dateTime)
    {
        await Init();

        var result = await _connection.QueryAsync<UserTask>(UserDatabaseStatements.GetUserTasksForDeadline, dateTime);
        return result;
    }

    private async Task PopulateUserTasksFromExams()
    {
        IEnumerable<Exam> exams = await GetExamsAsync();
        IEnumerable<CourseContent> allExamContent = exams.SelectMany(e => e.Content);

        DateTime dateTime = DateTime.Today;

        foreach (CourseContent content in allExamContent)
        {
            // Create a task representing this exam content.
            UserTask task = new()
            {
                Deadline = dateTime.Date,
                CourseContent = content,
            };

            // Set the task's ID using a hashing algorithm.
            task.Id = task.GetHashCode();
            
            // Add this user task to the database.
            await AddUserTaskAsync(task);

            dateTime += TimeSpan.FromHours(5);
        }

        Debug.WriteLine("Populated user tasks.");
    }

    /// <summary>
    /// Initialises the SQL connection and creates the database tables if necessary.
    /// </summary>
    private async Task Init()
    {
        // Avoid running the initialisation process more than once.
        if (_connection is not null)
            return;

        // Connect to the SQL database located in FilePath.
        _connection = new SQLiteAsyncConnection(FilePath, Flags);

        // Create the database tables.
        foreach (string statement in UserDatabaseStatements.CreateTables)
            await _connection.ExecuteAsync(statement);

        // Create the default user.
        await _connection.ExecuteAsync(UserDatabaseStatements.InsertUser, UserId);

        Debug.WriteLine("Initialised user database.");
    }
}