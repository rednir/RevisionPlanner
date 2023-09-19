using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public static class UserDatabaseStatements
{
    /// <summary>
    /// Represents a list of SQL statements which will initalise the required tables for the user database.
    /// </summary>
    public static string[] CreateTables => new[]
    {
        @"
            CREATE TABLE IF NOT EXISTS user (
                id INT PRIMARY KEY,
                user_qualification INT,
                study_day INT
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS user_subject (
                id INT PRIMARY KEY,
                name VARCHAR(50) NOT NULL,
                exam_board VARCHAR(50),
                qualification VARCHAR(50)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS user_topic (
                id INT PRIMARY KEY,
                user_subject_id NOT NULL,
                name VARCHAR(50) NOT NULL,
                FOREIGN KEY (user_subject_id) REFERENCES user_subject(id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS user_subtopic (
                id INT PRIMARY KEY,
                user_topic_id NOT NULL,
                name VARCHAR(50) NOT NULL,
                FOREIGN KEY (user_topic_id) REFERENCES user_topic(id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS exam (
                id INT PRIMARY KEY,
                user_subject_id INT NOT NULL,
                deadline_date REAL NOT NULL,
                name VARCHAR(50),
                FOREIGN KEY (user_subject_id) REFERENCES user_subject(id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS exam_topic (
                exam_id INT,
                user_topic_id INT,
                PRIMARY KEY (exam_id, user_topic_id),
                FOREIGN KEY (exam_id) REFERENCES exam(id),
                FOREIGN KEY (user_topic_id) REFERENCES user_topic(id)
            );
        ",
        @"
            CREATE TABLE IF NOT EXISTS exam_subtopic (
                exam_id INT,
                user_subtopic_id INT,
                PRIMARY KEY (exam_id, user_subtopic_id),
                FOREIGN KEY (exam_id) REFERENCES exam(id),
                FOREIGN KEY (user_subtopic_id) REFERENCES user_subtopic(id)
            );
        ",
    };

    /// <summary>
    /// Represents the SQL statement which will insert the default user into the database.
    /// </summary>
    public const string InsertDefaultUser =
    @"
        INSERT OR IGNORE INTO user (id, user_qualification, study_day)
        VALUES (1, 0, 0);
    ";

    /// <summary>
    /// Represents the SQL statement that sets the user's qualification.
    /// </summary>
    public const string SetUserQualification =
    $@"
        UPDATE user
        SET user_qualification = ?
        WHERE id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that gets the user's qualification.
    /// </summary>
    public const string GetUserQualification =
    @"
        SELECT user_qualification
        FROM user
        WHERE id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that sets the days of the week the user would like tasks to be allocated to.
    /// </summary>
    public const string SetStudyDay =
    @"
        UPDATE user
        SET study_day = ?
        WHERE id = ?
    ";

    /// <summary>
    /// Represents the SQL statement that gets the days of the week the user would like tasks to be allocated to.
    /// </summary>
    public const string GetStudyDay =
    @"
        SELECT study_day
        FROM user
        WHERE id = ?
    ";
}