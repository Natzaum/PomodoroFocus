namespace PomodoroFocus;

public class AchievementService
{
    private readonly DatabaseService _db;

    public AchievementService(DatabaseService db) => _db = db;

    public async Task CheckAchievementsAsync()
    {
        var sessions = await _db.GetSessionsAsync();
        int total = sessions.Count;

        var allAchievements = await _db.GetAchievementsAsync();

        foreach (var ach in allAchievements)
        {
            if (!ach.Unlocked && ach.ConditionKey == "COMPLETE_10_POMODOROS" && total >= 10)
            {
                ach.Unlocked = true;
                ach.UnlockedAt = DateTime.Now;
                await _db.UpdateAchievementAsync(ach);
            }
        }
    }
}
