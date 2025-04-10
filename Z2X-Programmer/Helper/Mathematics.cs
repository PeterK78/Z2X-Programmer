/*

Z2X-Programmer
Copyright (C) 2025
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

namespace Z2XProgrammer.Helper
{
    internal static class Mathematics
    {
        /// <summary>
        /// Scale a value from one range to another.
        /// </summary>
        /// <param name="value">The value to scale.</param>
        /// <param name="minInputScale">The mimimum value of the input scale.</param>
        /// <param name="maxInputScale">The maximum value of the input scale.</param>
        /// <param name="minOutputScale">The mimimum value of the output scale.</param>
        /// <param name="maxOutputScale">The maximum value of the output scale.</param>
        /// <returns></returns>
        internal static double ScaleRange(double value , double minInputScale, double maxInputScale, double minOutputScale, double maxOutputScale)
        {
            try
            {
                return minOutputScale + (double)(value - minInputScale) / (maxInputScale - minInputScale) * (maxOutputScale - minOutputScale);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    
    }
}
