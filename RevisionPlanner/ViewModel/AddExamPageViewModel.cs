using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class AddExamPageViewModel : ViewModelBase
{
    public ICommand AddExamCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    public AddExamPageViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;
    }
}
