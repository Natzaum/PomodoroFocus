namespace PomodoroFocus;

public class AchievementService
{
    private List<Achievements> _achievements = new();
    private int _completedPomodoros = 0;

    public AchievementService()
    {
        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        _achievements = new List<Achievements>
        {
            new Achievements
            {
                Id = 1,
                Title = "Primeiro Pomodoro",
                Description = "Complete seu primeiro pomodoro",
                ConditionKey = "COMPLETE_1_POMODORO",
                Unlocked = false
            },
            new Achievements
            {
                Id = 2,
                Title = "Produtivo",
                Description = "Complete 10 pomodoros",
                ConditionKey = "COMPLETE_10_POMODOROS",
                Unlocked = false
            },
            new Achievements
            {
                Id = 3,
                Title = "Mestre do Foco",
                Description = "Complete 50 pomodoros",
                ConditionKey = "COMPLETE_50_POMODOROS",
                Unlocked = false
            }
        };
    }

    public void IncrementPomodoro()
    {
        _completedPomodoros++;
        CheckAchievements();
    }

    private void CheckAchievements()
    {
        foreach (var achievement in _achievements.Where(a => !a.Unlocked))
        {
            bool shouldUnlock = achievement.ConditionKey switch
            {
                "COMPLETE_1_POMODORO" => _completedPomodoros >= 1,
                "COMPLETE_10_POMODOROS" => _completedPomodoros >= 10,
                "COMPLETE_50_POMODOROS" => _completedPomodoros >= 50,
                _ => false
            };

            if (shouldUnlock)
            {
                achievement.Unlocked = true;
                achievement.UnlockedAt = DateTime.Now;
                System.Diagnostics.Debug.WriteLine($"🏆 Conquista desbloqueada: {achievement.Title}");
            }
        }
    }

    public List<Achievements> GetAchievements() => _achievements;
    
    public int GetCompletedPomodoros() => _completedPomodoros;
}
