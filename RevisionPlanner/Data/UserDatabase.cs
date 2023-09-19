using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public class UserDatabase
{
    public const int userId = 0;

    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public const string FileName = "user.db3";

    public static string FilePath => Path.Combine(App.AppDataRoot, FileName);

    private SQLiteAsyncConnection _connection;

    public async Task SetUserQualificationAsync(UserQualification userQualification)
    {
        await Init();
        await _connection.ExecuteAsync(UserDatabaseStatements.SetUserQualification, (int)userQualification, userId);
    }

    public async Task<UserQualification> GetUserQualificationAsync()
    {
        await Init();
        throw new NotImplementedException();
    }

    public async Task SetStudyDayAsync(StudyDay studyDay)
    {
        await Init();
        await _connection.ExecuteAsync(UserDatabaseStatements.SetStudyDay, (int)studyDay, userId);
    }

    public async Task<StudyDay> GetStudyDayAsync()
    {
        await Init();
        throw new NotImplementedException();
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

        Debug.WriteLine("Initialised user database.");
    }
}