using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class DashboardPageViewModel : ViewModelBase
{
    public ICommand AddExamCommand { get; set; }

    public string TotalExamCount { get; set; }

    // The list of upcoming exams to be displayed in the user interface.
    public ObservableCollection<UpcomingExamViewModel> UpcomingExamViewModels { get; set; } = new();

    private readonly UserDatabase _userDatabase;

    public DashboardPageViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        // Initialise the command that is executed when the user presses the "add exam" button.
        AddExamCommand = new Command(async () => await OnAddExamButtonPressed());

        // Initialise the list of upcoming exams.
        Task.Run(InitUpcomingExams);
    }

    private async Task InitUpcomingExams()
    {
        // Avoid duplicates if this is not the first time this method has been run.
        UpcomingExamViewModels.Clear();

        // Get total number of exams created by the user
        int count = await _userDatabase.GetExamCountAsync();
        TotalExamCount = $"Total exam count: {count}";

        IEnumerable<Exam> exams = await _userDatabase.GetExamsAsync();

        foreach (Exam exam in exams)
        {
            // Create a view model for each exam and add it to the list of upcoming exams to be displayed.
            UpcomingExamViewModel viewModel = new(exam);
            UpcomingExamViewModels.Add(viewModel);
	    }
    }

    private async Task OnAddExamButtonPressed()
    {
        UserSubject examSubject = await PromptExamSubject();

        // Do not continue if the user chose to cancel the exam creation.
        if (examSubject is null)
            return;

        AddExamPage page = new(
	        new AddExamPageViewModel(_userDatabase, examSubject));

        // Show the "add exam" page.
        await Shell.Current.Navigation.PushModalAsync(page);

        // Update the list of upcoming exams shown to the user once the user has created a new exam.
        page.Unloaded += async (s, e) => await InitUpcomingExams();
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
