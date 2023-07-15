using System.ComponentModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectQualificationViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    public SelectQualificationViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

