using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectSubjectsViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    public IEnumerable<PresetSubject> PresetSubjects { get; set; } = new List<PresetSubject>()
    {
        new PresetSubject(),
        new PresetSubject(),
        new PresetSubject(),
    };

    public SelectSubjectsViewModel(Action next)
    {
        NextCommand = new Command(next);
    }
}

