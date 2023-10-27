using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RevisionPlanner.ViewModel;

public class DashboardPageViewModel : ViewModelBase
{
    public ICommand AddExamCommand { get; set; }

    private readonly UserDatabase _userDatabase;

    public DashboardPageViewModel(UserDatabase userDatabase)
    {
        _userDatabase = userDatabase;

        AddExamCommand = new Command(async () => await OnAddExamButtonPressed());
    }

    private async Task OnAddExamButtonPressed()
    {
        AddExamPage page = new();

        await Shell.Current.Navigation.PushModalAsync(page);
    }
}
