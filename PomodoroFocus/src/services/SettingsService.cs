namespace PomodoroFocus;

public class SettingsService
{
    private Settings _settings = new();

    public Settings GetSettings() => _settings;

    public void SaveSettings(Settings settings)
    {
        _settings = settings;
    }

    public int GetFocusSeconds() => _settings.FocusMinutes * 60;
    public int GetShortBreakSeconds() => _settings.ShortBreakMinutes * 60;
    public int GetLongBreakSeconds() => _settings.LongBreakMinutes * 60;
}
