namespace PomodoroFocus;

/// <summary>
/// Tipos de sons customizáveis
/// </summary>
public enum SoundType
{
    Notification,    // Som de notificação do timer
    Achievement,     // Som de conquista
    Click,          // Som de clique de botão
    Break           // Som de pausa
}

/// <summary>
/// Sons disponíveis para cada tipo
/// </summary>
public enum SoundOption
{
    Default,
    Gentle,
    Electronic,
    Classic,
    None
}

/// <summary>
/// Gerencia customização de sons
/// </summary>
public class SoundCustomizationService
{
    private const string PrefsPrefix = "sound_";
    private readonly SoundService _soundService;

    private Dictionary<SoundType, SoundOption> _soundSettings = new();
    private Dictionary<(SoundType, SoundOption), string> _soundMappings = new();

    public event EventHandler<(SoundType, SoundOption)>? SoundChanged;

    public SoundCustomizationService(SoundService soundService)
    {
        _soundService = soundService;
        InitializeSoundMappings();
        LoadSettings();
    }

    /// <summary>
    /// Mapeia cada tipo de som para seus arquivos
    /// </summary>
    private void InitializeSoundMappings()
    {
        _soundMappings = new Dictionary<(SoundType, SoundOption), string>
        {
            // Notificações
            { (SoundType.Notification, SoundOption.Default), "notif_time.wav" },
            { (SoundType.Notification, SoundOption.Gentle), "notif_time.wav" },
            { (SoundType.Notification, SoundOption.Electronic), "notif_time.wav" },
            { (SoundType.Notification, SoundOption.Classic), "notif_time.wav" },
            
            // Conquistas
            { (SoundType.Achievement, SoundOption.Default), "achievement_notif.wav" },
            { (SoundType.Achievement, SoundOption.Gentle), "achievement_notif.wav" },
            { (SoundType.Achievement, SoundOption.Electronic), "achievement_notif.wav" },
            { (SoundType.Achievement, SoundOption.Classic), "achievement_notif.wav" },
            
            // Cliques
            { (SoundType.Click, SoundOption.Default), "tap_click.wav" },
            { (SoundType.Click, SoundOption.Gentle), "tap_click.wav" },
            { (SoundType.Click, SoundOption.Electronic), "tap_click.wav" },
            { (SoundType.Click, SoundOption.Classic), "tap_click.wav" },
            
            // Pausas
            { (SoundType.Break, SoundOption.Default), "notif_time.wav" },
            { (SoundType.Break, SoundOption.Gentle), "notif_time.wav" },
            { (SoundType.Break, SoundOption.Electronic), "notif_time.wav" },
            { (SoundType.Break, SoundOption.Classic), "notif_time.wav" }
        };
    }

    /// <summary>
    /// Carrega as configurações de som salvas
    /// </summary>
    private void LoadSettings()
    {
        foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
        {
            var key = $"{PrefsPrefix}{soundType}";
            var saved = Preferences.Get(key, SoundOption.Default.ToString());
            
            if (Enum.TryParse<SoundOption>(saved, out var option))
            {
                _soundSettings[soundType] = option;
            }
            else
            {
                _soundSettings[soundType] = SoundOption.Default;
            }
        }
    }

    /// <summary>
    /// Obtém o som configurado para um tipo
    /// </summary>
    public SoundOption GetSoundOption(SoundType soundType)
    {
        if (_soundSettings.TryGetValue(soundType, out var option))
        {
            return option;
        }

        return SoundOption.Default;
    }

    /// <summary>
    /// Define o som para um tipo
    /// </summary>
    public void SetSoundOption(SoundType soundType, SoundOption option)
    {
        _soundSettings[soundType] = option;
        Preferences.Set($"{PrefsPrefix}{soundType}", option.ToString());
        SoundChanged?.Invoke(this, (soundType, option));
        
        System.Diagnostics.Debug.WriteLine($"🔊 Som alterado: {soundType} = {option}");
    }

    /// <summary>
    /// Toca um som do tipo especificado
    /// </summary>
    public void PlaySound(SoundType soundType)
    {
        try
        {
            var option = GetSoundOption(soundType);
            
            if (option == SoundOption.None)
            {
                System.Diagnostics.Debug.WriteLine($"🔇 Som desativado: {soundType}");
                return;
            }

            // Por enquanto, todos os sons usam a mesma opção padrão
            // Você pode expandir isso adicionando mais arquivos de áudio
            switch (soundType)
            {
                case SoundType.Notification:
                    _soundService.PlayNotificationSound();
                    break;
                case SoundType.Achievement:
                    _soundService.PlayAchievementSound();
                    break;
                case SoundType.Click:
                    _soundService.PlayClickSound();
                    break;
                case SoundType.Break:
                    _soundService.PlayNotificationSound();
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao tocar som: {ex}");
        }
    }

    /// <summary>
    /// Obtém todas as opções de som disponíveis
    /// </summary>
    public List<SoundOption> GetAvailableSoundOptions()
    {
        return new List<SoundOption>
        {
            SoundOption.Default,
            SoundOption.Gentle,
            SoundOption.Electronic,
            SoundOption.Classic,
            SoundOption.None
        };
    }

    /// <summary>
    /// Obtém o nome legível de uma opção de som
    /// </summary>
    public string GetSoundOptionName(SoundOption option)
    {
        return option switch
        {
            SoundOption.Default => "Padrão",
            SoundOption.Gentle => "Suave",
            SoundOption.Electronic => "Eletrônico",
            SoundOption.Classic => "Clássico",
            SoundOption.None => "Desativado",
            _ => "Desconhecido"
        };
    }

    /// <summary>
    /// Obtém o nome legível de um tipo de som
    /// </summary>
    public string GetSoundTypeName(SoundType type)
    {
        return type switch
        {
            SoundType.Notification => "Notificação",
            SoundType.Achievement => "Conquista",
            SoundType.Click => "Clique",
            SoundType.Break => "Pausa",
            _ => "Desconhecido"
        };
    }
}
