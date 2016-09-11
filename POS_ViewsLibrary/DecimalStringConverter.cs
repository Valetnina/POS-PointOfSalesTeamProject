using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace POS_ViewsLibrary
{
    /// <summary>
    /// One-way converter from System.Drawing.Image to System.Windows.Media.ImageSource
    /// </summary>
    [ValueConversion(typeof(System.Decimal), typeof(System.String))]
    public class NumericalStringConverter : IValueConverter
    {
        public int EmptyStringValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
                return value;
            else if (value is decimal && (decimal)value == EmptyStringValue)
                return string.Empty;
            else if (value is Int32)
                return value.ToString();
            else if (value is Double)
                return ((double)value).ToString("#.##");
            else
        return ((decimal)value).ToString("#.##");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string s = (string)value;
                decimal d;
                if (decimal.TryParse(s, out d))
                    return d;
                else
                    return EmptyStringValue;
            }
            return value;
        }
    }
}
