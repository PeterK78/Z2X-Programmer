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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Model;
using Z2XProgrammer.Resources.Strings;
using Z2XProgrammer.Traincontroller;

namespace Z2XProgrammer.Helper
{
    public static class LocoList
    {
        #region REGION: PUBLIC PROPERTIES

        public static string ActiveSystem
        {
            get
            {
                return Preferences.Default.Get(AppConstants.PREFERNECES_LOCOLIST_SYSTEM_KEY, AppConstants.PREFERNECES_LOCOLIST_SYSTEM_VALUE);
            }
        }

        public static int PortNumber
        { get
          {
                return int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_PORTNR_KEY, AppConstants.PREFERENCES_LOCOLIST_PORTNR_VALUE));
          }
        }

        public static IPAddress IPAddress
        {
            get
            {
                string ipAddress = Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_IPADDRESS_KEY, AppConstants.PREFERENCES_LOCOLIST_IPADDRESS_VALUE);
                return IPAddress.Parse(ipAddress);
            }
        }

        public static string Folder
        {
            get
            {
                return Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_FOLDER_KEY, AppConstants.PREFERENCES_LOCOLIST_FOLDER_VALUE);
            }
        }
        #endregion

        #region REGION: PUBLIC FUNCTIONS

        /// <summary>
        /// Returns a list with descriptions of available languages.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableSystems()
        {
            List<string> languages = new List<string>();
            languages.Add(GetSystemNotAvailable());
            languages.Add(GetSystemRocrailDescription());
            return languages;
        }

        /// <summary>
        /// Returns TRUE if the description describes the Rocrail train controller software.
        /// </summary>
        /// <param name="trainControllerSoftware">The train controller system description.</param>
        /// <returns></returns>
        public static bool IsRocrail(string trainControllerSoftware)
        {
            if (trainControllerSoftware == AppResources.FrameSettingsAppLocoListRocrail) return true;
            return false;
        }
        
        public static string GetSystemNotAvailable()
        {
            return AppResources.FrameSettingsAppLocoListNoTrainControllerNotAvailable;
        }

        public static string GetSystemRocrailDescription()
        {
            return AppResources.FrameSettingsAppLocoListRocrail;
        }

        /// <summary>
        /// Returns the locomotive list of the currently selected locomotive
        /// list system (Rocrail, File system etc.).
        /// </summary>
        /// <returns></returns>
        public async static Task<List<LocoListType>> GetLocomotiveList()
        {
            List<LocoListType> locomotiveList = new List<LocoListType>();
            if (IsRocrail(ActiveSystem) == true)
            {
                IPAddress address = IPAddress.Parse("192.168.178.49");
                locomotiveList =  await Task.Run(() => GetLocomotiveListRocrail());
            }
            else
            {
                return await Task.Run(() => GetLocomotiveFileSystem());
            }
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

            //  Grab the locomotive list from Rocrail
            locomotiveList = await Task.Run(() =>  Rocrail.GetLocomotiveList(IPAddress, PortNumber));

            //  Add the path to the local Z2X file
            string[] fileEntries = Directory.GetFiles(Folder);
            foreach (LocoListType loco in locomotiveList)
            {
                foreach (string fileName in fileEntries)
                {
                    Stream fs = File.OpenRead(fileName);

                    Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();

                    var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

                    // Call the Deserialize method and cast to the object type.
                    myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(fs)!;

                    if(myFile.LocomotiveAddress == loco.LocomotiveAddress)
                    {
                        loco.FilePath = fileName;
                    }
                }
            }
            return locomotiveList;
        }


        /// <summary>
        /// Returns the locomotive list from Z2X files.
        /// </summary>
        /// <returns></returns>
        private  static Task<List<LocoListType>> GetLocomotiveFileSystem()
        {

            List<LocoListType> locomotiveList = new List<LocoListType>();
            string[] fileEntries = Directory.GetFiles(Folder);

            foreach (string fileName in fileEntries)
            {
                Stream fs = File.OpenRead(fileName);

                Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();

                var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

                // Call the Deserialize method and cast to the object type.
                myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(fs)!;

                LocoListType entry = new LocoListType();
                entry.LocomotiveAddress = myFile.CVs[1].Value;
                entry.UserDefindedDecoderDescription = myFile.UserDefindedDecoderDescription;
                entry.FilePath = fileName;

                locomotiveList.Add(entry);
            }
            return Task.FromResult(locomotiveList);
         
        }

        #endregion

    }
}
