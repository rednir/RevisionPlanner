namespace RevisionPlanner.Model;

public class UserTopic : CourseContent
{
    public uint Id { get; set; }

    public UserSubtopic[] Subtopics { get; set; }

    public UserSubject Subject { get; set; }
}
