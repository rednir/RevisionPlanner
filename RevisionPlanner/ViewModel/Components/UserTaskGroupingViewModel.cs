using RevisionPlanner.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class UserTaskGroupingViewModel : ViewModelBase
{
    /// <summary>
    /// Shows the dates of the user tasks in this collection in a string format.
    /// </summary>
    public string DateText => $"{Date.ToLongDateString()}";

    public DateTime Date { get; set; }
    
    public ObservableCollection<UserTaskViewModel> UserTaskViewModels { get; set; } = new();

    public UserTaskGroupingViewModel(IEnumerable<UserTaskViewModel> viewModels, DateTime date)
    {
        Date = date;
        foreach (var viewModel in viewModels)
            UserTaskViewModels.Add(viewModel);
    }
}
