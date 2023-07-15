using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SetupView : NavigationPage
{
	private SelectQualificationPage _selectQualificationPage;

	public SetupView()
	{
		InitialisePages();
		InitializeComponent();

		PushAsync(_selectQualificationPage);
	}

	private void InitialisePages()
	{
		_selectQualificationPage = new SelectQualificationPage(
			new SelectQualificationViewModel(
				next: () => { }
		));
    }
}
