using RevisionPlanner.Data;
using RevisionPlanner.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class TimetableUpcomingViewModel : ViewModelBase
{
    public ObservableCollection<UserTaskViewModel> UserTaskViewModels { get; set; } = new();

    private readonly UserDatabase _userDatabase;

    public TimetableUpcomingViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        IEnumerable<UserTask> userTasksTomorrow = await _userDatabase.GetUserTasksForDateAsync(DateTime.Today.AddDays(1));

        foreach (UserTask task in userTasksTomorrow)
        {
            UserTaskViewModel viewModel = new(task);
            UserTaskViewModels.Add(viewModel);
	    }
    }
}
