using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.Model.Enums;
using System.Collections.ObjectModel;
namespace RevisionPlanner.ViewModel;

public class TimetableUpcomingViewModel : ViewModelBase
{
    private const int MAX_DAYS = 100;

    public ObservableCollection<UserTaskGroupingViewModel> UserTaskGroupingViewModels { get; set; } = new();

    private readonly UserDatabase _userDatabase;

    public TimetableUpcomingViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        Task.Run(InitUserTasksAsync);
    }

    public async Task InitUserTasksAsync()
    {
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
            UserTaskGroupingViewModel grouping = new(userTasks.Select(t => new UserTaskViewModel(t)), currentDate);
            UserTaskGroupingViewModels.Add(grouping);

            // Defensive design: if the user has too many tasks, no need to render them all.
            if (currentDate > DateTime.Today.AddDays(MAX_DAYS))
                break;
	    }
    }
}
