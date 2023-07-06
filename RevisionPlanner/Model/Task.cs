namespace RevisionPlanner.Model;

public class Task
{
    public uint ID { get; set; }

    public DateOnly Date { get; set; }

    //public string Notes { get; set; }

    public CourseContent Content { get; set; }
}
