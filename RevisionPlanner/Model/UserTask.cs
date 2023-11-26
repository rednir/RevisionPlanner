namespace RevisionPlanner.Model;

public class UserTask
{
    public int Id { get; set; }

    public DateTime Deadline { get; set; }

    public ICourseContent CourseContent { get; set; }

    public bool Completed { get; set; }

    public string Title => CourseContent.Name;

    public string Subtitle => CourseContent is UserTopic ? "Topic" : "Subtopic";

    public int? ExamTopicId { get; set; }

    public int? ExamSubtopicId { get; set; }

    // GROUP A: Hashing algorithm.
    public override int GetHashCode()
    {
        int hashCode = 0;

        // Take into account the deadline.
        hashCode += (int)Deadline.ToFileTime();

        // Take into account the course content's name.
        foreach (char c in CourseContent.Name)
            hashCode += c;
        
        // Use a negative value if the task's represents a subtopic, otherwise for topics use a positive value.
        // Multiply by a prime number to reduce risk of collisions for the ID attribute.
        return hashCode * (CourseContent is UserSubtopic ? -7 : 7);
    }
}
