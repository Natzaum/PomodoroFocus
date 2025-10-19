using System.Windows.Input;

public class HomeViewModel : BaseViewModel
{
    private readonly PomodoroService _pomodoroService;
    private int _remainingTime = 25 * 60;
    private string _currentState = "focus";

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
        set { _currentState = value; OnPropertyChanged(); }
    }

    public ICommand StartCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand ResetCommand { get; }

    public HomeViewModel(PomodoroService pomodoroService)
    {
        _pomodoroService = pomodoroService;
    StartCommand = new Command(StartTimer);
    PauseCommand = new Command(PauseTimer);
    ResetCommand = new Command(ResetTimer);
    }

    private void StartTimer() => _pomodoroService.Start(this);
    private void PauseTimer() => _pomodoroService.Pause();
    private void ResetTimer() => _pomodoroService.Reset(this);
}
