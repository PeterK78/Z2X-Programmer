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
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.DataModel
{
    internal class PIKOSmartDecoderV41ConfigurationType
    {

        internal List<ConfigurationVariableType> configurationVariables = new List<ConfigurationVariableType>();

        public PIKOSmartDecoderV41ConfigurationType(ref List<ConfigurationVariableType> cvList)
        {
            configurationVariables = cvList;
        }

        /// <summary>
        /// Contains the maximum speed of CV5.
        /// </summary>
        public byte MaximumSpeed
        {
            get
            {
                return configurationVariables[5].Value;
            }
            set
            {
                configurationVariables[5].Value = value;
            }
        }

        /// <summary>
        /// Contains the minimum speed of CV2.
        /// </summary>
        public byte MinimumSpeed
        {
            get
            {
                return configurationVariables[2].Value;
            }
            set
            {
                configurationVariables[2].Value = value;
            }
        }

        /// <summary>
        /// Returns the ID of the decoder (CV261-264).
        /// </summary>
        public string DecoderID
        {
            get
            {
                int pikoDecoderID = (configurationVariables[264].Value * 16777216) + (configurationVariables[263].Value * 65536) + (configurationVariables[262].Value * 256) + configurationVariables[261].Value;
                return pikoDecoderID.ToString();
            }
        }

    }
}
