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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Z21Lib.Events;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Communication
{
    internal static class CommandStation
    {
        public static Z21Lib.Client Z21 = new Z21Lib.Client();

        internal static List<ProgrammingModeType> _programmingModes = new List<ProgrammingModeType>();


        #region REGION: PUBLIC DELEGATES
        
        //  Will be called if the status of the command station has been changed (e.g. track power, programming mode etc.)
        public static event EventHandler<StateEventArgs> OnStatusChanged = default!;

        /// <summary>
        /// OnReachabilityChanged is raised when the reachability to the Z21 has changed.    
        /// </summary>
        public static event EventHandler<bool> OnReachabilityChanged = default!;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        static CommandStation()
        {
            //  Setup the programming modes
            ProgrammingModeType itemPOMMainTrack = new ProgrammingModeType();
            itemPOMMainTrack.Mode = Helper.NMRA.DCCProgrammingModes.POMMainTrack;
            itemPOMMainTrack.Description = GetProgrammingModeDescription(Helper.NMRA.DCCProgrammingModes.POMMainTrack);
            _programmingModes.Add(itemPOMMainTrack);

            ProgrammingModeType itemProgramTrack = new ProgrammingModeType();
            itemProgramTrack.Mode = Helper.NMRA.DCCProgrammingModes.DirectProgrammingTrack;
            itemProgramTrack.Description = GetProgrammingModeDescription(Helper.NMRA.DCCProgrammingModes.DirectProgrammingTrack);
            _programmingModes.Add(itemProgramTrack);


            //  Register the status changed event of the Z21 command station
            CommandStation.Z21.OnStatusChanged += OnZ21StatusChanged;
            CommandStation.Z21.OnReachabilityChanged += OnZ21ReachabilityChanged;

        }

        internal static NMRA.DCCProgrammingModes GetProgrammingModeFromDescription (string modeDescription)
        {

            if(String.Compare(modeDescription, AppResources.DCCProgrammngModeProgramTrack) == 0)
            {
                return NMRA.DCCProgrammingModes.DirectProgrammingTrack;
            }
            if (String.Compare(modeDescription, AppResources.DCCProgrammingModePOM) == 0)
            {
                return NMRA.DCCProgrammingModes.POMMainTrack;
            }
            return NMRA.DCCProgrammingModes.POMMainTrack;
        }

        internal static string GetProgrammingModeDescription (NMRA.DCCProgrammingModes mode)
        {
            switch (mode)
            {
                case NMRA.DCCProgrammingModes.DirectProgrammingTrack:   return AppResources.DCCProgrammngModeProgramTrack; 
                case NMRA.DCCProgrammingModes.POMMainTrack:             return AppResources.DCCProgrammingModePOM; 
                default: return "Unknown Programming mode (internal error)"; 
            }
        }


        /// <summary>
        /// Returns a list of supported programming modes for the selected command station
        /// </summary>
        /// <returns></returns>
        internal static List<ProgrammingModeType> GetAvailableProgrammingModes()
        {
            return _programmingModes;
        }

        internal static List<string> GetAvailableProgrammingModeNames()
        {
            List<string> Names = new List<string>();
            foreach (ProgrammingModeType item in _programmingModes)
            {
                if(item.Description != null) Names.Add(item.Description);
            }
            return Names;
        }

        /// <summary>
        /// Connects to the selected command station.
        /// </summary>
        /// <returns>True if the connection was successfull. False if the connection could not be established.</returns>
        public static bool Connect()
        {

            //  Timeout while connecting to the command station
            int ElapsedTime = 0;
     
            //  Check if we need to connect to the command station
            if (CommandStation.Z21.IsReachable == false)
            {
                IPAddress address = IPAddress.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT));
                CommandStation.Z21.Connect(address);

                //  We wait approx. 5 seconds for a response of the Z21
                while ((CommandStation.Z21.IsReachable == false) && (ElapsedTime < 5000))
                {
                    Thread.Sleep(1);
                    ElapsedTime++;
                    if (CommandStation.Z21.IsReachable == true) return true;
                }

                //  The Z21 is not reachable
                return false;
                
            }

            return true;
        }      

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// The event OnZ21StatusChanged is raised when the Z21 changes its operating mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnZ21StatusChanged(object? sender, StateEventArgs e)
        {
            OnStatusChanged.Invoke(sender, e);
        }

        /// <summary>
        /// The event OnZ21ReachabilityChanged is raised when the Z21 is reachable by a ping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnZ21ReachabilityChanged(object? sender, bool e)
        {
            OnReachabilityChanged.Invoke(sender, e);
        }

        #endregion
    }
}
