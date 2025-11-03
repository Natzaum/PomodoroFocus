namespace PomodoroFocus;

public partial class TasksPage : ContentPage
{
    private readonly TasksViewModel _viewModel;

    public TasksPage()
    {
        InitializeComponent();
        
        _viewModel = ServiceHelper.GetService<TasksViewModel>()!;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTasks();
    }
}
