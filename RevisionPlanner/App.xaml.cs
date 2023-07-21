using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	public App()
	{
		MainPage = new SetupView(OnSetupNext);
	}

	private void OnSetupNext()
	{
		MainPage = new MainTabbedView();
    }
}

