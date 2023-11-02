using RevisionPlanner.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class TimetableTodayViewModel : ViewModelBase
{
    public ObservableCollection<UserTaskViewModel> UserTaskViewModels { get; set; } = new()
    {
        new(new()),
        new(new()),
    };

    public ICommand AddCustomTaskCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    public TimetableTodayViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        AddCustomTaskCommand = new Command(() => throw new NotImplementedException());
    }
}
