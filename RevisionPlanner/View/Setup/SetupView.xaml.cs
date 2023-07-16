using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SetupView : NavigationPage
{
	private SelectSubjectsPage _selectSubjectsPage;

	private SelectQualificationPage _selectQualificationPage;

	private SelectStudyDaysPage _selectStudyDaysPage;

	private Action _nextAction;

	public SetupView(Action next)
	{
		_nextAction = next;

		InitialisePages();
		InitializeComponent();

		PushAsync(_selectQualificationPage);
	}

	private void InitialisePages()
	{
		_selectStudyDaysPage = new SelectStudyDaysPage(new SelectStudyDaysViewModel(
			next: async () => await OnSelectStudyDaysNext()));

		_selectSubjectsPage = new SelectSubjectsPage(new SelectSubjectsViewModel(
			next: async () => await OnSelectSubjectsNext()));

		_selectQualificationPage = new SelectQualificationPage(new SelectQualificationViewModel(
			next: async () => await OnSelectQualificationNext()));
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
