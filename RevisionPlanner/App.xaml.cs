using RevisionPlanner.Data;
using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	public static readonly string AppDataRoot = Path.Combine(FileSystem.AppDataDirectory, "RevisionPlanner");

	public static async Task DisplayAlert(string title, string message, string cancel)
		=> await Current.MainPage.DisplayAlert(title, message, cancel);

    private UserDatabase _userDatabase;

    private StaticDatabase _staticDatabase;

	public App(UserDatabase userDatabase, StaticDatabase staticDatabase)
	{
		_userDatabase = userDatabase;
		_staticDatabase = staticDatabase;

		InitAppData();

		MainPage = new SetupView(_userDatabase, _staticDatabase, OnSetupNext);
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

