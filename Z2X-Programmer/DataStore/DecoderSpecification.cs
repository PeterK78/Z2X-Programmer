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

using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.DataStore
{
    /// <summary>
    /// This class contains the information about the currently used specification of the locomotive decoder (= decoder specification).
    /// The decoder specification defines all supported features of the locomotive decoder.
    /// Example: Is the configuration of the AC mode in CV29.2 available?
    /// </summary>
    internal static class DecoderSpecification
    {
        private static string decSpecName = "";

        /// <summary>
        /// The name of the selected decoder specification
        /// </summary>
        internal static string DeqSpecName
        {
            get
            {
                return decSpecName;
            }
            set
            {
                //  Set the new decoder specification name ...
                decSpecName = value;

                //   ... and read the supported features from the decoder specification file
                string decSpecFolder = FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath;
                ManufacturerID = DeqSpecReader.GetManufacturerID(decSpecName, decSpecFolder);
                ZIMO_SUBVERSIONNR_CV65 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SUBVERSIONNR_CV65, decSpecFolder);
                ZIMO_DECODERTYPE_CV250 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_DECODERTYPE_CV250, decSpecFolder);
                ZIMO_DECODERID_CV25X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_DECODERID_CV25X, decSpecFolder);
                ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156, decSpecFolder);
                RCN225_RAILCOMENABLED_CV29_3 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_RAILCOMENABLED_CV29_3, decSpecFolder);
                RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0, decSpecFolder);
                RCN225_RAILCOMCHANNEL2DATA_CV28_1 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_RAILCOMCHANNEL2DATA_CV28_1, decSpecFolder);
                RCN225_DECODERLOCK_CV15X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_DECODERLOCK_CV15X, decSpecFolder);
                RCN225_DIRECTION_CV29_0 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_DIRECTION_CV29_0, decSpecFolder);
                RCN225_MEDIUMSPEED_CV6 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_MEDIUMSPEED_CV6, decSpecFolder);
                ZIMO_LIGHT_DIM_CV60 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_LIGHT_DIM_CV60, decSpecFolder);
                DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERHAAS_MOTORIMPULSWIDTH_CV49, decSpecFolder);
                ZIMO_BOOTLOADER_VERSION_24X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_BOOTLOADER_VERSION_24X, decSpecFolder);
                RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X, decSpecFolder);
                RCN225_ANALOGMODE_CV29_2 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_ANALOGMODE_CV29_2, decSpecFolder);
                ZIMO_MXMOTORCONTROLFREQUENCY_CV9 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MXMOTORCONTROLFREQUENCY_CV9, decSpecFolder);
                ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57, decSpecFolder);
                ZIMO_MXUPDATELOCK_CV144 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MXUPDATELOCK_CV144, decSpecFolder);
                ZIMO_MXFX_SECONDADDRESS_CV64 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MXFX_SECONDADDRESS_CV64, decSpecFolder);
                RCN225_DECODERRESET_CV8 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_DECODERRESET_CV8, decSpecFolder);
                ZIMO_MXMOTORCONTROLPID_CV56 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MXMOTORCONTROLPID_CV56, decSpecFolder);
                ZIMO_SOUND_VOLUME_GENERIC_C266 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUND_VOLUME_GENERIC_C266, decSpecFolder);
                ZIMO_BRAKESQUEAL_CV287 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_BRAKESQUEAL_CV287, decSpecFolder);
                RCN225_FUNCTIONKEYMAPPING_CV3346 =  DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_FUNCTIONKEYMAPPING_CV3346, decSpecFolder);
                ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61, decSpecFolder);
                RCN225_CONSISTADDRESS_CV19X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_CONSISTADDRESS_CV19X, decSpecFolder);
                ZIMO_LIGHT_EFFECTS_CV125X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_LIGHT_EFFECTS_CV125X, decSpecFolder);
                DOEHLERANDHAAS_DECODERTYPE_CV261 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERANDHAAS_DECODERTYPE_CV261, decSpecFolder);
                DOEHLERANDHAAS_FIRMWAREVERSION_CV262x = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERANDHAAS_DECODERTYPE_CV261, decSpecFolder);
                DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137, decSpecFolder);
                DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133, decSpecFolder);
                DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132, decSpecFolder);
                RCN225_AUTOMATICREGISTRATION_CV28_7 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_AUTOMATICREGISTRATION_CV28_7, decSpecFolder);
                RCN225_HLU_CV27_2 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_HLU_CV27_2, decSpecFolder);
                ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397, decSpecFolder);
                ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396, decSpecFolder);
                ZIMO_FUNCKEY_SOUNDALLOFF_CV310 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEY_SOUNDALLOFF_CV310, decSpecFolder);
                ZIMO_SELFTEST_CV30 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SELFTEST_CV30, decSpecFolder);
                ZIMO_FUNCKEY_CURVESQUEAL_CV308 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEY_CURVESQUEAL_CV308, decSpecFolder);
                ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57, decSpecFolder);
                ZIMO_SOUND_STARTUPDELAY_CV273 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUND_STARTUPDELAY_CV273, decSpecFolder);
                ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285, decSpecFolder);
                ZIMO_SOUND_VOLUME_STEAM_CV27X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUND_VOLUME_STEAM_CV27X, decSpecFolder);
                ZIMO_SOUND_VOLUME_DIESELELEC_CV29X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUND_VOLUME_DIESELELEC_CV29X, decSpecFolder);
                ZIMO_FUNCKEY_MUTE_CV313 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCKEY_MUTE_CV313, decSpecFolder);
                RCN225_ABC_CV27_X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_ABC_CV27_X, decSpecFolder);
                RCN225_SPEEDTABLE_CV29_4 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_SPEEDTABLE_CV29_4, decSpecFolder);
                RCN225_OPERATINGMODES_CV12 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.RCN225_OPERATINGMODES_CV12, decSpecFolder);
                ZIMO_MSOPERATINGMODES_CV12= DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MSOPERATINGMODES_CV12, decSpecFolder);
                ZIMO_SOUNDPROJECTNR_CV254 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SOUNDPROJECTNR_CV254, decSpecFolder);
                ZIMO_SUSIPORT1CONFIG_CV201 = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_SUSIPORT1CONFIG_CV201, decSpecFolder);
                ZIMO_INPUTMAPPING_CV4XX = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_INPUTMAPPING_CV4XX, decSpecFolder);
                ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X, decSpecFolder);
                ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X = DeqSpecReader.FeatureSupported(decSpecName, FileAndFolderManagement.DeqSpecReader.ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X, decSpecFolder);

                //  Inform the app that we have just read a new decoder specification file
                //WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
            }
        }
        internal static byte ManufacturerID { get; set; }
        internal static bool ZIMO_SUBVERSIONNR_CV65 { get; set; }
        internal static bool ZIMO_DECODERTYPE_CV250 { get ; set; }
        internal static bool ZIMO_DECODERID_CV25X { get; set; }
        internal static bool ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 { get; set; }
        internal static bool RCN225_RAILCOMENABLED_CV29_3 { get; set; }
        internal static bool RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 { get; set; }
        internal static bool RCN225_RAILCOMCHANNEL2DATA_CV28_1 { get; set; }
        internal static bool RCN225_DECODERLOCK_CV15X { get; set; }
        internal static bool RCN225_DIRECTION_CV29_0 { get; set; }
        internal static bool RCN225_MEDIUMSPEED_CV6 { get; set; }
        internal static bool ZIMO_LIGHT_DIM_CV60 { get; set; }
        internal static bool DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 {  get; set; }
        internal static bool ZIMO_BOOTLOADER_VERSION_24X { get; set; }
        internal static bool RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X { get; set; }
        internal static bool RCN225_ANALOGMODE_CV29_2 { get; set; }
        internal static bool ZIMO_MXMOTORCONTROLFREQUENCY_CV9 { get; set; }
        internal static bool ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 { get; set; }
        internal static bool ZIMO_MXUPDATELOCK_CV144 { get; set; }
        internal static bool ZIMO_MXFX_SECONDADDRESS_CV64 { get; set; }
        internal static bool RCN225_DECODERRESET_CV8 { get; set; }
        internal static bool ZIMO_MXMOTORCONTROLPID_CV56 { get; set; }
        internal static bool ZIMO_SOUND_VOLUME_GENERIC_C266 { get; set; }
        internal static bool ZIMO_BRAKESQUEAL_CV287 { get; set; }
        internal static bool RCN225_FUNCTIONKEYMAPPING_CV3346 { get; set; }
        internal static bool ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61 { get; set; }
        internal static bool RCN225_CONSISTADDRESS_CV19X  { get; set; }
        internal static bool ZIMO_LIGHT_EFFECTS_CV125X { get; set; }
        internal static bool DOEHLERANDHAAS_DECODERTYPE_CV261 { get; set; }
        internal static bool DOEHLERANDHAAS_FIRMWAREVERSION_CV262x { get; set; }
        internal static bool DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137 { get; set; }
        internal static bool DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 { get; set; }
        internal static bool DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 { get; set; }
        internal static bool RCN225_AUTOMATICREGISTRATION_CV28_7 { get; set; }
        internal static bool RCN225_HLU_CV27_2 { get; set; }
        internal static bool ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 { get; set; }
        internal static bool ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 { get; set; }
        internal static bool ZIMO_FUNCKEY_SOUNDALLOFF_CV310 { get; set; }
        internal static bool ZIMO_SELFTEST_CV30 { get; set; }
        internal static bool ZIMO_FUNCKEY_CURVESQUEAL_CV308 { get; set; }
        internal static bool ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 { get; set; }
        internal static bool ZIMO_SOUND_STARTUPDELAY_CV273 { get; set; }
        internal static bool ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 { get; set; }
        internal static bool ZIMO_SOUND_VOLUME_STEAM_CV27X { get; set; }
        internal static bool ZIMO_SOUND_VOLUME_DIESELELEC_CV29X { get; set; }
        internal static bool ZIMO_FUNCKEY_MUTE_CV313 { get; set; }
        internal static bool RCN225_ABC_CV27_X { get; set; }
        internal static bool RCN225_SPEEDTABLE_CV29_4 { get; set; }
        internal static bool RCN225_OPERATINGMODES_CV12 { get ; set; }
        internal static bool ZIMO_MSOPERATINGMODES_CV12 { get; set; }
        internal static bool ZIMO_SOUNDPROJECTNR_CV254 { get; set; }
        internal static bool ZIMO_SUSIPORT1CONFIG_CV201 { get ; set; }
        internal static bool ZIMO_INPUTMAPPING_CV4XX { get; set; }
        internal static bool ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X { get; set; }
        internal static bool ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X { get; set; }

    }
}
