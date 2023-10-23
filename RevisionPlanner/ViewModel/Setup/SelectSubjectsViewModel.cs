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

    public IEnumerable<PresetSubjectGroupingViewModel> PresetSubjectGroupingViewModels { get; set; }

    public SelectSubjectsViewModel(UserDatabase userDatabase, StaticDatabase staticDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _staticDatabase = staticDatabase;
        _next = next;

        NextCommand = new Command(async () => await OnNext());

        SetPresetSubjectGroupings().Wait();
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

        foreach (var presetSubjectViewModel  in presetSubjectViewModels)
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
            PresetSubjectGroupingViewModel newGrouping = new()
            {
                Name = presetSubjectViewModel.Subject.Name,
                SubjectViewModels = new List<PresetSubjectViewModel> { presetSubjectViewModel },
            };

            groupings.Add(newGrouping);
        }

        PresetSubjectGroupingViewModels = groupings;
    }

    private async Task OnNext()
    {
        _next();
    }
}

