using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SelectSubjectsPage : ContentPage
{
	public SelectSubjectsPage(SelectSubjectsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
