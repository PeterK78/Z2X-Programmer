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

using System.Collections.ObjectModel;
using Z2XProgrammer.DataStore;

namespace Z2XProgrammer.Helper
{
    internal static class FunctionOutputs
    {
        /// <summary>
        /// Returns the user defined names of the function outputs.
        /// </summary>
        /// <param name="useDefaultNames">If no user defined function output name is defined, if useDefaultNames is TRUE the function returns the default function output name. If FALSE an empty string will be returned.</param>
        /// <returns>An obserable collection with the function output names.</returns>
        internal static ObservableCollection<string> GetNames(bool useDefaultNames)
        {
            ObservableCollection<string> namesList = new ObservableCollection<string>();

            for (int i = 0; i < DecoderConfiguration.UserDefinedFunctionOutputNames.Count - 1; i++)
            {
                string defaultName = "";
                if (i == 0)
                {
                    defaultName = useDefaultNames ? "0v" : "";
                }
                else if (i == 1)
                {
                    defaultName = useDefaultNames ? "0r" : "";
                }
                else
                {
                    defaultName = useDefaultNames ? i.ToString() : "";
                }
                namesList.Add(DecoderConfiguration.UserDefinedFunctionOutputNames[i].UserDefinedDescription == "" ? defaultName : DecoderConfiguration.UserDefinedFunctionOutputNames[i].UserDefinedDescription);
            }
            return namesList;
        }
    }

}
