using SQLite;

public class PomoSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Type { get; set; } = string.Empty; // "focus", "short_break", "long_break"
}