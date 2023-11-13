namespace RevisionPlanner.Model;

public class PresetTopic : DatabaseObject
{
    public string Name { get; set; }

    public PresetSubtopic[] Subtopics { get; set; }
}
