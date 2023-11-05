namespace RevisionPlanner.Model;

public class UserTask
{
    public int Id { get; set; }

    public DateTime Deadline { get; set; }

    public CourseContent CourseContent { get; set; }

    public string Title => "Title";

    public string Subtitle => "Subtitle";

    public override int GetHashCode()
    {
        int hashCode = 0;

        long deadlineBinary = Deadline.ToBinary();

        // Take into account deadline.
        foreach (char c in deadlineBinary.ToString())
            hashCode += c;

        // Take into account the course content's name.
        foreach (char c in CourseContent.Name)
            hashCode += c;
        
        // Use a negative value if the task's represents a subtopic, otherwise for topics use a positive value.
        // Multiply by a prime number to reduce risk of collisions for the ID attribute.
        return hashCode * (CourseContent is UserSubtopic ? -7 : 7);
    }

    /*
    public CourseContent CourseContent { get; set; } = new UserSubtopic
    {
        Name = "Subtopic name",
        Topic = new UserTopic
        {
            Name = "Topic name",
            Subject = new UserSubject
            {
                Name = "Subject name",
            },
        },
    };

    public string Title
    {
	    get
        {
            if (CourseContent is UserSubject subject)
                return subject.Name;

            if (CourseContent is UserTopic topic)
                return topic.Subject.Name;

            if (CourseContent is UserSubtopic subtopic)
                return subtopic.Topic.Subject.Name;

            return "Title";
	    } 
    }

    public string Subtitle
    {
        get
        {
            if (CourseContent is UserSubject)
                return string.Empty;

            if (CourseContent is UserTopic topic)
                return topic.Name;

            if (CourseContent is UserSubtopic subtopic)
                return $"{subtopic.Topic.Name} / {subtopic.Name}";

            return "Subtitle";
        }
    }*/


    //public string Notes { get; set; }
}
