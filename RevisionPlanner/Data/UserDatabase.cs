using SQLite;

namespace RevisionPlanner.Model;

public class UserDatabase
{
    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public const string FileName = "user.db3";

    public static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    private SQLiteAsyncConnection _connection;

    /// <summary>
    /// Initialises the SQL connection and creates the database tables if necessary.
    /// </summary>
    public async Task Init()
    {
        if (_connection is not null)
            return;

        _connection = new SQLiteAsyncConnection(FilePath, Flags);
        await _connection.CreateTableAsync<Subject>();
    }
}