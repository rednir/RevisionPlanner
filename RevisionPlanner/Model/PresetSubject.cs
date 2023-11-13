namespace RevisionPlanner.Model;

public class PresetSubject : DatabaseObject
{
    public string Name { get; set; }

    public string ExamBoard { get; set; }

    public string Qualification { get; set; }

    public PresetTopic[] Topics { get; set; }
}
