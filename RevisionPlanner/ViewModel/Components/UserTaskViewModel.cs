using RevisionPlanner.Data;
using RevisionPlanner.Model;
using System.Threading.Tasks;

namespace RevisionPlanner.ViewModel;

public class UserTaskViewModel : ViewModelBase
{
    public UserTask UserTask { get; set; }

    public string Title => UserTask.Title;

    public string Subtitle => UserTask.Subtitle;

    public bool Completed
    {
        get => UserTask.Completed;
        set
        {
            UserTask.Completed = value;
            
            // Set the task as completed in the database.
            Task.Run(async () => await _userDatabase.SetUserTaskCompleted(UserTask.Id, value));
            OnPropertyChanged();
        }
    }

    private readonly UserDatabase _userDatabase;

    public UserTaskViewModel(UserTask userTask, UserDatabase userDatabase)
    {
        UserTask = userTask;
        _userDatabase = userDatabase;
    }
}
