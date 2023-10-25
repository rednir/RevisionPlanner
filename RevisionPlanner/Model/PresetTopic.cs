namespace RevisionPlanner.Model;

public class PresetTopic
{
    public int Id { get; set; }

    public string Name { get; set; }

    public PresetSubtopic[] Subtopics { get; set; }
}
