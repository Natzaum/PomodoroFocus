using SQLite;

namespace PomodoroFocus;

public class Achievements
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Unlocked { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public string ConditionKey { get; set; } = string.Empty; // ex: "COMPLETE_10_POMODOROS"
}