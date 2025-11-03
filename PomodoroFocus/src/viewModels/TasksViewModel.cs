using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PomodoroFocus;

/// <summary>
/// ViewModel para gerenciar tasks
/// </summary>
public class TasksViewModel : BaseViewModel
{
    private readonly TaskService _taskService;
    private ObservableCollection<Task> _tasks = new();
    private Task? _selectedTask;
    private string _newTaskTitle = string.Empty;
    private TaskPriority _newTaskPriority = TaskPriority.Medium;
    private int _totalTasks;
    private int _completedTasks;
    private int _inProgressTasks;
    private int _todoTasks;

    public ObservableCollection<Task> Tasks
    {
        get => _tasks;
        set
        {
            if (_tasks != value)
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }
    }

    public Task? SelectedTask
    {
        get => _selectedTask;
        set
        {
            if (_selectedTask != value)
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }
    }

    public string NewTaskTitle
    {
        get => _newTaskTitle;
        set
        {
            if (_newTaskTitle != value)
            {
                _newTaskTitle = value;
                OnPropertyChanged();
            }
        }
    }

    public TaskPriority NewTaskPriority
    {
        get => _newTaskPriority;
        set
        {
            if (_newTaskPriority != value)
            {
                _newTaskPriority = value;
                OnPropertyChanged();
            }
        }
    }

    public int TotalTasks
    {
        get => _totalTasks;
        set
        {
            if (_totalTasks != value)
            {
                _totalTasks = value;
                OnPropertyChanged();
            }
        }
    }

    public int CompletedTasks
    {
        get => _completedTasks;
        set
        {
            if (_completedTasks != value)
            {
                _completedTasks = value;
                OnPropertyChanged();
            }
        }
    }

    public int InProgressTasks
    {
        get => _inProgressTasks;
        set
        {
            if (_inProgressTasks != value)
            {
                _inProgressTasks = value;
                OnPropertyChanged();
            }
        }
    }

    public int TodoTasks
    {
        get => _todoTasks;
        set
        {
            if (_todoTasks != value)
            {
                _todoTasks = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand AddTaskCommand { get; }
    public ICommand CompleteTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand StartTaskCommand { get; }
    public ICommand ResetTaskCommand { get; }

    public TasksViewModel()
    {
        _taskService = ServiceHelper.GetService<TaskService>()!;
        
        AddTaskCommand = new Command(AddTask);
        CompleteTaskCommand = new Command<Task>(CompleteTask);
        DeleteTaskCommand = new Command<Task>(DeleteTask);
        StartTaskCommand = new Command<Task>(StartTask);
        ResetTaskCommand = new Command<Task>(ResetTask);
    }

    /// <summary>
    /// Carrega todas as tasks
    /// </summary>
    public async global::System.Threading.Tasks.Task LoadTasks()
    {
        try
        {
            IsLoading = true;
            var tasks = await _taskService.GetAllTasks();
            Tasks = new ObservableCollection<Task>(tasks);
            
            var stats = await _taskService.GetTaskStatistics();
            TotalTasks = stats.Total;
            CompletedTasks = stats.Done;
            InProgressTasks = stats.InProgress;
            TodoTasks = stats.Todo;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar tasks: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Adiciona uma nova task
    /// </summary>
    private async void AddTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle))
            return;

        try
        {
            var task = new Task
            {
                Title = NewTaskTitle,
                Priority = NewTaskPriority,
                Status = TaskStatus.Todo
            };

            await _taskService.AddTask(task);
            NewTaskTitle = string.Empty;
            NewTaskPriority = TaskPriority.Medium;
            
            await LoadTasks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao adicionar task: {ex}");
        }
    }

    /// <summary>
    /// Marca uma task como concluída
    /// </summary>
    private async void CompleteTask(Task task)
    {
        if (task is null) return;

        try
        {
            await _taskService.CompleteTask(task.Id);
            await LoadTasks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao completar task: {ex}");
        }
    }

    /// <summary>
    /// Deleta uma task
    /// </summary>
    private async void DeleteTask(Task task)
    {
        if (task is null) return;

        try
        {
            await _taskService.DeleteTask(task.Id);
            await LoadTasks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao deletar task: {ex}");
        }
    }

    /// <summary>
    /// Inicia uma task (muda status para In Progress)
    /// </summary>
    private async void StartTask(Task task)
    {
        if (task is null) return;

        try
        {
            await _taskService.StartTask(task.Id);
            await LoadTasks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao iniciar task: {ex}");
        }
    }

    /// <summary>
    /// Reseta uma task para "A Fazer"
    /// </summary>
    private async void ResetTask(Task task)
    {
        if (task is null) return;

        try
        {
            await _taskService.ResetTask(task.Id);
            await LoadTasks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao resetar task: {ex}");
        }
    }
}
