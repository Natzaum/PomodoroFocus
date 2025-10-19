namespace PomodoroFocus;

public static class ServiceHelper
{
    public static IServiceProvider Services { get; set; } = default!;

    public static T GetService<T>() where T : notnull
        => Services.GetRequiredService<T>();
}
