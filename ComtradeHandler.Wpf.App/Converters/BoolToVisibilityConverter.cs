using System.Globalization;
using System.Windows;

namespace ComtradeHandler.Wpf.App.Converters;

public class BoolToVisibilityConverter : BaseValueConverter<BoolToVisibilityConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (bool?)value switch {
            true => Visibility.Visible,
            false => Visibility.Collapsed,
            null => DependencyProperty.UnsetValue
        };
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
