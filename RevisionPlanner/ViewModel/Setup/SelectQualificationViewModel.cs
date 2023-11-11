using System.Windows.Input;
using RevisionPlanner.Data;
using RevisionPlanner.Model.Enums;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectQualificationViewModel : ViewModelBase
{
    public ICommand GcseCommand { get; private set; }

    public ICommand ALevelCommand { get; private set; }

    public ICommand OtherCommand { get; private set; }

    private readonly UserDatabase _userDatabase;

    private readonly Action _next;

    public SelectQualificationViewModel(UserDatabase userDatabase, Action next)
    {
        _userDatabase = userDatabase;
        _next = next;

        GcseCommand = new Command(async () => await OnQualificationSelected(UserQualification.Gcse));
        ALevelCommand = new Command(async () => await OnQualificationSelected(UserQualification.ALevel));
        OtherCommand = new Command(async () => await OnQualificationSelected(UserQualification.Other));
    }

    private async Task OnQualificationSelected(UserQualification qualification)
    {
        await _userDatabase.SetUserQualificationAsync(qualification);
        _next();
	}
}

