using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecordGui.Pages.Helpers
{
    public class SelectedEmployeeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = value.Equals(parameter);
            return isSelected ? Colors.LightGray : Colors.Transparent; // Change colors as needed
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "White" : "LightGray";
        }
    }
}
