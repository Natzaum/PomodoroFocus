using System.Timers;

public class PomodoroService
{
    private System.Timers.Timer? _timer;
    private int _secondsRemaining;
    private DateTime _sessionStart;
    private string _sessionType = "focus";
    private readonly DatabaseService _db;
    private readonly AchievementService _achievements;

    public PomodoroService(DatabaseService db, AchievementService achievements)
    {
        _db = db;
        _achievements = achievements;
    }

    public void Start(HomeViewModel vm)
    {
        // Garantir um único timer em execução
        _timer?.Stop();
        _timer?.Dispose();

        _secondsRemaining = vm.RemainingTime > 0 ? vm.RemainingTime : 25 * 60;
        _sessionStart = DateTime.Now;
        _sessionType = vm.CurrentState;
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
            // Salvar sessão concluída e checar conquistas
            var start = _sessionStart;
            var end = DateTime.Now;
            var type = _sessionType;
            Task.Run(async () =>
            {
                try
                {
                    await _db.SaveSessionAsync(new PomoSession
                    {
                        StartTime = start,
                        EndTime = end,
                        Type = type
                    });
                    await _achievements.CheckAchievementsAsync();
                }
                catch { /* logging opcional */ }
            });
            MainThread.BeginInvokeOnMainThread(() => vm.IsRunning = false);
        }
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
        int defaultSeconds = 25 * 60;
        if (string.Equals(vm.CurrentState, "short_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = 5 * 60;
        else if (string.Equals(vm.CurrentState, "long_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = 15 * 60;
        _secondsRemaining = defaultSeconds;
        vm.RemainingTime = defaultSeconds;
        vm.IsRunning = false;
    }

    public void SkipToNext(HomeViewModel vm)
    {
        // Alterna entre foco (25min) e pausa curta (5min)
        var nextIsBreak = string.Equals(vm.CurrentState, "focus", StringComparison.OrdinalIgnoreCase);
        var nextSeconds = nextIsBreak ? 5 * 60 : 25 * 60;
        vm.CurrentState = nextIsBreak ? "short_break" : "focus";

        // Parar qualquer timer atual e deixar aguardando novo Start
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        _secondsRemaining = nextSeconds;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            vm.RemainingTime = nextSeconds;
            vm.IsRunning = false; // Só volta a rodar quando clicar em Iniciar
        });
    }
}
