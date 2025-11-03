using SQLite;

namespace PomodoroFocus;

public class AchievementService
{
    private SQLiteAsyncConnection? _database;
    private string _dbPath;
    private int _completedPomodoros = 0;
    private readonly string _dbFileName = "pomodorofocus.db3";

    public AchievementService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, _dbFileName);
    }

    private async Task InitializeDatabase()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<Achievements>();
        
        // Verifica se já existem conquistas no banco
        var count = await _database.Table<Achievements>().CountAsync();
        if (count == 0)
        {
            await SeedInitialAchievements();
        }
        
        // Carrega o total de pomodoros completados
        await LoadCompletedPomodoros();
    }

    private async Task SeedInitialAchievements()
    {
        var achievements = new List<Achievements>
        {
            new Achievements
            {
                Title = "Primeiro Pomodoro",
                Description = "Complete seu primeiro pomodoro",
                ConditionKey = "COMPLETE_1_POMODORO",
                Unlocked = false
            },
            new Achievements
            {
                Title = "Produtivo",
                Description = "Complete 10 pomodoros",
                ConditionKey = "COMPLETE_10_POMODOROS",
                Unlocked = false
            },
            new Achievements
            {
                Title = "Mestre do Foco",
                Description = "Complete 50 pomodoros",
                ConditionKey = "COMPLETE_50_POMODOROS",
                Unlocked = false
            },
            new Achievements
            {
                Title = "Guerreiro Incansável",
                Description = "Complete 100 pomodoros",
                ConditionKey = "COMPLETE_100_POMODOROS",
                Unlocked = false
            },
            new Achievements
            {
                Title = "Lenda da Produtividade",
                Description = "Complete 250 pomodoros",
                ConditionKey = "COMPLETE_250_POMODOROS",
                Unlocked = false
            }
        };

        foreach (var achievement in achievements)
        {
            await _database!.InsertAsync(achievement);
        }
    }

    private async Task LoadCompletedPomodoros()
    {
        var unlockedAchievements = await _database!
            .Table<Achievements>()
            .Where(a => a.Unlocked)
            .OrderByDescending(a => a.UnlockedAt)
            .ToListAsync();

        // Detecta o maior threshold de pomodoros desbloqueado
        _completedPomodoros = unlockedAchievements
            .Select(a => int.Parse(a.ConditionKey.Replace("COMPLETE_", "").Replace("_POMODOROS", "")))
            .DefaultIfEmpty(0)
            .Max();
    }

    public async Task IncrementPomodoro()
    {
        if (_database is null)
            await InitializeDatabase();

        _completedPomodoros++;
        await CheckAchievements();
    }

    private async Task CheckAchievements()
    {
        var unlockedAchievements = await _database!.Table<Achievements>().Where(a => !a.Unlocked).ToListAsync();

        foreach (var achievement in unlockedAchievements)
        {
            bool shouldUnlock = achievement.ConditionKey switch
            {
                "COMPLETE_1_POMODORO" => _completedPomodoros >= 1,
                "COMPLETE_10_POMODOROS" => _completedPomodoros >= 10,
                "COMPLETE_50_POMODOROS" => _completedPomodoros >= 50,
                "COMPLETE_100_POMODOROS" => _completedPomodoros >= 100,
                "COMPLETE_250_POMODOROS" => _completedPomodoros >= 250,
                _ => false
            };

            if (shouldUnlock)
            {
                achievement.Unlocked = true;
                achievement.UnlockedAt = DateTime.Now;
                await _database!.UpdateAsync(achievement);
                System.Diagnostics.Debug.WriteLine($"🏆 Conquista desbloqueada: {achievement.Title}");
            }
        }
    }

    public async Task<List<Achievements>> GetAchievements()
    {
        if (_database is null)
            await InitializeDatabase();

        return await _database!.Table<Achievements>().ToListAsync();
    }

    public async Task<int> GetCompletedPomodoros()
    {
        if (_database is null)
            await InitializeDatabase();

        return _completedPomodoros;
    }
}
