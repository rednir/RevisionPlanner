using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Data;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectSubjectsViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    private readonly UserDatabase _userDatabase;

    private readonly StaticDatabase _staticDatabase;

    private readonly Action _next;

    private IEnumerable<PresetSubjectGroupingViewModel> _presetSubjectGroupingViewModels;

    public IEnumerable<PresetSubjectGroupingViewModel> PresetSubjectGroupingViewModels
    {
	    get => _presetSubjectGroupingViewModels;
	    private set
        {
            _presetSubjectGroupingViewModels = value;
            OnPropertyChanged();
	    }
    }

    public SelectSubjectsViewModel(UserDatabase userDatabase, StaticDatabase staticDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _staticDatabase = staticDatabase;
        _next = next;

        NextCommand = new Command(async () => await OnNext());

        Task.Run(SetPresetSubjectGroupings);
    }

    /// <summary>
    /// Fetches the list of preset subjects and groups them by name to show to the user.
    /// </summary>
    private async Task SetPresetSubjectGroupings()
    {
        // Get the list of preset subjects from the static database
        IEnumerable<PresetSubject> presetSubjects = await _staticDatabase.GetPresetSubjectsAsync();

        // Convert the types of all PresetSubject objects into PresetSubjectViewModel objects.
        var presetSubjectViewModels = presetSubjects.Select(s => new PresetSubjectViewModel() { Subject = s });

        List<PresetSubjectGroupingViewModel> groupings = new();

        foreach (var presetSubjectViewModel in presetSubjectViewModels)
        {
            bool existingGroupingFound = false;

            // Check if we already have a group for this subject.
            foreach (PresetSubjectGroupingViewModel grouping in groupings)
            {
                if (grouping.Name == presetSubjectViewModel.Subject.Name)
                {
                    // Add the subject to its corresponding group.
                    grouping.SubjectViewModels.Add(presetSubjectViewModel);
                    existingGroupingFound = true;
                    break;
                }
            }

            if (existingGroupingFound)
                continue;

            // If we haven't found an existing group, create a new one.
            PresetSubjectGroupingViewModel newGrouping = new() { Name = presetSubjectViewModel.Subject.Name };
            newGrouping.SubjectViewModels.Add(presetSubjectViewModel);

            groupings.Add(newGrouping);
        }

        PresetSubjectGroupingViewModels = groupings;
    }

    private async Task OnNext()
    {
        // Get the subjects that the user has selected, not including null subjects which are unselected.
        IEnumerable<PresetSubjectViewModel> selectedSubjects = PresetSubjectGroupingViewModels.Select(g => g.SelectedSubject).Where(s => s is not null);
     
        // Validate the user input by showing an error message if no subjects were selected.
        if (!selectedSubjects.Any())
        {
            await App.DisplayAlert("Error", "Select at least one subject", "OK");
            return;
	    }

        _next();
    }
}

