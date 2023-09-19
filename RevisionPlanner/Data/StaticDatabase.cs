using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public class StaticDatabase
{
    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadOnly;

    public const string FileName = "static.db";

    public static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    private SQLiteAsyncConnection _connection;

    /// <summary>
    /// Initialises the SQL connection to the static database.
    /// </summary>
    private async Task Init()
    {
        // Avoid running the initialisation process more than once.
        if (_connection is not null)
            return;

        // Avoid creating the static database more than once.
        // TODO: error handling here? or idk maybe not needed.
        if (!File.Exists(FilePath))
        {
            // Read the bytes of the static database stream and copy it to FilePath where it can be connected to as an SQL database.
            using Stream dataStream = await FileSystem.OpenAppPackageFileAsync(FileName);
            using FileStream fileStream = File.OpenWrite(FilePath);

            await dataStream.CopyToAsync(fileStream);
        }

        // Connect to the SQL database located in FilePath.
        _connection = new SQLiteAsyncConnection(FilePath, Flags);

        Debug.WriteLine("Initialised static database.");
    }
}