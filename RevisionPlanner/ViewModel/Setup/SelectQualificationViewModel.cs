using System.ComponentModel;
using System.Windows.Input;
using RevisionPlanner.Model;

namespace RevisionPlanner.ViewModel.Setup;

public class SelectQualificationViewModel : ViewModelBase
{
    public Action<UserQualification> Next { get; set; }

    public ICommand GcseCommand { get; private set; }

    public ICommand ALevelCommand { get; private set; }

    public ICommand OtherCommand { get; private set; }

    public SelectQualificationViewModel()
    {
        GcseCommand = new Command(() => OnQualificationSelected(UserQualification.Gcse));
        ALevelCommand = new Command(() => OnQualificationSelected(UserQualification.ALevel));
        OtherCommand = new Command(() => OnQualificationSelected(UserQualification.Other));
    }

    private void OnQualificationSelected(UserQualification qualification)
    {
        Next(qualification);
	}
}

