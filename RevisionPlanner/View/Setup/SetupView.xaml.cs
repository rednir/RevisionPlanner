using RevisionPlanner.Data;
using RevisionPlanner.Model;
using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SetupView : NavigationPage
{
	private SelectSubjectsPage _selectSubjectsPage;

	private SelectQualificationPage _selectQualificationPage;

	private SelectStudyDaysPage _selectStudyDaysPage;

	private UserDatabase _userDatabase;

	private StaticDatabase _staticDatabase;

	private Action _nextAction;

	public SetupView(UserDatabase userDatabase, StaticDatabase staticDatabase, Action next)
	{
		_userDatabase = userDatabase;
		_staticDatabase = staticDatabase;
		_nextAction = next;

		InitialisePages();
		InitializeComponent();

		PushAsync(_selectQualificationPage);
    }

	private void InitialisePages()
	{
		_selectStudyDaysPage = new SelectStudyDaysPage(
			new SelectStudyDaysViewModel(async () => await OnSelectStudyDaysNext()));

		_selectSubjectsPage = new SelectSubjectsPage(
			new SelectSubjectsViewModel(async () => await OnSelectSubjectsNext()));

		_selectQualificationPage = new SelectQualificationPage(
			new SelectQualificationViewModel(_userDatabase, async () => await OnSelectQualificationNext()));
    }

    private async Task OnSelectQualificationNext()
	{
        await PushAsync(_selectSubjectsPage);
    }

	private async Task OnSelectSubjectsNext()
	{
		await PushAsync(_selectStudyDaysPage);
    }

    private async Task OnSelectStudyDaysNext()
	{
		_nextAction();
    }
}
