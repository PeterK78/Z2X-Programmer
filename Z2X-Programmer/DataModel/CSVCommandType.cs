﻿/*

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


namespace Z2XProgrammer.DataModel
{
    internal class CSVCommandType
    {
        /// <summary>
        /// The commando (SETBYTE,SETBIT,CLEARBIT). 
        /// </summary>
        public string CommandName { get; set; } = string.Empty;

        /// <summary>
        /// The number of the configuration variable.
        /// </summary>
        /// 
        public int CVNumber { get; set; }

        /// <summary>
        /// The value of the parameter.
        /// </summary>
        public byte Parameter { get; set; }

        /// <summary>
        /// The comment.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        
    }
}
