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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Converter
{   
    /// <summary>
    /// This class implements an IValueConverter to convert a switch state (true or false) to a text (on or off).
    /// </summary>       
    public class SwitchStateToTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string)) throw new InvalidOperationException("The target must be a string.");

            if (value == null) return "";

            if ((bool)value == true) return AppResources.SwitchStateOn;
            return AppResources.SwitchStateOff;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter,System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
