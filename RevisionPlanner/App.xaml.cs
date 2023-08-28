using RevisionPlanner.Model;
using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	public static readonly string AppDataRoot = Path.Combine(FileSystem.AppDataDirectory, "RevisionPlanner");

    private UserDatabase _userDatabase = new();

	public App(UserDatabase userDatabase)
	{
		_userDatabase = userDatabase;

		InitAppData();

		MainPage = new SetupView(_userDatabase, OnSetupNext);
	}

	private void InitAppData()
	{ 
		try
		{
			Directory.CreateDirectory(AppDataRoot);
		}
		catch
		{
			// TODO
			throw new NotImplementedException();
		}
    }

	private void OnSetupNext()
	{
		MainPage = new MainTabbedView();
    }
}

