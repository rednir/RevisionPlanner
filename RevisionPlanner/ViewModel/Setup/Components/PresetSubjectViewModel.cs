using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class PresetSubjectViewModel : ViewModelBase
{
    public PresetSubject Subject { get; set; }

    public string Name => Subject.Name;

    public string Details => $"{Subject.ExamBoard} {Subject.Qualification}";
}

