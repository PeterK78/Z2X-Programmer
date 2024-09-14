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
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// This class contains internal constants
    /// </summary>
    internal static class AppConstants
    {
        /// <summary>
        /// The programming modes. Direct or POM.       
        /// </summary>
        internal const string AppName = "Z2X Programmer";
        internal const string FileDialogFilter = "z2x files (*.z2x)|*.z2x|All files (*.*)|*.*";

        //  The file name of the decoder specification ZIP archive.
        //  Note: .NET MAUI is critical on using file types. Do not use ZIP - it will break the call OpenAppPackageFileAsync
        internal const string DecSpecsZIPArchiveFileName = "decspecs.bin";

        internal const string PREFERENCES_LOCOLIST_FOLDER_KEY = "LOCOLIST_FOLDER";
        internal const string PREFERENCES_LOCOLIST_FOLDER_VALUE = "";
        internal const string PREFERENCES_LOCOLIST_PORTNR_KEY = "LOCOLIST_PORTNR";
        internal const string PREFERENCES_LOCOLIST_PORTNR_VALUE = "8080";
        internal const string PREFERENCES_LOCOLIST_IPADDRESS_KEY = "LOCOLIST_IPADRESS";
        internal const string PREFERENCES_LOCOLIST_IPADDRESS_VALUE = "192.168.0.0";
        internal const string PREFERNECES_LOCOLIST_SYSTEM_KEY = "LOCOLIST_SYSTEM";
        internal const string PREFERNECES_LOCOLIST_SYSTEM_VALUE = "";
        internal const string PREFERENCES_LANGUAGE_AUTOCONFIGURE_DONE_KEY = "LANGUAGE_AUTOCONFIGURE_DONE";
        internal const string PREFERENCES_LANGUAGE_AUTOCONFIGURE_DONE_VALUE = "0";
        internal const string PREFERENCES_COMMANDSTATIONIP_KEY = "COMMAND_STATION_IP_KEY";
        internal const string PREFERENCES_COMMANDSTATIONIP_DEFAULT = "192.168.0.111";
        internal const string PREFERENCES_PROGRAMMINGMODE_KEY = "PROGRAMMINGMODE";
        internal const string PREFERENCES_PROGRAMMINGMODE_DEFAULT = "0";
        internal const string PREFERENCES_LOCOMOTIVEADDRESS_KEY = "PREFERENCES_LOCOMOTIVE_ADDRESS";
        internal const string PREFERENCES_LOCOMOTIVEADDRESS_DEFAULT = "3";
        internal const string PREFERENCES_AUTODECODER_DETECT_KEY = "AUTODECODERDETECT";
        internal const string PREFERENCES_AUTODECODER_DETECT_DEFAULT = "1";
        internal const string PREFERENCES_LOGGING_KEY = "PREFERENCES_LOGGING_KEY";
        internal const string PREFERENCES_LOGGING_DEFAULT = "0";
        internal const string PREFERENCES_LICENSE_KEY = "PREFERENCES_LICENSE_KEY";
        internal const string PREFERENCES_LICENSE_DEFAULT = "0";
        internal const string PREFERENCES_WINDOWWIDTH_KEY = "WINDOWWIDTH";
        internal const string PREFERENCES_WINDOWWIDTH_DEFAULT = "1024";
        internal const string PREFERENCES_WINDOWHEIGHT_KEY = "WINDOWHEIGHT";
        internal const string PREFERENCES_WINDOWHEIGHT_DEFAULT = "600";
        internal const string PREFERENCES_WINDOWPOSX_KEY = "WINDOWPOSX";
        internal const string PREFERENCES_WINDOWPOSX_DEFAULT = "-1";
        internal const string PREFERENCES_WINDOWPOSY_KEY = "WINDOWPOSY";
        internal const string PREFERENCES_WINDOWPOSY_DEFAULT = "-1";
        internal const string PREFERENCES_LANGUAGE_KEY = "PREFERENCES_LANGUAGE_KEY";
        internal const string PREFERENCES_LANGUAGE_KEY_DEFAULT = PREFERENCES_LANGUAGE_KEY_GERMAN;
        internal const string PREFERENCES_LANGUAGE_KEY_GERMAN = "GERMAN";
        internal const string PREFERENCES_LANGUAGE_KEY_ENGLISH = "ENGLISH";

    }
}
