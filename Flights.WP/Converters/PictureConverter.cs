using System;
using Windows.UI.Xaml.Data;

namespace Flights.WP.Converters 
{
    public class PictureConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                string picture = ((bool)value) ? "ms-appx:///Assets/fly_return.png" : "ms-appx:///Assets/fly.png";
                return picture;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
