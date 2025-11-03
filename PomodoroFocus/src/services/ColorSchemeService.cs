namespace PomodoroFocus;

/// <summary>
/// Define os esquemas de cores disponíveis
/// </summary>
public enum ColorScheme
{
    Red,      // Vermelho e Azul (Padrão)
    Blue,     // Azuis claros
    Green,    // Verde e Laranja
    Purple    // Roxo e Rosa
}

/// <summary>
/// Modelo para definir cores de um esquema
/// </summary>
public class ColorSchemeDefinition
{
    public ColorScheme Scheme { get; set; }
    public string Name { get; set; } = "";
    public Color PomodoroColor { get; set; }
    public Color PomodoroCardColor { get; set; }
    public Color BreakColor { get; set; }
    public Color BreakCardColor { get; set; }
    public Color PrimaryColor { get; set; }
    public Color SecondaryColor { get; set; }
    public Color AccentColor { get; set; }
}

/// <summary>
/// Gerencia esquemas de cores da aplicação
/// </summary>
public class ColorSchemeService
{
    private const string ColorSchemeKey = "color_scheme";
    
    private ColorScheme _currentScheme = ColorScheme.Red;
    public ColorScheme CurrentScheme 
    { 
        get => _currentScheme;
        set
        {
            if (_currentScheme != value)
            {
                _currentScheme = value;
                SaveScheme(value);
                ApplyColorScheme();
                ColorSchemeChanged?.Invoke(this, value);
            }
        }
    }

    public event EventHandler<ColorScheme>? ColorSchemeChanged;

    private Dictionary<ColorScheme, ColorSchemeDefinition> _schemeDefinitions = new();

    public ColorSchemeService()
    {
        InitializeSchemes();
        LoadScheme();
    }

    /// <summary>
    /// Inicializa os 4 esquemas de cores disponíveis
    /// </summary>
    private void InitializeSchemes()
    {
        _schemeDefinitions = new Dictionary<ColorScheme, ColorSchemeDefinition>
        {
            // Esquema Red (Padrão)
            {
                ColorScheme.Red,
                new ColorSchemeDefinition
                {
                    Scheme = ColorScheme.Red,
                    Name = "Energético",
                    PomodoroColor = Color.FromArgb("#EF4444"),
                    PomodoroCardColor = Color.FromArgb("#991B1B"),
                    BreakColor = Color.FromArgb("#3B82F6"),
                    BreakCardColor = Color.FromArgb("#1E3A8A"),
                    PrimaryColor = Color.FromArgb("#EF4444"),
                    SecondaryColor = Color.FromArgb("#3B82F6"),
                    AccentColor = Color.FromArgb("#F59E0B")
                }
            },
            
            // Esquema Blue (Calmo)
            {
                ColorScheme.Blue,
                new ColorSchemeDefinition
                {
                    Scheme = ColorScheme.Blue,
                    Name = "Calmo",
                    PomodoroColor = Color.FromArgb("#0EA5E9"),
                    PomodoroCardColor = Color.FromArgb("#0C4A6E"),
                    BreakColor = Color.FromArgb("#06B6D4"),
                    BreakCardColor = Color.FromArgb("#082F49"),
                    PrimaryColor = Color.FromArgb("#0EA5E9"),
                    SecondaryColor = Color.FromArgb("#06B6D4"),
                    AccentColor = Color.FromArgb("#14B8A6")
                }
            },
            
            // Esquema Green (Natural)
            {
                ColorScheme.Green,
                new ColorSchemeDefinition
                {
                    Scheme = ColorScheme.Green,
                    Name = "Natural",
                    PomodoroColor = Color.FromArgb("#10B981"),
                    PomodoroCardColor = Color.FromArgb("#064E3B"),
                    BreakColor = Color.FromArgb("#F97316"),
                    BreakCardColor = Color.FromArgb("#7C2D12"),
                    PrimaryColor = Color.FromArgb("#10B981"),
                    SecondaryColor = Color.FromArgb("#F97316"),
                    AccentColor = Color.FromArgb("#8B5CF6")
                }
            },
            
            // Esquema Purple (Neon)
            {
                ColorScheme.Purple,
                new ColorSchemeDefinition
                {
                    Scheme = ColorScheme.Purple,
                    Name = "Neon",
                    PomodoroColor = Color.FromArgb("#A855F7"),
                    PomodoroCardColor = Color.FromArgb("#4C0519"),
                    BreakColor = Color.FromArgb("#EC4899"),
                    BreakCardColor = Color.FromArgb("#500724"),
                    PrimaryColor = Color.FromArgb("#A855F7"),
                    SecondaryColor = Color.FromArgb("#EC4899"),
                    AccentColor = Color.FromArgb("#06B6D4")
                }
            }
        };
    }

    /// <summary>
    /// Carrega o esquema salvo
    /// </summary>
    private void LoadScheme()
    {
        var saved = Preferences.Get(ColorSchemeKey, ColorScheme.Red.ToString());
        if (Enum.TryParse<ColorScheme>(saved, out var scheme))
        {
            _currentScheme = scheme;
        }
        else
        {
            _currentScheme = ColorScheme.Red;
        }

        ApplyColorScheme();
    }

    /// <summary>
    /// Salva o esquema de cores nas preferências
    /// </summary>
    private void SaveScheme(ColorScheme scheme)
    {
        Preferences.Set(ColorSchemeKey, scheme.ToString());
    }

    /// <summary>
    /// Obtém o esquema de cores ativo
    /// </summary>
    public ColorSchemeDefinition GetCurrentScheme()
    {
        if (_schemeDefinitions.TryGetValue(_currentScheme, out var scheme))
        {
            return scheme;
        }

        return _schemeDefinitions[ColorScheme.Red];
    }

    /// <summary>
    /// Obtém todos os esquemas disponíveis
    /// </summary>
    public List<ColorSchemeDefinition> GetAllSchemes()
    {
        return _schemeDefinitions.Values.ToList();
    }

    /// <summary>
    /// Aplica o esquema de cores na aplicação
    /// </summary>
    private void ApplyColorScheme()
    {
        var scheme = GetCurrentScheme();

        if (Application.Current?.Resources is not null)
        {
            // Atualiza dicionário de recursos dinamicamente
            Application.Current.Resources["PomodoroRed"] = scheme.PomodoroColor;
            Application.Current.Resources["PomodoroRedCard"] = scheme.PomodoroCardColor;
            Application.Current.Resources["BreakBlue"] = scheme.BreakColor;
            Application.Current.Resources["BreakBlueCard"] = scheme.BreakCardColor;
            Application.Current.Resources["Primary"] = scheme.PrimaryColor;
            Application.Current.Resources["Secondary"] = scheme.SecondaryColor;
            Application.Current.Resources["Accent"] = scheme.AccentColor;
        }

        System.Diagnostics.Debug.WriteLine($"🎨 Esquema de cores aplicado: {scheme.Name}");
    }
}
