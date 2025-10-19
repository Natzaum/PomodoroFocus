using System.Timers;

public class PomodoroService
{
    private System.Timers.Timer _timer;
    private int _secondsRemaining;

    public void Start(HomeViewModel vm)
    {
        _secondsRemaining = vm.RemainingTime > 0 ? vm.RemainingTime : 25 * 60;
    _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => Tick(vm);
        _timer.Start();
    }

    private void Tick(HomeViewModel vm)
    {
        _secondsRemaining--;
        MainThread.BeginInvokeOnMainThread(() => vm.RemainingTime = _secondsRemaining);
        if (_secondsRemaining <= 0)
        {
            _timer.Stop();
            _timer.Dispose();
            // TODO: Notificação local e salvar sessão
        }
    }

    public void Pause() => _timer?.Stop();
    public void Reset(HomeViewModel vm)
    {
        _timer?.Stop();
        _timer?.Dispose();
        vm.RemainingTime = 25 * 60;
    }
}
