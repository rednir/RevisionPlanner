namespace RevisionPlanner.Model;

public class UserSubject : DatabaseObject
{
    public string Name { get; set; }

    public string ExamBoard { get; set; }

    public string Qualification { get; set; }

    public UserTopic[] Topics { get; set; }

    public UserSubject()
    { 
    }

    public UserSubject(PresetSubject presetSubject)
    {
        Name = presetSubject.Name;
        ExamBoard = presetSubject.ExamBoard;
        Qualification = presetSubject.Qualification;

        // Use the same ID as the preset subject but negative, as negative IDs are reserved for user subjects taken from a preset subject.
        Id = -presetSubject.Id;

        // Convert preset topics in this subject to user topics.
        List<UserTopic> userTopics = new();
        foreach (PresetTopic topic in presetSubject.Topics)
        {
            UserTopic userTopic = new(topic);
            userTopics.Add(userTopic);
	    }

        Topics = userTopics.ToArray();
    }

    public UserSubject(string name, string examBoard, string qualification)
    {
        Name = name;
        ExamBoard = examBoard;
        Qualification = qualification;
        Id = GetHashCode();
    }

    public override string ToString()
        => $"{Qualification} {ExamBoard} {Name}";

    public override int GetHashCode()
    {
        int hashCode = 0;

        // Take into account only the name of the subject as it should be unique for the user subject table.
        foreach (char c in Name.ToUpper())
            hashCode += c;

        // Multiply by a prime number to reduce collision risk.
        return hashCode * 11;
    }
}
