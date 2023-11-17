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

        // Keep the timetable up to date by listening for when a new exam is added, and running this function when the event is recieved.
        _userDatabase.ExamAdded += async () => await InitUserTasksAsync();

        // Run the method that initialises the list of user tasks.
        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        // Avoid duplicate tasks if this is not the first time this method has been called.
        UserTaskViewModels.Clear();

        IEnumerable<UserTask> userTasksToday = await _userDatabase.GetUserTasksForDateAsync(DateTime.Today);

        // Iterate through the user tasks due today and display each one to the user.
        foreach (UserTask task in userTasksToday)
        {
            UserTaskViewModel viewModel = new(task, _userDatabase);
            UserTaskViewModels.Add(viewModel);
	    }
    }
}
