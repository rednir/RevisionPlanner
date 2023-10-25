using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class PresetSubjectGroupingViewModel : ViewModelBase
{
    public string Name { get; set; }

    public ObservableCollection<PresetSubjectViewModel> SubjectViewModels { get; private set; } = new();

    public PresetSubjectViewModel SelectedSubject { get; private set; }

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
        SubjectViewModels.CollectionChanged += OnPresetSubjectCollectionChanged;
    }

    private void ToggleExpand()
    {
        IsExpanded = !IsExpanded;
    }

    private void OnPresetSubjectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Return if no new items were added.
        if (e.NewItems == null)
            return;

        // Start listening for property changes for the new items.
        foreach (PresetSubjectViewModel item in e.NewItems)
            item.PropertyChanged += OnPresetSubjectCollectionChanged;
    }

    private void OnPresetSubjectCollectionChanged(object sender, PropertyChangedEventArgs e)
    {
        // We only care about state changes for the check box.
        if (e.PropertyName != "IsChecked")
            return;

        // Initially assume that no subject is selected.
        SelectedSubject = null;

        // If one subject is checked, disable all other subjects in this grouping. Otherwise, enable all subjects.
        bool noSubjectChecked = !SubjectViewModels.Any(s => s.IsChecked);
        foreach (var subject in SubjectViewModels)
        {
            if (subject.IsChecked)
            {
                subject.IsEnabled = true;
                SelectedSubject = subject;
                continue;
            }

            subject.IsEnabled = noSubjectChecked;
        }
    }
}

