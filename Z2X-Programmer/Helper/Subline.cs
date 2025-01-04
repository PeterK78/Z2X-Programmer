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

using Z2XProgrammer.DataStore;

namespace Z2XProgrammer.Helper
{
    internal static class Subline
    {
        /// <summary>
        /// Creates the subline text based on a given list of CV numbers.
        /// </summary>
        /// <param name="configurationVariableNumbers">A list with numbers of the required configuration variables.</param>
        /// <returns></returns>
        internal static string Create(List<uint> configurationVariableNumbers)
        {
            try
            {
                if (configurationVariableNumbers == null) return string.Empty;
                if (configurationVariableNumbers.Count == 0) return string.Empty;

                string msg = string.Empty;
                foreach (uint value in configurationVariableNumbers)
                {
                    msg += "CV" + value.ToString() + "=" + DecoderConfiguration.ConfigurationVariables[(int)value].Value.ToString() + " | ";
                }
                return msg[..^3];
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("Subline.Create:" + ex.Message);
                return string.Empty;    
            }
        }
    }
}
