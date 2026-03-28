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
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Model;
using Z2XProgrammer.Resources.Strings;
using Z2XProgrammer.Traincontroller;

namespace Z2XProgrammer.Helper
{
    public static class LocoList
    {
        #region REGION: PUBLIC PROPERTIES

        /// <summary>
        /// Returns the active loco list system name.         
        /// </summary>
        /// <returns>The name of the active loco list system in the current application language.</returns>
        public static string ActiveSystem
        {
            get
            {
                return ConvertLocoListSystem2AppLanguage(Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_SYSTEM_KEY, AppConstants.PREFERENCES_LOCOLIST_SYSTEM_VALUE));
            }
        }

        /// <summary>
        /// Returns the port address of the configured train controller software.
        /// </summary>
        /// <returns>Port number of the loco list system.</returns>
        public static int PortNumber
        { 
            get
            {
                //  Try to parse the user specific traincontroller port number. If the parsing fails, return the default port number
                bool success = int.TryParse(Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_PORTNR_KEY, AppConstants.PREFERENCES_LOCOLIST_PORTNR_VALUE), out int portNumber);
                if (success)
                {
                    return portNumber;
                }
                else
                {
                    return 8051;
                }
            }
        }

        /// <summary>
        /// Returns the IP address of the loco list system.
        /// </summary>
        /// <returns>IP address of the loco list system</returns>
        public static IPAddress IPAddress
        {
            get
            {
                string ipAddress = Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_IPADDRESS_KEY, AppConstants.PREFERENCES_LOCOLIST_IPADDRESS_VALUE);
                return IPAddress.Parse(ipAddress);
            }
        }

        /// <summary>
        /// Return the folder where the Z2X files for the locomotive list are stored.
        /// </summary>
        public static string Z2XFileFolder
        {
            get
            {
                return Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_FOLDER_KEY, AppConstants.PREFERENCES_LOCOLIST_FOLDER_VALUE);
            }
        }
        #endregion

        #region REGION: PUBLIC FUNCTIONS

        /// <summary>
        /// Returns a list with currently available loco list systems.
        /// </summary>
        /// <returns>A list of available loco list systems in the current application language.</returns>
        public static List<string> GetAvailableSystems()
        {
            List<string> locoListSystems = new List<string>();
            locoListSystems.Add(GetSystemNameNotAvailable(""));
            locoListSystems.Add(GetSystemNameRocrail(""));
            return locoListSystems;
        }

        /// <summary>
        /// Returns TRUE if the description describes the Rocrail train controller software.
        /// </summary>
        /// <param name="selectedLocoListDataSource">The train controller system description.</param>
        /// <returns></returns>
        public static bool IsSystemRocRail(string selectedLocoListDataSource)
        {
            if (selectedLocoListDataSource == AppResources.FrameSettingsAppLocoListRocrail) return true;
            return false;
        }

        /// <summary>
        /// Determines whether the specified data source represents the system's file-based source.
        /// </summary>
        /// <param name="selectedLocoListDataSource">The data source identifier to evaluate. Cannot be null.</param>
        /// <returns>true if the specified data source is the system's file-based source; otherwise, false.</returns>
        public static bool IsSystemFileSystem(string selectedLocoListDataSource)
        {
            if (selectedLocoListDataSource == GetSystemNameNotAvailable("")) return true;
            return false;
        }

        /// <summary>
        ///  Returns the description of a not available loco list system.
        /// </summary>
        /// <param name="culture">If emtpy, the current application language is used. Set to de-DE, or en-EN to get a specific language.</param>
        /// <returns></returns>
        public static string GetSystemNameNotAvailable(string culture)
        {
            if(culture == "") return AppResources.FrameSettingsAppLocoListNoTrainControllerNotAvailable;
            CultureInfo cultureInfo = new CultureInfo(culture);
            string? systemName = AppResources.ResourceManager.GetString("FrameSettingsAppLocoListNoTrainControllerNotAvailable", cultureInfo);
            if (systemName != null) return systemName;
            return "";
        }

        /// <summary>
        /// Returns the description of the loco list system "Rocrail".
        /// </summary>
        /// <returns>Returns the product name Rocrail.</returns>
        public static string GetSystemNameRocrail(string culture)
        {
            if(culture == "") return AppResources.FrameSettingsAppLocoListRocrail;
            CultureInfo cultureInfo = new CultureInfo(culture);
            string? systemName =  AppResources.ResourceManager.GetString("FrameSettingsAppLocoListRocrail", cultureInfo);
            if (systemName != null) return systemName;
            return "";
        }

        /// <summary>
        /// Converts the loco list system name to english.
        /// </summary>
        /// <param name="selectedLocoListSystem">The name of the locolist system.</param>
        /// <returns></returns>
        public static string ConvertLocoListSystem2English(string selectedLocoListSystem)
        {
            if (selectedLocoListSystem == GetSystemNameNotAvailable("de-DE")) return GetSystemNameNotAvailable("en-US");
            return selectedLocoListSystem;
        }

        /// <summary>
        /// Converts the loco list system name to the app language.
        /// </summary>
        /// <param name="locoListSystem">The name of the locolist system.</param>
        /// <returns></returns>
        public static string ConvertLocoListSystem2AppLanguage(string locoListSystem)
        {            
            if (locoListSystem == GetSystemNameNotAvailable("en-US")) return GetSystemNameNotAvailable("");
            if (locoListSystem == GetSystemNameNotAvailable("de-DE")) return GetSystemNameNotAvailable("");
               
            if(locoListSystem == GetSystemNameRocrail("en-US")) return GetSystemNameRocrail("");
            if(locoListSystem == GetSystemNameRocrail("de-DE")) return GetSystemNameRocrail("");

            return locoListSystem;
        }

        /// <summary>
        /// Returns the locomotive list of the currently selected locomotive
        /// list system (Rocrail, File system etc.).
        /// </summary>
        /// <returns></returns>
        public async static Task<List<LocoListType>> GetLocomotiveList()
        {
            //  Note:
            //  We disable the undo/redo manager while we grab the locomotive list, because otherwise we might get into troubles with the undo/redo
            //  information if we change the locomotive list while the user is doing an undo/redo action.
            UndoRedoManager.Enabled = false;

            List<LocoListType> locomotiveList = new List<LocoListType>();
            if (IsSystemRocRail(ActiveSystem) == true)
            {
                locomotiveList =  await Task.Run(() => GetLocomotiveListRocrail());
            }
            else
            {
                return await Task.Run(() => GetLocomotiveFileSystem());
            }

            UndoRedoManager.Enabled = true;

            return locomotiveList;  
        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS        

        /// <summary>
        /// Returns the locomotive list from the train controller software Rocrail.
        /// </summary>
        /// <returns></returns>
        private async static Task<List<LocoListType>> GetLocomotiveListRocrail()
        {
            List<LocoListType> locomotiveList = new List<LocoListType>();

            //  Grab the locomotive list from Rocrail.
            locomotiveList = await Task.Run(() =>  Rocrail.GetLocomotiveList(IPAddress, PortNumber));

            //  Add the path to the local Z2X file
            string[] fileEntries = Directory.GetFiles(Z2XFileFolder);
            foreach (LocoListType loco in locomotiveList)
            {
                foreach (string fileName in fileEntries)
                {
                    using (Stream fs = File.OpenRead(fileName))
                    {   
                        Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();

                        var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

                        // Call the Deserialize method and cast to the object type.
                        myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(fs)!;

                        if(myFile.LocomotiveAddress == loco.LocomotiveAddress)
                        {
                            loco.Z2XFileAvailable = true;
                            loco.FilePath = fileName;
                            loco.UserDefindedImage =  Base64StringToImage.ConvertBase64String2ImageSource(myFile.UserDefinedImage);
                        }
                    }   
                }
            }
            return locomotiveList;
        }

        /// <summary>
        /// Returns the locomotive list based on the Z2X files stored in the file system.
        /// </summary>
        /// <returns></returns>
        private static Task<List<LocoListType>> GetLocomotiveFileSystem()
        {
            List<LocoListType> locomotiveList = new List<LocoListType>();

            //  Loop through all files in the Z2X file folder.
            string[] fileEntries = Directory.GetFiles(Z2XFileFolder);
            foreach (string fileName in fileEntries)
            {
                using (Stream fs = File.OpenRead(fileName))
                {
                    Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();

                    var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

                    try
                    {
                        // Call the Deserialize method and cast to the object type.
                        myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(fs)!;

                        LocoListType entry = new LocoListType();
                        entry.LocomotiveAddress = myFile.CVs[1].Value;
                        entry.UserDefindedDecoderDescription = myFile.UserDefindedDecoderDescription;
                        entry.UserDefindedImage = Base64StringToImage.ConvertBase64String2ImageSource(myFile.UserDefinedImage);
                        entry.FilePath = fileName;
                        entry.Z2XFileAvailable = true;

                        locomotiveList.Add(entry);
                    }
                    catch (InvalidOperationException)
                    {
                        //  If we try to deserialize a wrong file format we receive a InvalidOperationException.
                        //  Do nothing - just skip this file.
                    }
                };
            }
            return Task.FromResult(locomotiveList);
         
        }
        #endregion
    }
}
