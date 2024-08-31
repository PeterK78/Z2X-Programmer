/*

Z2X-Programmer
Copyright (C) 2024
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
    internal static class DoehlerAndHaass
    {

        /// <summary>
        /// Returns a textual description of the CV value 261
        /// </summary>
        /// <param name="value">Value of CV261</param>
        /// <returns></returns>
        internal static string GetDecoderTypeDesciption(byte value)
        {
            switch (value)
            {
                case 41: return "FH05B"; 
                case 52: return "DH05C"; 
                case 141: return "DH14B";
                default: return value.ToString(); 
            }
        }


    }
}
