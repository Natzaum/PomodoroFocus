using SQLite;

namespace PomodoroFocus;

/// <summary>
/// Define o status de uma task
/// </summary>
public enum TaskStatus
{
    Todo,
    InProgress,
    Done
}

/// <summary>
/// Define a prioridade de uma task
/// </summary>
public enum TaskPriority
{
    Low,
    Medium,
    High
}

/// <summary>
/// Modelo de uma task para o sistema de gerenciamento
/// </summary>
public class Task
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [NotNull]
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [NotNull]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [NotNull]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? CompletedAt { get; set; }

    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Número de pomodoros associados a esta task
    /// </summary>
    public int PomodorCount { get; set; } = 0;

    /// <summary>
    /// Número de pomodoros completados para esta task
    /// </summary>
    public int CompletedPomodorCount { get; set; } = 0;

    /// <summary>
    /// Obtém a descrição do status
    /// </summary>
    public string StatusDisplay => Status switch
    {
        TaskStatus.Todo => "A Fazer",
        TaskStatus.InProgress => "Em Progresso",
        TaskStatus.Done => "Concluída",
        _ => "Desconhecido"
    };

    /// <summary>
    /// Obtém a descrição da prioridade
    /// </summary>
    public string PriorityDisplay => Priority switch
    {
        TaskPriority.Low => "Baixa",
        TaskPriority.Medium => "Média",
        TaskPriority.High => "Alta",
        _ => "Desconhecido"
    };

    /// <summary>
    /// Obtém a cor da prioridade
    /// </summary>
    public string PriorityColor => Priority switch
    {
        TaskPriority.Low => "#4CAF50",      // Verde
        TaskPriority.Medium => "#FF9800",   // Laranja
        TaskPriority.High => "#F44336",     // Vermelho
        _ => "#999999"
    };

    /// <summary>
    /// Obtém a cor do status
    /// </summary>
    public string StatusColor => Status switch
    {
        TaskStatus.Todo => "#9E9E9E",       // Cinza
        TaskStatus.InProgress => "#2196F3", // Azul
        TaskStatus.Done => "#4CAF50",       // Verde
        _ => "#999999"
    };
}
