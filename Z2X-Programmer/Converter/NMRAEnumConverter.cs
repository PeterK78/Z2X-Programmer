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

using Z2XProgrammer.DataModel;
using Z2XProgrammer.Resources.Strings;
using static Z2XProgrammer.Helper.NMRA;

namespace Z2XProgrammer.Helper
{
    /// <summary>
    /// This class implements different enum to text mapper
    /// </summary>
    internal static class NMRAEnumConverter
    {

        private static List<DCCAddressModeType> _DCCAddressModes = new List<DCCAddressModeType>();
        private static List<DCCSpeedStepModeType> _DCCSpeedStepModes = new List<DCCSpeedStepModeType>();
        private static List<DCCABCBreakModeType> _DCCABCBreakModes = new List<DCCABCBreakModeType>();


        static NMRAEnumConverter()
        {
            //
            //  Setup the address modes
            //
            DCCAddressModeType itemAddressModeShort = new DCCAddressModeType();
            itemAddressModeShort.Description = GetDCCAddressModeDescription(DCCAddressModes.Short);
            itemAddressModeShort.Mode = DCCAddressModes.Short;
            _DCCAddressModes.Add(itemAddressModeShort);

            DCCAddressModeType itemAddressModeLong = new DCCAddressModeType();
            itemAddressModeLong.Description = GetDCCAddressModeDescription(DCCAddressModes.Extended);
            itemAddressModeLong.Mode = DCCAddressModes.Short;
            _DCCAddressModes.Add(itemAddressModeLong);

            //
            //  Setup the speed steps modes
            //
            DCCSpeedStepModeType itemMode14 = new DCCSpeedStepModeType();
            itemMode14.Description = GetDCCSpeedStepModeDescription(DCCSpeedStepsModes.Steps14);
            itemMode14.Mode = DCCSpeedStepsModes.Steps14;
            _DCCSpeedStepModes.Add(itemMode14);

            DCCSpeedStepModeType itemMode28128 = new DCCSpeedStepModeType();
            itemMode28128.Description = GetDCCSpeedStepModeDescription(DCCSpeedStepsModes.Step28to128);
            itemMode28128.Mode = DCCSpeedStepsModes.Step28to128;
            _DCCSpeedStepModes.Add(itemMode28128);

            //
            //  Setup the ABC break modes
            //
            DCCABCBreakModeType itemABCBreakModeOff = new DCCABCBreakModeType();
            itemABCBreakModeOff.Description = GetDCCABCBreakModeDescription(DCCABCBreakModes.Off);
            itemABCBreakModeOff.Mode = DCCABCBreakModes.Off;
            _DCCABCBreakModes.Add(itemABCBreakModeOff);

            DCCABCBreakModeType itemABCBreakModeRightTrack = new DCCABCBreakModeType();
            itemABCBreakModeRightTrack.Description = GetDCCABCBreakModeDescription(DCCABCBreakModes.RightTrack);
            itemABCBreakModeRightTrack.Mode = DCCABCBreakModes.RightTrack;
            _DCCABCBreakModes.Add(itemABCBreakModeRightTrack);

            DCCABCBreakModeType itemABCBreakModeLeftTrack = new DCCABCBreakModeType();
            itemABCBreakModeLeftTrack.Description = GetDCCABCBreakModeDescription(DCCABCBreakModes.LeftTrack);
            itemABCBreakModeLeftTrack.Mode = DCCABCBreakModes.LeftTrack;
            _DCCABCBreakModes.Add(itemABCBreakModeLeftTrack);

        }



        internal static string GetDCCAddressModeDescription(NMRA.DCCAddressModes mode)
        {
            switch (mode)
            {
                case NMRA.DCCAddressModes.Short: return AppResources.DCCAddressModeShort;
                case NMRA.DCCAddressModes.Extended: return AppResources.DCCAddressModeLong;
                default: return "Unknown DCC address mode (internal error)";
            }
        }

        internal static NMRA.DCCAddressModes GetDCCAddressModeFromDescription(string modeDescription)
        {

            if (String.Compare(modeDescription, AppResources.DCCAddressModeLong) == 0)
            {
                return NMRA.DCCAddressModes.Extended;
            }
            if (String.Compare(modeDescription, AppResources.DCCAddressModeShort) == 0)
            {
                return NMRA.DCCAddressModes.Short;
            }
            return NMRA.DCCAddressModes.Short;
        }

        /// <summary>
        /// Returns a list with function outputs.
        /// </summary>
        /// <param name="min">The minimum function output number.</param>
        /// <param name="max">The maximum function output number.</param>
        /// <param name="addNotDefined">If TRUE a "Not defined" function output will be added.</param>
        /// <returns></returns>
        internal static List<string> GetAvailableFunctionOutputs (int min, int max, bool addNotDefined)
        {
            List<string> FunctionOutputs = new List<string>();

            if (addNotDefined) { FunctionOutputs.Add(AppResources.FunctionOutputNotDefined); }

            for (int i = min; i <= max; i++)
            {
                string newItem = "FA" + i.ToString();
                FunctionOutputs.Add(newItem);
            }
            return FunctionOutputs;
        }

        /// <summary>
        /// Returns a list of strings with all available function keys (F0 → F28)
        /// </summary>
        /// <param name="addNotDefined"></param>
        /// <returns></returns>
        internal static List<string> GetAvailableFunctionKeys(bool addNotDefined)
        {
            List<string> FunctionKeys = new List<string>();

            if (addNotDefined) { FunctionKeys.Add(AppResources.FunctionKeysNotDefined); }

            for (int i = 1; i < NumberOfFunctionKeys; i++)
            {
                string newItem = "F" + i.ToString();
                FunctionKeys.Add(newItem);
            }
            return FunctionKeys;
        }


        internal static List<string> GetAvailableDCCAddressModes()
        {
            List<string> Names = new List<string>();
            foreach (DCCAddressModeType item in _DCCAddressModes)
            {
                if (item.Description != null) Names.Add(item.Description);
            }
            return Names;
        }

        /// <summary>
        /// Returns a description of the specified ABC braking mode.
        /// </summary>
        /// <param name="mode">DCC ABC braking mode</param>
        /// <returns></returns>
        internal static string GetDCCABCBreakModeDescription(NMRA.DCCABCBreakModes mode)
        {
            switch (mode)
            {
                case NMRA.DCCABCBreakModes.Off: return AppResources.DCCABCBreakModeOff;
                case NMRA.DCCABCBreakModes.RightTrack: return AppResources.DCCABCBreakModeRightTrack;
                case NMRA.DCCABCBreakModes.LeftTrack: return AppResources.DCCABCBreakModeLeftTrack;
                default: return "Unknown DCC ABC break mode (internal error)";

            }
        }

        internal static string GetDCCSpeedStepModeDescription(NMRA.DCCSpeedStepsModes mode)
        {

            switch (mode)
            {
                case NMRA.DCCSpeedStepsModes.Steps14: return AppResources.DCCSpeedStepMode14;
                case NMRA.DCCSpeedStepsModes.Step28to128: return AppResources.DCCSpeedStepMode28128;
                default: return "Unknown DCC speed step mode (internal error)";
            }
        }

        /// <summary>
        /// Returns a list with all available ABC break modes according to RCN225 in CV27
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAvailableDCCABCBreakModes()
        {
            List<string> Names = new List<string>();
            foreach (DCCABCBreakModeType item in _DCCABCBreakModes)
            {
                if(item.Description != null) Names.Add(item.Description);
            }
            return Names;
        }

        
        internal static List<string> GetAvailableDCCSpeedStepModes()
        {

            List<string> Names = new List<string>();
            foreach (DCCSpeedStepModeType item in _DCCSpeedStepModes)
            {
                if(item.Description != null) Names.Add(item.Description);
            }
            return Names;
        }

        internal static NMRA.DCCABCBreakModes GetDCCABCBreakModeFromDescription (string modeDescription)
        {
            if(String.Compare(modeDescription, AppResources.DCCABCBreakModeOff) == 0)
            {
                return NMRA.DCCABCBreakModes.Off;
            }

            if (String.Compare(modeDescription, AppResources.DCCABCBreakModeRightTrack) == 0)
            {
                return NMRA.DCCABCBreakModes.RightTrack;
            }

            if (String.Compare(modeDescription, AppResources.DCCABCBreakModeLeftTrack) == 0)
            {
                return NMRA.DCCABCBreakModes.LeftTrack;
            }

            return NMRA.DCCABCBreakModes.Off;
        }


        internal static NMRA.DCCSpeedStepsModes GetDCCSpeedStepsModeFromDescription(string modeDescription)
        {

            if (String.Compare(modeDescription, AppResources.DCCSpeedStepMode14) == 0)
            {
                return NMRA.DCCSpeedStepsModes.Steps14;
            }
            if (String.Compare(modeDescription, AppResources.DCCSpeedStepMode28128) == 0)
            {
                return NMRA.DCCSpeedStepsModes.Step28to128;
            }
            return NMRA.DCCSpeedStepsModes.Steps14;
        }



    }
}
