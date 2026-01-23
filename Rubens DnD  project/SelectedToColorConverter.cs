using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Rubens_DnD__project
{
    public class SelectedToColorConverter : IValueConverter
    {
        // Konverterar bool (IsSelected) till en bakgrundsfärg
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                if (isSelected)
                {
                    // Lite mer opak när vald
                    return Color.FromArgb("#FFF5F0E1"); // ljus pergament, 80% opacitet
                }
                else
                {
                    // Genomskinlig pergament när ej vald
                    return Color.FromArgb("#66F5F0E1"); // samma färg, 20% opacitet
                }
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}



