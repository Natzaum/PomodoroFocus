using Microsoft.Extensions.DependencyInjection;

namespace PomodoroFocus;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("MainPage constructor starting...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("MainPage InitializeComponent completed!");
            
            BindingContext = ServiceHelper.GetService<HomeViewModel>();
            System.Diagnostics.Debug.WriteLine("MainPage BindingContext set!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in MainPage: {ex}");
            throw;
        }
    }
}
