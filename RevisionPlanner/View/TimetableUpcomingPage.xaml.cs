using RevisionPlanner.ViewModel;

namespace RevisionPlanner.View;

public partial class TimetableUpcomingPage : ContentPage
{
	public TimetableUpcomingPage(TimetableUpcomingViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
