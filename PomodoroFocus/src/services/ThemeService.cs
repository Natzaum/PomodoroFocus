namespace PomodoroFocus;

/// <summary>
/// Define os temas disponíveis
/// </summary>
public enum AppThemeMode
{
    Light,
    Dark,
    Auto
}

/// <summary>
/// Gerencia temas (claro/escuro) da aplicação
/// </summary>
public class ThemeService
{
    private const string ThemeKey = "app_theme";
    private const string AllowAutoThemeKey = "allow_auto_theme";
    
    private AppThemeMode _currentTheme = AppThemeMode.Auto;
    public AppThemeMode CurrentTheme 
    { 
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                SaveTheme(value);
                ApplyTheme();
                ThemeChanged?.Invoke(this, value);
            }
        }
    }

    public event EventHandler<AppThemeMode>? ThemeChanged;

    public ThemeService()
    {
        LoadTheme();
    }

    /// <summary>
    /// Carrega o tema salvo ou usa Auto por padrão
    /// </summary>
    private void LoadTheme()
    {
        var saved = Preferences.Get(ThemeKey, AppThemeMode.Auto.ToString());
        if (Enum.TryParse<AppThemeMode>(saved, out var theme))
        {
            _currentTheme = theme;
        }
        else
        {
            _currentTheme = AppThemeMode.Auto;
        }

        ApplyTheme();
    }

    /// <summary>
    /// Salva o tema nas preferências
    /// </summary>
    private void SaveTheme(AppThemeMode theme)
    {
        Preferences.Set(ThemeKey, theme.ToString());
    }

    /// <summary>
    /// Aplica o tema na aplicação
    /// </summary>
    private void ApplyTheme()
    {
        if (Application.Current is null)
            return;

        AppThemeMode themeToApply = _currentTheme;

        // Se Auto, detecta o tema do sistema
        if (_currentTheme == AppThemeMode.Auto)
        {
            themeToApply = Application.Current.RequestedTheme == AppTheme.Dark 
                ? AppThemeMode.Dark 
                : AppThemeMode.Light;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Application.Current!.UserAppTheme = themeToApply switch
            {
                AppThemeMode.Dark => AppTheme.Dark,
                AppThemeMode.Light => AppTheme.Light,
                _ => AppTheme.Dark
            };

            System.Diagnostics.Debug.WriteLine($"✨ Tema aplicado: {themeToApply}");
        });
    }

    /// <summary>
    /// Obtém o tema ativo no momento (considerando Auto)
    /// </summary>
    public AppThemeMode GetActiveTheme()
    {
        if (_currentTheme != AppThemeMode.Auto)
            return _currentTheme;

        return Application.Current?.RequestedTheme == AppTheme.Dark 
            ? AppThemeMode.Dark 
            : AppThemeMode.Light;
    }

    /// <summary>
    /// Verifica se está usando tema escuro
    /// </summary>
    public bool IsDarkTheme()
    {
        return GetActiveTheme() == AppThemeMode.Dark;
    }
}
