using RevisionPlanner.ViewModel;

namespace RevisionPlanner.View;

public partial class AddExamPage : ContentPage
{
	public AddExamPage(AddExamPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
