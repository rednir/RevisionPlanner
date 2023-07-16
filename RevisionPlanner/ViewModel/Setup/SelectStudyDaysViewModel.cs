using System.ComponentModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectStudyDaysViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    public SelectStudyDaysViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

