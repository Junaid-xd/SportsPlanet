using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SportsPlanet.Converters
{
    public class UnixToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            long unixTime = (long)value;

            var date = DateTimeOffset
                .FromUnixTimeSeconds(unixTime)
                .ToLocalTime()
                .DateTime;

            return date.ToString("dd MMM yyyy, hh:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
