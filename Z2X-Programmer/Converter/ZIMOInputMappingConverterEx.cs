/*

Z2X-Programmer
Copyright (C) 2024 - 2026
PeterK78

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see:

https://github.com/PeterK78/Z2X-Programmer?tab=GPL-3.0-1-ov-file.

*/

using System.Globalization;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Converter
{
    /// <summary>
    /// This class implements an IValueConverter to convert ZIMO input mapping values to user-friendly strings.
    /// </summary>       
    public class ZIMOInputMappingConverterEx : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a string");
            if ((values[0] == null) || (values[1] == null)) return "Wrong input parameters (both null).";

            //  0 = 1:1 DIRECT MAPPING
            if ((int)values[0] == 0) return "F" + values[1].ToString();

            // 29 = F0
            if ((int)values[0] == 29) return "F0";

            if (((int)values[0] > 0) && ((int)values[0] < NMRA.NumberOfFunctionKeys)) return "F" + values[0].ToString();

            return "Unknown Mapping";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

