using System.Threading.Tasks;

namespace PomodoroFocus;

public class SoundService
{
    private const string ClickSoundPath = "tap_click.wav";
    private const string NotificationSoundPath = "notif_time.wav";
    private const string AchievementSoundPath = "achievement_notif.wav";

    public void PlayClickSound()
    {
        PlaySound(ClickSoundPath);
    }

    public void PlayNotificationSound()
    {
        PlaySound(NotificationSoundPath);
    }

    public void PlayAchievementSound()
    {
        PlaySound(AchievementSoundPath);
    }

    private void PlaySound(string soundFileName)
    {
        try
        {
#if WINDOWS
            global::System.Threading.Tasks.Task.Run(() =>
            {
                var soundPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, soundFileName);
                if (System.IO.File.Exists(soundPath))
                {
                    using (var player = new System.Media.SoundPlayer(soundPath))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Som não encontrado: {soundPath}");
                }
            });
#elif ANDROID
            Task.Run(() => PlaySoundAndroid(soundFileName));
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao tocar som {soundFileName}: {ex.Message}");
        }
    }

#if ANDROID
    private void PlaySoundAndroid(string soundFileName)
    {
        try
        {
            var context = Android.App.Application.Context;
            var resourceName = soundFileName.Replace(".wav", "").Replace("-", "_");
            var resourceId = context.Resources.GetIdentifier(
                resourceName,
                "raw",
                context.PackageName);

            if (resourceId != 0)
            {
                var mediaPlayer = new Android.Media.MediaPlayer();
                var afd = context.Resources.OpenRawResourceFd(resourceId);
                mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                afd.Close();
                mediaPlayer.Prepare();
                mediaPlayer.Start();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Recurso de áudio não encontrado: {resourceName}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao tocar som Android: {ex.Message}");
        }
    }
#endif
}
