namespace PomodoroFocus;

/// <summary>
/// Serviço para notificações de conquistas
/// </summary>
public class AchievementNotificationService
{
    private readonly SoundService _soundService;
    
    // Evento que dispara quando uma conquista é desbloqueada
    public event EventHandler<AchievementUnlockedEventArgs>? AchievementUnlocked;

    public AchievementNotificationService(SoundService soundService)
    {
        _soundService = soundService;
    }

    /// <summary>
    /// Notifica que uma conquista foi desbloqueada
    /// </summary>
    public void NotifyAchievementUnlocked(Achievements achievement)
    {
        try
        {
            // Toca o som de conquista
            _soundService.PlayAchievementSound();

            // Dispara o evento
            AchievementUnlocked?.Invoke(this, new AchievementUnlockedEventArgs(achievement));

            System.Diagnostics.Debug.WriteLine($"Notificação de conquista: {achievement.Title}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao notificar conquista: {ex}");
        }
    }
}

/// <summary>
/// Argumentos do evento de conquista desbloqueada
/// </summary>
public class AchievementUnlockedEventArgs : EventArgs
{
    public Achievements Achievement { get; }

    public AchievementUnlockedEventArgs(Achievements achievement)
    {
        Achievement = achievement;
    }
}
