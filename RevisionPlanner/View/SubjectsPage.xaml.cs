using RevisionPlanner.ViewModel;

namespace RevisionPlanner.View;

public partial class SubjectsPage : ContentPage
{
	public SubjectsPage(SubjectsPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
