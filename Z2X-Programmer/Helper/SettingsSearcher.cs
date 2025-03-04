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

using System.Reflection;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Helper
{

    /// <summary>
    /// Implements the "search for settings" functionality of Z2X-Programmer.
    /// </summary>
    internal static class SettingsSearcher
    {
        static readonly string[,] SearchDatabase = new string[89, 3] {
                                                                        { "AddressPage", "FrameAddressVehicleAddressLabel", "" },
                                                                        { "AddressPage", "FrameAddressVehicleAddressModeLabel", "" },
                                                                        { "AddressPage", "FrameLocomotiveAddressUseConsistAddressLabel", "RCN225_CONSISTADDRESS_CV19X" },
                                                                        { "AddressPage", "FrameLocomotiveAddressConsistAdrLabel","RCN225_CONSISTADDRESS_CV19X"},
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsDirectionLabel", "RCN225_DIRECTION_CV29_0"},
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsConsistDirectionLabel", "RCN225_CONSISTADDRESS_CV19X"},
                                                                        { "DriveCharacteristicsPage","FrameSpeedStepsSystemLabel", "" },
                                                                        { "DriveCharacteristicsPage","FrameDriveCharacteristicsAccDecAccTimeEnabledDisabled", "" },
                                                                        { "DriveCharacteristicsPage","FrameDriveCharacteristicsDecTimeLabel", "" },
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsBrakingTrackABCLabel","RCN225_ABC_CV27_X"},
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsBrakingTrackHLULabel", "RCN225_HLU_CV27_2" },
                                                                        { "DecoderInformationPage","FrameGenericDecoderInformationManufacturer", "" },
                                                                        { "DecoderInformationPage","FrameGenericDecoderInformationVersionNumber", "" },
                                                                        { "DecoderInformationPage","FrameDecoderZIMOInfoDecoderType", "ZIMO_DECODERTYPE_CV250" },
                                                                        { "DecoderInformationPage","FrameDecoderZIMOInfoSWVersion", "ZIMO_SUBVERSIONNR_CV65" },
                                                                        { "DecoderInformationPage","FrameDecoderZIMOInfoDecoderID", "ZIMO_DECODERID_CV25X" },
                                                                        { "DecoderInformationPage","FrameDecoderZIMOInfoBootloaderVersion", "ZIMO_BOOTLOADER_VERSION_24X" },
                                                                        { "DecoderInformationPage","FrameDecoderDoehlerAndHaasInfoDecoderType", "DOEHLERANDHAAS_DECODERTYPE_CV261" },
                                                                        { "DecoderInformationPage","FrameDecoderDoehlerAndHaassInfoSWVersion", "DOEHLERANDHAAS_FIRMWAREVERSION_CV262x" },
                                                                        { "DecoderInformationPage","FrameDecoderDescriptionLabel", "" },
                                                                        { "DecoderInformationPage","FrameDecoderInfoPictureTitle", "" },
                                                                        { "DecoderInformationPage","FrameDecoderPersonalNotesTitle", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsBasicCurveMinimumSpeedTitle", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsBasicCurveMediumSpeedTitle", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsBasicCurveMaximumSpeedTitle", "RCN225_MAXIMALSPEED_CV5" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep1CV67", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep2CV68", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep3CV69", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep4CV70", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep5CV71", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep6CV72", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep7CV73", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep8CV74", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep9CV75", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep10CV76", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep11CV77", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep12CV78", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep13CV79", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep14CV80", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep15CV81", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep16CV82", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep17CV83", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep18CV84", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep19CV85", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep20CV86", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep21CV87", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep22CV88", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep23CV89", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep24CV90", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep25CV91", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep26CV92", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep27CV93", "" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsExtendedSpeedCurveStep28CV94", "" },
                                                                        { "MaintenancePage","FrameSecurityDecoderResetLabel", "RCN225_DECODERRESET_CV8" },
                                                                        { "SoundPage","FrameSoundOverallVolumeTitle", "ZIMO_SOUND_VOLUME_GENERIC_C266" },
                                                                        { "SecurityPage","FrameSecurityDecoderLockLabelCV16", "RCN225_DECODERLOCK_CV15X" },
                                                                        { "SecurityPage","FrameSecurityDecoderLockLabelCV15", "RCN225_DECODERLOCK_CV15X" },
                                                                        { "RailComPage","FrameRailComEnableDisableRailCom", "RCN225_RAILCOMENABLED_CV29_3" },
                                                                        { "RailComPage","FrameRailComConfigurationAddressBroadCast", "RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0" },
                                                                        { "RailComPage","FrameRailComConfigurationDataChannel2", "RCN225_RAILCOMCHANNEL2DATA_CV28_1" },
                                                                        { "FunctionKeysPage","FrameFunctionsKeysZIMOMappingType", "ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysSoundVolumeOnOffLabel", "ZIMO_FUNCKEY_SOUNDALLOFF_CV310" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysSoundVolumeQuieterLabel", "ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysSoundVolumeLouderLabel", "ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysSoundMuteLabel", "ZIMO_FUNCKEY_MUTE_CV313" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysCurveSquealLabel", "ZIMO_FUNCKEY_CURVESQUEAL_CV308" },
                                                                        { "LightPage","FrameLightDimmingEnabled", "ZIMO_LIGHT_DIM_CV60" },
                                                                        { "SoundPage","FrameSoundBreakSquealLevelLabel", "ZIMO_BRAKESQUEAL_CV287" },
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsCharacteristicCurveTypeLabel", "RCN225_SPEEDTABLE_CV29_4" },
                                                                        { "RailComPage","FrameProtocolAnalogModeLabel", "RCN225_ANALOGMODE_CV29_2" },
                                                                        { "RailComPage","FrameProtocolAnalogDCLabel", "ZIMO_MSOPERATINGMODES_CV12" },
                                                                        { "RailComPage","FrameProtocolAnalogACLabel", "ZIMO_MSOPERATINGMODES_CV12" },
                                                                        { "RailComPage","FrameProtocolDCCModeLabel", "ZIMO_MSOPERATINGMODES_CV12" },
                                                                        { "RailComPage","FrameProtocolMMModeLabel", "ZIMO_MSOPERATINGMODES_CV12" },
                                                                        { "RailComPage","FrameProtocolMFXModeLabel", "ZIMO_MSOPERATINGMODES_CV12" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysDeactivateAccDecTimeLabel", "ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156" },
                                                                        { "DecoderInformationPage","FrameDecoderZIMOInfoSoundProjectNr", "ZIMO_SOUNDPROJECTNR_CV254" },
                                                                        { "FunctionOutputsPage","FrameProtocolZIMOSUSIPinClockLabel", "ZIMO_SUSIPORT1CONFIG_CV201" },
                                                                        { "FunctionKeysPage","FrameFunctionKeysZIMOInputMappingDescription", "ZIMO_INPUTMAPPING_CV4XX" },
                                                                        { "FunctionKeysSecondaryAddressPage", "FrameFunctionKeysMappingSecondaryAddressTitle","ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X"},
                                                                        { "MotorCharacteristicsPage", "FrameMotorCharacteristicsReferenceVoltageAutoMode", "ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57" },
                                                                        { "MotorCharacteristicsPage", "FrameMotorCharacteristicsReferenceVoltageAutoModeZimoMS", "ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57" },
                                                                        { "MotorCharacteristicsPage", "FrameMotorCharacteristicsMotorControlFreqType", "ZIMO_MXMOTORCONTROLFREQUENCY_CV9" },
                                                                        { "LightPage", "FrameLightEffectFadeInTimeLabel", "ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X" },
                                                                        { "LightPage", "FrameLightEffectFadeOutTimeLabel", "ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X" },
                                                                        { "FunctionKeysPage", "FrameFunctionKeysHighBeamDimmingTitle", "ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X" },
                                                                        { "FunctionKeysPage", "FrameFunctionKeysHighBeamDimmingDescription", "ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X" },
                                                                        { "MotorCharacteristicsPage", "FrameMotorCharacteristicsDHImpulswidth", "DOEHLERHAAS_MOTORIMPULSWIDTH_CV49" },
                                                                        { "MotorCharacteristicsPage", "FrameMotorCharacteristicsDoehlerHaasMaxSpeedLabel", "DOEHLERANDHAASS_MAXIMALSPEED_CV5" }
                                                                    };

        /// <summary>
        /// Returns entries in the search database that contain the search string
        /// and are supported by the current decoder specification.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        internal static List<string> GetResults(string searchTerm)
        {
            try
            {
                List<string> results = new List<string>();

                //  Input parameter checking.
                if (searchTerm == "") return results;
                if (searchTerm.Length < 3) return results;

                //  Search the database for the search term.
                for (int i = 0; i < SearchDatabase.GetLength(0); i++)
                {
                    //  Get the next database string.
                    if (AppResources.ResourceManager == null) continue;
                    string SearchTerm  = AppResources.ResourceManager.GetString(SearchDatabase[i, 1])!;
                    if ((SearchTerm == null) || (SearchTerm == "")) continue;

                    //  Check if the search term is contained in the database string.
                    if (SearchTerm.ToUpper().Contains(searchTerm.ToUpper()))
                    {
                        //  Check if the feature is supported by the current decoder specification.
                        Type myDecoderSpecificationType = typeof(DecoderSpecification);
                        if (myDecoderSpecificationType != null)
                        {
                            PropertyInfo myPropInfo = myDecoderSpecificationType.GetProperty(SearchDatabase[i, 2].ToUpper(), BindingFlags.Static | BindingFlags.NonPublic)!;
                            if (myPropInfo != null)
                            {
                                {
                                    if ((bool)myPropInfo.GetValue(null, null)! == true)
                                    {
                                        results.Add(SearchTerm);
                                    }
                                }
                            }
                        }
                    }
                }

                //  Add a "nothing found" message if no results have been found.
                if (results.Count == 0)
                {
                    results.Add(AppResources.FrameSearchNothingFound);
                }

                //  Sort the results.   
                results.Sort();

                return results;

            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("SettingsSearcher:GetResults:" + ex.Message);
                return new List<string>();
            }

          
        }

        /// <summary>
        /// Returns the target page and the target GUI control for the transferred search text. If a valid target has
        /// been found, the return value will be TRUE.
        /// </summary>
        /// <param name="searchResult">A result of a GetResult call.</param>
        /// <param name="pageName">The target page.</param>
        /// <param name="labelName">The target label.</param>
        /// <returns></returns>
        internal static bool GetNavigationTarget(string searchResult, out string pageName, out string labelName)
        {
            pageName = "";
            labelName = "";

            if (searchResult == null) return false;

            try
            {
                for (int i = 0; i < SearchDatabase.GetLength(0); i++)
                {
                    string SearchTerm = AppResources.ResourceManager.GetString(SearchDatabase[i, 1])!;

                    //  Did we find a valid search term?
                    if (SearchTerm == null) continue;
                    
                    if (SearchTerm.ToUpper() == searchResult.ToUpper())
                    {
                        pageName = SearchDatabase[i, 0];
                        labelName = SearchDatabase[i, 1];

                        //  Check if the feature is supported by the current decoder specification.
                        string DecoderConfigurationProperty = SearchDatabase[i, 2].ToUpper();
                        if (DecoderConfigurationProperty == "") return true;
                    
                        //  Check if the feature is supported by the current decoder specification.
                        Type myDecoderSpecificationType = typeof(DecoderSpecification);
                        if (myDecoderSpecificationType != null)
                        {
                            PropertyInfo myPropInfo = myDecoderSpecificationType.GetProperty(DecoderConfigurationProperty, BindingFlags.Static | BindingFlags.NonPublic)!;
                            if (myPropInfo != null)
                            {
                                {
                                    if((bool)myPropInfo.GetValue(null, null)! == true)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
               return false;
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("SettingsSearcher:GetNavigationTarget:" + ex.Message);
                return false;   
            }
        }
    }       
}
