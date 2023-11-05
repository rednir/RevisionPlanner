using RevisionPlanner.Data;
using RevisionPlanner.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class TimetableTodayViewModel : ViewModelBase
{
    public ObservableCollection<UserTaskViewModel> UserTaskViewModels { get; set; } = new();

    public ICommand AddCustomTaskCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    public TimetableTodayViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;
        AddCustomTaskCommand = new Command(() => throw new NotImplementedException());

        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        IEnumerable<UserTask> userTasksToday = await _userDatabase.GetUserTasksForDateAsync(DateTime.Today);

        foreach (UserTask task in userTasksToday)
        {
            UserTaskViewModel viewModel = new(task);
            UserTaskViewModels.Add(viewModel);
	    }
    }
}
