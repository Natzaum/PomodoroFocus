using System.Collections.ObjectModel;

namespace PomodoroFocus;

public class AchievementsViewModel : BaseViewModel
{
    private readonly AchievementService _achievementService;
    private ObservableCollection<Achievements> _achievements = new();
    private int _totalPomodoros;
    private int _unlockedCount;

    public ObservableCollection<Achievements> Achievements
    {
        get => _achievements;
        set
        {
            if (_achievements != value)
            {
                _achievements = value;
                OnPropertyChanged();
            }
        }
    }

    public int TotalPomodoros
    {
        get => _totalPomodoros;
        set
        {
            if (_totalPomodoros != value)
            {
                _totalPomodoros = value;
                OnPropertyChanged();
            }
        }
    }

    public int UnlockedCount
    {
        get => _unlockedCount;
        set
        {
            if (_unlockedCount != value)
            {
                _unlockedCount = value;
                OnPropertyChanged();
            }
        }
    }

    public AchievementsViewModel()
    {
        _achievementService = ServiceHelper.GetService<AchievementService>()!;
    }

    public async Task LoadAchievements()
    {
        try
        {
            IsLoading = true;

            var achievements = await _achievementService.GetAchievements();
            var totalPomodoros = await _achievementService.GetCompletedPomodoros();

            Achievements = new ObservableCollection<Achievements>(achievements);
            TotalPomodoros = totalPomodoros;
            UnlockedCount = achievements.Count(a => a.Unlocked);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar conquistas: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
