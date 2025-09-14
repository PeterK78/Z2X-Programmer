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

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        //  Will be called if we receive railcom data.
        public static event EventHandler<RailComInfoEventArgs> OnRailComInfoReceived = default!;

        //  Will be called if we receive RM bus data.
        public static event EventHandler<RmBusInfoEventArgs> OnRmBusInfoReceived = default!;

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
            CommandStation.Z21.OnRailComInfoReceived += OnZ21RailComInfoReceived;
            CommandStation.Z21.OnReachabilityChanged += OnZ21ReachabilityChanged;
            CommandStation.Z21.OnRmBusInfoReceived += OnZ21RmBusInfoReceived;

        }

        /// <summary>
        /// Turns the track power on.
        /// </summary>
        public static void SetTrackPowerOn()
        {
            CommandStation.Z21.SetTrackPowerOn();
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
        /// Disconnects from the digital command station.
        /// </summary>
        internal static void Disconnect()
        {
            Z21.Disconnect();
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
        /// Returns TRUE if the command station is reachable.
        /// </summary>  
        public static bool IsReachable
        {
            get
            {
                return Z21.IsReachable;
            }
        }


        /// <summary>
        /// Connects to the selected digital command station.
        /// </summary>
        /// <param name="cancelToken">A cancelation toke to cancel the connection process.</param>
        /// <param name="timeOut">A timeout in milliseconds.</param>
        /// <returns></returns>
        public static bool Connect(CancellationToken cancelToken, long timeOut)
        {
            try
            {
                //  We check whether the digital command station is already reachable.
                //  If so, we return.
                if (CommandStation.Z21.IsReachable == true) return true;

                // We get the configured IP address from the settings and connect to the configured digital control center.
                IPAddress address = IPAddress.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT));
                CommandStation.Z21.Connect(address);

                //  We are now waiting for the command center to connect.
                //  The wait is ended prematurely when the timeout has expired or when the cancelation token is set.
                Stopwatch stopwatch = Stopwatch.StartNew();
                while ((CommandStation.Z21.IsReachable == false) && (stopwatch.ElapsedMilliseconds < timeOut) && (cancelToken.IsCancellationRequested == false))
                {
                    Thread.Sleep(1);
                    if (CommandStation.Z21.IsReachable == true) return true;
                }

                //  The digital command center is not reachable.
                return false;
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("CommandStation.Connect:" + ex.Message);
                return false;
            }
                
        }

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>   
        /// The event OnRmBusInfoReceived is raised when the Z21 receives RM bus data.
        /// </summary>
        private static void OnZ21RmBusInfoReceived(object? sender, RmBusInfoEventArgs e)
        {
            if(OnRmBusInfoReceived != null) OnRmBusInfoReceived.Invoke(sender, e);
        }

        /// <summary>
        /// The event OnRailComInfoReceived is raised when the Z21 receives railcom data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnZ21RailComInfoReceived(object? sender, RailComInfoEventArgs e)
        {
            OnRailComInfoReceived.Invoke(sender, e);
        }

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
