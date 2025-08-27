using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace The_Movies_WPF_app.Helpers
{
    public class TimeOnlyConverter : IValueConverter
    {
        // We need this for the StartTime in RegisterScreeningView
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeOnly time)
                return time.ToString("HH:mm"); // display as HH:mm
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (TimeOnly.TryParse(value as string, out var time))
                return time;
            return TimeOnly.MinValue; // fallback if invalid
        }
    }
}
