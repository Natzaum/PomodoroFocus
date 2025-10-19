using SQLite;

namespace PomodoroFocus;

public class Settings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int FocusTime { get; set; } = 25;
    public int ShortBreak { get; set; } = 5;
    public int LongBreak { get; set; } = 15;
    public bool DarkMode { get; set; } = false;
}