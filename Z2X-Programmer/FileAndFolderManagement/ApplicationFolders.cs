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
        /// Contains the standard path to the Z2X files folder.
        /// </summary>
        public static string Z2XFolderPath
        {   
            get => GetZ2XFolderPath();
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
