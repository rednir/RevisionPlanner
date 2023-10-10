using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Data;
using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectStudyDaysViewModel : ViewModelBase
{
    public bool Monday { get; set; } = true;

    public bool Tuesday { get; set; } = true;

    public bool Wednesday { get; set; } = true;

    public bool Thursday { get; set; } = true;

    public bool Friday { get; set; } = true;

    public bool Saturday { get; set; } = false;

    public bool Sunday { get; set; } = false;

    public ICommand NextCommand { get; private set; }

    private readonly UserDatabase _userDatabase;

    private readonly Action _next;

    public SelectStudyDaysViewModel(UserDatabase userDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _next = next;

        NextCommand = new Command(async () => await OnNext());
    }

    private async Task OnNext()
    {
        await _userDatabase.SetStudyDayAsync(CreateStudyDay());
        _next();
    }

    private StudyDay CreateStudyDay()
    {
        StudyDay days = StudyDay.Default;

        if (Monday)
            days |= StudyDay.Monday;
        if (Tuesday)
            days |= StudyDay.Tuesday;
        if (Wednesday)
            days |= StudyDay.Wednesday;
        if (Thursday)
            days |= StudyDay.Thursday;
        if (Friday)
            days |= StudyDay.Friday;
        if (Saturday)
            days |= StudyDay.Saturday;
        if (Sunday)
            days |= StudyDay.Sunday;

        return days;
    }
}

