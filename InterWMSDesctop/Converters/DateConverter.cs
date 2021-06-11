using System;
using System.Globalization;
using System.Windows.Data;
using ApiApp.Extensions;

namespace InterWMSDesctop.Converters
{
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var date = long.Parse(value.ToString()).GetNormalTime();

            return date.ToString("dd.MM.yyy HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
