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
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Z2XProgrammer.Helper;
using Z2XProgrammer.DataModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.Messages;
using System.Collections.ObjectModel;

namespace Z2XProgrammer.DataStore
{

    /// <summary>
    /// This class contains the current configuration of the locomotive decoder.
    /// </summary>
    internal static class DecoderConfiguration
    {

        /// <summary>
        /// Returns TRUE if the decoder configuration contains valid data either by loading from a Z2X file
        /// or by uploading data from the decoder.
        /// </summary>
        public static bool IsValid { get; set; }

        /// <summary>
        /// The actual configuration variables.
        /// </summary>
        public static List<ConfigurationVariableType> ConfigurationVariables = new List<ConfigurationVariableType>();

        /// <summary>
        /// The last known backup of the configuration variables. This backup is created during the
        /// upload of the data from the decoder
        /// </summary>
        public static List<ConfigurationVariableType> BackupCVs = new List<ConfigurationVariableType>();
        
        /// <summary>
        /// The RCN225 specific decoder settings
        /// </summary>
        public static RCN225DecoderConfigurationType RCN225 = new RCN225DecoderConfigurationType(ref ConfigurationVariables);
        public static RCN225DecoderConfigurationType RCN225Backup = new RCN225DecoderConfigurationType(ref BackupCVs);

        /// <summary>
        /// The ZIMO specific decoder settings
        /// </summary>
        public static ZIMODecoderConfigurationType ZIMO = new ZIMODecoderConfigurationType(ref ConfigurationVariables);
        public static ZIMODecoderConfigurationType ZIMOBackup = new ZIMODecoderConfigurationType(ref BackupCVs);

        /// <summary>
        /// The Doehler and Haass specific decoder settings
        /// </summary>
        public static DoehlerHaassDecoderConfigurationType DoehlerHaas = new DoehlerHaassDecoderConfigurationType(ref ConfigurationVariables);
        public static DoehlerHaassDecoderConfigurationType DoehlerHaasBackup = new DoehlerHaassDecoderConfigurationType(ref BackupCVs);

        /// <summary>
        /// Stores the user defined decoder description.
        /// </summary>
        public static string UserDefindedDecoderDescription { get; set; }

        /// <summary>
        /// Stores the use defined notes.
        /// </summary>
        public static string UserDefindedNotes { get; set; }

        /// <summary>
        /// Stores the use defined image.
        /// </summary>
        public static string UserDefindedImage { get; set; }

        /// <summary>
        /// The currently selected programming mode.
        /// </summary>
        internal static NMRA.DCCProgrammingModes ProgrammingMode { get; set; }

        /// <summary>
        /// A list of user defined function output names.
        /// </summary>
        public static List<FunctionOutputType> UserDefinedFunctionOutputNames = new List<FunctionOutputType>();
       
        /// <summary>
        /// Constructor
        /// </summary>
        static DecoderConfiguration()
        {
            UserDefindedNotes = string.Empty;
            UserDefindedImage = string.Empty;
            UserDefindedDecoderDescription = string.Empty;

            Init(NMRA.StandardLocomotiveAddress, "");
        }

        /// <summary>
        /// Initializes the data store.
        /// </summary>
        public static void Init(ushort locomotiveAddress, string description)
        {
            BackupCVs.Clear();
            ConfigurationVariables.Clear();
            UserDefinedFunctionOutputNames.Clear();

            //  Setup the list of configuration variables
            for (int i = 0; i <= Helper.NMRA.MaxCVValues + 1; i++)
            {
                BackupCVs.Add(new ConfigurationVariableType() { Number = i, Enabled = false, Description = "", Value = 0 });
                ConfigurationVariables.Add(new ConfigurationVariableType() { Number = i, Enabled = false, Description = "", Value = 0 });
            }

            //  Setup the list of function outputs
            UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = "0v" }); 
            UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = "0r" }); 
            for (int i = 0; i<= Helper.NMRA.MaxFunctionOutputs-1;i++ )
            {
                UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = (i+1).ToString() }); 
            }

            //  Reset the user defined settings
            UserDefindedNotes = string.Empty;
            UserDefindedImage = string.Empty;
            UserDefindedDecoderDescription = description;

            //  Set the NMRA default addresses
            RCN225.LocomotiveAddress = locomotiveAddress;
            RCN225Backup.LocomotiveAddress = NMRA.StandardLocomotiveAddress;

            //  The current data in the data store is not valid
            IsValid = false;

        }

        /// <summary>
        /// Clears the data store and sets the default values of specific CVs
        /// </summary>
        public static void ClearBackupCVs()
        {
            
            foreach (ConfigurationVariableType v in BackupCVs)
            {
                v.Value = 0;
                v.Enabled = false;
                v.Description = "";
            }
        }
    }
}
