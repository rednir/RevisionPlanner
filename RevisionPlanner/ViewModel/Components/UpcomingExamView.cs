using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel;

public class UpcomingExamViewModel : ViewModelBase
{
    public Exam Exam { get; set; }

    public string Name => Exam.Name;

    public string RemainingTimeText => $"In {(int)(Exam.Deadline - DateTime.Now).TotalDays} days";

    public float Progress { get; set; } = 0;

    public UpcomingExamViewModel(Exam exam)
    {
        Exam = exam;
    }
}
