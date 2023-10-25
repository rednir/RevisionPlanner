namespace RevisionPlanner.Model;

public class UserTask
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

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

    public CourseContent CourseContent { get; set; }

    public string Title => "Title";

    public string Subtitle => "Subtitle";

    //public string Notes { get; set; }
}
