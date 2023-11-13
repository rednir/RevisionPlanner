using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.Model;

public class User : DatabaseObject
{
    public UserQualification UserQualification { get; set; }

    public StudyDay StudyDay { get; set; }
}
