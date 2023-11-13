namespace RevisionPlanner.Model;

/// <summary>
/// Represents an object model that can be stored in the backend SQL database.
/// </summary>
public abstract class DatabaseObject
{
    /// <summary>
    /// The primary key of the object that is used in the SQL database.
    /// </summary>
    public int Id { get; set; }
}
