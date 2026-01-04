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

namespace Z2XProgrammer.DataModel
{
    internal class ConfigurationVariableDownloadInfo
    {
        /// <summary>
        /// The number of the CV.
        /// </summary>
        /// 
        public int Number { get; set; }

        /// <summary>
        /// The value of the CV.
        /// </summary>
        public byte ValueZ2X {  get; set; }

        /// <summary>
        /// The value of the CV.
        /// </summary>
        public byte ValueDecoder {  get; set; }
        
        /// <summary>
        /// Enable or disables the CV.
        /// </summary>
        public bool Enabled {  get; set; }

        /// <summary>
        /// A short description of th CV.
        /// </summary>
        public string Description { get; set; } = string.Empty;

    }
}
