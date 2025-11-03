using System.Timers;

namespace PomodoroFocus;

public class PomodoroService
{
    private System.Timers.Timer? _timer;
    private int _secondsRemaining;
    private readonly AchievementService _achievements;
    private readonly SettingsService _settingsService;
    private readonly SoundService _soundService;
    private int _completedPomodoros = 0;
    private const int POMODOROS_UNTIL_LONG_BREAK = 4;

    public int CompletedPomodoros => _completedPomodoros;
    public int PomodorosUntilLongBreak => POMODOROS_UNTIL_LONG_BREAK;

    public PomodoroService(AchievementService achievements, SettingsService settingsService, SoundService soundService)
    {
        _achievements = achievements;
        _settingsService = settingsService;
        _soundService = soundService;
    }

    private int GetFocusTime() => _settingsService.GetFocusSeconds();
    private int GetShortBreakTime() => _settingsService.GetShortBreakSeconds();
    private int GetLongBreakTime() => _settingsService.GetLongBreakSeconds();

    public void Start(HomeViewModel vm)
    {
        _timer?.Stop();
        _timer?.Dispose();

        _secondsRemaining = vm.RemainingTime > 0 ? vm.RemainingTime : GetFocusTime();
        _timer = new System.Timers.Timer(1000) { AutoReset = true };
        _timer.Elapsed += (s, e) => Tick(vm);
        _timer.Start();
        MainThread.BeginInvokeOnMainThread(() => vm.IsRunning = true);
    }

    private void Tick(HomeViewModel vm)
    {
        _secondsRemaining--;
        MainThread.BeginInvokeOnMainThread(() => vm.RemainingTime = _secondsRemaining);
        if (_secondsRemaining <= 0)
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _soundService.PlayNotificationSound();
                PrepareNextState(vm, incrementCounter: true);
                vm.IsRunning = false;
            });
        }
    }

    private void PrepareNextState(HomeViewModel vm, bool incrementCounter)
    {
        string nextState;
        int nextSeconds;

        if (vm.CurrentState == "focus")
        {
            if (incrementCounter)
            {
                _completedPomodoros++;
                MainThread.BeginInvokeOnMainThread(async () => await _achievements.IncrementPomodoro());
                System.Diagnostics.Debug.WriteLine($"Pomodoro completado! Total: {_completedPomodoros}");
            }

            if (_completedPomodoros >= POMODOROS_UNTIL_LONG_BREAK)
            {
                nextState = "long_break";
                nextSeconds = GetLongBreakTime();
                _completedPomodoros = 0;
                System.Diagnostics.Debug.WriteLine($"Preparando pausa longa ({nextSeconds} seg) - aguardando START do usuário");
            }
            else
            {
                nextState = "short_break";
                nextSeconds = GetShortBreakTime();
                System.Diagnostics.Debug.WriteLine($"Preparando pausa curta ({nextSeconds} seg). Faltam {POMODOROS_UNTIL_LONG_BREAK - _completedPomodoros} pomodoros para pausa longa - aguardando START do usuário");
            }
        }
        else
        {
            nextState = "focus";
            nextSeconds = GetFocusTime();
            System.Diagnostics.Debug.WriteLine($"Preparando sessão de foco ({nextSeconds} seg) - aguardando START do usuário");
        }

        vm.CurrentState = nextState;
        vm.RemainingTime = nextSeconds;
        vm.UpdatePomodoroProgress();
    }

    public void Pause(HomeViewModel vm)
    {
        _timer?.Stop();
        vm.IsRunning = false;
    }
    public void Reset(HomeViewModel vm)
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        int defaultSeconds = GetFocusTime();
        if (string.Equals(vm.CurrentState, "short_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = GetShortBreakTime();
        else if (string.Equals(vm.CurrentState, "long_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = GetLongBreakTime();
        _secondsRemaining = defaultSeconds;
        vm.RemainingTime = defaultSeconds;
        vm.IsRunning = false;
    }

    public void SkipToNext(HomeViewModel vm)
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        if (string.Equals(vm.CurrentState, "focus", StringComparison.OrdinalIgnoreCase))
        {
            _completedPomodoros++;
            System.Diagnostics.Debug.WriteLine($"Pomodoro pulado! Total: {_completedPomodoros}");
        }

        PrepareNextState(vm, incrementCounter: false);
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            vm.IsRunning = false;
        });
    }
}
