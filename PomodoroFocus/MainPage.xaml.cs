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

    private async void OnButtonPressed(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            System.Diagnostics.Debug.WriteLine($"Botão pressionado: {button.Text}");
            // Escurece o botão
            await button.FadeTo(0.5, 100);
            // Volta ao normal
            await button.FadeTo(1.0, 100);
        }
    }
}
