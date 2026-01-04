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

using Z2XProgrammer.DataModel;

namespace Z2XProgrammer.FileAndFolderManagement
{
    /// <summary>
    /// This class implements different functions for file and folder management.
    /// </summary>
    public static class ApplicationFolders
    {
        #region REGION: PUBLIC PROPERTIES

        /// <summary>
        /// Contains the path to the decoder specification files.
        /// </summary>
        public static string DecSpecsFolderPath
        {
            get => GetDecSpecsFolderPath();
        }

        /// <summary>
        /// Contains the path to the user specific decoder specification files.
        /// </summary>
        public static string UserSpecificDecSpecsFolderPath
        {
            get => GetUserSpecificDecSpecsFolderPath();
        }

        /// <summary>
        /// Contains the default path to the user specific decoder specification files.
        /// </summary>
        public static string DefaultUserSpecificDecSpecsFolderPath
        {
            get => GetDefaultUserSpecificDecSpecsFolderPath();
        }

        /// <summary>
        /// Contains the standard path to the Z2X files folder.
        /// </summary>
        public static string Z2XFolderPath
        {   
            get => GetZ2XFolderPath();
        }

        /// <summary>
        /// Contains the standard path to manufacturer database folder.
        /// </summary>
        public static string ManufacturerDBFolderPath
        {
            get => GetManufacturerDBFolderPath();
        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Returns the path to the DecSpeqs folder. The folder is located in a subfolder
        /// "DeqSpecs" in the app data folder.
        /// </summary>
        /// <returns>The path to the DecSpeqs folder</returns>
        private static string GetDecSpecsFolderPath()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
                    return FileSystem.Current.AppDataDirectory + "\\DeqSpecs";
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    return FileSystem.Current.AppDataDirectory + "/DeqSpecs";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the path to the user specific DecSpeqs folder.
        /// </summary>
        /// <returns>The path to the user specific DecSpeqs folder</returns>
        private static string GetUserSpecificDecSpecsFolderPath()
        {
            try
            {
                //  Check if the user specific decoder specification folder is defined in the preferences
                string userSpecificDecSpecsFolderPath = Preferences.Default.Get(AppConstants.PREFERENCES_USERSPECIFICDECSPECFOLDER_KEY, AppConstants.PREFERENCES_USERSPECIFICDECSPECFOLDER_VALUE);

                //  If the user specific decoder specification folder is available, we return the path.
                if (Directory.Exists(userSpecificDecSpecsFolderPath) == true)
                {
                    return userSpecificDecSpecsFolderPath;
                }

                //  If the user specific decoder specification folder is not available, we return default path.
                return GetDefaultUserSpecificDecSpecsFolderPath();
                
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the default path to the user specific DecSpeqs folder.
        /// The folder is located in a subfolder "DeqSpecs" in the app data folder.
        /// </summary>
        /// <returns>The path to the default user specific decoder specification folder</returns>
        private static string GetDefaultUserSpecificDecSpecsFolderPath()
        {
            return GetDecSpecsFolderPath() + "\\UserSpecific";
        }

        /// <summary>
        /// Returns the path to the NMRA manufacturer folder.
        /// </summary>
        /// <returns></returns>
        private static string GetManufacturerDBFolderPath()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
                    return FileSystem.Current.AppDataDirectory + "\\ManufacturesDB";
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    return FileSystem.Current.AppDataDirectory + "/ManufacturesDB";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the path to the Z2X-Programmer standard Z2X folder.
        ///     
        /// Note: This is the standard Z2X folder. The folder can be modified by the user in the Z2X-Programmer settings.        
        /// 
        /// </summary>
        /// <returns>The path to the standard Z2X folder.</returns>
        private static string GetZ2XFolderPath()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
                    return FileSystem.Current.AppDataDirectory + "\\Z2X";
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    return FileSystem.Current.AppDataDirectory + "/Z2X";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }



        #endregion
    }
}
