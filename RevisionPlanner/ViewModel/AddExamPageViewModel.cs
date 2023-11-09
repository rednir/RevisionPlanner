using RevisionPlanner.Data;
using RevisionPlanner.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class AddExamPageViewModel : ViewModelBase
{
    public const string SELECT_ALL_TEXT = "Select all";

    public const string CANCEL_TEXT = "Cancel";

    public string HeaderText => $"Edit {_examSubject.Name} exam";

    public string CustomName { get; set; }

    /// <summary>
    /// Represents the minimum date that the user can choose. This validates that the user inputted date is tomorrow or later.
    /// </summary>
    public DateTime MinDate { get; set; } = DateTime.Today + TimeSpan.FromDays(1);

    public DateTime SelectedDate { get; set; } = DateTime.Today + TimeSpan.FromDays(1);

    public ICommand AddTopicsCommand { get; set; }

    public ICommand AddSubtopicsCommand { get; set; }

    public ICommand SaveExamCommand { get; set; }

    public ObservableCollection<ExamContentViewModel> ContentViewModels { get; set; } = new();

    /// <summary>
    /// When the user presses the save exam button, remove the exam with this ID, effectively replacing it.
    /// </summary>
    public int? ExamIdToReplace { get; set; } = null;

    private readonly UserDatabase _userDatabase;

    private readonly UserSubject _examSubject;

    public AddExamPageViewModel(UserDatabase userDatabase, UserSubject examSubject)
    {
        _userDatabase = userDatabase;
        _examSubject = examSubject;

        AddTopicsCommand = new Command(async () => await OnAddTopicsButtonPressed());
        AddSubtopicsCommand = new Command(async () => await OnAddSubtopicsButtonPressed());
        SaveExamCommand = new Command(async () => await OnSaveExamButtonPressed());
    }

    private async Task OnAddTopicsButtonPressed()
    {
        // Select topics which have not yet been selected by the user.
        IEnumerable<UserTopic> topics = _examSubject.Topics
            .Where(t => !ContentViewModels.Select(c => c.Content.Name).Contains(t.Name));

        // Return if there are no topics to select.
        if (!topics.Any())
            return;

        string[] topicNames = topics.Select(t => t.Name).ToArray();

        // Display the names of all topics in a list and get the user choice.
        string chosenTopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select topic", CANCEL_TEXT, SELECT_ALL_TEXT, topicNames);

        AddCourseContent(chosenTopicName, topics);
    }

    private async Task OnAddSubtopicsButtonPressed()
    {
        // Select subtopics which have not yet been selected by the user.
        IEnumerable<UserSubtopic> subtopics = _examSubject.Topics.SelectMany(t => t.Subtopics)
            .Where(s => !ContentViewModels.Select(c => c.Content.Name).Contains(s.Name));

        // Return if there are no subtopics to select.
        if (!subtopics.Any())
            return;

        string[] subtopicNames = subtopics.Select(s => s.Name).ToArray();

        // Display the names of all subtopics in a list and get the user choice.
        string chosenSubtopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select subtopic", CANCEL_TEXT, SELECT_ALL_TEXT, subtopicNames);

        AddCourseContent(chosenSubtopicName, subtopics);
    }

    /// <summary>
    /// Adds course content to the list of content for this exam. Content could be either a topic or a subtopic. so this method uses polymorpism to process all course content with the same code regardless of type.
    /// </summary>
    private void AddCourseContent(string chosenContentName, IEnumerable<CourseContent> contentToAdd)
    {
        if (chosenContentName == SELECT_ALL_TEXT)
        {
            // Enumerate though all remaining content and add them to the list.
            foreach (CourseContent content in contentToAdd)
            {
                ExamContentViewModel thisContentViewModel = new(content, OnContentRemoveButtonPressed);
                ContentViewModels.Add(thisContentViewModel);
            }

            return;
        }

        // Select the chosen content from its name and create the view model to show to the user in a list.
        CourseContent chosenContent = contentToAdd.FirstOrDefault(s => s.Name == chosenContentName);

        // Assume the user cancelled if null so do not continue.
        if (chosenContent is null)
            return;

        ExamContentViewModel contentViewModel = new(chosenContent, OnContentRemoveButtonPressed);

        ContentViewModels.Add(contentViewModel);
    }

    private void OnContentRemoveButtonPressed(ExamContentViewModel contentViewModel)
        => ContentViewModels.Remove(contentViewModel);

    private async Task OnSaveExamButtonPressed()
    {
        if (ContentViewModels.Count <= 0)
        {
            // Validate user input, do not continue if no content is added to the exam.
            await Application.Current.MainPage.DisplayAlert("Error", "You must add at least one piece of content to the exam.", "OK");
            return;
        }

        if (ExamIdToReplace is not null)
        {
            // Replace the existing exam the user wants to edit.
            await _userDatabase.RemoveExamAsync(ExamIdToReplace.Value);
        }

        // Add the exam to the user database.
        Exam exam = BuildExamObject();
        await _userDatabase.AddExamAsync(exam);
	    
	    // Pop this page from the navigation stack.
        await Application.Current.MainPage.Navigation.PopModalAsync();
    }

    private Exam BuildExamObject()
    {
        // Initialise the exam object using the state of this view model from the user input.
        Exam exam = new()
        {
            CustomName = string.IsNullOrWhiteSpace(CustomName) ? null : CustomName,
            SubjectId = _examSubject.Id,
            SubjectName = _examSubject.Name,
            Deadline = SelectedDate,
            Content = ContentViewModels.Select(c => c.Content).ToArray(),
        };

        // Set the exam ID to the unique hash code generated from its attributes.
        exam.Id = exam.GetHashCode();

        return exam;
    }
}
