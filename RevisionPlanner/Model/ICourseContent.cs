using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public interface ICourseContent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Confidence GetConfidence();
}
