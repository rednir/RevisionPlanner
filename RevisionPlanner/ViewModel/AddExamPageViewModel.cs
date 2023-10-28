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

    public string HeaderText => $"Edit {_examSubject.Name} exam";

    public string CustomName { get; set; }

    /// <summary>
    /// Represents the minimum date that the user can choose. This validates that the user inputted date is tomorrow or later.
    /// </summary>
    public DateTime MinDate { get; set; } = DateTime.Today + TimeSpan.FromDays(1);

    public DateTime SelectedDate { get; set; }

    public ICommand AddTopicsCommand { get; set; }

    public ICommand AddSubtopicsCommand { get; set; }

    public ObservableCollection<UserTopic> ExamTopics { get; set; } = new();

    //public List<UserTopic> ExamTopics
    //{
    //    get => _examTopics;
    //    set
    //    {
    //        _examTopics = value;
    //        OnPropertyChanged();
	   // }
    //}

    public ObservableCollection<UserSubtopic> ExamSubtopics { get; set; } = new();

    //public List<UserTopic> ExamSubtopics
    //{
    //    get => _examSubtopics;
    //    set
    //    {
    //        _examSubtopics = value;
    //        OnPropertyChanged();
	   // }
    //}

    private readonly UserDatabase _userDatabase;

    private readonly UserSubject _examSubject;

    public AddExamPageViewModel(UserDatabase userDatabase, UserSubject examSubject)
    {
        _userDatabase = userDatabase;
        _examSubject = examSubject;

        AddTopicsCommand = new Command(async () => await OnAddTopicsButtonPressed());
        AddSubtopicsCommand = new Command(async () => await OnAddSubtopicsButtonPressed());
    }

    private async Task OnAddTopicsButtonPressed()
    {
        string[] topicNames = _examSubject.Topics.Select(t => t.Name).ToArray();

        // Display the names of all topics in a list and get the user choice.
        string chosenTopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select topic", SELECT_ALL_TEXT, "Cancel", topicNames);

        if (chosenTopicName == SELECT_ALL_TEXT)
        {
            foreach (UserTopic topic in _examSubject.Topics)
                ExamTopics.Add(topic);

            return;
	    }

        ExamTopics.Add(_examSubject.Topics.First(t => t.Name == chosenTopicName));
    }

    private async Task OnAddSubtopicsButtonPressed()
    {
        IEnumerable<UserSubtopic> subtopics = _examSubject.Topics.SelectMany(t => t.Subtopics);
        string[] subtopicNames = subtopics.Select(s => s.Name).ToArray();

        // Display the names of all subtopics in a list and get the user choice.
        string chosenSubtopicName = await Application.Current.MainPage.DisplayActionSheet(
            "Select subtopic", SELECT_ALL_TEXT, "Cancel", subtopicNames);

        if (chosenSubtopicName == SELECT_ALL_TEXT)
        {
            foreach (UserSubtopic subtopic in subtopics)
                ExamSubtopics.Add(subtopic);

            return;
        }

        ExamSubtopics.Add(subtopics.First(s => s.Name == chosenSubtopicName));
    }
}
