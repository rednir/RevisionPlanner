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
    ";
}