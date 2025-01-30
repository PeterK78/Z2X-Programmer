﻿/*

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
namespace Z2XProgrammer.DataModel
{
    public class LocoListType
    {

        /// <summary>
        /// User defined decoder description.
        /// </summary>
        public string UserDefindedDecoderDescription { get; set; } = string.Empty;

        /// <summary>
        /// Locomotive address.
        /// </summary>
        public ushort LocomotiveAddress { get; set;  }

        /// <summary>
        /// Path to the Z2X file.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Stores the use defined image.
        /// </summary>
        public ImageSource UserDefindedImage { get; set; } = string.Empty;

        /// <summary>
        /// Z2X file available.
        /// </summary> 
        public bool Z2XFileAvailable { get; set; } = false;

        public LocoListType() { }

    }
}
