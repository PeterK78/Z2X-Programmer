/*
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

using CommunityToolkit.Maui.Converters;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.FileAndFolderManagement
{

   /// <summary>
   /// This class contains the implementation of the decoder specification file reader.
   /// </summary>
    public static class DeqSpecReader
    {
      
        //
        //  RCN225 standard features
        //  
        public const string RCN225_BASEADDRESS_CV1 = "RCN225_BASEADDRESS_CV1";
        public const string RCN225_MINIMALSPEED_CV2 = "RCN225_MINIMALSPEED_CV2";
        public const string RCN225_ACCELERATIONFACTOR_CV3 = "RCN225_ACCELERATIONFACTOR_CV3";
        public const string RCN225_DECELERATIONFACTOR_CV4 = "RCN225_DECELERATIONFACTOR_CV4";
        public const string RCN225_MAXIMALSPEED_CV5 = "RCN225_MAXIMALSPEED_CV5";
        public const string RCN225_MEDIUMSPEED_CV6 = "RCN225_MEDIUMSPEED_CV6";
        public const string RCN225_DECODERVERSION_CV7 = "RCN225_DECODERVERSION_CV7";
        public const string RCN225_MANUFACTUERID_CV8 = "RCN225_MANUFACTUERID_CV8";
        public const string RCN225_LONGSHORTADDRESS_CV29_5 = "RCN225_LONGSHORTADDRESS_CV29_5";
        public const string RCN225_DECODERLOCK_CV15X = "RCN225_DECODERLOCK_CV15X";
        public const string RCN225_RAILCOMENABLED_CV29_3 = "RCN225_RAILCOMENABLED_CV29_3";
        public const string RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 = "RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0";
        public const string RCN225_RAILCOMCHANNEL2DATA_CV28_1 = "RCN225_RAILCOMCHANNEL2DATA_CV28_1";
        public const string RCN225_DIRECTION_CV29_0 = "RCN225_DIRECTION_CV29_0";
        public const string RCN225_SPEEDSTEPS_CV29_1 = "RCN225_SPEEDSTEPS_CV29_1";
        public const string RCN225_SPEEDTABLE_CV29_4 = "RCN225_SPEEDTABLE_CV29_4";
        public const string RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X = "RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X";
        public const string RCN225_ANALOGMODE_CV29_2 = "RCN225_ANALOGMODE_CV29_2";
        public const string RCN225_DECODERRESET_CV8 = "RCN225_DECODERRESET_CV8";
        public const string RCN225_FUNCTIONOUTPUTMAPPING_CV3346 = "RCN225_FUNCTIONOUTPUTMAPPING_CV3346";
        public const string RCN225_CONSISTADDRESS_CV19X = "RCN225_CONSISTADDRESS_CV19X";        
        public const string RCN225_AUTOMATICREGISTRATION_CV28_7 = "RCN225_AUTOMATICREGISTRATION_CV28_7";
        public const string RCN225_HLU_CV27_2 = "RCN225_HLU_CV27_2";
        public const string RCN225_ABC_CV27_X = "RCN225_ABC_CV27_X";
        public const string RCN225_OPERATINGMODES_CV12 = "RCN225_OPERATINGMODES_CV12";

        //
        //  ZIMO specific features.   
        //
        public const string ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 = "ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61";
        public const string ZIMO_LIGHT_EFFECTS_CV125X = "ZIMO_LIGHT_EFFECTS_CV125X";
        public const string ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 = "ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396";
        public const string ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 = "ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397";
        public const string ZIMO_FUNCKEY_SOUNDALLOFF_CV310 = "ZIMO_FUNCKEY_SOUNDALLOFF_CV310";
        public const string ZIMO_SELFTEST_CV30 = "ZIMO_SELFTEST_CV30";
        public const string ZIMO_FUNCKEY_CURVESQUEAL_CV308 = "ZIMO_FUNCKEY_CURVESQUEAL_CV308";
        public const string ZIMO_SUBVERSIONNR_CV65 = "ZIMO_SUBVERSIONNR_CV65";
        public const string ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 = "ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156";
        public const string ZIMO_DECODERTYPE_CV250 = "ZIMO_DECODERTYPE_CV250";
        public const string ZIMO_DECODERID_CV25X = "ZIMO_DECODERID_CV25X";
        public const string ZIMO_LIGHT_DIM_CV60 = "ZIMO_LIGHT_DIM_CV60";
        public const string ZIMO_BOOTLOADER_VERSION_24X = "ZIMO_BOOTLOADER_VERSION_24X";
        public const string ZIMO_MXMOTORCONTROLFREQUENCY_CV9 = "ZIMO_MXMOTORCONTROLFREQUENCY_CV9";
        public const string ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 = "ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57";
        public const string ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 = "ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57";
        public const string ZIMO_MXUPDATELOCK_CV144 = "ZIMO_MXUPDATELOCK_CV144";
        public const string ZIMO_MXFX_SECONDADDRESS_CV64 = "ZIMO_MXFX_SECONDADDRESS_CV64";
        public const string ZIMO_MXMOTORCONTROLPID_CV56 = "ZIMO_MXMOTORCONTROLPID_CV56";
        public const string ZIMO_BRAKESQUEAL_CV287 = "ZIMO_BRAKESQUEAL_CV287";
        public const string ZIMO_SOUND_VOLUME_GENERIC_C266 = "ZIMO_SOUND_VOLUME_GENERIC_C266";
        public const string ZIMO_SOUND_VOLUME_STEAM_CV27X = "ZIMO_SOUND_VOLUME_STEAM_CV27X";
        public const string ZIMO_SOUND_VOLUME_DIESELELEC_CV29X = "ZIMO_SOUND_VOLUME_DIESELELEC_CV29X";
        public const string ZIMO_FUNCKEY_MUTE_CV313 = "ZIMO_FUNCKEY_MUTE_CV313";
        public const string ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X = "ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X";
        public const string ZIMO_SOUND_STARTUPDELAY_CV273 = "ZIMO_SOUND_STARTUPDELAY_CV273";
        public const string ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 = "ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285";
        public const string ZIMO_MSOPERATINGMODES_CV12 = "ZIMO_MSOPERATINGMODES_CV12";
        public const string ZIMO_SOUNDPROJECTNR_CV254 = "ZIMO_SOUNDPROJECTNR_CV254";
        public const string ZIMO_SUSIPORT1CONFIG_CV201 = "ZIMO_SUSIPORT1CONFIG_CV201";
        public const string ZIMO_INPUTMAPPING_CV4XX = "ZIMO_INPUTMAPPING_CV4XX";
        public const string ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X = "ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X";
        public const string ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X = "ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X";
        public const string ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X = "ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X";
        public const string ZIMO_MXBRIGHTENINGUPANDIMMINGTIMES_CV190X = "ZIMO_MXBRIGHTENINGUPANDIMMINGTIMES_CV190X";
        public const string ZIMO_FUNCKEY_SHUNTINGKEY_CV155 = "ZIMO_FUNCKEY_SHUNTINGKEY_CV155";
        

        //  Döhler & Haass specific features
        public const string DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 = "DOEHLERHAAS_MOTORIMPULSWIDTH_CV49";
        public const string DOEHLERANDHAAS_DECODERTYPE_CV261 = "DOEHLERANDHAAS_DECODERTYPE_CV261";
        public const string DOEHLERANDHAAS_FIRMWAREVERSION_CV262x = "DOEHLERANDHAAS_FIRMWAREVERSION_CV262x";
        public const string DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137 = "DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137";
        public const string DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 = "DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133";
        public const string DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 = "DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132";
        public const string DOEHLERANDHAASS_MAXIMALSPEED_CV5 = "DOEHLERANDHAASS_MAXIMALSPEED_CV5";

        private const string UNKNOWN_DECDODER_EN = "Unknown decoder";
        private const string UNKNOWN_DECDODER_DE = "Unbekannter Decoder";


        /// <summary>
        /// Returns TRUE if a decoder specification with the given name is available. It also returns the name of the decoder specification
        /// converted in the given language.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification file (language independent).</param>
        /// <returns>TRUE if the decoder specification file is available. FALSE if the decoder specification file is missing.</returns>
        public static bool IsDecoderSpecificationAvailable (string decSpecName)    
        {
            try
            {
                if (GetDecSpecFileName(decSpecName, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath) != string.Empty) return true;
                if (GetDecSpecFileName(decSpecName, FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath) != string.Empty) return true;
                return false;
            }
            catch   { return false; } 
        }

        /// <summary>   
        /// Returns TRUE if the given decoder specification supports any light
        /// function keys.
        /// </summary> 
        public static bool AnyLightFunctionKeysSupported(string decSpecName)
        {
            if(decSpecName == "") return false;

            string[] soundFunctionKeysFeatures = new string[]
            {
                "ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X",
                " ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X"
            };

            //  Loop trough all function keys and check if the feature is supported
            foreach (string feature in soundFunctionKeysFeatures)
            {
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath)) return true;
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath)) return true; 
            }
            return false;
        }

        /// <summary>
        /// This function returns TRUE if the selected DecoderSpecification contains functions
        /// for mapping of function outputs
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification.</param>
        /// <returns></returns>
        public static bool AnyFunctionOuputMappingSupported (string decSpecName)
        {
            if(decSpecName == "") return false;

            string[] soundFunctionKeysFeatures = new string[]
            {
            "RCN225_FUNCTIONOUTPUTMAPPING_CV3346",
            "ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61",
            "DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137",
            };

            //  Loop trough all function keys and check if the feature is supported
            foreach (string feature in soundFunctionKeysFeatures)
            {
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath)) return true;
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath)) return true; 
            }
            return false;


        }


        /// <summary>   
        /// Returns TRUE if the given decoder specification supports any function keys
        /// to configure the drive and motor characteristics.
        /// </summary> 
        public static bool AnyDriveAndMotorCharacteristicKeysSupported(string decSpecName)
        {
             if(decSpecName == "") return false;

            string[] soundFunctionKeysFeatures = new string[]
            {
            "ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156",
            "DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133",
            "ZIMO_FUNCKEY_SHUNTINGKEY_CV155",
            "DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132",
            };

            //  Loop trough all function keys and check if the feature is supported
            foreach (string feature in soundFunctionKeysFeatures)
            {
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath)) return true;
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath)) return true; 
            }
            return false;

        }

        /// <summary>   
        /// Returns TRUE if the given decoder specification supports any sound
        /// function keys.
        /// </summary>  
        public static bool AnySoundFunctionKeysSupported(string decSpecName)
        {
            if(decSpecName == "") return false;

            string[] soundFunctionKeysFeatures = new string[]
            {
            "ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396",
            "ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397",
            "ZIMO_FUNCKEY_SOUNDALLOFF_CV310",
            "ZIMO_FUNCKEY_CURVESQUEAL_CV308",
            "ZIMO_FUNCKEY_MUTE_CV313"
            };

            //  Loop trough all function keys and check if the feature is supported
            foreach (string feature in soundFunctionKeysFeatures)
            {
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath)) return true;
                if (FeatureSupported(decSpecName, feature, FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath)) return true; 
            }
            return false;
        }


        /// <summary>
        /// Returns the decoder specification notes.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification.</param>
        /// <returns></returns>
        public static string GetDecSpecNotes(string decSpecName, string folder, string languageKey)
        {
            try
            {
                //  Check the input parameters
                if ((decSpecName == "") || (decSpecName is null)) return string.Empty;
                if ((folder == "") || (folder is null)) return string.Empty;

                //  Get the file name of the decoder specification
                string decSpecFileName = GetDecSpecFileName(decSpecName, folder);

                if (decSpecFileName == "") return string.Empty;

                XDocument xdoc = XDocument.Load(decSpecFileName);
                if (xdoc == null) return "";

                if (languageKey == AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN)
                {
                    if (xdoc.Element("decoderseries")!.Attribute("notes_de") == null) return string.Empty;
                    if (xdoc.Element("decoderseries")!.Attribute("notes_de")!.Value != null) return xdoc.Element("decoderseries")!.Attribute("notes_de")!.Value;
                }
                if (xdoc.Element("decoderseries")!.Attribute("notes_en") == null) return string.Empty;
                return xdoc.Element("decoderseries")!.Attribute("notes_en")!.Value;
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Returns the name of the RCN225 compatible decoder specification file for unknown decoders.
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultDecSpecName()
        {
            try
            {
                if (Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT) == AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN)
                {
                    return UNKNOWN_DECDODER_DE;
                }
                else
                {
                    return UNKNOWN_DECDODER_EN;
                }
            } catch { return string.Empty; }
        }

        /// <summary>
        /// Returns TRUE if the variables associated with the feature can be safely described. 
        /// </summary>
        /// <param name="decSpecName">The decoder specification name.</param>
        /// <param name="featureName">The feature name.</param>
        /// <param name="folder">The folder of the decoder specification files.</param>
        /// <returns></returns>
        public static bool IsWriteable(string decSpecName, string featureName, string folder)
        {
            //  Check the input parameters
            if ((decSpecName == "") || (decSpecName is null)) return false;
            if ((featureName == "") || (featureName is null)) return false;
            if ((folder == "") || (folder is null)) return false;

            try
            {

                //  Get the file name of the decoder specification
                string decSpecFileName = GetDecSpecFileName(decSpecName, folder);

                if (decSpecFileName == "") return false;

                //  Open the decspec XML file of the given decoder specification
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(decSpecFileName);

                //  Try to get the node with the feature name
                XmlNodeList itemList = xmlDoc.GetElementsByTagName(featureName);

                //  Check if we have found any items in the decoder specification file
                if (itemList is null) return false;

                //  Check if we have find one, single matching feature - if not return FALSE
                if (itemList.Count != 1) return false;

                //  Check if we have found any attributes
                if (itemList[0]!.Attributes is null) return false;

                //  Check if we have found the "support" attribute
                if (itemList[0]!.Attributes!["writeable"] is null) return false;

                //  Get the content of the "support" attribute 
                string support = itemList[0]!.Attributes!["writeable"]!.Value;

                if (support.ToUpper() == "YES") return true;

                return false;
            }
            catch { return false; }
        }

        /// <summary>
        /// Checks whether the format of the existing decoder specification files is correct.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="errorFileName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool CheckDecSpecsFormatValid(string folder, out string errorFileName, out string errorMessage)
        {
            errorFileName = "";
            errorMessage = "";

            try
            {
                string[] fileEntries = Directory.GetFiles(folder, "*.decspec");
                foreach (string fileName in fileEntries)
                {
                    XDocument xdoc = XDocument.Load(fileName);
                    if (xdoc == null) continue;
                }
                return true;
            }
            catch (XmlException e)
            {
                errorMessage = e.Message;
                if (e.SourceUri != null)
                {
                    errorFileName = Path.GetFileName(e.SourceUri);
                }
                else
                {
                    errorFileName = "Unknown decoder specification file.";
                }

                return false;
            }
            catch (IOException e)
            {
                errorMessage = e.Message;
                errorFileName = "Unknown decoder specification file.";
                return false;
            }
            catch (Exception e)
            {
                errorFileName = "Unknown decoder specification file.";
                errorMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// Returns a list of strings with all available decoder specification files.
        /// </summary>
        /// <param name="folder">The path to the deqspeqs folder</param>
        /// <returns></returns>
        public static List<string> GetAvailableDecSpecs(string folder)
        {
            List<string> decSpecs = new List<string>();

            try
            {
                string[] fileEntries = Directory.GetFiles(folder, "*.decspec");
                foreach (string fileName in fileEntries)
                {
                    XDocument xdoc = XDocument.Load(fileName);
                    if (xdoc == null) continue;
                    
                    if (Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT) == AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN)
                    {
                        if (xdoc.Element("decoderseries")!.Attribute("description_de")!.Value != null)
                        {
                            decSpecs.Add(xdoc.Element("decoderseries")!.Attribute("description_de")!.Value);
                        }
                    }
                    else
                    {
                        if (xdoc.Element("decoderseries")!.Attribute("description_en")!= null)
                        {
                            decSpecs.Add(xdoc.Element("decoderseries")!.Attribute("description_en")!.Value);
                        }
                    }
                }
                return decSpecs;
            }
            catch (Exception e)
            {
                string temp = e.Message;
                decSpecs.Clear();
                return decSpecs ;
            }
        }

        /// <summary>
        /// Returns the name of the give decoder specification file.
        /// </summary>
        /// <param name="fileName">The file name of the decoder specification file.</param>
        /// <param name="languageKey">The language key.</param>
        /// <returns></returns>
        public static string GetDecSpecName (string fileName, string languageKey)
        {
            if ((fileName == null) || (fileName == string.Empty)) return "";
            if ((languageKey == null) || (languageKey == string.Empty)) return "";

            try
            {

                XDocument xdoc = XDocument.Load(fileName);
                if (xdoc == null) return "";

                if (languageKey == AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN) return xdoc.Element("decoderseries")!.Attribute("description_de")!.Value;
                return xdoc.Element("decoderseries")!.Attribute("description_en")!.Value;
            }
            catch { return string.Empty; }

        }

    
        /// <summary>
        /// Returns the file name of the given decoder specification.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification (language neutral).</param>
        /// <param name="filePath">The path to the decoder specification files.</param>
        /// <returns></returns>
        public static string GetDecSpecFileName (string decSpecName, string filePath)
        {
            if (decSpecName == "") return string.Empty;
            if (filePath == "") return string.Empty;

            string decSpecNameXML = string.Empty;

            try
            {
                List<string> decSpecs = new List<string>();
                string[] fileEntries = Directory.GetFiles(filePath, "*.decspec");
                foreach (string fileName in fileEntries)
                {
                    XDocument xdoc = XDocument.Load(fileName);
                    if (xdoc == null) continue;

                    if (decSpecName == xdoc.Element("decoderseries")!.Attribute("description_de")!.Value) return fileName;
                    if (decSpecName == xdoc.Element("decoderseries")!.Attribute("description_en")!.Value) return fileName;
                    
                }
                return string.Empty;

            }
            catch 
            {
                return string.Empty;
            }    

        }

        /// <summary>
        /// Returns the name of the given decoder ID.
        /// </summary>
        /// <param name="decSpecName"></param>
        /// <param name="decoderID"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetDecoderName(string decSpecName, ushort decoderID, string folder)
        {
            //  Check the input parameters
            if ((decSpecName == "") || (decSpecName is null)) return string.Empty;

            //  Get the file name of the decoder specification
            string decSpecFileName = GetDecSpecFileName(decSpecName, folder);
            if (decSpecFileName == "") return string.Empty;

            try
            {

                //  Open the decspec XML file of the given decoder specification
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(decSpecFileName);

                //  Try to get the node with the feature name
                XmlNodeList itemList = xmlDoc.GetElementsByTagName("decoder");

                //  Check if we have found any items in the decoder specification file
                if (itemList is null) return string.Empty;

                //  Check if we have find one, single matching feature - if not return FALSE
                if (itemList.Count == 0) return string.Empty;

                //  Check if we have found any attributes
                for (int i = 0; i <= itemList.Count - 1; i++)
                {
                    if (itemList[i]!.Attributes!["decoderid"] is null) continue;

                    if (itemList[i]!.Attributes!["decoderid"]!.Value == decoderID.ToString())
                    {
                        if (itemList[i]!.Attributes!["decodername"] is null) continue;
                        return itemList[i]!.Attributes!["decodername"]!.Value;
                    }
                }

                return string.Empty;
            }
            catch { return string.Empty; }  

        }


        /// <summary>
        /// Checks if the given feature is supported by the defined decoder specification. Returns TRUE is
        /// supported.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification</param>
        /// <param name="featureName">The feature</param>
        /// <param name="folder">The folder of the decoder specification files</param>
        /// <returns></returns>
        public static bool FeatureSupported (string decSpecName, string featureName, string folder)
        {
            //  Check the input parameters
            if ((decSpecName == "") || (decSpecName is null)) return false;        
            if((featureName == "") || (featureName is null)) return false;
            if ((folder == "") || (folder is null)) return false;

            try
            {

                //  Get the file name of the decoder specification
                string decSpecFileName = GetDecSpecFileName(decSpecName, folder);

                if (decSpecFileName == "") return false;

                //  Open the decspec XML file of the given decoder specification
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(decSpecFileName);

                //  Try to get the node with the feature name
                XmlNodeList itemList = xmlDoc.GetElementsByTagName(featureName);

                //  Check if we have found any items in the decoder specification file
                if (itemList is null) return false;

                //  Check if we have find one, single matching feature - if not return FALSE
                if (itemList.Count != 1) return false;

                //  Check if we have found any attributes
                if (itemList[0]!.Attributes is null) return false;

                //  Check if we have found the "support" attribute
                if (itemList[0]!.Attributes!["support"] is null) return false;

                //  Get the content of the "support" attribute 
                string support = itemList[0]!.Attributes!["support"]!.Value;

                if (support.ToUpper() == "YES") return true;

                return false;
            }
            catch {  return false; }    
        }

        /// <summary>
        /// Returns the manufacturer ID of the given decoder specification
        /// </summary>
        /// <param name="decSpecName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static byte GetManufacturerID (string decSpecName, string folder)
        {
            //  Check the input parameters
            if ((decSpecName == "") || (decSpecName is null)) return NMRA.ManufacurerID_Unknown;
            if ((folder == "") || (folder is null)) return NMRA.ManufacurerID_Unknown;

            //  Get the file name of the decoder specification
            string decSpecFileName = GetDecSpecFileName(decSpecName, folder);

            if (decSpecFileName == "") return NMRA.ManufacurerID_Unknown;

            try
            {

                //  Open the decspec XML file of the given decoder specification
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(decSpecFileName);

                //  Try to get the decoder node
                XmlNodeList itemList = xmlDoc.GetElementsByTagName("decoderseries");

                //  Check if we have found any items in the decoder specification file
                if (itemList is null) return NMRA.ManufacurerID_Unknown;

                //  Check if we have find one, single matching feature - if not return FALSE
                if (itemList.Count != 1) return NMRA.ManufacurerID_Unknown;

                //  Check if we have found any attributes
                if (itemList[0]!.Attributes is null) return NMRA.ManufacurerID_Unknown;

                //  Check if we have found the "support" attribute
                if (itemList[0]!.Attributes!["manufacturerid"] is null) return NMRA.ManufacurerID_Unknown;

                //  Get the content of the "support" attribute 
                string manufacturerID = itemList[0]!.Attributes!["manufacturerid"]!.Value;
                byte returnValue = 0;

                if (byte.TryParse(manufacturerID, out returnValue) == false) return NMRA.ManufacurerID_Unknown;

                return byte.Parse(manufacturerID);
            }
            catch { return NMRA.ManufacurerID_Unknown; }   

        }

        /// <summary>
        /// Returns the name of the decoder specification for the given manufacturer and decoder ID.
        /// Note: This feature is not suported by all decoder manufacturer.
        /// </summary>
        /// <param name="manufactuerID"></param>
        /// <param name="decoderID"></param>
        /// <returns></returns>
        public static string GetDecoderDecSpeqName(int manufactuerID, int decoderID)
        {
            List<string> decSpecs = new List<string>();

            //  If we do not get a unique decoder ID, we use the generic decoder description
            if (decoderID == 0) return GetDefaultDecSpecName();

            try
            {

                string[] fileEntries = Directory.GetFiles(ApplicationFolders.DecSpecsFolderPath, "*.decspec");
                foreach (string fileName in fileEntries)
                {

                    XDocument xdoc = XDocument.Load(fileName);
                    //  Formatting check of the XML file - check if the decoder section is available
                    if (xdoc.Element("decoderseries") == null) return "";
                    XElement xDecoderElement = xdoc.Element("decoderseries")!;

                    //  Formatting check of the XML file - check if the manufacturerid attribute is available
                    if (xDecoderElement.Attribute("manufacturerid") == null) return "";
                    XAttribute xManufacturerIDAttribute = xDecoderElement.Attribute("manufacturerid")!;

                    if (xManufacturerIDAttribute.Value == manufactuerID.ToString())
                    {
                        //  var query = from c in xdoc.Descendants("decoderseries") select c;
                        foreach (XElement element in xdoc.Descendants("decoder"))
                        {
                            if (element.Attribute("decoderid") == null) continue;
                            string decID = element.Attribute("decoderid")!.Value;
                            if (decID.ToUpper() == decoderID.ToString().ToUpper())
                            {
                                if (xDecoderElement.Attribute("description_de") == null) return "";
                                if (AppConstants.PREFERENCES_LANGUAGE_KEY_GERMAN == Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT))
                                {
                                    return xDecoderElement.Attribute("description_de")!.Value;
                                }
                                else
                                {
                                    return xDecoderElement.Attribute("description_en")!.Value;
                                }
                            }
                        }
                    }
                }
                return GetDefaultDecSpecName();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Writes the given decoder specification file to the decoder specification folder
        /// </summary>
        /// <param name="targetFileName"></param>
        /// <param name="text"></param>
        internal static bool WriteDeqSpecFile(string targetFileName, string text)
        {
            try
            {
                // Write the file content to the app data directory  
                string targetFile = System.IO.Path.Combine(ApplicationFolders.DecSpecsFolderPath, targetFileName);

                //  If the decoder specification file already exists - delete it.
                if (File.Exists(targetFile)) File.Delete(targetFile);

                //  Write the new decoder specification file
                using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
                using StreamWriter streamWriter = new StreamWriter(outputStream);
                streamWriter.Write(text);
                return true;
            }
            catch 
            {
                return false;
            }

        }

        
    public static string UnknownDecoderSpec =@"<!-- Specification file for an unknown, basic decoder -->
<decoderseries description_de=""" + UNKNOWN_DECDODER_DE + @""" description_en=""" + UNKNOWN_DECDODER_EN + @""" manufacturerid=""0"" decspecversion=""1"" notes_de=""Decoder mit obligatorischen RCN225-Funktionen"" notes_en=""Decoder with mandatory RCN225 functions"">

    <!-- Supported decoders -->

     <!-- Supported RCN225 features -->
    <RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
    <RCN225_MINIMALSPEED_CV2 support=""yes"" writeable=""yes""/>
    <RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
    <RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
    <RCN225_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_ANALOGMODE_CV29 support=""yes"" writeable=""yes""/>
    <RCN225_DIRECTION_CV29_0 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDTABLE_CV29_4 support=""yes"" writeable=""yes""/>
    <RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>

</decoderseries>
";

public static string RCN225Spec =@"<!-- Specification file for a RCN225 compatible standard decoder -->
<decoderseries description_en=""RCN225 compatible decoder"" description_de=""RCN225 kompatibler Decoder"" manufacturerid=""0"" decspecversion=""1"" notes_de=""Decoder mit obligatorischen und optionalen RCN225-Funktionen"" notes_en=""Decoder with mandatory and optional RCN225 functions"" >

    <!-- Supported decoders -->

     <!-- Supported RCN225 features -->
    <RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
    <RCN225_MINIMALSPEED_CV2 support=""yes"" writeable=""yes""/>
    <RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
    <RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
    <RCN225_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_DIRECTION_CV29_0 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDTABLE_CV29_4 support=""yes"" writeable=""yes""/>
    <RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>
    <RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_FUNCTIONOUTPUTMAPPING_CV3346 support=""yes"" writeable=""yes""/>
    <RCN225_CONSISTADDRESS_CV19X support=""yes"" writeable=""yes""/>
    <RCN225_AUTOMATICREGISTRATION_CV27_5 support=""yes"" writeable=""yes""/>
    <RCN225_HLU_CV27_2 support=""yes"" writeable=""yes""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>
    <RCN225_DECODERLOCK_CV15X support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_AUTOMATICREGISTRATION_CV28_7 support=""yes"" writeable=""yes""/>
    <RCN225_OPERATINGMODES_CV12 support=""yes"" writeable=""yes""/>

</decoderseries>
";

    public static string ZimoFXFunctionSpec = @"<!-- Specification file for ZIMO MX function decoders -->
<decoderseries description_en=""ZIMO MX function decoder"" description_de=""ZIMO MX Funktionsdecoder"" manufacturerid=""145"" decspecversion=""1"" notes_de=""ZIMO MX Funktionsdecoder sind Decoder für nicht angetriebene Fahrzeuge"" notes_en=""ZIMO MX function decoders are decoders for non powered vehicles"">

    <!-- Supported decoders -->
    <decoder decoderid=""171"" decodername=""MX671"" />/>

    <!-- Supported RCN225 features -->
    <RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>	
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>

    <!-- Supported ZIMO features -->
    <ZIMO_MXFX_SECONDADDRESS_CV64 support=""yes"" writeable=""yes""/>
    <ZIMO_SUBVERSIONNR_CV65 support=""yes"" writeable=""no""/>
    <ZIMO_DECODERTYPE_CV250 support=""yes"" writeable=""no""/>
    <ZIMO_DECODERID_CV25X support=""yes"" writeable=""no""/>
    <ZIMO_MXUPDATELOCK_CV144 support =""yes"" />
    <ZIMO_LIGHT_DIM_CV60 support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_EFFECTS_CV125X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X support=""yes"" writeable=""yes""/>
    
</decoderseries>";

    public static string ZimoMSLocomotiveSpec = @"<!-- Specification file for ZIMO MS sound decoders -->
<decoderseries description_en=""ZIMO MS sound decoder"" description_de=""ZIMO MS Sounddecoder""  manufacturerid=""145"" decspecversion=""1"" notes_de=""ZIMO MS Sounddecoder"" notes_en=""ZIMO MS sound decoder"">

    <!-- Supported decoders -->
    <decoder decoderid=""1"" decodername=""MS500"" />
    <decoder decoderid=""2"" decodername=""MS480""/>
    <decoder decoderid=""3"" decodername=""MS490""/>
    <decoder decoderid=""4"" decodername=""MS440""/>
    <decoder decoderid=""5"" decodername=""MS580""/>
    <decoder decoderid=""6"" decodername=""MS450""/>
    <decoder decoderid=""7"" decodername=""MS990""/>
    <decoder decoderid=""8"" decodername=""MS590""/>
    <decoder decoderid=""9"" decodername=""MS950""/>
    <decoder decoderid=""10"" decodername=""MS560""/>
    <decoder decoderid=""11"" decodername=""MS001""/>
    <decoder decoderid=""12"" decodername=""MS491""/>
    <decoder decoderid=""13"" decodername=""MS581""/>
    <decoder decoderid=""14"" decodername=""MS540""/>

    <!-- Supported RCN225 features -->
    <RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
    <RCN225_MINIMALSPEED_CV2 support=""yes"" writeable=""yes""/>
    <RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
    <RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
    <RCN225_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>
    <RCN225_MEDIUMSPEED_CV6 support=""yes"" writeable=""yes""/>		
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_DECODERLOCK_CV15X support=""yes"" writeable=""yes""/>
    <RCN225_DIRECTION_CV29_0 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDTABLE_CV29_4 support=""yes"" writeable=""yes""/>
    <RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>
    <RCN225_FUNCTIONOUTPUTMAPPING_CV3346 support=""yes"" writeable=""yes""/>
    <RCN225_CONSISTADDRESS_CV19X support=""yes"" writeable=""yes""/>
    <RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_HLU_CV27_2 support=""yes"" writeable=""yes""/>
    <RCN225_AUTOMATICREGISTRATION_CV28_7 support=""yes"" writeable=""yes""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>
    <RCN225_OPERATINGMODES_CV12 support=""yes"" writeable=""yes""/>
    
    <!-- Supported ZIMO features -->
    <ZIMO_SUBVERSIONNR_CV65 support=""yes"" writeable=""no""/>
    <ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 support=""yes"" writeable=""yes""/>
    <ZIMO_DECODERTYPE_CV250 support=""yes"" writeable=""no""/>
    <ZIMO_DECODERID_CV25X support=""yes"" writeable=""no""/>
    <ZIMO_BOOTLOADER_VERSION_24X support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_DIM_CV60 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_VOLUME_GENERIC_C266 support=""yes"" writeable=""yes""/>
    <ZIMO_BRAKESQUEAL_CV287 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDALLOFF_CV310 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_MUTE_CV313 support=""yes"" writeable=""yes""/>
    <ZIMO_SELFTEST_CV30 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_CURVESQUEAL_CV308 support=""yes"" writeable=""yes""/>
    <ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_STARTUPDELAY_CV273 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_VOLUME_STEAM_CV27X support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_VOLUME_DIESELELEC_CV29X support=""yes"" writeable=""yes""/>
    <ZIMO_MSOPERATINGMODES_CV12 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUNDPROJECTNR_CV254 support=""yes"" writeable=""yes""/>
    <ZIMO_SUSIPORT1CONFIG_CV201 support=""yes"" writeable=""yes""/>
    <ZIMO_INPUTMAPPING_CV4XX support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_EFFECTS_CV125X support=""yes"" writeable=""yes""/>
    <ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SHUNTINGKEY_CV155 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 support=""yes"" writeable=""yes""/>

</decoderseries>";

 public static string ZimoMNLocomotiveSpec = @"<!-- Specification file for ZIMO MN decoders -->
<decoderseries description_en=""ZIMO MN decoder"" description_de=""ZIMO MN Decoder""  manufacturerid=""145"" decspecversion=""1"" notes_de=""ZIMO MN Decoder"" notes_en=""ZIMO MN decoder"">

    <!-- Supported decoders -->
    <decoder decoderid=""119"" decodername=""MN140""/>
    <decoder decoderid=""120"" decodername=""MN250""/>
    <decoder decoderid=""121"" decodername=""MN150""/>
    <decoder decoderid=""122"" decodername=""MN160""/>
    <decoder decoderid=""123"" decodername=""MN340""/>
    <decoder decoderid=""124"" decodername=""MN170""/>
    <decoder decoderid=""125"" decodername=""MN300""/>
    <decoder decoderid=""126"" decodername=""MN330""/>
    <decoder decoderid=""127"" decodername=""MN180""/>

    <!-- Supported RCN225 features -->
    <RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
    <RCN225_MINIMALSPEED_CV2 support=""yes"" writeable=""yes""/>
    <RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
    <RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
    <RCN225_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>
    <RCN225_MEDIUMSPEED_CV6 support=""yes"" writeable=""yes""/>		
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_DECODERLOCK_CV15X support=""yes"" writeable=""yes""/>
    <RCN225_DIRECTION_CV29_0 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDTABLE_CV29_4 support=""yes"" writeable=""yes""/>
    <RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>
    <RCN225_FUNCTIONOUTPUTMAPPING_CV3346 support=""yes"" writeable=""yes""/>
    <RCN225_CONSISTADDRESS_CV19X support=""yes"" writeable=""yes""/>
    <RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_HLU_CV27_2 support=""yes"" writeable=""yes""/>
    <RCN225_AUTOMATICREGISTRATION_CV28_7 support=""yes"" writeable=""yes""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>
    <RCN225_OPERATINGMODES_CV12 support=""yes"" writeable=""yes""/>
    
    <!-- Supported ZIMO features -->
    <ZIMO_SUBVERSIONNR_CV65 support=""yes"" writeable=""no""/>
    <ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 support=""yes"" writeable=""yes""/>
    <ZIMO_DECODERTYPE_CV250 support=""yes"" writeable=""no""/>
    <ZIMO_DECODERID_CV25X support=""yes"" writeable=""no""/>
    <ZIMO_BOOTLOADER_VERSION_24X support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_DIM_CV60 support=""yes"" writeable=""yes""/>
    <ZIMO_SELFTEST_CV30 support=""yes"" writeable=""yes""/>
    <ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 support=""yes"" writeable=""yes""/>
    <ZIMO_MSOPERATINGMODES_CV12 support=""yes"" writeable=""yes""/>
    <ZIMO_SUSIPORT1CONFIG_CV201 support=""yes"" writeable=""yes""/>
    <ZIMO_INPUTMAPPING_CV4XX support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_EFFECTS_CV125X support=""yes"" writeable=""yes""/>
    <ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SHUNTINGKEY_CV155 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 support=""yes"" writeable=""yes""/>

</decoderseries>";

        public static string ZimoMXLocomotiveSpec = @"<!-- Specification file for ZIMO MX sound decoders -->
<decoderseries description_en=""ZIMO MX sound decoder"" description_de=""ZIMO MX Sounddecoder"" manufacturerid=""145"" decspecversion=""1"" notes_de=""ZIMO MX Sounddecoder"" notes_en=""ZIMO MX sound decoders"">

    <!-- Supported decoders -->
    <decoder decoderid=""130"" decodername=""MX630 (2022)"" />
    <decoder decoderid=""131"" decodername=""MX630 RevE"" />
    <decoder decoderid=""132"" decodername=""MX623 (2022)"" />
    <decoder decoderid=""133"" decodername=""MX633 (2020)"" />
    <decoder decoderid=""134"" decodername=""MX634 (2020)"" />
    <decoder decoderid=""135"" decodername=""MX635"" />
    <decoder decoderid=""136"" decodername=""MX636"" />
    <decoder decoderid=""137"" decodername=""MX637"" />
    <decoder decoderid=""138"" decodername=""MX622"" />
    <decoder decoderid=""142"" decodername=""MDS442"" />
    <decoder decoderid=""151"" decodername=""MX615 (2023)"" />
    <decoder decoderid=""152"" decodername=""MX152 Roco"" />
    <decoder decoderid=""158"" decodername=""MX685 RevE"" />
    <decoder decoderid=""160"" decodername=""MX660"" />
    <decoder decoderid=""161"" decodername=""MX616 (2023)"" />
    <decoder decoderid=""165"" decodername=""REE_DU65"" />
    <decoder decoderid=""166"" decodername=""MX600 (2021)"" />
    <decoder decoderid=""173"" decodername=""MX673"" />
    <decoder decoderid=""174"" decodername=""MX675"" />
    <decoder decoderid=""175"" decodername=""MX675"" />
    <decoder decoderid=""176"" decodername=""R72016"" />
    <decoder decoderid=""177"" decodername=""MX617"" />
    <decoder decoderid=""178"" decodername=""MX676"" />
    <decoder decoderid=""179"" decodername=""MXLIPL3 (380mm)"" />
    <decoder decoderid=""180"" decodername=""MX688 (2022)"" />
    <decoder decoderid=""181"" decodername=""MX618"" />
    <decoder decoderid=""182"" decodername=""MX682"" />
    <decoder decoderid=""183"" decodername=""MX689"" />
    <decoder decoderid=""184"" decodername=""MXLIPL1 160mm"" />
    <decoder decoderid=""185"" decodername=""MX685 (2020)"" />
    <decoder decoderid=""186"" decodername=""MX605N"" />
    <decoder decoderid=""187"" decodername=""MX605FL"" />
    <decoder decoderid=""188"" decodername=""MX605SL"" />
    <decoder decoderid=""189"" decodername=""MX605"" />
    <decoder decoderid=""190"" decodername=""MX659"" />
    <decoder decoderid=""192"" decodername=""MX622 (2020)"" />
    <decoder decoderid=""193"" decodername=""MX638"" />
    <decoder decoderid=""194"" decodername=""MX615"" />
    <decoder decoderid=""195"" decodername=""MX616"" />
    <decoder decoderid=""196"" decodername=""MXKISS"" />
    <decoder decoderid=""197"" decodername=""MX617"" />
    <decoder decoderid=""198"" decodername=""FLM_E69"" />
    <decoder decoderid=""199"" decodername=""MX600"" />
    <decoder decoderid=""200"" decodername=""MX82"" />
    <decoder decoderid=""201"" decodername=""MX620"" />
    <decoder decoderid=""202"" decodername=""MX62"" />
    <decoder decoderid=""203"" decodername=""MX63"" />
    <decoder decoderid=""204"" decodername=""MX64"" />
    <decoder decoderid=""205"" decodername=""MX64H"" />
    <decoder decoderid=""206"" decodername=""MX64D"" />
    <decoder decoderid=""207"" decodername=""MX680"" />
    <decoder decoderid=""208"" decodername=""MX690"" />
    <decoder decoderid=""209"" decodername=""MX69"" />
    <decoder decoderid=""210"" decodername=""MX640"" />
    <decoder decoderid=""211"" decodername=""MX630-P2520"" />
    <decoder decoderid=""212"" decodername=""MX632"" />
    <decoder decoderid=""213"" decodername=""MX631"" />
    <decoder decoderid=""214"" decodername=""MX642"" />
    <decoder decoderid=""215"" decodername=""MX643"" />
    <decoder decoderid=""216"" decodername=""MX647"" />
    <decoder decoderid=""217"" decodername=""MX646"" />
    <decoder decoderid=""218"" decodername=""MX630 (2011)"" />
    <decoder decoderid=""219"" decodername=""MX631 (2011)"" />
    <decoder decoderid=""220"" decodername=""MX632 (2011)"" />
    <decoder decoderid=""221"" decodername=""MX645"" />
    <decoder decoderid=""222"" decodername=""MX644"" />
    <decoder decoderid=""223"" decodername=""MX621"" />
    <decoder decoderid=""224"" decodername=""MX695 RevB"" />
    <decoder decoderid=""225"" decodername=""MX648"" />
    <decoder decoderid=""226"" decodername=""MX685"" />
    <decoder decoderid=""227"" decodername=""MX695 RevC"" />
    <decoder decoderid=""228"" decodername=""MX681"" />
    <decoder decoderid=""229"" decodername=""MX695N"" />
    <decoder decoderid=""230"" decodername=""MX696"" />
    <decoder decoderid=""231"" decodername=""MX696N"" />
    <decoder decoderid=""232"" decodername=""MX686"" />
    <decoder decoderid=""233"" decodername=""MX622"" />
    <decoder decoderid=""234"" decodername=""MX623"" />
    <decoder decoderid=""235"" decodername=""MX687"" />
    <decoder decoderid=""236"" decodername=""MX621-FLM"" />
    <decoder decoderid=""237"" decodername=""MX633"" />
    <decoder decoderid=""238"" decodername=""MX820 RevA"" />
    <decoder decoderid=""240"" decodername=""MX634"" />
    <decoder decoderid=""241"" decodername=""MX686B"" />
    <decoder decoderid=""242"" decodername=""MX820 RevB"" />
    <decoder decoderid=""243"" decodername=""MX618"" />
    <decoder decoderid=""244"" decodername=""Roco NextG"" />
    <decoder decoderid=""245"" decodername=""MX697 RevA"" />
    <decoder decoderid=""246"" decodername=""MX658"" />
    <decoder decoderid=""247"" decodername=""MX688"" />
    <decoder decoderid=""248"" decodername=""MX821"" />
    <decoder decoderid=""249"" decodername=""MX648 RevC,D"" />
    <decoder decoderid=""250"" decodername=""MX699"" />
    <decoder decoderid=""251"" decodername=""Roco 2067"" />
    <decoder decoderid=""252"" decodername=""Roco ICE"" />
    <decoder decoderid=""253"" decodername=""MX649"" />
    <decoder decoderid=""254"" decodername=""MX697 RevB"" />

    <!-- Supported RCN225 features -->
	<RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
	<RCN225_MINIMALSPEED_CV2 support=""yes"" writeable=""yes""/>
	<RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
	<RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
	<RCN225_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>		
	<RCN225_MEDIUMSPEED_CV6 support=""yes"" writeable=""yes""/>		
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
	<RCN225_DECODERLOCK_CV15X support=""no""/>
	<RCN225_DIRECTION_CV29_0 support=""yes"" writeable=""yes""/>
	<RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
	<RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDTABLE_CV29_4 support=""yes"" writeable=""yes""/>
	<RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>
    <RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X support=""yes"" writeable=""yes""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_FUNCTIONOUTPUTMAPPING_CV3346 support=""yes"" writeable=""yes""/>
    <RCN225_CONSISTADDRESS_CV19X support=""yes"" writeable=""yes""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>
 
    <!-- Supported ZIMO features -->
    <ZIMO_SUBVERSIONNR_CV65 support=""yes"" writeable=""no""/>
	<ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 support=""yes"" writeable=""yes""/>
    <ZIMO_DECODERTYPE_CV250 support=""yes"" writeable=""no""/>
    <ZIMO_DECODERID_CV25X support=""yes"" writeable=""no""/>
    <ZIMO_LIGHT_DIM_CV60 support=""yes"" />
    <ZIMO_MXMOTORCONTROLFREQUENCY_CV9 support=""yes"" writeable=""yes""/>
    <ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 support =""yes"" />
    <ZIMO_MXUPDATELOCK_CV144 support =""yes"" />
    <ZIMO_MXMOTORCONTROLPID_CV56 support =""yes""/>
    <ZIMO_VOLUME_CV266 support=""yes"" writeable=""yes""/>
    <ZIMO_BRAKESQUEAL_CV287 support=""yes"" writeable=""yes""/>
    <ZIMO_LIGHT_EFFECTS_CV125X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SOUNDALLOFF_CV310 support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_STARTUPDELAY_CV273 support =""yes""/>
    <ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 support =""yes""/>
    <ZIMO_SOUND_VOLUME_STEAM_CV27X support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_VOLUME_DIESELELEC_CV29X support=""yes"" writeable=""yes""/>
    <ZIMO_SOUND_VOLUME_GENERIC_C266 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_MUTE_CV313 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_CURVESQUEAL_CV308 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X support=""yes"" writeable=""yes""/>
    <ZIMO_MXBRIGHTENINGUPANDIMMINGTIMES_CV190X support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCKEY_SHUNTINGKEY_CV155 support=""yes"" writeable=""yes""/>
    <ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 support=""yes"" writeable=""yes""/>

 </decoderseries>";


public static string DoehlerAndHaassPDLocomotiveSpec = @"<!-- Specification file for Doehler &amp; Haass PD decoders -->
<decoderseries description_en=""Doehler &amp; Haass PD series"" description_de=""Doehler &amp; Haass PD Serie"" manufacturerid=""97"" decspecversion=""1"" notes_de=""ZIMO MX Sounddecoder"" notes_en=""ZIMO MX sound decoders"">

    <!-- Supported decoders -->
    <decoder decoderid=""130"" decodername=""PD12A"" />
    <decoder decoderid=""131"" decodername=""PD05A"" />
    <decoder decoderid=""132"" decodername=""PD06A"" />
    <decoder decoderid=""133"" decodername=""PD21A"" />
    <decoder decoderid=""134"" decodername=""PD18A"" /> 

    <!-- Supported RCN225 features -->
	<RCN225_BASEADDRESS_CV1 support=""yes"" writeable=""yes""/>
    <RCN225_ACCELERATIONFACTOR_CV3 support=""yes"" writeable=""yes""/>
    <RCN225_DECELERATIONFACTOR_CV4 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERVERSION_CV7 support=""yes"" writeable=""no""/>
    <RCN225_MANUFACTUERID_CV8 support=""yes"" writeable=""no""/>
    <RCN225_ANALOGMODE_CV29_2 support=""yes"" writeable=""yes""/>
    <RCN225_SPEEDSTEPS_CV29_1 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMENABLED_CV29_3 support=""yes"" writeable=""yes""/>
    <RCN225_LONGSHORTADDRESS_CV29_5 support=""yes"" writeable=""yes""/>
    <RCN225_DECODERRESET_CV8 support=""yes"" writeable=""no""/>
    <RCN225_FUNCTIONOUTPUTMAPPING_CV3346 support=""yes"" writeable=""yes""/>
    <RCN225_ABC_CV27_X support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 support=""yes"" writeable=""yes""/>
    <RCN225_RAILCOMCHANNEL2DATA_CV28_1 support=""yes"" writeable=""yes""/>


    <!-- Supported Döhler and Haass features -->                
    <DOEHLERANDHAAS_DECODERTYPE_CV261 support=""yes"" writeable=""no""/>
    <DOEHLERANDHAAS_FIRMWAREVERSION_CV262x support=""yes"" writeable=""no""/>
    <DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 support=""yes"" writeable=""yes""/>
    <DOEHLERANDHAASS_MAXIMALSPEED_CV5 support=""yes"" writeable=""yes""/>
    <DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 support=""yes"" writeable=""yes""/>
    <DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 support=""yes"" writeable=""yes""/>
    <DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137 support=""yes"" writeable=""yes""/>

 </decoderseries>";
    
    }
}

