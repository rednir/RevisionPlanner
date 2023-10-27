using RevisionPlanner.ViewModel;

namespace RevisionPlanner.View;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
