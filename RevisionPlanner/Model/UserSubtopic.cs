using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class UserSubtopic : CourseContent
{
    public int Id { get; set; }

    public Confidence Confidence { get; set; }

    public UserTopic Topic { get; set; }
}
