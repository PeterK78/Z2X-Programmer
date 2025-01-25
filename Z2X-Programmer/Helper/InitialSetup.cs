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

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Runtime.Intrinsics.X86;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.FileAndFolderManagement;

namespace Z2XProgrammer.Helper
{
    
    /// <summary>
    /// This class implements functions to do the first setup of the application
    /// </summary>
    public static class InitialSetup
    {

        /// <summary>
        /// Creates the DeqSpecs folder and extracts all available decoder specification files whitin decspecs.bin to this folder.
        /// </summary>
        /// <returns></returns>
        public static void DoFirstSetup()
        {
            //  Make sure that the folder DeqSpecs is exisiting in the app data folder. If the folder does not
            //  exist, create it.
            Logger.LogInformation("Setup decoder specification folder ...");
            
            //  Setup the folder for the decoder specification files.
            SetupDeqSpecFolders();

            //  Setup the folder for the Z2X files
            SetupZ2XFolder();
            

            //  Copy the decoder specification files
            DeqSpecReader.WriteDeqSpecFile("Generic.decspec", DeqSpecReader.UnknownDecoderSpec);
            DeqSpecReader.WriteDeqSpecFile("RCN225.decspec", DeqSpecReader.RCN225Spec);
            DeqSpecReader.WriteDeqSpecFile("ZIMO-MX-loc.decspec", DeqSpecReader.ZimoMXLocomotiveSpec);
            DeqSpecReader.WriteDeqSpecFile("ZIMO-MS-loc.decspec", DeqSpecReader.ZimoMSLocomotiveSpec);
            DeqSpecReader.WriteDeqSpecFile("ZIMO-MX-fx.decspec", DeqSpecReader.ZimoFXFunctionSpec);
            DeqSpecReader.WriteDeqSpecFile("ZIMO-MN-loc.decspec", DeqSpecReader.ZimoMNLocomotiveSpec);

            //  Automatically setup the the GUI language if we did not before ...
            if (Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_AUTOCONFIGURE_DONE_KEY, AppConstants.PREFERENCES_LANGUAGE_AUTOCONFIGURE_DONE_VALUE) != "1")
            {
                AppCulture.SetApplicationLanguageByKey(AppCulture.GetLanguageKeyUsedByOS());
                Preferences.Default.Set(AppConstants.PREFERENCES_LANGUAGE_KEY, AppCulture.GetLanguageKeyUsedByOS());
                Preferences.Default.Set(AppConstants.PREFERENCES_LANGUAGE_AUTOCONFIGURE_DONE_KEY, "1");
            }
            else
            {
                AppCulture.SetApplicationLanguageByKey(Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT));
            }

        }

        /// <summary>
        /// Creates the default decoder specification files folder within the AppData folder.
        /// </summary>
        /// <returns></returns>
        private static bool SetupDeqSpecFolders()
        {
            //  At first we check if the internal decoder specification folder. If not, we create it.
            if (Directory.Exists(ApplicationFolders.DecSpecsFolderPath) == false)
            {
                Directory.CreateDirectory(ApplicationFolders.DecSpecsFolderPath);
            }

            //  Now we check if the default user specific decoder specification folder is available. If not, we create it.   
            if (Directory.Exists(ApplicationFolders.DefaultUserSpecificDecSpecsFolderPath)  == false)
            {
                Directory.CreateDirectory(ApplicationFolders.DefaultUserSpecificDecSpecsFolderPath);
            }

            return true;
        }

        /// <summary>
        /// Creates the folder for the Z2X files within the AppData folder.
        /// </summary>
        /// <returns></returns>
        private static bool SetupZ2XFolder()
        {
            //  We check whether the directory for the Z2X files has already been configured.
            //  If not, we initialize the Z2X default folder.  
            if(Preferences.Default.Get(AppConstants.PREFERENCES_LOCOLIST_FOLDER_KEY, AppConstants.PREFERENCES_LOCOLIST_FOLDER_VALUE) == AppConstants.PREFERENCES_LOCOLIST_FOLDER_VALUE)
            { 
                Directory.CreateDirectory(ApplicationFolders.Z2XFolderPath);
                Preferences.Default.Set(AppConstants.PREFERENCES_LOCOLIST_FOLDER_KEY, ApplicationFolders.Z2XFolderPath);
            }
            return true;
        }
            
        /// <summary>
        /// Extracts the content of the given file to the folder of the decoder specification files
        /// </summary>
        /// <param name="filename">The file name of the decoder specification ZIP archive</param>
        /// <returns></returns>
        private static void ExtractDecSpecsArchive(string filename)
        {
            string zipArchiveFilePath = Path.Combine(ApplicationFolders.DecSpecsFolderPath, filename);

            ZipFile.ExtractToDirectory(zipArchiveFilePath, ApplicationFolders.DecSpecsFolderPath, true);

            File.Delete (zipArchiveFilePath);   
        }


        /// <summary>
        /// Copies an bundled RAW resource file to the decoder specification folder 
        /// </summary>
        /// <param name="filename">The file name of the RAW resource file (e. g. the decoder specification ZIP archive)</param>
        /// <returns></returns>
        private static async Task CopyFileToAppDataDirectory(string filename)
        {
            Logger.LogInformation("CopyFileToAppDataDirectory started ...");
            try
            {


                //  Check if the defined resource file exists in the setup bundle
                if (await FileSystem.Current.AppPackageFileExistsAsync(filename) == false) return;

                // Open the raw asset file
                Logger.LogInformation("OpenAppPackageFileAsync " + filename);
                using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

                //  Assemble the file path to the ZIP archive
                string targetFile = Path.Combine(ApplicationFolders.DecSpecsFolderPath, filename);

                //  Remove an already existing asset file in the decspecs folder
                if (File.Exists(targetFile)) { File.Delete(targetFile); }

                // Finally copy the file to the decoder specification folder
                Logger.LogInformation("Create file ");
                using FileStream outputStream = File.Create(targetFile);

                //  Note:
                // inputStream.CopyToAsync fails on Android. Use inputStream.CopyTo instead.
                Logger.LogInformation("Copy file ");
                inputStream.CopyTo(outputStream);
                //await inputStream.CopyToAsync(outputStream);               
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex.Message);
            }
        }
    }
 
}
