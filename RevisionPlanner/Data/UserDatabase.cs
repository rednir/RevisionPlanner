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

    public const int TasksPerDay = 3;

    public static string FilePath => Path.Combine(App.AppDataRoot, FileName);

    private SQLiteAsyncConnection _connection;

    public event Action ExamAdded;

    public async Task SetUserQualificationAsync(UserQualification userQualification)
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.SetUserQualification, (int)userQualification, UserId);
        Debug.WriteLine("Set user qualification");
    }

    public async Task<StudyDay> GetStudyDayAsync()
    {
        await Init();

        int result = await _connection.ExecuteScalarAsync<int>(UserDatabaseStatements.GetStudyDay, UserId);

        // Cast the integer we fetched from the database to a StudyDay enum.
        return (StudyDay)result;
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
            await _connection.ExecuteAsync(UserDatabaseStatements.AddUserSubtopic, subtopic.Id, subtopic.Confidence, userTopicId, subtopic.Name);
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

        // Remove all subtopics and topics the subject contains first due to the foreign key composition relationship.
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
        foreach (ICourseContent content in exam.Content)
        {
            // GROUP A: Complex user-defined use of OOP - Polymorphism
            // Polymorphism is used to process exam topics and subtopics differently but both under the CourseContent object, however they should be stored separately at a database level.
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

        // Let the rest of the program know that a new exam has been added.
        ExamAdded?.Invoke();

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

    public async Task<int> GetExamCountAsync()
    {
        await Init();

        var result = await _connection.ExecuteScalarAsync<int>(UserDatabaseStatements.GetExamCount);
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
        // GROUP A: Complex user-defined use of OOP - Polymorphism
        // Populate the course content of this exam. Cast both topics and subtopics to the same type, so they can be stored in the same collection and processed together using polymorphism.
        IEnumerable<ICourseContent> topics = await GetUserTopicsFromExamAsync(exam.Id);
        IEnumerable<ICourseContent> subtopics = await GetUserSubtopicsFromExamAsync(exam.Id);

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

        Debug.WriteLine($"Added user task for {userTask.Deadline.ToShortDateString()}: {userTask.Id} {userTask.CourseContent.Name}");
    }

    public async Task SetUserTaskCompleted(int userTaskId, bool completedValue)
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.SetUserTaskCompleted, completedValue, userTaskId);
        Debug.WriteLine($"Set user task completed to {completedValue}: {userTaskId}");
    }

    public async Task<IEnumerable<UserTask>> GetUserTasksForDateAsync(DateTime dateTime)
    {
        await Init();

        var result = await _connection.QueryAsync<UserTask>(UserDatabaseStatements.GetUserTasksForDeadline, dateTime);

        foreach (UserTask userTask in result)
        {
            // Populate the course content of this user task using another database query.
            if (userTask.ExamTopicId is not null)
            {
                var topicResult = await _connection.QueryAsync<UserTopic>(UserDatabaseStatements.GetUserTopicFromId, userTask.ExamTopicId);
                userTask.CourseContent = topicResult.FirstOrDefault();
            }
            else if (userTask.ExamSubtopicId is not null)
            {
                var subtopicResult = await _connection.QueryAsync<UserSubtopic>(UserDatabaseStatements.GetUserSubtopicFromId, userTask.ExamSubtopicId);
                userTask.CourseContent = subtopicResult.FirstOrDefault();
            }
        }

        return result;
    }

    private async Task PopulateUserTasksFromExams()
    {
        // Get the days the user wants to assign tasks to.
        StudyDay userStudyDay = await GetStudyDayAsync();

        // Get the list of exams that have been defined by the user.
        Exam[] exams = (await GetExamsAsync()).ToArray();

        // Remove all tasks first, before repopulating the user's timetable with updated tasks.
        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveAllUserTasks);

        // This array holds the exam indexes that we will not use to add tasks due to them being past the deadline.
        bool[] disabledExams = new bool[exams.Length];

        // This array holds the current course content for each exam we are looking at, and about to add a task for.
        int[] currentContentIndexForEachExam = new int[exams.Length];

        DateTime currentDate = DateTime.Today;
        int tasksForCurrentDate = 0;
        int currentExamIndex = 0;

        // GROUP A: List data structure that keeps track of the content that has been added for the current date.
        List<int> contentAddedForCurrentDate = new();

        while (disabledExams.Any(b => !b))
        {
            // Ignore disabled exams.
            if (disabledExams[currentExamIndex])
            {
                currentExamIndex = (currentExamIndex + 1) % exams.Length;
                continue;
            }

            // Get the current exam to add a task for.
            Exam exam = exams[currentExamIndex];

            if (currentDate >= exam.Deadline)
            {
                // Avoid adding tasks that are after the deadline of their respective exam by marking them as disabled.
                disabledExams[currentExamIndex] = true;
                continue;
            }

            // GROUP A: Dynamic generation of objects based on complex user-defined use of OOP model.
            // Initialise the user task object with the next content from the exam.
            int currentContentIndex = currentContentIndexForEachExam[currentExamIndex];
            UserTask userTask = new()
            {
                CourseContent = exam.Content[currentContentIndex],
                Deadline = currentDate,
            };
            
            userTask.Id = userTask.GetHashCode() + tasksForCurrentDate;

            // Add this user task to the database and increment the count of tasks for this day by 1.
            await AddUserTaskAsync(userTask);
            contentAddedForCurrentDate.Add(userTask.CourseContent.Id);
            tasksForCurrentDate += 1;

            if (tasksForCurrentDate >= TasksPerDay)
            {
                // If we have added enough tasks for this day, move onto the next day.
                moveToNextDay();
            }

            // Increment the exam index, and the content index. If we reach the end of the list, loop back to the start.
            currentContentIndexForEachExam[currentExamIndex] = (currentContentIndexForEachExam[currentExamIndex] + 1) % exam.Content.Length;
            currentExamIndex = (currentExamIndex + 1) % exams.Length;
        }

        Debug.WriteLine("Populated user tasks.");

        // GROUP A: Recursive algorithm as a local helper function.
        void moveToNextDay()
        {
            currentDate = currentDate.AddDays(1);

            StudyDay currentStudyDay = ConvertDayOfWeekToStudyDay(currentDate.DayOfWeek);
            if ((currentStudyDay & userStudyDay) == 0)
            {
                // If the current day is not a study day, move onto the next day by calling this routine again, recursively.
                moveToNextDay();
                return;
            }

            // Otherwise call the base case.
            contentAddedForCurrentDate.Clear();
            tasksForCurrentDate = 0;
        }
    }

    public static StudyDay ConvertDayOfWeekToStudyDay(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => StudyDay.Monday,
            DayOfWeek.Tuesday => StudyDay.Tuesday,
            DayOfWeek.Wednesday => StudyDay.Wednesday,
            DayOfWeek.Thursday => StudyDay.Thursday,
            DayOfWeek.Friday => StudyDay.Friday,
            DayOfWeek.Saturday => StudyDay.Saturday,
            DayOfWeek.Sunday => StudyDay.Sunday,
            _ => throw new ArgumentException("Not a day of the week."),
        };
    }

    /// <summary>
    /// Initialises the SQL connection and creates the database tables if necessary. This routine will be run every time a function is called that attempts to access the user database.
    /// </summary>
    private async Task Init()
    {
        // Avoid running the initialisation process more than once.
        if (_connection is not null)
            return;

        // Connect to the SQL database located in the FilePath constant.
        _connection = new SQLiteAsyncConnection(FilePath, Flags);

        // Create the database tables.
        foreach (string statement in UserDatabaseStatements.CreateTables)
            await _connection.ExecuteAsync(statement);

        // Create the default user.
        await _connection.ExecuteAsync(UserDatabaseStatements.InsertUser, UserId);

        Debug.WriteLine("Initialised user database.");
    }
}