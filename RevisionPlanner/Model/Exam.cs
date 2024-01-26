namespace RevisionPlanner.Model;

// GROUP A: Complex user-defined use of OOP - Inheritance
public class Exam : DatabaseObject
{
    public string Name => CustomName ?? $"{SubjectName} exam";

    public string CustomName { get; set; }

    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public DateTime Deadline { get; set; }

    public ICourseContent[] Content { get; set; }

    public override string ToString() => Name;

    // GROUP A: Hashing algorithm. Also, this overrides the default behaviour of this method.
    public override int GetHashCode()
    {
        int hashCode = 0;

        // Take into account the subject.
        foreach (char c in SubjectName)
            hashCode += c;

        string deadlineString = Deadline.ToShortDateString();

        // Take into account the deadline.
        foreach (char c in deadlineString)
            hashCode += c;

        // Take into account exam content, and multiply by a prime number to reduce risk of collisions for ID.
        return (hashCode + Content.Sum(c => c.Id)) * 7;
    }
}
