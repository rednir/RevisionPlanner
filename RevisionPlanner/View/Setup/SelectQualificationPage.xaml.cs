using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner.View;

public partial class SelectQualificationPage : ContentPage
{
	public SelectQualificationPage(SelectQualificationViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
