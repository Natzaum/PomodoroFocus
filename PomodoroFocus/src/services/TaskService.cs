using SQLite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PomodoroFocus;

/// <summary>
/// Serviço para gerenciar tasks com SQLite
/// </summary>
public class TaskService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;
    private const string DbFileName = "pomodorofocus.db3";

    public TaskService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
    }

    private async global::System.Threading.Tasks.Task InitializeDatabase()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);
        await _database.CreateTableAsync<Task>();
        System.Diagnostics.Debug.WriteLine("✅ Banco de dados de tasks inicializado");
    }

    /// <summary>
    /// Adiciona uma nova task
    /// </summary>
    public async global::System.Threading.Tasks.Task<int> AddTask(Task task)
    {
        if (_database is null)
            await InitializeDatabase();

        task.CreatedAt = DateTime.Now;
        int id = await _database!.InsertAsync(task);
        System.Diagnostics.Debug.WriteLine($"✅ Task criada: {task.Title} (ID: {id})");
        return id;
    }

    /// <summary>
    /// Obtém todas as tasks
    /// </summary>
    public async global::System.Threading.Tasks.Task<List<Task>> GetAllTasks()
    {
        if (_database is null)
            await InitializeDatabase();

        return await _database!.Table<Task>().OrderByDescending(t => t.Priority).ToListAsync();
    }

    /// <summary>
    /// Obtém tasks por status
    /// </summary>
    public async global::System.Threading.Tasks.Task<List<Task>> GetTasksByStatus(TaskStatus status)
    {
        if (_database is null)
            await InitializeDatabase();

        return await _database!.Table<Task>().Where(t => t.Status == status).OrderByDescending(t => t.Priority).ToListAsync();
    }

    /// <summary>
    /// Obtém uma task pelo ID
    /// </summary>
    public async global::System.Threading.Tasks.Task<Task?> GetTaskById(int id)
    {
        if (_database is null)
            await InitializeDatabase();

        return await _database!.Table<Task>().Where(t => t.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Atualiza uma task
    /// </summary>
    public async global::System.Threading.Tasks.Task UpdateTask(Task task)
    {
        if (_database is null)
            await InitializeDatabase();

        await _database!.UpdateAsync(task);
        System.Diagnostics.Debug.WriteLine($"✅ Task atualizada: {task.Title}");
    }

    /// <summary>
    /// Deleta uma task
    /// </summary>
    public async global::System.Threading.Tasks.Task DeleteTask(int id)
    {
        if (_database is null)
            await InitializeDatabase();

        await _database!.DeleteAsync<Task>(id);
        System.Diagnostics.Debug.WriteLine($"✅ Task deletada (ID: {id})");
    }

    /// <summary>
    /// Marca uma task como concluída
    /// </summary>
    public async global::System.Threading.Tasks.Task CompleteTask(int id)
    {
        var task = await GetTaskById(id);
        if (task is not null)
        {
            task.Status = TaskStatus.Done;
            task.CompletedAt = DateTime.Now;
            await UpdateTask(task);
            System.Diagnostics.Debug.WriteLine($"✅ Task concluída: {task.Title}");
        }
    }

    /// <summary>
    /// Marca uma task como em progresso
    /// </summary>
    public async global::System.Threading.Tasks.Task StartTask(int id)
    {
        var task = await GetTaskById(id);
        if (task is not null)
        {
            task.Status = TaskStatus.InProgress;
            await UpdateTask(task);
            System.Diagnostics.Debug.WriteLine($"▶️ Task iniciada: {task.Title}");
        }
    }

    /// <summary>
    /// Reseta uma task para "A Fazer"
    /// </summary>
    public async global::System.Threading.Tasks.Task ResetTask(int id)
    {
        var task = await GetTaskById(id);
        if (task is not null)
        {
            task.Status = TaskStatus.Todo;
            task.CompletedAt = null;
            await UpdateTask(task);
            System.Diagnostics.Debug.WriteLine($"🔄 Task resetada: {task.Title}");
        }
    }

    /// <summary>
    /// Adiciona um pomodoro à task
    /// </summary>
    public async global::System.Threading.Tasks.Task AddPomodoro(int taskId)
    {
        var task = await GetTaskById(taskId);
        if (task is not null)
        {
            task.CompletedPomodorCount++;
            await UpdateTask(task);
            System.Diagnostics.Debug.WriteLine($"🍅 Pomodoro adicionado à task: {task.Title} ({task.CompletedPomodorCount}/{task.PomodorCount})");
        }
    }

    /// <summary>
    /// Deleta todas as tasks
    /// </summary>
    public async global::System.Threading.Tasks.Task DeleteAllTasks()
    {
        if (_database is null)
            await InitializeDatabase();

        await _database!.DeleteAllAsync<Task>();
        System.Diagnostics.Debug.WriteLine("🗑️ Todas as tasks foram deletadas");
    }

    /// <summary>
    /// Obtém estatísticas das tasks
    /// </summary>
    public async global::System.Threading.Tasks.Task<(int Total, int Done, int InProgress, int Todo)> GetTaskStatistics()
    {
        if (_database is null)
            await InitializeDatabase();

        var tasks = await _database!.Table<Task>().ToListAsync();
        return (
            Total: tasks.Count,
            Done: tasks.Count(t => t.Status == TaskStatus.Done),
            InProgress: tasks.Count(t => t.Status == TaskStatus.InProgress),
            Todo: tasks.Count(t => t.Status == TaskStatus.Todo)
        );
    }
}

