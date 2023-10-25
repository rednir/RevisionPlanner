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

    public async Task<UserQualification> GetUserQualificationAsync()
    {
        await Init();
        throw new NotImplementedException();
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

        await _connection.ExecuteAsync(UserDatabaseStatements.AddUserSubject,
            userSubject.Id,
            userSubject.Name,
            userSubject.ExamBoard,
            userSubject.Qualification);

        Debug.WriteLine($"Added user subject: {userSubject}");
    }

    public async Task<UserSubject> GetUserSubjectAsync(int id)
    {
        await Init();

        var result = await _connection.QueryAsync<UserSubject>(UserDatabaseStatements.GetUserSubject, id);
        return result.FirstOrDefault();
    }

    public async Task RemoveAllUserSubjectsAsync()
    {
        await Init();

        await _connection.ExecuteAsync(UserDatabaseStatements.RemoveAllUserSubjectsAsync);
        Debug.WriteLine("Removed all user subjects.");
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