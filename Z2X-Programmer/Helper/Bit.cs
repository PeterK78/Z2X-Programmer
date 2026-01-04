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

namespace Z2XProgrammer.Helper
{
    public static class Bit
    {
        /// <summary>
        /// Returns a byte where the bit in position pos of the value value has been set to 1 or 0.
        /// </summary>
        /// <param name="b">The variable in which the bit is to be set.</param>
        /// <param name="pos">The number of the bit to be set (0 - 7).</param>
        /// <param name="value">The state of the bit (True = 1, False = 0).</param>
        /// <returns></returns>
        public static byte Set(byte b, int pos, bool value)
        {
            //  Check the desired position of the bit.
            if(pos > 7) throw new ArgumentOutOfRangeException("pos", "The position must be between 0 and 7.");

            if (value == true)
            {
                return  (byte)(b | (1 << pos));              
            }
            else
            {
                return (byte)(b & ~(1 << pos));
            }
        }

        public static bool IsSet(ushort b, int pos)
        {
            return (b & (1 << pos)) != 0;           
        }
    }
}
