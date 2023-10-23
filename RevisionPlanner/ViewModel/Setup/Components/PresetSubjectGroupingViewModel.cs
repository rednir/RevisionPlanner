using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class PresetSubjectGroupingViewModel : ViewModelBase
{
    public string Name { get; set; }

    public List<PresetSubjectViewModel> SubjectViewModels { get; set; }

    private bool _isExpanded = true;

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    public ICommand ToggleExpandCommand { get; set; }

	public PresetSubjectGroupingViewModel()
    {
        ToggleExpandCommand = new Command(ToggleExpand);
    }

    private void ToggleExpand()
    {
        IsExpanded = !IsExpanded;
    }
}

