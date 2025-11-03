using System.Globalization;

namespace PomodoroFocus;

/// <summary>
/// Converte um valor boolean para uma cor (verde para desbloqueado, cinza para bloqueado)
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Color.FromArgb("#4CAF50") : Color.FromArgb("#CCCCCC"); // Verde ou Cinza
        }
        return Color.FromArgb("#CCCCCC");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Converte um valor boolean para opacidade (1.0 para desbloqueado, 0.6 para bloqueado)
/// </summary>
public class BoolToOpacityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? 1.0 : 0.6;
        }
        return 0.6;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
