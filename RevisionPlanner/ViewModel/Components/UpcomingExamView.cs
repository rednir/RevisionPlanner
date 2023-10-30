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

public class UpcomingExamViewModel : ViewModelBase
{
    public Exam Exam { get; set; }

    public string Name => Exam.Name;

    public string RemainingTimeText => $"In {(Exam.Deadline - DateTime.Now).TotalDays} days";

    public UpcomingExamViewModel(Exam exam)
    {
        Exam = exam;
    }
}
