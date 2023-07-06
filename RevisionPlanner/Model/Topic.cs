namespace RevisionPlanner.Model;

public class Topic : CourseContent
{
    public uint ID { get; set; }

    public string Name { get; set; }

    public Subtopic[] Subtopics { get; set; }
}
