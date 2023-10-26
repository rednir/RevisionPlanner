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
                DeadlineDate REAL NOT NULL,
                Name VARCHAR(50),
                FOREIGN KEY (UserSubjectId) REFERENCES UserSubject(Id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS ExamTopic (
                ExamId INT,
                UserTopicId INT,
                PRIMARY KEY (ExamId, UserTopicId),
                FOREIGN KEY (ExamId) REFERENCES Exam(Id),
                FOREIGN KEY (UserTopicId) REFERENCES UserTopic(Id)
            );
        ",
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

    public const string RemoveAllUserSubjectsAsync =
    @"
        DELETE FROM UserSubject
    ";
}
