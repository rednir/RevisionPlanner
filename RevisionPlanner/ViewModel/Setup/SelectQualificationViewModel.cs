using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectQualificationViewModel : ViewModelBase
{
    public ICommand GcseCommand { get; private set; }

    public ICommand ALevelCommand { get; private set; }

    public ICommand OtherCommand { get; private set; }

    private readonly Action _next;

    public SelectQualificationViewModel(Action next)
    {
        _next = next;

        GcseCommand = new Command(() => OnQualificationSelected(UserQualification.Gcse));
        ALevelCommand = new Command(() => OnQualificationSelected(UserQualification.ALevel));
        OtherCommand = new Command(() => OnQualificationSelected(UserQualification.Other));
    }

    private void OnQualificationSelected(UserQualification qualification)
    {
        _next();
	}
}

