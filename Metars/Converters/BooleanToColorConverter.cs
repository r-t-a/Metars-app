using System;
using System.Globalization;
using Xamarin.Forms;

namespace Metars.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; }
        public Color DefaultColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueColor : DefaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Color)value == TrueColor;
        }
    }
}
