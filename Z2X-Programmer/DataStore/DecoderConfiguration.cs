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
using Z2XProgrammer.Communication;

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
        /// Returns TRUE if the backup data was read from a decoder (and not from a Z2X file).
        /// </summary>
        public static bool BackupDataFromDecoderIsValid { get; set; }

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
        public static string UserDefindedDecoderDescription { get; set; } = string.Empty;

        /// <summary>
        /// Stores the use defined notes.
        /// </summary>
        public static string UserDefindedNotes { get; set; } = string.Empty;

        /// <summary>
        /// Stores the use defined image.
        /// </summary>
        public static string UserDefindedImage { get; set; } = string.Empty;


        /// <summary>
        /// The path to the Z2X file.
        /// </summary>  
        public static string Z2XFilePath { get; set; } = string.Empty;

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
            Init(NMRA.StandardShortVehicleAddress, "");
        }

        /// <summary>
        /// Initializes the data store.
        /// </summary>
        public static void Init(ushort locomotiveAddress, string description)
        {
            BackupCVs.Clear();
            ConfigurationVariables.Clear();
            UserDefinedFunctionOutputNames.Clear();

            //  Setup the list of configuration variables.
            for (int i = 0; i <= Helper.NMRA.MaxCVValues + 1; i++)
            {
                BackupCVs.Add(new ConfigurationVariableType() { Number = i, Enabled = true, Description = "", Value = 0 });
                ConfigurationVariables.Add(new ConfigurationVariableType() { Number = i, Enabled = true, Description = "", Value = 0 });
            }

            //  Setup the list of function outputs.
            UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = "0v" }); 
            UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = "0r" }); 
            for (int i = 0; i<= Helper.NMRA.MaxFunctionOutputs-1;i++ )
            {
                UserDefinedFunctionOutputNames.Add(new FunctionOutputType() { Description = "", ID = (i+1).ToString() }); 
            }

            //  Reset the user defined settings.
            UserDefindedNotes = string.Empty;
            UserDefindedImage = string.Empty;
            UserDefindedDecoderDescription = description;

            //  Set the NMRA default addresses.
            RCN225.LocomotiveAddress = locomotiveAddress;
            RCN225Backup.LocomotiveAddress = NMRA.StandardShortVehicleAddress;

            //  The current data in the data store is not valid.
            IsValid = false;

            //  The backup data has not been read from the decoder.
            BackupDataFromDecoderIsValid = false;

        }

        /// <summary>
        /// Clears the data store and sets the default values of specific CVs.
        /// </summary>
        public static void ClearBackupCVs()
        {
            
            foreach (ConfigurationVariableType v in BackupCVs)
            {
                v.Value = 0;
                v.Enabled = true;
                v.Description = "";
            }
        }

        /// <summary>
        /// Returns TRUE if the given configuration variable is enabled. Returns FALSE if
        /// the configuration variable is disabled.
        /// </summary>
        /// <param name="cvNumber">TRUE if enabled, FALSE if disabled.</param>
        /// <returns></returns>
        public static bool IsCVEnabled(ushort cvNumber)
        {
            foreach (ConfigurationVariableType item in ConfigurationVariables)
            {
                if (item.Number == cvNumber) return item.Enabled;
            }
            return true;
        }
    
        /// <summary>
        /// Enable each configuration variable if its supported by the given decoder specification.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification.</param>
        public static void EnableAllCVsSupportedByDecSpec(string decSpecName)
        {
            try
            {
                DecoderConfiguration.ConfigurationVariables.ForEach(c => { c.Enabled = false; });
                foreach (int CVNumber in ReadWriteDecoder.GetAllReadableConfigurationVariables(decSpecName))
                {
                    ConfigurationVariableType variable = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == CVNumber);
                    variable.Enabled = true;
                }
            }
            catch (Exception e)
            {
                Logger.PrintDevConsole(e.Message);
            }
        }

        /// <summary>
        ///  Returns TRUE if all configuration variables are enabled and supported by the current decoder specification.
        ///  </summary>
        public static bool AllSupportedCVsEnabled()
        {
            foreach (ConfigurationVariableType item in ConfigurationVariables)
            {
                if (item.DeqSecSupported == true && item.Enabled == false)
                {
                    Logger.LogInformation("DecoderConfiguration.AllSupportedCVsEnabled disabled CV found:" + item.Number.ToString());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Marks each configuration variable with the information whether it is supported by the current decoder specification.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification.</param>
        public static void SetDecoderSpecification (string decSpecName)
        {
            try
            {
                DecoderConfiguration.ConfigurationVariables.ForEach(c => { c.DeqSecSupported = false; });
                foreach (int CVNumber in ReadWriteDecoder.GetAllReadableConfigurationVariables(decSpecName))
                {
                    ConfigurationVariableType variable = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == CVNumber);
                    variable.DeqSecSupported = true;
                }
            }
            catch (Exception e)
            {
                Logger.PrintDevConsole(e.Message);
            }
            
        }
        
    }
}
