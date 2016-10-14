using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Flights.WP.Converters
{
    public class BoolToVisibilityFavoriteIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as bool?;
            if (boolValue == null)
            {
                throw new ArgumentException("BoolToVisibility work only with bool type");
            }
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}