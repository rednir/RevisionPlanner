using RevisionPlanner.Data;
using RevisionPlanner.View;

namespace RevisionPlanner;

public partial class App : Application
{
	public static readonly string AppDataRoot = Path.Combine(FileSystem.AppDataDirectory, "RevisionPlanner");

    private readonly UserDatabase _userDatabase;

    private readonly StaticDatabase _staticDatabase;

	public App(UserDatabase userDatabase, StaticDatabase staticDatabase)
	{
		_userDatabase = userDatabase;
		_staticDatabase = staticDatabase;

		if (!InitAppData())
		{
			// If there was an error in intialising the application, handle the exception by displaying an error message to the user.
			MainPage = new ContentPage();
			MainPage.Loaded += async(o, e) => await MainPage.DisplayAlert("Error", "Could not create the application data directory", "OK");
			return;
		}

		// Otherwise continue launching the application as normal.
		MainPage = new SetupView(_userDatabase, _staticDatabase, OnSetupNext);
	}

	private bool InitAppData()
	{ 
		try
		{
			Directory.CreateDirectory(AppDataRoot);
		}
		catch
		{
			return false;
		}

		return true;
    }

	private void OnSetupNext()
	{
		MainPage = new MainTabbedView();
    }
}

