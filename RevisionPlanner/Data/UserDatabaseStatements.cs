using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

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
        // Composite primary key.
        @"
            CREATE TABLE IF NOT EXISTS ExamTopic (
                ExamId INT,
                UserTopicId INT,
                PRIMARY KEY (ExamId, UserTopicId),
                FOREIGN KEY (ExamId) REFERENCES Exam(Id),
                FOREIGN KEY (UserTopicId) REFERENCES UserTopic(Id)
            );
        ",
        // Composite primary key.
        @"
            CREATE TABLE IF NOT EXISTS ExamSubtopic (
                ExamId INT,
                UserSubtopicId INT,
                PRIMARY KEY (ExamId, UserSubtopicId),
                FOREIGN KEY (ExamId) REFERENCES Exam(Id),
                FOREIGN KEY (UserSubtopicId) REFERENCES UserSubtopic(Id)
            );
        ",
    };

    /// <summary>
    /// Represents the SQL statement which will insert a user into the database.
    /// </summary>
    public const string InsertUser =
    @"
        INSERT OR IGNORE INTO User (Id, UserQualification, StudyDay)
        VALUES (?, 0, 0);
    ";

    /// <summary>
    /// Represents the SQL statement that sets the user's qualification.
    /// </summary>
    public const string SetUserQualification =
    $@"
        UPDATE User
        SET UserQualification = ?
        WHERE Id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that sets the days of the week the user would like tasks to be allocated to.
    /// </summary>
    public const string SetStudyDay =
    @"
        UPDATE User
        SET StudyDay = ?
        WHERE Id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that gets a user record.
    /// </summary>
    public const string GetUser =
    @"
        SELECT *
        FROM User
        WHERE Id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that adds a new user subject.
    /// </summary>
    public const string AddUserSubject =
    @"
        INSERT INTO UserSubject (Id, Name, ExamBoard, Qualification)
        VALUES (?, ?, ?, ?)
    ";

    /// <summary>
    /// Represents the SQL statement that adds a new user topic.
    /// </summary>
    public const string AddUserTopic =
    @"
        INSERT INTO UserTopic (Id, UserSubjectId, Name)
        VALUES (?, ?, ?)
    ";

    /// <summary>
    /// Represents the SQL statement that adds a new user subtopic.
    /// </summary>
    public const string AddUserSubtopic =
    @"
        INSERT INTO UserSubtopic (Id, UserTopicId, Name)
        VALUES (?, ?, ?)
    ";

    /// <summary>
    /// Represents the SQL statement that retrieves a single user subject by its ID.
    /// </summary>
    public const string GetUserSubject =
    @"
        SELECT *
        FROM UserSubject
        WHERE Id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that retrieves all user subjects.
    /// </summary>
    public const string GetAllUserSubjects =
    @"
        SELECT *
        FROM UserSubject
    ";

    /// <summary>
    /// Represents the SQL statement that gets all user topics for a specific user subject.
    /// </summary>
    public const string GetUserTopics =
    @"
        SELECT *
        FROM UserTopic
        WHERE UserSubjectId = ?
    ";

    /// <summary>
    /// Represents the SQL statement that gets all user subtopics for a specific user topic.
    /// </summary>
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

    public const string GetExamTopic =
    @"
        SELECT *
        FROM ExamTopic
        WHERE ExamId = ?
    ";

    public const string GetExamSubtopic =
    @"
        SELECT *
        FROM ExamSubtopic
        WHERE ExamId = ?
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
}
