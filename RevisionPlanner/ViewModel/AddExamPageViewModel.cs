using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    public DateTime SelectedDate { get; set; }

    public ICommand AddTopicsCommand { get; set; }

    public ICommand AddSubtopicsCommand { get; set; }

    public ICommand SaveExamCommand { get; set; }

    public ObservableCollection<ExamContentViewModel> Content { get; set; } = new();

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
            .Where(t => !Content.Select(c => c.Content.Name).Contains(t.Name));

        // Return if there are no topics to select.
        if (!topics.Any())
            return;

        string[] topicNames = topics.Select(t => t.Name).ToArray();

        // Display the names of all topics in a list and get the user choice.
        string chosenTopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select topic", SELECT_ALL_TEXT, CANCEL_TEXT, topicNames);

        if (chosenTopicName == SELECT_ALL_TEXT)
        {
            // Enumerate though all remaining topics of this subject and add them to the list.
            foreach (UserTopic topic in topics)
            {
                ExamContentViewModel thisContentViewModel = new(topic, OnContentRemoveButtonPressed);
                Content.Add(thisContentViewModel);
            }

            return;
	    }

        // Select the chosen topic from its name and create the view model to show to the user in a list.
        UserTopic chosenTopic = topics.First(t => t.Name == chosenTopicName);

        // Assume the user cancelled if null so do not continue.
        if (chosenTopic is null)
            return;

        ExamContentViewModel contentViewModel = new(chosenTopic, OnContentRemoveButtonPressed);

        Content.Add(contentViewModel);
    }

    private async Task OnAddSubtopicsButtonPressed()
    {
        // Select subtopics which have not yet been selected by the user.
        IEnumerable<UserSubtopic> subtopics = _examSubject.Topics.SelectMany(t => t.Subtopics)
            .Where(s => !Content.Select(c => c.Content.Name).Contains(s.Name));

        // Return if there are no subtopics to select.
        if (!subtopics.Any())
            return;

        string[] subtopicNames = subtopics.Select(s => s.Name).ToArray();

        // Display the names of all subtopics in a list and get the user choice.
        string chosenSubtopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select subtopic", SELECT_ALL_TEXT, CANCEL_TEXT, subtopicNames);

        if (chosenSubtopicName == SELECT_ALL_TEXT)
        {
            foreach (UserSubtopic subtopic in subtopics)
            {
                ExamContentViewModel thisContentViewModel = new(subtopic, OnContentRemoveButtonPressed);
                Content.Add(thisContentViewModel);
            }

            return;
        }

        UserSubtopic chosenSubtopic = subtopics.FirstOrDefault(s => s.Name == chosenSubtopicName);

        // Assume the user cancelled if null so do not continue.
        if (chosenSubtopic is null)
            return;

        ExamContentViewModel contentViewModel = new(chosenSubtopic, OnContentRemoveButtonPressed);

        Content.Add(contentViewModel);
    }

    private void OnContentRemoveButtonPressed(ExamContentViewModel contentViewModel)
        => Content.Remove(contentViewModel);

    private async Task OnSaveExamButtonPressed()
    {
        await Application.Current.MainPage.Navigation.PopModalAsync();
    }
}
