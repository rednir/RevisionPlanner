using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.Model.Enums;
using System.Collections.ObjectModel;
namespace RevisionPlanner.ViewModel;

public class TimetableUpcomingViewModel : ViewModelBase
{
    // Constant that represents the maximum number of days to display in the timetable.
    private const int MAX_DAYS = 200;

    // The list of user tasks that will be displayed in the user interface grouped by date.
    public ObservableCollection<UserTaskGroupingViewModel> UserTaskGroupingViewModels { get; set; } = new();

    private readonly UserDatabase _userDatabase;

    public TimetableUpcomingViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        // Keep the timetable up to date by listening for when a new exam is added, and running this function when the event is recieved
        _userDatabase.ExamAdded += async () => await InitUserTasksAsync();

        // Run the method that initialises the list of upcoming user tasks.
        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
        // Avoid duplicate tasks if this is not the first time this method has been called.
        UserTaskGroupingViewModels.Clear();

        StudyDay userStudyDay = await _userDatabase.GetStudyDayAsync();

        bool endOfTimetable = false;
        DateTime currentDate = DateTime.Today;

        while (!endOfTimetable)
        {
            // Increment the date we are adding tasks for by one.
            currentDate = currentDate.AddDays(1);

            StudyDay currentStudyDay = UserDatabase.ConvertDayOfWeekToStudyDay(currentDate.DayOfWeek);

            // The user doesn't want to study on this day of the week so ignore it.
            if ((currentStudyDay & userStudyDay) == 0)
                continue;
	
            // Get the user tasks due for this date.
            IEnumerable<UserTask> userTasks = await _userDatabase.GetUserTasksForDateAsync(currentDate);

            // Check if no tasks are due for this day and break out of the loop if so.
            if (!userTasks.Any())
            {
                endOfTimetable = true;
                continue;
            }

            // Map each UserTask to a new object of UserTaskViewModel.
            UserTaskGroupingViewModel grouping = new(
                userTasks.Select(t => new UserTaskViewModel(t, _userDatabase)), currentDate);

            UserTaskGroupingViewModels.Add(grouping);

            // CODING STYLE: defensive design - if the user has too many tasks, no need to render them all as it can cause slowdowns and crashes.
            if (currentDate > DateTime.Today.AddDays(MAX_DAYS))
                break;

            // Stagger loading to avoid crashes from adding too many objects at once.
            await Task.Delay(500);
	    }
    }
}
