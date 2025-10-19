using Microsoft.Extensions.Logging;
using System.IO;

namespace PomodoroFocus;

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
			});

		// DI de serviços
		builder.Services.AddSingleton<PomodoroService>();
		builder.Services.AddSingleton<AchievementService>();
		builder.Services.AddSingleton<HomeViewModel>();

		// Database path (SQLite)
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "pomodoro.db3");
		builder.Services.AddSingleton(new DatabaseService(dbPath));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();
		ServiceHelper.Services = app.Services;
		return app;
	}
}
