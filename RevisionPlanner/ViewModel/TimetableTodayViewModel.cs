using RevisionPlanner.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RevisionPlanner.ViewModel;

public class TimetableTodayViewModel : ViewModelBase
{
    public IEnumerable<UserTask> Tasks
    {
        get => _tasks;
        private set
        {
            _tasks = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<UserTask> _tasks = new List<UserTask>()
    {
        new UserTask(),
        new UserTask(),
        new UserTask(),
        new UserTask(),
        new UserTask(),
    };

    private readonly UserDatabase _userDatabase;

    public TimetableTodayViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;
    }
}
