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

    /// <summary>
    /// </summary>
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

    /// <summary>
    /// Gets data about the current user record in the database.
    /// </summary>
    /// <returns>
    /// Returns a user object representing the record.
    /// </returns>
    public async Task<User> GetUserAsync()
    {
        await Init();

        List<User> result = await _connection.QueryAsync<User>(
            $@"
                SELECT * FROM user
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
                INSERT OR IGNORE INTO user (id, user_qualification, study_day)
                VALUES (0, 0, 0);
            "
        );

        Debug.WriteLine("Initialised user database.");
    }
}