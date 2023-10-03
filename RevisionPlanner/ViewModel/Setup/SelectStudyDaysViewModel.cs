using System.ComponentModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectStudyDaysViewModel : ViewModelBase
{
    public bool Monday { get; set; } = true;

    public bool Tuesday { get; set; } = true;

    public bool Wednesday { get; set; } = true;

    public bool Thursday { get; set; } = true;

    public bool Friday { get; set; } = true;

    public bool Saturday { get; set; }

    public bool Sunday { get; set; }

    public ICommand NextCommand { get; private set; }

    public SelectStudyDaysViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

