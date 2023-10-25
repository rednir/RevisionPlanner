namespace RevisionPlanner.Model;

public class UserSubject : CourseContent
{
    public uint Id { get; set; }

    public string ExamBoard { get; set; }

    public string Qualification { get; set; }

    public UserTopic[] Topics { get; set; }

    public override string ToString()
        => $"{Qualification} {ExamBoard} {Name}";
}
