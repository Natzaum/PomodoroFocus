using SQLite;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<PomoSession>().Wait();
        _database.CreateTableAsync<Achievements>().Wait();
        _database.CreateTableAsync<Settings>().Wait();
    }

    public Task<List<PomoSession>> GetSessionsAsync() => _database.Table<PomoSession>().ToListAsync();
    public Task<int> SaveSessionAsync(PomoSession session) => _database.InsertAsync(session);

    public Task<List<Achievements>> GetAchievementsAsync() => _database.Table<Achievements>().ToListAsync();
    public Task<int> UpdateAchievementAsync(Achievements ach) => _database.UpdateAsync(ach);
}
