using RevisionPlanner.ViewModel;

namespace RevisionPlanner.View;

public partial class TimetableTodayPage : ContentPage
{
	public TimetableTodayPage(TimetableTodayViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}

