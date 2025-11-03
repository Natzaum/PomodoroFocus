using System.Threading.Tasks;

namespace PomodoroFocus;

/// <summary>
/// Helper para debug e testes - permite resetar conquistas
/// </summary>
public static class AchievementDebugHelper
{
    /// <summary>
    /// Reseta todas as conquistas (marca como não desbloqueadas)
    /// </summary>
    public static async global::System.Threading.Tasks.Task ResetAllAchievements()
    {
        var achievementService = ServiceHelper.GetService<AchievementService>();
        if (achievementService is not null)
        {
            await achievementService.ResetAllAchievements();
            System.Diagnostics.Debug.WriteLine("Conquistas resetadas com sucesso!");
        }
    }

    /// <summary>
    /// Deleta completamente o banco de dados de conquistas
    /// </summary>
    public static async global::System.Threading.Tasks.Task DeleteAchievementDatabase()
    {
        var achievementService = ServiceHelper.GetService<AchievementService>();
        if (achievementService is not null)
        {
            await achievementService.DeleteDatabase();
            System.Diagnostics.Debug.WriteLine("Banco de dados deletado com sucesso!");
        }
    }

    /// <summary>
    /// Reseta o banco de dados completamente e reinicializa com dados padrão
    /// </summary>
    public static async global::System.Threading.Tasks.Task ResetAchievementDatabase()
    {
        var achievementService = ServiceHelper.GetService<AchievementService>();
        if (achievementService is not null)
        {
            await achievementService.ResetDatabase();
            System.Diagnostics.Debug.WriteLine("Banco de dados resetado com sucesso!");
        }
    }

    /// <summary>
    /// Usa no Debug Console do VS: Debug.WriteLine("cmd: ResetAchievements");
    /// </summary>
    public static void ProcessCommand(string command)
    {
        System.Diagnostics.Debug.WriteLine($"Executando comando: {command}");
        
        var task = command.ToLower() switch
        {
            "resetachievements" => ResetAllAchievements(),
            "deleteachievementdb" => DeleteAchievementDatabase(),
            "resetachievementdb" => ResetAchievementDatabase(),
            _ => global::System.Threading.Tasks.Task.CompletedTask
        };

        task.Wait();
    }
}
