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

    private readonly UserSubject _examSubject;

    public AddExamPageViewModel(UserDatabase userDatabase, UserSubject examSubject)
    {
        _userDatabase = userDatabase;

        _examSubject = examSubject;
    }
}
