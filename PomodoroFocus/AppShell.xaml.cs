namespace PomodoroFocus;

public partial class AppShell : Shell
{
    public AppShell()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("AppShell constructor starting...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("AppShell InitializeComponent completed!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in AppShell: {ex}");
            throw;
        }
    }
}
