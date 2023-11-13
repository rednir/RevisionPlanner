using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class UserSubtopic : DatabaseObject, ICourseContent
{
    public string Name { get; set; }

    public Confidence Confidence { get; set; } = Confidence.Neutral;

    public UserSubtopic()
    { 
    }

    public UserSubtopic(PresetSubtopic presetSubtopic)
    {
        Name = presetSubtopic.Name;

        // Use the same ID as the preset subtopic but negative, as negative IDs are reserved for user subtopics taken from a preset subtopic.
        Id = -presetSubtopic.Id;
    }

    public Confidence GetConfidence()
    {
        return Confidence;
    }
}
