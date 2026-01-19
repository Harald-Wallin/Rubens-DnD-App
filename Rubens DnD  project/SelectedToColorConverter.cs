using System.Globalization;
using Microsoft.Maui.Controls;

namespace Rubens_DnD__project;

public class SelectedToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Character selected && parameter is Character current)
            return selected == current ? Colors.LightGray : Colors.Transparent;

        return Colors.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


