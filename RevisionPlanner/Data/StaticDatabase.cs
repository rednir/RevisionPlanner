using System.Diagnostics;
using RevisionPlanner.Model;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public class StaticDatabase
{
    public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadOnly;

    public const string FileName = "static.db";

    public static string FilePath => Path.Combine(App.AppDataRoot, FileName);

    private SQLiteAsyncConnection _connection;

    /// <summary>
    /// Initialises the SQL connection to the static database.
    /// </summary>
    private async Task Init()
    {
        // Avoid running the initialisation process more than once.
        if (_connection is not null)
            return;

        // Delete the static database if it already exists, so it is updated with new changes if there are any.
        if (File.Exists(FilePath))
            File.Delete(FilePath);
	
        // Read the bytes of the static database stream and copy it to FilePath where it can be connected to as an SQL database.
        using Stream dataStream = await FileSystem.OpenAppPackageFileAsync(FileName);
        using FileStream fileStream = File.OpenWrite(FilePath);
        await dataStream.CopyToAsync(fileStream);

        // Connect to the SQL database located in FilePath.
        _connection = new SQLiteAsyncConnection(FilePath, Flags);

        Debug.WriteLine($"Initialised static database at {FilePath}");
    }

    public async Task<IEnumerable<PresetSubject>> GetPresetSubjectsAsync()
    {
        await Init();

        var result = await _connection.QueryAsync<PresetSubject>(StaticDatabaseStatements.GetPresetSubjects).ConfigureAwait(false);

        // Populate the topics for each subject object by making another database query.
        foreach (PresetSubject subject in result)
        {
            var topics = await GetPresetTopicsAsync(subject.Id);
            subject.Topics = topics.ToArray();
	    }

        return result;
    }

    public async Task<IEnumerable<PresetTopic>> GetPresetTopicsAsync(int presetSubjectId)
    {
        await Init();

        var result = await _connection.QueryAsync<PresetTopic>(StaticDatabaseStatements.GetPresetTopics, presetSubjectId);

        // Populate the subtopics for each topic object by making another database query.
        foreach (PresetTopic topic in result)
        {
            var subtopics = await GetPresetSubtopicsAsync(topic.Id);
            topic.Subtopics = subtopics.ToArray();
	    }

        return result;
    }

    public async Task<IEnumerable<PresetSubtopic>> GetPresetSubtopicsAsync(int presetTopicId)
    {
        await Init();

        var result = await _connection.QueryAsync<PresetSubtopic>(StaticDatabaseStatements.GetPresetSubtopics, presetTopicId);
        return result;
    }
}