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
    public static class GUI
    {
        /// <summary>
        /// Centers the given window.
        /// </summary>
        /// <param name="window">The window object to center.</param>    
        public static void CenterWindow(Window window)
        {
            // We get the current resolution of the screen.
            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            try
            {
                window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
            }
            catch { }
        }

        /// <summary>
        /// Resizes the given window.
        /// </summary>
        /// <param name="window">The window object to be resized.</param>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        public static void ResizeWindow(Window window,double width, double height)
        {
            window.Width = width;
            window.Height = height;
        }

    }
}
