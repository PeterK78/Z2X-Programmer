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

using System.Xml.Serialization;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Model;

namespace Z2XProgrammer.FileAndFolderManagement
{

    /// <summary>
    /// This class implements the functions to read and write z2x files
    /// </summary>
    internal static class Z2XReaderWriter
    {
        /// <summary>
        /// Returns the default Z2X file name
        /// </summary>
        /// <returns></returns>
        public static string GetZ2XStandardFileName()
        {
            string fileName = "";
            if ((DecoderConfiguration.UserDefindedDecoderDescription != null) && (DecoderConfiguration.UserDefindedDecoderDescription != ""))
            {
                fileName = DecoderConfiguration.RCN225.LocomotiveAddress.ToString() + " " + DecoderConfiguration.UserDefindedDecoderDescription;
            }
            else
            {
                fileName = "Settings Adr=" + DecoderConfiguration.RCN225.LocomotiveAddress.ToString();
            }

            if (fileName.Length > 40)
            {
                fileName = fileName.Substring(0, 40);
            }

            fileName = fileName.Trim();
            fileName = fileName + ".z2x";
            
            return fileName;
        }

        /// <summary>
        /// Creates an Z2X-Programmer file compatible XML memory stream with the current content
        /// of the data store. Used to save the content of the datastore to the file system.
        /// </summary>
        /// <returns></returns>
        public static Z2XProgrammerFileType CreateZ2XProgrammerFile()
        {
            //  
            // Setup the Z2X file content object
            //
            Z2XProgrammerFileType myZ2XFileContent = new Z2XProgrammerFileType();
            myZ2XFileContent.LocomotiveAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
            myZ2XFileContent.UserDefindedDecoderDescription = DecoderConfiguration.UserDefindedDecoderDescription;             
            myZ2XFileContent.CVs = DecoderConfiguration.ConfigurationVariables.ToList();
            myZ2XFileContent.DeqSpecName = DecoderSpecification.DeqSpecName;
            myZ2XFileContent.UserDefinedNotes = DecoderConfiguration.UserDefindedNotes;
            myZ2XFileContent.UserDefinedImage = DecoderConfiguration.UserDefindedImage;

            return myZ2XFileContent;
        }


        /// <summary>
        /// Reads the given file stream from a Z2X file and updates the data store.
        /// </summary>
        /// <param name="z2xFileStream">A file stream of a Z2X file.</param>
        /// <returns></returns>
        public static bool ReadFile(Stream z2xFileStream)
        {
            if (z2xFileStream == null) return false;

            Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();

            var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

            // Call the Deserialize method and cast to the object type.
            myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(z2xFileStream)!;

            DecoderConfiguration.ClearBackupCVs();
            
            for (int n = 0; n < DecoderConfiguration.ConfigurationVariables.Count; n++)
            {
                DecoderConfiguration.ConfigurationVariables[n].Value = myFile.CVs[n].Value;
                DecoderConfiguration.ConfigurationVariables[n].Number = myFile.CVs[n].Number;
                DecoderConfiguration.ConfigurationVariables[n].Description = myFile.CVs[n].Description;
                DecoderConfiguration.ConfigurationVariables[n].Enabled = myFile.CVs[n].Enabled;


                DecoderConfiguration.BackupCVs[n].Value = myFile.CVs[n].Value;
                DecoderConfiguration.BackupCVs[n].Number = myFile.CVs[n].Number;
                DecoderConfiguration.BackupCVs[n].Description = myFile.CVs[n].Description;
                DecoderConfiguration.BackupCVs[n].Enabled = myFile.CVs[n].Enabled;
            }

            DecoderConfiguration.UserDefindedDecoderDescription = myFile.UserDefindedDecoderDescription;
            DecoderConfiguration.UserDefindedNotes = myFile.UserDefinedNotes;
            DecoderConfiguration.UserDefindedImage = myFile.UserDefinedImage;

            //  Check if the decoder specification file is available.
            if (DeqSpecReader.IsDecoderSpecificationAvailable(myFile.DeqSpecName) == false) throw new FileNotFoundException("Missing decoder specification file " + myFile.DeqSpecName);

            //  Make sure to use the language specific decoder specification name.
            string decoderSpecificationFileName = DeqSpecReader.GetDecSpecFileName(myFile.DeqSpecName, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath);
            DecoderSpecification.DeqSpecName = DeqSpecReader.GetDecSpecName(decoderSpecificationFileName, Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT));
            
            return true;
        }



    }
}
