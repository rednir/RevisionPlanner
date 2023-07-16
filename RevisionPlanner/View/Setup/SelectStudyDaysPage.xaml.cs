using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SelectStudyDaysPage : ContentPage
{
	public SelectStudyDaysPage(SelectStudyDaysViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
