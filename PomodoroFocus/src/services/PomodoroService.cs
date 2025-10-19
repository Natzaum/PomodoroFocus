using System.Timers;

namespace PomodoroFocus;

public class PomodoroService
{
    // ========== CONFIGURAÇÃO DE TEMPOS (em segundos) ==========
    // Para testar: use valores pequenos como 5, 3, 7
    // Para produção: use 25*60, 5*60, 15*60
    private const int FOCUS_TIME = 25 * 60;        // 5 segundos para testar (normal: 25 * 60)
    private const int SHORT_BREAK_TIME = 5 * 60;  // 3 segundos para testar (normal: 5 * 60)
    private const int LONG_BREAK_TIME = 15 * 60;   // 7 segundos para testar (normal: 15 * 60)
    // ===========================================================
    
    private System.Timers.Timer? _timer;
    private int _secondsRemaining;
    private DateTime _sessionStart;
    private string _sessionType = "focus";
    private readonly DatabaseService _db;
    private readonly AchievementService _achievements;
    private int _completedPomodoros = 0; // Contador de pomodoros completados
    private const int POMODOROS_UNTIL_LONG_BREAK = 4; // 4 pomodoros antes da pausa longa

    // Propriedade pública para ler o contador
    public int CompletedPomodoros => _completedPomodoros;
    public int PomodorosUntilLongBreak => POMODOROS_UNTIL_LONG_BREAK;

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

        _secondsRemaining = vm.RemainingTime > 0 ? vm.RemainingTime : FOCUS_TIME;
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
            
            // Prepara o próximo estado, mas NÃO inicia automaticamente
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PrepareNextState(vm, incrementCounter: true); // Incrementa porque terminou naturalmente
                vm.IsRunning = false; // Para o timer, usuário precisa clicar START
            });
        }
    }

    private void PrepareNextState(HomeViewModel vm, bool incrementCounter)
    {
        string nextState;
        int nextSeconds;

        if (vm.CurrentState == "focus")
        {
            // Acabou um pomodoro de foco
            if (incrementCounter)
            {
                _completedPomodoros++;
                System.Diagnostics.Debug.WriteLine($"Pomodoro completado! Total: {_completedPomodoros}");
            }

            if (_completedPomodoros >= POMODOROS_UNTIL_LONG_BREAK)
            {
                // Após 4 pomodoros, pausa longa
                nextState = "long_break";
                nextSeconds = LONG_BREAK_TIME;
                _completedPomodoros = 0; // Reseta o contador
                System.Diagnostics.Debug.WriteLine($"Preparando pausa longa ({LONG_BREAK_TIME} seg) - aguardando START do usuário");
            }
            else
            {
                // Pausa curta
                nextState = "short_break";
                nextSeconds = SHORT_BREAK_TIME;
                System.Diagnostics.Debug.WriteLine($"Preparando pausa curta ({SHORT_BREAK_TIME} seg). Faltam {POMODOROS_UNTIL_LONG_BREAK - _completedPomodoros} pomodoros para pausa longa - aguardando START do usuário");
            }
        }
        else
        {
            // Acabou uma pausa (curta ou longa), volta para foco
            nextState = "focus";
            nextSeconds = FOCUS_TIME;
            System.Diagnostics.Debug.WriteLine($"Preparando sessão de foco ({FOCUS_TIME} seg) - aguardando START do usuário");
        }

        // Atualiza o estado mas NÃO inicia automaticamente
        vm.CurrentState = nextState;
        vm.RemainingTime = nextSeconds;
        vm.UpdatePomodoroProgress(); // Notifica mudança no progresso
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
        int defaultSeconds = FOCUS_TIME;
        if (string.Equals(vm.CurrentState, "short_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = SHORT_BREAK_TIME;
        else if (string.Equals(vm.CurrentState, "long_break", StringComparison.OrdinalIgnoreCase))
            defaultSeconds = LONG_BREAK_TIME;
        _secondsRemaining = defaultSeconds;
        vm.RemainingTime = defaultSeconds;
        vm.IsRunning = false;
    }

    public void SkipToNext(HomeViewModel vm)
    {
        // Parar qualquer timer atual
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        // Se estava em foco, conta como um pomodoro completado
        if (string.Equals(vm.CurrentState, "focus", StringComparison.OrdinalIgnoreCase))
        {
            _completedPomodoros++;
            System.Diagnostics.Debug.WriteLine($"Pomodoro pulado! Total: {_completedPomodoros}");
        }

        // Prepara o próximo estado (sem iniciar automaticamente)
        // Passa false porque já incrementamos acima se necessário
        PrepareNextState(vm, incrementCounter: false);
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            vm.IsRunning = false; // Para o timer, usuário precisa clicar START
        });
    }
}
