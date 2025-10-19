using System.Windows.Input;

namespace PomodoroFocus;

public class HomeViewModel : BaseViewModel
{
    public string TimerBackgroundColor =>
        CurrentState == "focus" ? "PomodoroRed" : "BreakBlue";

    public string TimerCardColor =>
        CurrentState == "focus" ? "PomodoroRedCard" : "BreakBlueCard";

    public string TimerTextColor => "White";

    // Tab colors for mode selection
    public string PomodoroTabBackground => CurrentState == "focus" ? "PomodoroRedCard" : "Transparent";
    public string PomodoroTabTextColor => CurrentState == "focus" ? "White" : "White";
    
    public string ShortBreakTabBackground => CurrentState == "short_break" ? "BreakBlueCard" : "Transparent";
    public string ShortBreakTabTextColor => CurrentState == "short_break" ? "White" : "White";
    
    public string LongBreakTabBackground => CurrentState == "long_break" ? "BreakBlueCard" : "Transparent";
    public string LongBreakTabTextColor => CurrentState == "long_break" ? "White" : "White";
    
    private double _buttonOpacity = 1.0;
    public double ButtonOpacity
    {
        get => _buttonOpacity;
        set
        {
            if (Math.Abs(_buttonOpacity - value) < 0.01) return;
            _buttonOpacity = value;
            OnPropertyChanged();
        }
    }
    
    private readonly PomodoroService _pomodoroService;
    private readonly SoundService _soundService;
    private int _remainingTime = 25 * 60; // Tempo inicial: 25 minutos (25 * 60 segundos)
    private string _currentState = "focus";
    private bool _isRunning;

    public int RemainingTime
    {
        get => _remainingTime;
        set
        {
            if (_remainingTime == value) return;
            _remainingTime = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(RemainingTimeFormatted));
        }
    }

    public string RemainingTimeFormatted =>
        TimeSpan.FromSeconds(Math.Max(0, _remainingTime)).ToString(@"mm\:ss");

    public string CurrentState
    {
        get => _currentState;
        set {
            if (_currentState == value) return;
            _currentState = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TimerBackgroundColor));
            OnPropertyChanged(nameof(TimerCardColor));
            OnPropertyChanged(nameof(TimerTextColor));
            OnPropertyChanged(nameof(PomodoroTabBackground));
            OnPropertyChanged(nameof(PomodoroTabTextColor));
            OnPropertyChanged(nameof(ShortBreakTabBackground));
            OnPropertyChanged(nameof(ShortBreakTabTextColor));
            OnPropertyChanged(nameof(LongBreakTabBackground));
            OnPropertyChanged(nameof(LongBreakTabTextColor));
        }
    }

    public bool IsRunning
    {
        get => _isRunning;
        set {
            if (_isRunning == value) return;
            _isRunning = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanPause));
        }
    }

    public bool CanPause => IsRunning;
    
    public string PomodoroProgress => $"{_pomodoroService.CompletedPomodoros}/{_pomodoroService.PomodorosUntilLongBreak}";

    public ICommand StartCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SkipCommand { get; }
    public ICommand SelectPomodoroCommand { get; }
    public ICommand SelectShortBreakCommand { get; }
    public ICommand SelectLongBreakCommand { get; }

    public HomeViewModel(PomodoroService pomodoroService, SoundService soundService)
    {
        _pomodoroService = pomodoroService;
        _soundService = soundService;
        StartCommand = new Command(StartTimer);
        PauseCommand = new Command(PauseTimer);
        ResetCommand = new Command(ResetTimer);
        SkipCommand = new Command(Skip);
        SelectPomodoroCommand = new Command(SelectPomodoro);
        SelectShortBreakCommand = new Command(SelectShortBreak);
        SelectLongBreakCommand = new Command(SelectLongBreak);
    }

    private void StartTimer()
    {
        System.Diagnostics.Debug.WriteLine("StartTimer chamado!");
        AnimateButtonPress();
        _soundService.PlayClickSound();
        _pomodoroService.Start(this);
    }

    private void PauseTimer()
    {
        System.Diagnostics.Debug.WriteLine("PauseTimer chamado!");
        AnimateButtonPress();
        _soundService.PlayClickSound();
        _pomodoroService.Pause(this);
    }

    private void ResetTimer()
    {
        AnimateButtonPress();
        _soundService.PlayClickSound();
        _pomodoroService.Reset(this);
    }
    
    private void Skip()
    {
        AnimateButtonPress();
        _soundService.PlayClickSound();
        _pomodoroService.SkipToNext(this);
    }

    private void SelectPomodoro()
    {
        if (IsRunning) return; // Não permite trocar enquanto está rodando
        AnimateButtonPress();
        _soundService.PlayClickSound();
        CurrentState = "focus";
        RemainingTime = 25 * 60; // Tempo de teste: 5 segundos (normal: 25 * 60)
    }

    private void SelectShortBreak()
    {
        if (IsRunning) return;
        AnimateButtonPress();
        _soundService.PlayClickSound();
        CurrentState = "short_break";
        RemainingTime = 5 * 60; // Tempo de teste: 5 segundos (normal: 5 * 60)
    }

    private void SelectLongBreak()
    {
        if (IsRunning) return;
        AnimateButtonPress();
        _soundService.PlayClickSound();
        CurrentState = "long_break";
        RemainingTime = 15 * 60; // Tempo de teste: 15 segundos (normal: 15 * 60)
    }
    
    public void UpdatePomodoroProgress()
    {
        OnPropertyChanged(nameof(PomodoroProgress));
    }
    
    private async void AnimateButtonPress()
    {
        // Escurece o botão
        ButtonOpacity = 0.5;
        await Task.Delay(100);
        // Volta ao normal
        ButtonOpacity = 1.0;
    }
}
