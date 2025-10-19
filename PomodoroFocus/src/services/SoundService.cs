namespace PomodoroFocus;

public class SoundService
{
    public void PlayClickSound()
    {
        try
        {
#if WINDOWS
            System.Media.SystemSounds.Beep.Play();
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao tocar som: {ex.Message}");
        }
    }
}
