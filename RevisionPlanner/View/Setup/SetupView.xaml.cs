using RevisionPlanner.Data;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SetupView : NavigationPage
{
	private readonly UserDatabase _userDatabase;

	private readonly StaticDatabase _staticDatabase;

	private readonly Action _nextAction;

	public SetupView(UserDatabase userDatabase, StaticDatabase staticDatabase, Action next)
	{
		_userDatabase = userDatabase;
		_staticDatabase = staticDatabase;
		_nextAction = next;

		InitializeComponent();

        // Intialise the first page of the setup, including the method that will be called next.
        SelectQualificationPage page = new(
            new SelectQualificationViewModel(_userDatabase, async () => await OnSelectQualificationNext()));

        // Push the first page of the setup to the stack, showing it to the user.
        PushAsync(page);
    }

    private async Task OnSelectQualificationNext()
	{
        SelectSubjectsPage page = new(
            new SelectSubjectsViewModel(_userDatabase, _staticDatabase, async () => await OnSelectSubjectsNext()));

        await PushAsync(page);
    }

	private async Task OnSelectSubjectsNext()
	{
        SelectStudyDaysPage page = new(
            new SelectStudyDaysViewModel(_userDatabase, async () => await OnSelectStudyDaysNext()));

        await PushAsync(page);
    }

    private async Task OnSelectStudyDaysNext()
	{
		_nextAction();
    }
}
