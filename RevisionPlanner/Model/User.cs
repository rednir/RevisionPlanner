using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class User
{
    public int Id { get; set; }

    public UserQualification UserQualification { get; set; }

    public StudyDay StudyDay { get; set; }
}
