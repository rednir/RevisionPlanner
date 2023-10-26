using System.Diagnostics;
using RevisionPlanner.Model.Enums;
using SQLite;

namespace RevisionPlanner.Data;

public static class StaticDatabaseStatements
{
    /// <summary>
    /// Represents the SQL statement which gets all preset subjects.
    /// </summary>
    public const string GetPresetSubjects =
    @"
        SELECT *
        FROM PresetSubject
        WHERE Qualification = ?
    ";

    /// <summary>
    /// Represents the SQL statement which gets all preset topics for a given preset subject.
    /// </summary>
    public const string GetPresetTopics =
    @"
        SELECT *
        FROM PresetTopic
        WHERE PresetSubjectId = ?
    ";

    /// <summary>
    /// Represents the SQL statement which gets all preset subtopics for a given preset topic.
    /// </summary>
    public const string GetPresetSubtopics =
    @"
        SELECT *
        FROM PresetSubtopic
        WHERE PresetTopicId = ?
    ";
}