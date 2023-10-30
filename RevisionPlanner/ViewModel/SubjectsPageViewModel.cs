using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class SubjectsPageViewModel : ViewModelBase
{
    public ObservableCollection<UserSubject> Subjects { get; set; } = new();

    public ICommand AddSubjectCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    private readonly StaticDatabase _staticDatabase;

    public SubjectsPageViewModel(UserDatabase userDatabase, StaticDatabase staticDatabase)
    {
        _userDatabase = userDatabase;
        _staticDatabase = staticDatabase;

        AddSubjectCommand = new Command(async () => await OnAddSubjectButtonPressed());

        Task.Run(InitSubjects);
    }

    public async Task InitSubjects()
    {
        // Clear any existing subjects to avoid duplicating if this is not the first time this method has been called.
        Subjects.Clear();

        IEnumerable<UserSubject> subjects = await _userDatabase.GetAllUserSubjectsAsync();
        foreach (var subject in subjects)
            Subjects.Add(subject);
    }

    private async Task OnAddSubjectButtonPressed()
    {
        // Initialise the select subjects page by passing in the database services and the next action.
        SelectSubjectsPage page = new(
            new SelectSubjectsViewModel(_userDatabase, _staticDatabase, async () => await OnSelectSubjectPageNext()));

        await Shell.Current.Navigation.PushModalAsync(page);
    }

    private async Task OnSelectSubjectPageNext()
    {
        // Since the list of subjects has changed, reinitialise the list of subjects displayed on this page.
        await InitSubjects();

        await Shell.Current.Navigation.PopModalAsync();
    }
}
