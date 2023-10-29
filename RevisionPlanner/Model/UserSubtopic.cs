using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class UserSubtopic : CourseContent
{
    public Confidence Confidence { get; set; }

    public UserSubtopic()
    { 
    }

    public UserSubtopic(PresetSubtopic presetSubtopic)
    {
        Name = presetSubtopic.Name;

        // Use the same ID as the preset subtopic but negative, as negative IDs are reserved for user subtopics taken from a preset subtopic.
        Id = -presetSubtopic.Id;
    }
}
