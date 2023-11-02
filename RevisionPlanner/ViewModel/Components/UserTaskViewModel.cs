using RevisionPlanner.Model;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class UserTaskViewModel : ViewModelBase
{
    public UserTask Task { get; set; }

    public string Title => Task.Title;

    public string Subtitle => Task.Subtitle;

    public ICommand RemoveCommand { get; set; }

    public UserTaskViewModel(UserTask userTask)
    {
        Task = userTask;
    }
}
