using RevisionPlanner.Model;
using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	private UserDatabase _userDatabase = new();

	public App(UserDatabase userDatabase)
	{
		_userDatabase = userDatabase;
		MainPage = new SetupView(_userDatabase, OnSetupNext);
	}

	private void OnSetupNext()
	{
		MainPage = new MainTabbedView();
    }
}

