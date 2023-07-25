using SQLite;

namespace RevisionPlanner.Model;

public class UserDatabase
{
    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public const string FileName = "user.db3";

    public static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    private SQLiteAsyncConnection _connection;

    /// <summary>
    /// Updates or creates a user record in the database according to a user object.
    /// </summary>
    public async Task SetUserAsync(User user)
    {
        await Init();

        bool userExists = await GetUserAsync(user.ID) is not null;

        if (userExists)
        {
            await _connection.ExecuteAsync(
		        @"
                    UPDATE user
                    SET user_qualification = ?, study_day = ?
                    WHERE id = ?;
                ",
                (int)user.UserQualification,
                (int)user.StudyDay,
                user.ID
		    );
            
	        return;
	    }

        await _connection.ExecuteAsync(
            @"
                INSERT INTO user
                VALUES (?, ?, ?)
            ",
            user.ID,
            (int)user.UserQualification,
            (int)user.StudyDay
        );
    }

    /// <summary>
    /// Gets data about a user record in the database from its ID.
    /// </summary>
    /// <returns>
    /// Returns a user object representing the record or null if there is no user with that ID.
    /// </returns>
    public async Task<User> GetUserAsync(int id = 0)
    {
        await Init();

        List<User> result = await _connection.QueryAsync<User>(
            @"
                SELECT * FROM user
                WHERE id = ?
            ",
            id
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

        await _connection.ExecuteAsync(
	        @"
	        CREATE TABLE IF NOT EXISTS user (
	            id INT PRIMARY KEY,
	            user_qualification INT,
	            study_day INT
            )
	        "
	    );
    }
}