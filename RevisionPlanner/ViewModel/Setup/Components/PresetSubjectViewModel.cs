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

    private bool _isChecked;

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    private bool _isEnabled = true;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            OnPropertyChanged();
        }
    }
}

