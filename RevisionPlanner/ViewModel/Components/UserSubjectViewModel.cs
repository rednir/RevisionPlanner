using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel;

public class UserSubjectViewModel : ViewModelBase
{
    public UserSubject Subject { get; set; }

    public string Name => Subject.Name;

    public string Subtext => $"Qualification: {Subject.Qualification}   |   Exam board: {Subject.ExamBoard}";

    public UserSubjectViewModel(UserSubject subject)
    {
        Subject = subject;
    }
}
