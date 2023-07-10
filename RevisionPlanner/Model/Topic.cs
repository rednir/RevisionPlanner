namespace RevisionPlanner.Model;

public class Topic : CourseContent
{
    public uint ID { get; set; }

    public Subtopic[] Subtopics { get; set; }
}
