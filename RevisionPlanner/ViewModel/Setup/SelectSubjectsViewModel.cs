using System.ComponentModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectSubjectsViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    public SelectSubjectsViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

