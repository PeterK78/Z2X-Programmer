using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Z2XProgrammer.Converter
{
    public class DoubleByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0.0;
            try { return System.Convert.ToDouble(value, culture); }
            catch { return 0.0; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return (byte)0;
            double d;
            try { d = System.Convert.ToDouble(value, culture); }
            catch { d = 0.0; }
            var i = (int)Math.Round(d);
            if (i < byte.MinValue) i = byte.MinValue;
            if (i > byte.MaxValue) i = byte.MaxValue;
            return (byte)i;
        }
    }
}