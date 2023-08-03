namespace RevisionPlanner.Model;

public class UserTopic : CourseContent
{
    public uint ID { get; set; }

    public UserSubtopic[] Subtopics { get; set; }

    public UserSubject Subject { get; set; }
}
