using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class UserTopic : DatabaseObject, ICourseContent
{
    public string Name { get; set; }

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

    public Confidence GetConfidence()
    {
        int length = Subtopics.Length;
        int total = 0;

        // Sum the values of the confidence levels of each subtopic of this topic.
        foreach (UserSubtopic subtopic in Subtopics)
            total += (int)subtopic.GetConfidence();

        // Get the mean of the confidence levels.
        float average = total / length;

        // Return the closest confidence level that represents this average value;
        return (Confidence)Math.Round(average);
    }
}
