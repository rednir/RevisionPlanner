using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Data;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectSubjectsViewModel : ViewModelBase
{
    public ICommand NextCommand { get; private set; }

    private readonly UserDatabase _userDatabase;

    private readonly Action _next;

    public IEnumerable<PresetSubjectGroupingViewModel> PresetSubjectGroupingViewModels { get; set; } = new List<PresetSubjectGroupingViewModel>()
    {
        new PresetSubjectGroupingViewModel()
        {
            Name = "Grouping 1",
            Subjects = new List<PresetSubject>()
            {
                new PresetSubject
                {
                    Name = "Subject 1",
                },
                new PresetSubject
                {
                    Name = "Subject 2",
                },
                new PresetSubject
                {
                    Name = "Subject 3",
                },
            },
        },
        new PresetSubjectGroupingViewModel()
        {
            Name = "Grouping 2",
            Subjects = new List<PresetSubject>()
            {
                new PresetSubject
                {
                    Name = "Subject 1",
                },
            },
        },
    };

    public SelectSubjectsViewModel(UserDatabase userDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _next = next;

        NextCommand = new Command(next);
    }
}

