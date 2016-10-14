using System;
using Windows.UI.Xaml.Data;

namespace Flights.WP.Converters
{
    public class StringToNotNullStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value != null) ? value : "No info";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
