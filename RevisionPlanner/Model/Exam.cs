namespace RevisionPlanner.Model;

public class Exam
{
    public int Id { get; set; }

    public string Name => CustomName ?? $"{SubjectName} exam";

    public string CustomName { get; set; }

    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public DateTime Deadline { get; set; }

    public CourseContent[] Content { get; set; }

    public override string ToString() => Name;

    public override int GetHashCode()
    {
        int hashCode = 0;

        // Take into account subject.
        foreach (char c in SubjectName)
            hashCode += c;

        long deadlineBinary = Deadline.ToBinary();

        // Take into account deadline.
        foreach (char c in deadlineBinary.ToString())
            hashCode += c;

        // Take into account exam content.
        return (hashCode + Content.Length) * 11;
    }
}
