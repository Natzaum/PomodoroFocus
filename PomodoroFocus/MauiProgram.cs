using Microsoft.Extensions.Logging;
using System.IO;

namespace PomodoroFocus;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		try
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			// DI de serviços
			builder.Services.AddSingleton<DatabaseService>(sp =>
			{
				string dbPath = Path.Combine(FileSystem.AppDataDirectory, "pomodoro.db3");
				return new DatabaseService(dbPath);
			});
			builder.Services.AddSingleton<AchievementService>();
			builder.Services.AddSingleton<PomodoroService>();
			builder.Services.AddSingleton<SoundService>();
			builder.Services.AddSingleton<HomeViewModel>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

			var app = builder.Build();
			ServiceHelper.Services = app.Services;
			
			System.Diagnostics.Debug.WriteLine("MauiApp created successfully!");
			return app;
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error in CreateMauiApp: {ex}");
			throw;
		}
	}
}
