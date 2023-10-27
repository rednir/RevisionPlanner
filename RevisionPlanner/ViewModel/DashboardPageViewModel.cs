using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class DashboardPageViewModel : ViewModelBase
{
    public ICommand AddExamCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    public DashboardPageViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        AddExamCommand = new Command(async () => await OnAddExamButtonPressed());
    }

    private async Task OnAddExamButtonPressed()
    {
        UserSubject examSubject = await PromptExamSubject();

        // Do not continue if the user chose to cancel the exam creation.
        if (examSubject is null)
            return;

        AddExamPage page = new(
	        new AddExamPageViewModel(_userDatabase, examSubject));

        await Shell.Current.Navigation.PushModalAsync(page);
    }

    private async Task<UserSubject> PromptExamSubject()
    {
        var userSubjects = await _userDatabase.GetAllUserSubjectsAsync();

        // Put the names of each subject in an array to be displayed in a modal dialog.
        string[] subjectNames = userSubjects.Select(s => s.Name).ToArray();

        // Display the dialog and get the action that the user chooses.
        string chosenSubjectName = await Application.Current.MainPage.DisplayActionSheet(
	        "Choose exam subject", "Cancel", null, subjectNames);

        // Return the UserSubject object that represents the chosen subject or null if cancel is selected.
        return userSubjects.FirstOrDefault(s => s.Name == chosenSubjectName);
    }
}
