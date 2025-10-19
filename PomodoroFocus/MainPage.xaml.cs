using Microsoft.Extensions.DependencyInjection;

namespace PomodoroFocus;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<HomeViewModel>();
    }
}
