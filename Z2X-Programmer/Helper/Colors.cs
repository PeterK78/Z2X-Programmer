/*

Z2X - Programmer
Copyright(C) 2024
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

namespace Z2XProgrammer.Helper
{
    public static class Colors
    {

        /// <summary>
        /// Returns a color object for the given ressource identifiers.
        /// </summary>
        /// <param name="colorKeyLight">Resource identifier for the light theme.</param>
        /// <param name="colorKeyDark">Resource identifier for the dark theme.</param>
        /// <returns></returns>
        public static Color GetColor(string colorKeyLight, string colorKeyDark)
        {
            string colorKey = colorKeyLight;

            if (Application.Current != null)
            {
                AppTheme currentTheme = (AppTheme)Application.Current.RequestedTheme;
                colorKey = (currentTheme == AppTheme.Dark) ? colorKeyDark : colorKeyLight;
                if (App.Current.Resources.TryGetValue(colorKey, out var colorvalue)) return (Color)colorvalue;
            }
            return Color.FromRgb(0, 0, 0);
        }
    }
}
