using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel;

public class UserSubjectViewModel : ViewModelBase
{
    public UserSubject Subject { get; set; }

    public string Name => Subject.Name;

    public UserSubjectViewModel(UserSubject subject)
    {
        Subject = subject;
    }
}
