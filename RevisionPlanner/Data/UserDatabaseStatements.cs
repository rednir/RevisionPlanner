namespace RevisionPlanner.Data;

public static class UserDatabaseStatements
{
    /// <summary>
    /// Represents a list of SQL statements which will initialise the required tables for the user database.
    /// </summary>
    public static string[] CreateTables => new[]
    {
        @"
            CREATE TABLE IF NOT EXISTS User (
                Id INT PRIMARY KEY,
                UserQualification INT,
                StudyDay INT
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS UserSubject (
                Id INT PRIMARY KEY,
                Name VARCHAR(50) NOT NULL,
                ExamBoard VARCHAR(50),
                Qualification VARCHAR(50)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS UserTopic (
                Id INT PRIMARY KEY,
                UserSubjectId NOT NULL,
                Name VARCHAR(50) NOT NULL,
                FOREIGN KEY (UserSubjectId) REFERENCES UserSubject(Id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS UserSubtopic (
                Id INT PRIMARY KEY,
                UserTopicId NOT NULL,
                Name VARCHAR(50) NOT NULL,
                FOREIGN KEY (UserTopicId) REFERENCES UserTopic(Id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS Exam (
                Id INT PRIMARY KEY,
                UserSubjectId INT NOT NULL,
                Deadline TEXT NOT NULL,
                CustomName VARCHAR(50),
                FOREIGN KEY (UserSubjectId) REFERENCES UserSubject(Id)
            );
        ",
        // Uses Composite primary key.
        @"
            CREATE TABLE IF NOT EXISTS ExamTopic (
                ExamId INT,
                UserTopicId INT,
                PRIMARY KEY (ExamId, UserTopicId),
                FOREIGN KEY (ExamId) REFERENCES Exam(Id),
                FOREIGN KEY (UserTopicId) REFERENCES UserTopic(Id)
            );
        ",
        // Uses Composite primary key.
        @"
            CREATE TABLE IF NOT EXISTS ExamSubtopic (
                ExamId INT,
                UserSubtopicId INT,
                PRIMARY KEY (ExamId, UserSubtopicId),
                FOREIGN KEY (ExamId) REFERENCES Exam(Id),
                FOREIGN KEY (UserSubtopicId) REFERENCES UserSubtopic(Id)
            );
        ",
        // Uses CHECK constraint to ensure both subtopic and topic are not defined.
        @"
            CREATE TABLE IF NOT EXISTS UserTask (
                Id INT PRIMARY KEY,
                ExamTopicId INT,
                ExamSubtopicId INT,
                Deadline TEXT NOT NULL,
                FOREIGN KEY (ExamTopicId) REFERENCES ExamTopic(Id),
                FOREIGN KEY (ExamSubtopicId) REFERENCES ExamSubtopic(Id),
                CHECK (
                    (ExamTopicId IS NOT NULL AND ExamSubtopicId IS NULL) OR
                    (ExamTopicId IS NULL AND ExamSubtopicId IS NOT NULL)
                )
            );
        "
    };

    public const string InsertUser =
    @"
        INSERT OR IGNORE INTO User (Id, UserQualification, StudyDay)
        VALUES (?, 0, 0);
    ";

    public const string SetUserQualification =
    $@"
        UPDATE User
        SET UserQualification = ?
        WHERE Id = ?
    ";

    public const string SetStudyDay =
    @"
        UPDATE User
        SET StudyDay = ?
        WHERE Id = ?
    ";

    public const string GetUser =
    @"
        SELECT *
        FROM User
        WHERE Id = ?
    ";

    public const string AddUserSubject =
    @"
        INSERT INTO UserSubject (Id, Name, ExamBoard, Qualification)
        VALUES (?, ?, ?, ?)
    ";

    public const string AddUserTopic =
    @"
        INSERT INTO UserTopic (Id, UserSubjectId, Name)
        VALUES (?, ?, ?)
    ";

    public const string AddUserSubtopic =
    @"
        INSERT INTO UserSubtopic (Id, UserTopicId, Name)
        VALUES (?, ?, ?)
    ";

    public const string GetUserSubject =
    @"
        SELECT *
        FROM UserSubject
        WHERE Id = ?
    ";

    public const string GetAllUserSubjects =
    @"
        SELECT *
        FROM UserSubject
    ";

    public const string GetUserTopicFromId =
    @"
        SELECT *
        FROM UserTopic
        WHERE Id = ?
    ";

    public const string GetUserTopics =
    @"
        SELECT *
        FROM UserTopic
        WHERE UserSubjectId = ?
    ";

    public const string GetUserSubtopicFromId =
    @"
        SELECT *
        FROM UserSubtopic
        WHERE Id = ?
    ";

    public const string GetUserSubtopics =
    @"
        SELECT *
        FROM UserSubtopic
        WHERE UserTopicId = ?
    ";

    public const string RemoveAllUserSubjects =
    @"
        DELETE FROM UserSubject;
    ";

    public const string RemoveAllUserTopics =
    @"
        DELETE FROM UserTopic;
    ";

    public const string RemoveAllUserSubtopics =
    @"
        DELETE FROM UserSubtopic;
    ";

    public const string AddExam =
    @"
        INSERT INTO Exam (Id, UserSubjectId, Deadline, CustomName)
        VALUES (?, ?, ?, ?)
    ";

    public const string AddExamTopic =
    @"
        INSERT INTO ExamTopic (ExamId, UserTopicId)
        VALUES (?, ?)
    ";

    public const string AddExamSubtopic =
    @"
        INSERT INTO ExamSubtopic (ExamId, UserSubtopicId)
        VALUES (?, ?)
    ";

    // Cross-table statement
    public const string GetExams =
    @"
        SELECT Exam.Id, Exam.UserSubjectId, Exam.Deadline, Exam.CustomName, UserSubject.Id as SubjectId, UserSubject.Name as SubjectName
        FROM Exam
        INNER JOIN UserSubject ON Exam.UserSubjectId = UserSubject.Id
    ";

    // Cross-table statement
    public const string GetExam =
    @"
        SELECT Exam.Id, Exam.UserSubjectId, Exam.Deadline, Exam.CustomName, UserSubject.Id as SubjectId, UserSubject.Name as SubjectName
        FROM Exam
        INNER JOIN UserSubject ON Exam.UserSubjectId = UserSubject.Id
        WHERE ExamId = ?
    ";

    // Cross-table statement
    public const string GetUserTopicsFromExam =
    @"
        SELECT UserTopic.*
        FROM UserTopic
        INNER JOIN ExamTopic ON UserTopic.Id = ExamTopic.UserTopicId
        WHERE ExamTopic.ExamId = ?
    ";

    // Cross-table statement
    public const string GetUserSubtopicsFromExam =
    @"
        SELECT UserSubtopic.*
        FROM UserSubtopic
        INNER JOIN ExamSubtopic ON UserSubtopic.Id = ExamSubtopic.UserSubtopicId
        WHERE ExamSubtopic.ExamId = ?
    ";

    public const string RemoveExam =
    @"
        DELETE FROM Exam
        WHERE Id = ?
    ";

    public const string RemoveExamTopic =
    @"
        DELETE FROM ExamTopic
        WHERE ExamId = ?
    ";

    public const string RemoveExamSubtopic =
    @"
        DELETE FROM ExamSubtopic
        WHERE ExamId = ?
    ";

    public const string AddUserTask =
    @"
        INSERT INTO UserTask (Id, ExamTopicId, ExamSubtopicId, Deadline)
        VALUES (?, ?, ?, ?)
    ";

    public const string GetUserTasksForDeadline =
    @"
        SELECT *
        FROM UserTask
        WHERE Deadline == ?
    ";

    public const string RemoveAllUserTasks =
    @"
        DELETE FROM UserTask;
    ";
}
