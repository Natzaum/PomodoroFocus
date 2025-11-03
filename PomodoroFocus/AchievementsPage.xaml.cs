namespace PomodoroFocus;

public partial class AchievementsPage : ContentPage
{
    private readonly AchievementsViewModel _viewModel;

    public AchievementsPage()
    {
        InitializeComponent();
        
        _viewModel = ServiceHelper.GetService<AchievementsViewModel>()!;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAchievements();
    }
}
