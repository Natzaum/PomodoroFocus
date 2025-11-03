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

		builder.Services.AddSingleton<SettingsService>();
		builder.Services.AddSingleton<SoundService>();
		builder.Services.AddSingleton<ThemeService>();
		builder.Services.AddSingleton<ColorSchemeService>();
		builder.Services.AddSingleton<SoundCustomizationService>();
		builder.Services.AddSingleton<AchievementNotificationService>();
		builder.Services.AddSingleton<AchievementService>();
		builder.Services.AddSingleton<PomodoroService>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<AchievementsViewModel>();
		builder.Services.AddSingleton<AchievementsPage>();

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
