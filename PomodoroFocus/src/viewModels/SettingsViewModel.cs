using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PomodoroFocus;

public class SettingsViewModel : BaseViewModel
{
    private readonly SettingsService _settingsService;
    private readonly AchievementService _achievementService;
    private readonly ThemeService _themeService;
    private readonly ColorSchemeService _colorSchemeService;
    private readonly SoundCustomizationService _soundCustomizationService;
    
    private int _focusMinutes;
    private int _shortBreakMinutes;
    private int _longBreakMinutes;
    private AppThemeMode _selectedTheme;
    private ColorScheme _selectedColorScheme;
    private ObservableCollection<ColorSchemeDefinition> _availableSchemes;
    private ObservableCollection<string> _themeOptions;

    public int FocusMinutes
    {
        get => _focusMinutes;
        set
        {
            if (_focusMinutes == value) return;
            _focusMinutes = Math.Clamp(value, 1, 60);
            OnPropertyChanged();
        }
    }

    public int ShortBreakMinutes
    {
        get => _shortBreakMinutes;
        set
        {
            if (_shortBreakMinutes == value) return;
            _shortBreakMinutes = Math.Clamp(value, 1, 60);
            OnPropertyChanged();
        }
    }

    public int LongBreakMinutes
    {
        get => _longBreakMinutes;
        set
        {
            if (_longBreakMinutes == value) return;
            _longBreakMinutes = Math.Clamp(value, 1, 60);
            OnPropertyChanged();
        }
    }

    public AppThemeMode SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_selectedTheme != value)
            {
                _selectedTheme = value;
                _themeService.CurrentTheme = value;
                OnPropertyChanged();
            }
        }
    }

    public ColorScheme SelectedColorScheme
    {
        get => _selectedColorScheme;
        set
        {
            if (_selectedColorScheme != value)
            {
                _selectedColorScheme = value;
                _colorSchemeService.CurrentScheme = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<ColorSchemeDefinition> AvailableSchemes
    {
        get => _availableSchemes;
        set
        {
            if (_availableSchemes != value)
            {
                _availableSchemes = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<string> ThemeOptions
    {
        get => _themeOptions;
        set
        {
            if (_themeOptions != value)
            {
                _themeOptions = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand IncrementFocusCommand { get; }
    public ICommand DecrementFocusCommand { get; }
    public ICommand IncrementShortBreakCommand { get; }
    public ICommand DecrementShortBreakCommand { get; }
    public ICommand IncrementLongBreakCommand { get; }
    public ICommand DecrementLongBreakCommand { get; }
    public ICommand ResetAchievementsCommand { get; }
    public ICommand SelectColorSchemeCommand { get; }

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        _achievementService = ServiceHelper.GetService<AchievementService>()!;
        _themeService = ServiceHelper.GetService<ThemeService>()!;
        _colorSchemeService = ServiceHelper.GetService<ColorSchemeService>()!;
        _soundCustomizationService = ServiceHelper.GetService<SoundCustomizationService>()!;
        
        var settings = _settingsService.GetSettings();
        _focusMinutes = settings.FocusMinutes;
        _shortBreakMinutes = settings.ShortBreakMinutes;
        _longBreakMinutes = settings.LongBreakMinutes;
        
        _selectedTheme = _themeService.CurrentTheme;
        _selectedColorScheme = _colorSchemeService.CurrentScheme;
        
        _availableSchemes = new ObservableCollection<ColorSchemeDefinition>(_colorSchemeService.GetAllSchemes());
        _themeOptions = new ObservableCollection<string> { "Claro", "Escuro", "Auto" };
        
        SaveCommand = new Command(SaveSettings);
        IncrementFocusCommand = new Command(() => FocusMinutes++);
        DecrementFocusCommand = new Command(() => FocusMinutes--);
        IncrementShortBreakCommand = new Command(() => ShortBreakMinutes++);
        DecrementShortBreakCommand = new Command(() => ShortBreakMinutes--);
        IncrementLongBreakCommand = new Command(() => LongBreakMinutes++);
        DecrementLongBreakCommand = new Command(() => LongBreakMinutes--);
        ResetAchievementsCommand = new Command(ResetAchievements);
        SelectColorSchemeCommand = new Command<ColorSchemeDefinition>(SelectColorScheme);
    }

    private async void SaveSettings()
    {
        _settingsService.SaveSettings(new Settings
        {
            FocusMinutes = FocusMinutes,
            ShortBreakMinutes = ShortBreakMinutes,
            LongBreakMinutes = LongBreakMinutes
        });

        if (Application.Current?.Windows.Count > 0)
        {
            await Application.Current.Windows[0].Page!.DisplayAlert("Sucesso", "Configurações salvas!", "OK");
        }
    }

    private async void ResetAchievements()
    {
        if (Application.Current?.Windows.Count > 0)
        {
            var result = await Application.Current.Windows[0].Page!.DisplayAlert(
                "Resetar Conquistas",
                "Tem certeza que deseja resetar todas as conquistas desbloqueadas?",
                "Sim",
                "Não");

            if (result)
            {
                try

                {
                    await _achievementService.ResetAllAchievements();
                    await Application.Current.Windows[0].Page!.DisplayAlert(
                        "Sucesso",
                        "Todas as conquistas foram resetadas!",
                        "OK");
                }
                catch (Exception ex)
                {
                    await Application.Current.Windows[0].Page!.DisplayAlert(
                        "Erro",
                        $"Erro ao resetar conquistas: {ex.Message}",
                        "OK");
                }
            }
        }
    }


    private void SelectColorScheme(ColorSchemeDefinition scheme)
    {
        if (scheme is not null)
        {
            SelectedColorScheme = scheme.Scheme;
        }
    }
}
