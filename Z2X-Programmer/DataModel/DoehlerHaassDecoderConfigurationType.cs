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
    internal class DoehlerHaassDecoderConfigurationType
    {

        internal List<ConfigurationVariableType> configurationVariables = new List<ConfigurationVariableType>();

        public DoehlerHaassDecoderConfigurationType(ref List<ConfigurationVariableType> cvList)
        {
            configurationVariables = cvList;
        }

        /// <summary>
        /// Returns the raw value of the impuls with in CV49
        /// </summary>
        public byte MotorImpulsWidth
        {
            get
            {
                return configurationVariables[49].Value;
            }
            set
            {
                configurationVariables[49].Value = value;
            }
        }

        /// <summary>
        /// Returns the decoder type (CV261)
        /// </summary>
        public byte DecoderType
        {
            get
            {
                return configurationVariables[261].Value;
            }
        }

        /// <summary>
        /// Returns the decoder firmware version (CV262 + 264).
        /// </summary>
        public string FirmwareVersion
        {
            get
            {
                return configurationVariables[262].Value.ToString() + "." + configurationVariables[264].Value.ToString();
            }
        }

        /// <summary>
        ///  The type of the function mapping in CV137 bit 4.
        /// </summary>
        public bool ExtendedFunctionKeyMappingEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[137].Value, 4);
            }
            set
            {
                configurationVariables[137].Value = Bit.Set(configurationVariables[137].Value, 4, value);
            }
        }

        /// <summary>
        /// The number of the function key (F1-F28) to disable the accerleration and deceleration times
        /// in CV133.
        /// </summary>
        public byte FuncKeysAccDecDisableFuncKeyNumber
        {
            get
            {
                return configurationVariables[133].Value;
            }
            set
            {
                configurationVariables[133].Value = value;
            }
        }

        /// <summary>
        /// The number of the function key (F1-F28) to enable the shunting gear
        /// in CV132.
        /// </summary>
        public byte FuncKeysShuntingFuncKeyNumber
        {
            get
            {
                return configurationVariables[132].Value;
            }
            set
            {
                configurationVariables[132].Value = value;
            }
        }



    }
}
