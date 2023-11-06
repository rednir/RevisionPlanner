namespace RevisionPlanner.Model;

public class UserTask
{
    public int Id { get; set; }

    public DateTime Deadline { get; set; }

    public CourseContent CourseContent { get; set; }

    public string Title => CourseContent.Name;

    public string Subtitle => CourseContent is UserTopic ? "Topic" : "Subtopic";

    public int? ExamTopicId { get; set; }

    public int? ExamSubtopicId { get; set; }

    public override int GetHashCode()
    {
        int hashCode = 0;

        string deadline = Deadline.ToShortDateString();

        // Take into account deadline.
        foreach (char c in deadline)
            hashCode += c;

        // Take into account the course content's name.
        foreach (char c in CourseContent.Name)
            hashCode += c;
        
        // Use a negative value if the task's represents a subtopic, otherwise for topics use a positive value.
        // Multiply by a prime number to reduce risk of collisions for the ID attribute.
        return hashCode * (CourseContent is UserSubtopic ? -7 : 7);
    }

    //public string Notes { get; set; }
}
