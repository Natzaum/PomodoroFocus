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
       var soundPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Resources", "Raw", soundFileName);
           if (System.IO.File.Exists(soundPath))
       {
          using (var player = new System.Media.SoundPlayer(soundPath))
     {
            player.PlaySync();
           }
   }
        else
{
          System.Diagnostics.Debug.WriteLine($"Som não encontrado em: {soundPath}");
                }
      });
#elif ANDROID
            global::System.Threading.Tasks.Task.Run(() => PlaySoundAndroid(soundFileName));
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
        Android.Media.MediaPlayer mediaPlayer = null;
        try
  {
 var context = Android.App.Application.Context;
    var resourceName = soundFileName.Replace(".wav", "");
        var resourceId = context.Resources.GetIdentifier(
     resourceName,
   "raw",
  context.PackageName);

      if (resourceId != 0)
     {
        mediaPlayer = new Android.Media.MediaPlayer();
 var afd = context.Resources.OpenRawResourceFd(resourceId);
       mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
    afd.Close();
     mediaPlayer.Prepare();
         
      // Liberar recursos ao terminar
    mediaPlayer.Completion += (sender, e) =>
 {
    mediaPlayer?.Release();
            mediaPlayer?.Dispose();
        };
        
    mediaPlayer.Start();
    }
     else
   {
 System.Diagnostics.Debug.WriteLine($"Recurso de áudio não encontrado: {resourceName} (ID: {resourceId}) - Recurso esperado no pacote {context.PackageName}");
            }
    }
        catch (Exception ex)
        {
System.Diagnostics.Debug.WriteLine($"Erro ao tocar som Android: {ex.Message}");
     mediaPlayer?.Release();
  mediaPlayer?.Dispose();
      }
    }
#endif
}
