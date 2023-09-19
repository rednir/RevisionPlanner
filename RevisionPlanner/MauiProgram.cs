using Microsoft.Extensions.Logging;
using RevisionPlanner.Data;
using RevisionPlanner.View;
using RevisionPlanner.View.Setup;
using RevisionPlanner.ViewModel;
using RevisionPlanner.ViewModel.Setup;

namespace RevisionPlanner;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.AddServices();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	/// <summary>
	/// Register services into the application dependency injection container. These services are resolved throughout the application.
	/// </summary>
	private static MauiAppBuilder AddServices(this MauiAppBuilder builder)
	{
		// Register database services.
		builder.Services.AddSingleton<UserDatabase>();
		builder.Services.AddSingleton<StaticDatabase>();

		// Register view models.
		builder.Services.AddSingleton<TimetableTodayViewModel>();
		builder.Services.AddTransient<SelectQualificationViewModel>();
		builder.Services.AddTransient<SelectStudyDaysViewModel>();
		builder.Services.AddTransient<SelectSubjectsViewModel>();

		// Register views.
		builder.Services.AddSingleton<TimetableTodayPage>();
		builder.Services.AddTransient<SelectQualificationPage>();
		builder.Services.AddTransient<SelectStudyDaysPage>();
		builder.Services.AddTransient<SelectSubjectsPage>();

		return builder;
    }
}

