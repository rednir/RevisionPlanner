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

        // Listen for when a new exam is added, and run this function when the event is recieved.
        _userDatabase.ExamAdded += async () => await InitUserTasksAsync();

        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        // Avoid duplicate tasks if this is not the first time this method has been called.
        UserTaskViewModels.Clear();

        IEnumerable<UserTask> userTasksToday = await _userDatabase.GetUserTasksForDateAsync(DateTime.Today);

        foreach (UserTask task in userTasksToday)
        {
            UserTaskViewModel viewModel = new(task, _userDatabase);
            UserTaskViewModels.Add(viewModel);
	    }
    }
}
