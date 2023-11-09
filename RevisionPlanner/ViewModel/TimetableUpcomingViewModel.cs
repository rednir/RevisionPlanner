using RevisionPlanner.Data;
using RevisionPlanner.Model;
using System.Collections.ObjectModel;
namespace RevisionPlanner.ViewModel;

public class TimetableUpcomingViewModel : ViewModelBase
{
    public ObservableCollection<UserTaskGroupingViewModel> UserTaskGroupingViewModels { get; set; } = new();

    private readonly UserDatabase _userDatabase;

    public TimetableUpcomingViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        bool endOfTimetable = false;
        DateTime currentDate = DateTime.Today;

        while (!endOfTimetable)
        { 
            // TODO: get study day.
	
            // Get the user tasks due for this date.
            IEnumerable<UserTask> userTasks = await _userDatabase.GetUserTasksForDateAsync(currentDate);

            // Check if no tasks are due for this day and break out of the loop if so.
            if (!userTasks.Any())
                break;

            // Map each UserTask to a new object of UserTaskViewModel.
            UserTaskGroupingViewModel grouping = new(userTasks.Select(t => new UserTaskViewModel(t)), currentDate);
            UserTaskGroupingViewModels.Add(grouping);

            // Increment the date we are adding tasks for by one.
            currentDate = currentDate.AddDays(1);

            // Defensive design: if the user has too many tasks, no need to render them all
            if (currentDate > DateTime.Today.AddDays(30))
                break;

            await Task.Delay(500);
	    }
    }
}
