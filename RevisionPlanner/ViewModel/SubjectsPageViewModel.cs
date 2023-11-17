using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class SubjectsPageViewModel : ViewModelBase
{
    public ObservableCollection<UserSubjectViewModel> SubjectViewModels { get; set; } = new();

    public ICommand AddSubjectCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    private readonly StaticDatabase _staticDatabase;

    public SubjectsPageViewModel(UserDatabase userDatabase, StaticDatabase staticDatabase)
    {
        _userDatabase = userDatabase;
        _staticDatabase = staticDatabase;

        // Initialise the command that will be executed when the "add subject" button is pressed.
        AddSubjectCommand = new Command(async () => await OnAddSubjectButtonPressed());

        // Run the method that initialises the list of subjects.
        Task.Run(InitSubjects);
    }

    public async Task InitSubjects()
    {
        // Clear any existing subjects to avoid duplicating if this is not the first time this method has been called.
        SubjectViewModels.Clear();

        // Iterate through the user subject list and display each one to the user.
        IEnumerable<UserSubject> subjects = await _userDatabase.GetAllUserSubjectsAsync();
        foreach (var subject in subjects)
        {
            // Initialise the view model object for the subject and add it to the list of subjects to be displayed.
            UserSubjectViewModel viewModel = new(subject);
            SubjectViewModels.Add(viewModel);
        }
    }

    private async Task OnAddSubjectButtonPressed()
    {
        // Initialise the "select subjects" page by passing in the database services and the next action.
        SelectSubjectsPage page = new(
            new SelectSubjectsViewModel(_userDatabase, _staticDatabase, async () => await OnSelectSubjectPageNext()));

        // Show the select "subjects page" to the user.
        await Shell.Current.Navigation.PushModalAsync(page);
    }

    private async Task OnSelectSubjectPageNext()
    {
        // Since the list of subjects has changed, reinitialise the list of subjects displayed on this page to update the list of subjects.
        await InitSubjects();

        // Return to this page.
        await Shell.Current.Navigation.PopModalAsync();
    }
}
