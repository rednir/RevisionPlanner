using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View.Setup;

public partial class SelectSubjectsPage : ContentPage
{
	public SelectSubjectsPage(SelectSubjectsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
