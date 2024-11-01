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
    /// Implements the search for settings functionality of Z2X-Programmer.
    /// </summary>
    internal static class SettingsSearcher
    {
        static readonly string[,] SearchDatabase = new string[79, 3] {
                                                                        { "AddressPage", "FrameAddressVehicleAddressLabel", "" },
                                                                        { "AddressPage", "FrameAddressVehicleAddressModeLabel", "" },
                                                                        { "AddressPage", "FrameLocomotiveAddressUseConsistAddressLabel", "RCN225_CONSISTADDRESS_CV19" },
                                                                        { "AddressPage", "FrameLocomotiveAddressConsistAdrLabel","RCN225_CONSISTADDRESS_CV19"},
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsDirectionLabel", "RCN225_DIRECTION_CV29_0"},
                                                                        { "DriveCharacteristicsPage", "FrameDriveCharacteristicsConsistDirectionLabel", "RCN225_CONSISTADDRESS_CV19"},
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
                                                                        { "MotorCharacteristicsPage","FrameMotorCharacteristicsBasicCurveMaximumSpeedTitle", "" },
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
                                                                        { "FunctionKeysPage","FrameFunctionKeysZIMOInputMappingDescription", "ZIMO_INPUTMAPPING_CV4XX" }
                                                                    };

        /// <summary>
        /// Searches the database for the specified keywords and returns the entries found.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        internal static List<string> GetResults(string keyword)
        {
            List<string> results = new List<string>();

            if (keyword == "") return results;
            if (keyword.Length < 3) return results;

            for (int i = 0; i < SearchDatabase.GetLength(0); i++)
            {
                if (AppResources.ResourceManager == null) continue;
                string SearchTerm  = AppResources.ResourceManager.GetString(SearchDatabase[i, 1])!;
                if ((SearchTerm == null) || (SearchTerm == "")) continue;

                if (SearchTerm.ToUpper().Contains(keyword.ToUpper()))
                {
                    results.Add(SearchTerm);
                }
            }

            if (results.Count == 0)
            {
                results.Add(AppResources.FrameSearchNothingFound);
            }

            results.Sort();

            return results;
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

            for (int i = 0; i < SearchDatabase.GetLength(0); i++)
            {
                string SearchTerm = AppResources.ResourceManager.GetString(SearchDatabase[i, 1])!.ToUpper();
                if (SearchTerm.ToUpper() == searchResult.ToUpper())
                {
                    pageName = SearchDatabase[i, 0];
                    labelName = SearchDatabase[i, 1];

                    string DecoderConfigurationProperty = SearchDatabase[i, 2].ToUpper();

                    if (DecoderConfigurationProperty == "") return true;
                    
                    //// Get the PropertyInfo object by passing the property name.
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
    }       
}
