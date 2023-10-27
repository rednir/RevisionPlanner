using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectStudyDaysViewModel : ViewModelBase
{
    private bool _monday = true;
    private bool _tuesday = true;
    private bool _wednesday = true;
    private bool _thursday = true;
    private bool _friday = true;
    private bool _saturday = false;
    private bool _sunday = false;

    public bool Monday
    {
        get => _monday;
        set
        {
            _monday = value;
            OnPropertyChanged();
        }
    }

    public bool Tuesday
    {
        get => _tuesday;
        set
        {
            _tuesday = value;
            OnPropertyChanged();
        }
    }

    public bool Wednesday
    {
        get => _wednesday;
        set
        {
            _wednesday = value;
            OnPropertyChanged();
        }
    }

    public bool Thursday
    {
        get => _thursday;
        set
        {
            _thursday = value;
            OnPropertyChanged();
        }
    }

    public bool Friday
    {
        get => _friday;
        set
        {
            _friday = value;
            OnPropertyChanged();
        }
    }

    public bool Saturday
    {
        get => _saturday;
        set
        {
            _saturday = value;
            OnPropertyChanged();
        }
    }

    public bool Sunday
    {
        get => _sunday;
        set
        {
            _sunday = value;
            OnPropertyChanged();
        }
    }

    public ICommand NextCommand { get; private set; }

    private readonly UserDatabase _userDatabase;

    private readonly Action _next;

    public SelectStudyDaysViewModel(UserDatabase userDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _next = next;

        NextCommand = new Command(async () => await OnNext());

        Task.Run(SetCheckBoxes);
    }

    /// <summary>
    /// Initialises each checkbox by setting each checkbox to what they are set to in the database.
    /// </summary>
    private async Task SetCheckBoxes()
    {
        User user = await _userDatabase.GetUserAsync();

        // Leave the checkbox states as default if the user or study days has not yet been initalised.
        if (user is null || user.StudyDay == StudyDay.Default)
            return;

        StudyDay studyDay = user.StudyDay;

        // Set the checkbox states to match with the current state of the database using bitwise AND comparisons.
        Monday = (studyDay & StudyDay.Monday) == StudyDay.Monday;
        Tuesday = (studyDay & StudyDay.Tuesday) == StudyDay.Tuesday;
        Wednesday = (studyDay & StudyDay.Wednesday) == StudyDay.Wednesday;
        Thursday = (studyDay & StudyDay.Thursday) == StudyDay.Thursday;
        Friday = (studyDay & StudyDay.Friday) == StudyDay.Friday;
        Saturday = (studyDay & StudyDay.Saturday) == StudyDay.Saturday;
        Sunday = (studyDay & StudyDay.Sunday) == StudyDay.Sunday;
    }

    private async Task OnNext()
    {
        StudyDay studyDay = CreateStudyDay();

        // Validate user input by refusing to continue with zero study days selected.
        if (studyDay == StudyDay.Default)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Select at least one study day.", "OK");
            return;
	    }

        await _userDatabase.SetStudyDayAsync(studyDay);
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

