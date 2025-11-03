using System.Globalization;

namespace PomodoroFocus;

/// <summary>
/// Converte status de task para visibilidade (mostra botão "Iniciar" apenas para tasks "A Fazer")
/// </summary>
public class StatusToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TaskStatus status)
        {
            return status == TaskStatus.Todo;
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
