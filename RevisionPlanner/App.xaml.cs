using RevisionPlanner.View;
using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new SelectQualificationPage(new SelectQualificationViewModel());
	}
}

