namespace RevisionPlanner.Model;

public class UserTopic : CourseContent
{
    public UserSubtopic[] Subtopics { get; set; }

    public UserTopic()
    { 
    }

    public UserTopic(PresetTopic presetTopic)
    {
        Name = presetTopic.Name;

        // Use the same ID as the preset topic but negative, as negative IDs are reserved for user topics taken from a preset topic.
        Id = -presetTopic.Id;

        // Convert preset subtopics in this topic to user subtopics.
        List<UserSubtopic> userSubtopics = new();
        foreach (PresetSubtopic subtopic in presetTopic.Subtopics)
            userSubtopics.Add(new UserSubtopic(subtopic));

        // Set the subtopics that this topic consists of.
        Subtopics = userSubtopics.ToArray();
    }
}
