namespace RevisionPlanner.Model;

public class Subtopic : CourseContent
{
    public uint ID { get; set; }

    public string Name { get; set; }

    public Confidence Confidence { get; set; }
}
