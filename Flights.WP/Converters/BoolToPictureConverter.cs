using System;
using Windows.UI.Xaml.Data;

namespace Flights.WP.Converters 
{
    public class BoolToPictureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as bool?;
            if (boolValue == null)
            {
                throw new ArgumentException("BoolToPicture work only with bool type");
            }
            return ((bool)value) ? "ms-appx:///Assets/fly_return.png" : "ms-appx:///Assets/fly.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
