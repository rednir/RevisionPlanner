using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Model;

public class UserDatabase
{
    public const int userId = 0;

    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public const string FileName = "user.db3";

    public static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    private SQLiteAsyncConnection _connection;

    /// <summary>
    /// </summary>
    public async Task SetUserQualificationAsync(UserQualification userQualification)
    {
        await Init();

        await _connection.ExecuteAsync(
            $@"
                UPDATE user
                SET user_qualification = ?
                WHERE id = {userId}
            ",
            (int)userQualification
        );
    }

    public async Task SetStudyDayAsync(StudyDay studyDay)
    {
        await Init();

        await _connection.ExecuteAsync(
            $@"
                UPDATE user
                SET study_day = ?
                WHERE id = {userId}
            ",
            (int)studyDay
        );
    }

    public async Task<UserQualification> GetUserQualificationAsync()
    {
        await Init();

        List<UserQualification> result = await _connection.QueryAsync<UserQualification>(
            $@"
                SELECT user_qualification
                FROM user
                WHERE id = {userId}
            "
        );

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Initialises the SQL connection and creates the database tables if necessary.
    /// </summary>
    private async Task Init()
    {
        // Avoid running the initialisation process more than once.
        if (_connection is not null)
            return;

        _connection = new SQLiteAsyncConnection(FilePath, Flags);

        // Create the user table and insert the default user.
        await _connection.ExecuteAsync(
	        @"
	            CREATE TABLE IF NOT EXISTS user (
	                id INT PRIMARY KEY,
	                user_qualification INT,
	                study_day INT
                );
	        "
	    );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS user_subject (
                    id INT PRIMARY KEY,
                    name VARCHAR(50) NOT NULL,
                    exam_board VARCHAR(50),
                    qualification VARCHAR(50)
                );
            "
        );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS user_topic (
                    id INT PRIMARY KEY,
                    user_subject_id NOT NULL,
                    name VARCHAR(50) NOT NULL,
                    FOREIGN KEY (user_subject_id) REFERENCES user_subject(id)
                );
            "
        );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS user_subtopic (
                    id INT PRIMARY KEY,
                    user_topic_id NOT NULL,
                    name VARCHAR(50) NOT NULL,
                    FOREIGN KEY (user_topic_id) REFERENCES user_topic(id)
                );
            "
        );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS exam (
                    id INT PRIMARY KEY,
                    user_subject_id INT NOT NULL,
                    deadline DATE NOT NULL,
                    name VARCHAR(50),
                    FOREIGN KEY (user_subject_id) REFERENCES user_subject(id)
                );
            "
        );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS exam_topic (
                    exam_id INT,
                    user_topic_id INT,
                    PRIMARY KEY (exam_id, user_topic_id)
                    FOREIGN KEY (user_topic_id) REFERENCES user_topic(id)
                );
            "
        );

        await _connection.ExecuteAsync(
            @"
                CREATE TABLE IF NOT EXISTS exam_subtopic (
                    exam_id INT,
                    user_subtopic_id INT,
                    PRIMARY KEY (exam_id, user_subtopic_id)
                    FOREIGN KEY (user_subtopic_id) REFERENCES user_subtopic(id)
                );
            "
        );

        // Insert the default user into the user table.
        await _connection.ExecuteAsync(
            @"
                INSERT OR IGNORE INTO user (id, user_qualification, study_day)
                VALUES (0, 0, 0);
            "
        );

        Debug.WriteLine("Initialised user database.");
    }
}