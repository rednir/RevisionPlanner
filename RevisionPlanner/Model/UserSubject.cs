namespace RevisionPlanner.Model;

public class UserSubject : CourseContent
{
    public uint Id { get; set; }

    public string ExamBoard { get; set; }

    public string Qualification { get; set; }

    //public Color Color { get; set; }

    public UserTopic[] Topics { get; set; }
}
