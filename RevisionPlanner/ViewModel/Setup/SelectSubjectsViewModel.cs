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
        // Get the list of preset subjects from the static database.
        IEnumerable<PresetSubject> presetSubjects = await _staticDatabase.GetPresetSubjectsAsync();

        // Convert the types of all PresetSubject objects into PresetSubjectViewModel objects which we can display.
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

            // Add this group to the list of groups.
            groupings.Add(newGrouping);
        }

        // Set the list of groups to be displayed in the GUI.
        PresetSubjectGroupingViewModels = groupings;

        await SetPresetSubjectCheckedState();
    }

    /// <summary>
    /// Sets the checked state of the preset subjects based on whether they already exist in the user database. 
    /// </summary>
    private async Task SetPresetSubjectCheckedState()
    {
        // Get the list of all of the preset subject view models.
        IEnumerable<PresetSubjectViewModel> subjectViewModels = PresetSubjectGroupingViewModels.SelectMany(g => g.SubjectViewModels);

        foreach (PresetSubjectViewModel subjectViewModel in subjectViewModels)
        {
            // Get the corresponding user subject if it exists.
            UserSubject userSubject = await _userDatabase.GetUserSubjectAsync(-subjectViewModel.Subject.Id);

            // If there is no corresponding user subject, this preset subject was not selected by the user.
            if (userSubject is null)
                continue;

            // Otherwise display this preset subject as already selected.
            subjectViewModel.IsChecked = true;
	    }
    }

    /// <summary>
    /// The method that gets called when the next button is pressed.
    /// </summary>
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

        await _userDatabase.RemoveAllUserSubjectsAsync();

        foreach (var selectedSubjectViewModel in selectedSubjects)
        {
            PresetSubject presetSubject = selectedSubjectViewModel.Subject;

            // Initialise the new user subject from the preset subject's attributes
            UserSubject userSubject = new(presetSubject);
            await _userDatabase.AddUserSubjectAsync(userSubject);
	    }

        _next();
    }
}

