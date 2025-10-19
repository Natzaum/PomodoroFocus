namespace PomodoroFocus;

public partial class App : Application
{
    const int WindowWidth = 400;
    const int WindowHeight = 750;

    public App()
    {
        try
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("App InitializeComponent completed");

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(
                nameof(IWindow),
                (handler, view) =>
                {
#if WINDOWS
                    var mauiWindow = handler.VirtualView;
                    var nativeWindow = handler.PlatformView;
                    nativeWindow.Activate();
                    IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                    var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                    appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));
#endif
                }
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in App constructor: {ex}");
            System.IO.File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "error.log"), ex.ToString());
            throw;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("Creating window...");
            var window = new Window(new AppShell());
            System.Diagnostics.Debug.WriteLine("Window created successfully!");
            return window;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating window: {ex}");
            System.IO.File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "window_error.log"), ex.ToString());
            throw;
        }
    }
}
