using RevisionPlanner.View;
using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner;

public partial class App : Application
{
	public App()
	{
		MainPage = new SetupView(OnSetupNext);

		InitializeComponent();
	}

	private void OnSetupNext()
	{
		MainPage = new MainTabbedView();
    }
}

