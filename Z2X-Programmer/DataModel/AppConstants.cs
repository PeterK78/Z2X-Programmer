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
        //internal const string DecSpecsZIPArchiveFileName = "decspecs.bin";

        internal const string PREFERENCES_LOCOLIST_FOLDER_KEY = "LOCOLIST_FOLDER";
        internal const string PREFERENCES_LOCOLIST_FOLDER_VALUE = "";
        internal const string PREFERENCES_LOCOLIST_PORTNR_KEY = "LOCOLIST_PORTNR";
        internal const string PREFERENCES_LOCOLIST_PORTNR_VALUE = "8051";
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
        internal const string PREFERENCES_AUTODECODER_DETECT_KEY = "AUTODECODERDETECT";
        internal const string PREFERENCES_AUTODECODER_DETECT_DEFAULT = "1";
        internal const string PREFERENCES_LOGGING_KEY = "PREFERENCES_LOGGING_KEY";
        internal const string PREFERENCES_LOGGING_DEFAULT = "0";
        internal const string PREFERENCES_LICENSE_KEY = "PREFERENCES_LICENSE_KEY";
        internal const string PREFERENCES_LICENSE_DEFAULT = "0";
        internal const string PREFERENCES_LANGUAGE_KEY = "PREFERENCES_LANGUAGE_KEY";
        internal const string PREFERENCES_LANGUAGE_KEY_DEFAULT = PREFERENCES_LANGUAGE_KEY_GERMAN;
        internal const string PREFERENCES_LANGUAGE_KEY_GERMAN = "GERMAN";
        internal const string PREFERENCES_LANGUAGE_KEY_ENGLISH = "ENGLISH";
        internal const string PREFERENCES_QUITONREADERROR_KEY = "QUITONREADERROR";
        internal const string PREFERENCES_QUITONREADERROR_VALUE = "1";
        internal const string PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY = "ADDITIONALDISPLAYOFCVVALUES";
        internal const string PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE = "0";
        internal const string PREFERENCES_USERSPECIFICDECSPECFOLDER_KEY = "USERSPECIFICDECSPECFOLDER";
        internal const string PREFERENCES_USERSPECIFICDECSPECFOLDER_VALUE = "";

        //  Size and position of the main window.
        internal const string PREFERENCES_WINDOW_MAIN_WIDTH_KEY = "MAINWINDOWWIDTH";
        internal const string PREFERENCES_WINDOW_MAINWIDTH_DEFAULT = "1024";
        internal const string PREFERENCES_WINDOW_MAIN_HEIGHT_KEY = "MAINWINDOWHEIGHT";
        internal const string PREFERENCES_WINDOW_MAIN_HEIGHT_DEFAULT = "600";
        internal const string PREFERENCES_WINDOW_MAIN_POSX_KEY = "MAINWINDOWPOSX";
        internal const string PREFERENCES_WINDOW_MAIN_POSX_DEFAULT = "-1";
        internal const string PREFERENCES_WINDOW_MAIN_POSY_KEY = "MAINWINDOWPOSY";
        internal const string PREFERENCES_WINDOW_MAIN_POSY_DEFAULT = "-1";

        //  Size and position of the controller window.
        internal const string PREFERENCES_WINDOW_CONTROLLER_WIDTH_KEY = "CONTROLLERWINDOWWIDTH";
        internal const string PREFERENCES_WINDOW_CONTROLLER_WIDTH_DEFAULT = "480";
        internal const string PREFERENCES_WINDOW_CONTROLLER_HEIGHT_KEY = "CONTROLLERWINDOWHEIGHT";
        internal const string PREFERENCES_WINDOW_CONTROLLER_HEIGHT_DEFAULT = "750";
        internal const string PREFERENCES_WINDOW_CONTROLLER_POSX_KEY = "CONTROLLERWINDOWPOSX";
        internal const string PREFERENCES_WINDOW_CONTROLLER_POSX_DEFAULT = "-1";
        internal const string PREFERENCES_WINDOW_CONTROLLER_POSY_KEY = "CONTROLLERWINDOWPOSY";
        internal const string PREFERENCES_WINDOW_CONTROLLER_POSY_DEFAULT = "-1";

        // Measurement section
        internal const string PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_KEY = "SENSOR1NR";
        internal const string PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_DEFAULT = "0";
        internal const string PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_KEY = "SENSOR2NR";
        internal const string PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_DEFAULT = "0";
        internal const string PREFERENCES_MEASUREMENTSECTION_LENGTHMM_KEY = "LENGTHMM";
        internal const string PREFERENCES_MEASUREMENTSECTION_LENGTHMM_DEFAULT = "100";
        internal const string PREFERENCES_MEASUREMENTSECTION_SCALE_KEY = "SCALE";
        internal const string PREFERENCES_MEASUREMENTSECTION_SCALE_DEFAULT = "160";



    }
}
