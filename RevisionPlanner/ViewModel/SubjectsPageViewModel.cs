using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class SubjectsPageViewModel : ViewModelBase
{
    private IEnumerable<UserSubject> _subjects;

    public IEnumerable<UserSubject> Subjects
    {
        get => _subjects;
        private set
        {
            _subjects = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddSubjectCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    private readonly StaticDatabase _staticDatabase;

    public SubjectsPageViewModel(UserDatabase userDatabase, StaticDatabase staticDatabase)
    {
        _userDatabase = userDatabase;
        _staticDatabase = staticDatabase;

        AddSubjectCommand = new Command(async () => await OnAddSubjectButtonPressed());
    }

    private async Task OnAddSubjectButtonPressed()
    {
        SelectSubjectsPage page = new(
            new SelectSubjectsViewModel(_userDatabase, _staticDatabase, async () => await OnSelectSubjectPageNext()));

        await Shell.Current.Navigation.PushModalAsync(page);
    }

    private async Task OnSelectSubjectPageNext()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }
}
