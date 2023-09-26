using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class PresetSubjectGroupingViewModel : ViewModelBase
{
    public IEnumerable<PresetSubject> PresetSubject { get; set; } = new List<PresetSubject>()
    {
        new PresetSubject(),
        new PresetSubject(),
        new PresetSubject(),
    };
}

