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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Reflection;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Popups;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    /// <summary>
    /// Implementation of the view model for the function keys page.
    /// </summary>
    public partial class FunctionKeysViewModel: ObservableObject
    {

        internal ObservableCollection<ZIMOInputMappingType>? _ZIMOInputMappingCVs= new ObservableCollection<ZIMOInputMappingType>();

        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

         // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion

        #region REGION: DECODER FEATURES
        
        //  Any sound function keys supported?
        [ObservableProperty]
        bool soundFunctionKeysSupported = false;

        //  Any drive- an motor characeteristics related keys supported?
        [ObservableProperty]
        bool driveAndMotorCharacteristicsKeysSupported = false;

        //  RCN225 features
        [ObservableProperty]
        bool rCN225_FUNCTIONKEYMAPPING_CV3346;

        //  ZIMO features
        [ObservableProperty]
        bool zIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X = false;

        [ObservableProperty]
        bool zIMO_INPUTMAPPING_CV4XX;
        
        [ObservableProperty]
        bool zIMO_FUNCKEY_MUTE_CV313;

        [ObservableProperty]
        bool zIMO_FUNCTIONKEYMAPPINGTYPE_CV61;

        [ObservableProperty]
        bool zIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDALLOFF_CV310;

        [ObservableProperty]
        bool zIMO_FUNCKEY_CURVESQUEAL_CV308;

        //  ZIMO: Shunting function key
        [ObservableProperty]
        bool zIMO_FUNCKEY_SHUNTINGKEY_CV155 = false;

        //  ZIMO: Function keys for high beam and dimming in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        [ObservableProperty]
        bool zIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X = false;

        //  ZIMO: Function mapping in CV33 - CV46 for FX decoder
        [ObservableProperty]
        bool zIMO_MXFXFUNCTIONKEYMAPPING_CV3346 = false;

        //   Döhler & Haass
        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYSHUNTING_CV132;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        // ZIMO: ZIMO input mapping in CV400 - CV428 (ZIMO_INPUTMAPPING_CV4XX)
        //  SelectedInputMapping contains the data for the currently selected input mapping (if the user edits a mapping).
        [ObservableProperty]
        internal ZIMOInputMappingType selectedInputMapping = new ZIMOInputMappingType();

        //  ZIMOInputMappingCVs contains all input mappings for the current decoder.
        [ObservableProperty]
        internal ObservableCollection<ZIMOInputMappingType>? zIMOInputMappingCVs= new ObservableCollection<ZIMOInputMappingType>();

        //  ZIMO: Function keys for high beam and dipped beam in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        #region ZIMO: Function keys for high beam and dipped beam in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        [ObservableProperty]
        ushort cV119Value;

        [ObservableProperty]
        bool cV119ValueBit0;
        partial void OnCV119ValueBit0Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 0, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit1;
        partial void OnCV119ValueBit1Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 1, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit2;
        partial void OnCV119ValueBit2Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 2, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit3;
        partial void OnCV119ValueBit3Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 3, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit4;
        partial void OnCV119ValueBit4Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 4, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit5;
        partial void OnCV119ValueBit5Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 5, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit6;
        partial void OnCV119ValueBit6Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 6, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        bool cV119ValueBit7;
        partial void OnCV119ValueBit7Changed(bool value)
        {
            CV119Value = Bit.Set((byte)CV119Value, 7, value);
            DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam = (byte)CV119Value;
        }

        [ObservableProperty]
        ushort cV120Value;

        [ObservableProperty]
        bool cV120ValueBit0;
        partial void OnCV120ValueBit0Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 0, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit1;
        partial void OnCV120ValueBit1Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 1, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit2;
        partial void OnCV120ValueBit2Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 2, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit3;
        partial void OnCV120ValueBit3Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 3, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit4;
        partial void OnCV120ValueBit4Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 4, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit5;
        partial void OnCV120ValueBit5Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 5, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit6;
        partial void OnCV120ValueBit6Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 6, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }

        [ObservableProperty]
        bool cV120ValueBit7;
        partial void OnCV120ValueBit7Changed(bool value)
        {
            CV120Value = Bit.Set((byte)CV120Value, 7, value);
            DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam = (byte)CV120Value;
        }


        #endregion

        #region FUNCTION OUTPUT DESCRIPTION
        [ObservableProperty]
        string output0vDescription = string.Empty;

        [ObservableProperty]
        string output0rDescription = string.Empty;

        [ObservableProperty]
        string output1Description = string.Empty;

        [ObservableProperty]
        string output2Description = string.Empty;
        
        [ObservableProperty]
        string output3Description = string.Empty;
                
        [ObservableProperty]
        string output4Description = string.Empty;

        [ObservableProperty]
        string output5Description = string.Empty;

        [ObservableProperty]
        string output6Description = string.Empty;

        [ObservableProperty]
        string output7Description = string.Empty;

        [ObservableProperty]
        string output8Description = string.Empty;

        [ObservableProperty]
        string output9Description = string.Empty;

        [ObservableProperty]
        string output10Description = string.Empty;

        [ObservableProperty]
        string output11Description = string.Empty;

        [ObservableProperty]
        string output12Description = string.Empty;
        #endregion

        #region CV33 FUNCTION OUTPUT MAPPING OF KEY F0 (FORWARD)
        [ObservableProperty]
        ushort cV33Value;
        
        [ObservableProperty]
        bool cV33ValueBit0;
        partial void OnCV33ValueBit0Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }

        [ObservableProperty]
        bool cV33ValueBit1;
        partial void OnCV33ValueBit1Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit2;
        partial void OnCV33ValueBit2Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit3;
        partial void OnCV33ValueBit3Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit4;
        partial void OnCV33ValueBit4Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit5;
        partial void OnCV33ValueBit5Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit6;
        partial void OnCV33ValueBit6Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;
        }
        [ObservableProperty]
        bool cV33ValueBit7;
        partial void OnCV33ValueBit7Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 7, value);
                DecoderConfiguration.RCN225.FunctionMappingF0Forward = (byte)CV33Value;     
        }
        #endregion

        #region CV34 FUNCTION OUTPUT MAPPING OF KEY F0 (BACKWARD)
        [ObservableProperty]
        ushort cV34Value;

        [ObservableProperty]
        bool cV34ValueBit0;
        partial void OnCV34ValueBit0Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }

        [ObservableProperty]
        bool cV34ValueBit1;
        partial void OnCV34ValueBit1Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit2;
        partial void OnCV34ValueBit2Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit3;
        partial void OnCV34ValueBit3Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit4;
        partial void OnCV34ValueBit4Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit5;
        partial void OnCV34ValueBit5Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit6;
        partial void OnCV34ValueBit6Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        [ObservableProperty]
        bool cV34ValueBit7;
        partial void OnCV34ValueBit7Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF0Backward = (byte)CV34Value;
        }
        #endregion

        #region CV35 FUNCTION OUTPUT MAPPING OF KEY F1
        [ObservableProperty]
        ushort cV35Value;

        [ObservableProperty]
        bool cV35ValueBit0;
        partial void OnCV35ValueBit0Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }

        [ObservableProperty]
        bool cV35ValueBit1;
        partial void OnCV35ValueBit1Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit2;
        partial void OnCV35ValueBit2Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit3;
        partial void OnCV35ValueBit3Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit4;
        partial void OnCV35ValueBit4Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit5;
        partial void OnCV35ValueBit5Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit6;
        partial void OnCV35ValueBit6Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        [ObservableProperty]
        bool cV35ValueBit7;
        partial void OnCV35ValueBit7Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF1 = (byte)CV35Value;
        }
        #endregion

        #region CV36 FUNCTION OUTPUT MAPPING OF KEY F2
        [ObservableProperty]
        ushort cV36Value;

        [ObservableProperty]
        bool cV36ValueBit0;
        partial void OnCV36ValueBit0Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }

        [ObservableProperty]
        bool cV36ValueBit1;
        partial void OnCV36ValueBit1Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit2;
        partial void OnCV36ValueBit2Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit3;
        partial void OnCV36ValueBit3Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit4;
        partial void OnCV36ValueBit4Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit5;
        partial void OnCV36ValueBit5Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit6;
        partial void OnCV36ValueBit6Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        [ObservableProperty]
        bool cV36ValueBit7;
        partial void OnCV36ValueBit7Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF2 = (byte)CV36Value;
        }
        #endregion

        #region CV37 FUNCTION OUTPUT MAPPING OF KEY F3
        [ObservableProperty]
        ushort cV37Value;

        [ObservableProperty]
        bool cV37ValueBit0;
        partial void OnCV37ValueBit0Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }

        [ObservableProperty]
        bool cV37ValueBit1;
        partial void OnCV37ValueBit1Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit2;
        partial void OnCV37ValueBit2Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit3;
        partial void OnCV37ValueBit3Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit4;
        partial void OnCV37ValueBit4Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit5;
        partial void OnCV37ValueBit5Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit6;
        partial void OnCV37ValueBit6Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        [ObservableProperty]
        bool cV37ValueBit7;
        partial void OnCV37ValueBit7Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF3 = (byte) CV37Value;
        }
        #endregion

        #region CV38 FUNCTION OUTPUT MAPPING OF KEY F4
        [ObservableProperty]
        ushort cV38Value;

        [ObservableProperty]
        bool cV38ValueBit0;
        partial void OnCV38ValueBit0Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }

        [ObservableProperty]
        bool cV38ValueBit1;
        partial void OnCV38ValueBit1Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit2;
        partial void OnCV38ValueBit2Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit3;
        partial void OnCV38ValueBit3Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit4;
        partial void OnCV38ValueBit4Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit5;
        partial void OnCV38ValueBit5Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit6;
        partial void OnCV38ValueBit6Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        [ObservableProperty]
        bool cV38ValueBit7;
        partial void OnCV38ValueBit7Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF4 = (byte) CV38Value;
        }
        #endregion

        #region CV39 FUNCTION OUTPUT MAPPING OF KEY F5
        [ObservableProperty]
        ushort cV39Value;

        [ObservableProperty]
        bool cV39ValueBit0;
        partial void OnCV39ValueBit0Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }

        [ObservableProperty]
        bool cV39ValueBit1;
        partial void OnCV39ValueBit1Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit2;
        partial void OnCV39ValueBit2Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit3;
        partial void OnCV39ValueBit3Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit4;
        partial void OnCV39ValueBit4Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit5;
        partial void OnCV39ValueBit5Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit6;
        partial void OnCV39ValueBit6Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        [ObservableProperty]
        bool cV39ValueBit7;
        partial void OnCV39ValueBit7Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF5 =(byte) CV39Value;
        }
        #endregion

        #region CV40 FUNCTION OUT MAPPING OF KEY F6
        [ObservableProperty]
        ushort cV40Value;

        [ObservableProperty]
        bool cV40ValueBit0;
        partial void OnCV40ValueBit0Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }

        [ObservableProperty]
        bool cV40ValueBit1;
        partial void OnCV40ValueBit1Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit2;
        partial void OnCV40ValueBit2Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit3;
        partial void OnCV40ValueBit3Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit4;
        partial void OnCV40ValueBit4Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit5;
        partial void OnCV40ValueBit5Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit6;
        partial void OnCV40ValueBit6Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        [ObservableProperty]
        bool cV40ValueBit7;
        partial void OnCV40ValueBit7Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF6 = (byte)CV40Value;
        }
        #endregion

        #region CV41 FUNCTION OUTPUT MAPPING OF KEY F7
        [ObservableProperty]
        ushort cV41Value;

        [ObservableProperty]
        bool cV41ValueBit0;
        partial void OnCV41ValueBit0Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }

        [ObservableProperty]
        bool cV41ValueBit1;
        partial void OnCV41ValueBit1Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit2;
        partial void OnCV41ValueBit2Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit3;
        partial void OnCV41ValueBit3Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit4;
        partial void OnCV41ValueBit4Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit5;
        partial void OnCV41ValueBit5Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit6;
        partial void OnCV41ValueBit6Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;
        }
        [ObservableProperty]
        bool cV41ValueBit7;
        partial void OnCV41ValueBit7Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF7 = (byte)CV41Value;        
        }
        #endregion

        #region CV42 FUNCTION OUTPUT MAPPING OF KEY F8
        [ObservableProperty]
        ushort cV42Value;

        [ObservableProperty]
        bool cV42ValueBit0;
        partial void OnCV42ValueBit0Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;
        }

        [ObservableProperty]
        bool cV42ValueBit1;
        partial void OnCV42ValueBit1Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit2;
        partial void OnCV42ValueBit2Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit3;
        partial void OnCV42ValueBit3Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit4;
        partial void OnCV42ValueBit4Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit5;
        partial void OnCV42ValueBit5Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit6;
        partial void OnCV42ValueBit6Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        [ObservableProperty]
        bool cV42ValueBit7;
        partial void OnCV42ValueBit7Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF8 =(byte)CV42Value;

        }
        #endregion

        #region CV43 FUNCTION OUTPUT MAPPING OF KEY F9
        [ObservableProperty]
        ushort cV43Value;

        [ObservableProperty]
        bool cV43ValueBit0;
        partial void OnCV43ValueBit0Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }

        [ObservableProperty]
        bool cV43ValueBit1;
        partial void OnCV43ValueBit1Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit2;
        partial void OnCV43ValueBit2Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit3;
        partial void OnCV43ValueBit3Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit4;
        partial void OnCV43ValueBit4Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit5;
        partial void OnCV43ValueBit5Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit6;
        partial void OnCV43ValueBit6Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        [ObservableProperty]
        bool cV43ValueBit7;
        partial void OnCV43ValueBit7Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF9 = (byte)CV43Value;
        }
        #endregion

        #region CV44 FUNCTION OUTPUT MAPPING OF KEY F10
        [ObservableProperty]
        ushort cV44Value;

        [ObservableProperty]
        bool cV44ValueBit0;
        partial void OnCV44ValueBit0Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }

        [ObservableProperty]
        bool cV44ValueBit1;
        partial void OnCV44ValueBit1Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit2;
        partial void OnCV44ValueBit2Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit3;
        partial void OnCV44ValueBit3Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit4;
        partial void OnCV44ValueBit4Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit5;
        partial void OnCV44ValueBit5Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit6;
        partial void OnCV44ValueBit6Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        [ObservableProperty]
        bool cV44ValueBit7;
        partial void OnCV44ValueBit7Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF10 = (byte) CV44Value;
        }
        #endregion

        #region CV45 FUNCTION OUTPUT MAPPING OF KEY F11
        [ObservableProperty]
        ushort cV45Value;

        [ObservableProperty]
        bool cV45ValueBit0;
        partial void OnCV45ValueBit0Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }

        [ObservableProperty]
        bool cV45ValueBit1;
        partial void OnCV45ValueBit1Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit2;
        partial void OnCV45ValueBit2Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit3;
        partial void OnCV45ValueBit3Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit4;
        partial void OnCV45ValueBit4Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit5;
        partial void OnCV45ValueBit5Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit6;
        partial void OnCV45ValueBit6Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        [ObservableProperty]
        bool cV45ValueBit7;
        partial void OnCV45ValueBit7Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF11= (byte) CV45Value;
        }
        #endregion

        #region CV46 FUNCTION OUTPUT MAPPING OF KEY F12
        [ObservableProperty]
        ushort cV46Value;

        [ObservableProperty]
        bool cV46ValueBit0;
        partial void OnCV46ValueBit0Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 0, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }

        [ObservableProperty]
        bool cV46ValueBit1;
        partial void OnCV46ValueBit1Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 1, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit2;
        partial void OnCV46ValueBit2Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 2, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit3;
        partial void OnCV46ValueBit3Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 3, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit4;
        partial void OnCV46ValueBit4Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 4, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit5;
        partial void OnCV46ValueBit5Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 5, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit6;
        partial void OnCV46ValueBit6Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 6, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        [ObservableProperty]
        bool cV46ValueBit7;
        partial void OnCV46ValueBit7Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 7, value);
            DecoderConfiguration.RCN225.FunctionMappingF12 = (byte)CV46Value;
        }
        #endregion

        [ObservableProperty]
        internal bool rCN225StandardFunctionMapping = true;

        // ZIMO: Extended function key mapping - without left-shift (ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61)
        [ObservableProperty]
        internal bool zIMOExtendedFunctionMapping = false;
        partial void OnZIMOExtendedFunctionMappingChanged(bool value)
        {
            RCN225StandardFunctionMapping = !value;
            DecoderConfiguration.ZIMO.ExtendedFunctionKeyMapping = value;
            CV61Configuration = Subline.Create(new List<uint>{61});
        }
        [ObservableProperty]
        string cV61Configuration = Subline.Create(new List<uint>{61});


        // ZIMO: SHUNTING key in CV155 (ZIMO_FUNCKEY_SHUNTINGKEY_CV155)
        [ObservableProperty]
        int zIMOFuncKeysShuntingKeyNumber = 0;
        partial void OnZIMOFuncKeysShuntingKeyNumberChanged(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte tempValue = DataStore.DecoderConfiguration.ZIMO.ShuntingKeyAndShuntingSpeed;
                
                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                tempValue = (byte)(tempValue & 0xE0);

                //  Now we add the new function output number.  
                tempValue += (byte)value;

                DataStore.DecoderConfiguration.ZIMO.ShuntingKeyAndShuntingSpeed = (byte)tempValue;
            }
            CV155Configuration = Subline.Create(new List<uint> { 155 });
        }
        [ObservableProperty]
        string cV155Configuration = Subline.Create(new List<uint>{155});



        // ZIMO: ABV key in CV156 (ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156)
        [ObservableProperty]
        internal ObservableCollection<string> availableFunctionKeys;

        [ObservableProperty]
        int zIMOFuncKeysAccDecDisableFuncKeyNumber;
        partial void OnZIMOFuncKeysAccDecDisableFuncKeyNumberChanged(int value)
        {
            if (value == -1)
            {
                DataStore.DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber = 0;
            }
            else
            {
                DataStore.DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber = (byte)value;
            }
            CV156Configuration = Subline.Create(new List<uint> { 156 });
        }

        [ObservableProperty]
        string cV156Configuration = Subline.Create(new List<uint>{156});


        // Döhler & Haass: Function key mapping type in CV137 (DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137)
        [ObservableProperty]
        bool doehlerAndHaassExtendedFunctionMappingEnabled;
        partial void OnDoehlerAndHaassExtendedFunctionMappingEnabledChanged(bool value)
        {
            RCN225StandardFunctionMapping = !value;
            DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled = value;
            CV137Configuration = Subline.Create(new List<uint>{137});
        }

        [ObservableProperty]
        string cV137Configuration = Subline.Create(new List<uint>{137});

        // Döhler & Haass: Shunting key in CV133 (DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132)
        [ObservableProperty]
        int doehlerAndHaassFuncKeysShuntingFuncKeyNumber;
        partial void OnDoehlerAndHaassFuncKeysShuntingFuncKeyNumberChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber = 0;
            }
            else
            {
                DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber = (byte)value;
            }
            CV132Configuration = Subline.Create(new List<uint> { 132 });
        }

        [ObservableProperty]
        string cV132Configuration = Subline.Create(new List<uint>{132});

        // Döhler & Haass: ABV key in CV133 (DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133)
        [ObservableProperty]
        int doehlerAndHaassFuncKeysAccDecDisableFuncKeyNumber;
        partial void OnDoehlerAndHaassFuncKeysAccDecDisableFuncKeyNumberChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber = 0;
            }
            else
            {
                DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber = (byte)value;
            }
            CV133Configuration = Subline.Create(new List<uint> { 133 });
        }

        [ObservableProperty]
        string cV133Configuration = Subline.Create(new List<uint>{133});

        //  ZIMO: Sound mute in CV313 (ZIMO_FUNCKEY_MUTE_CV313)
        [ObservableProperty]
        int zIMOFuncKeysMute;
        partial void OnZIMOFuncKeysMuteChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.ZIMO.FuncKeyNrMute = 0;

            }
            else
            {
                if (ZIMOFuncKeysMuteInverted == false)
                {
                    DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)value;
                }
                else
                {
                    DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)(value + 100);
                }

            }
            CV313Configuration = Subline.Create(new List<uint> { 313 });
        }

        [ObservableProperty]
        string cV313Configuration = Subline.Create(new List<uint>{313});

        [ObservableProperty]
        bool zIMOFuncKeysMuteInverted;

        partial void OnZIMOFuncKeysMuteInvertedChanged(bool value)
        {
            if(value == true)            
            {
                DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)(ZIMOFuncKeysMute + 100);
            }
            else
            {
                DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)ZIMOFuncKeysMute;
            }
            CV313Configuration = Subline.Create(new List<uint> { 313 });
        }

        // ZIMO: Sound on and off CV310 (ZIMO_FUNCKEY_SOUNDALLOFF_CV310)
        [ObservableProperty]
        int zIMOFuncKeysSoundOnOff;
        partial void OnZIMOFuncKeysSoundOnOffChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundOnOff = 0;

            }
            else
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundOnOff = (byte)value;
                CV310Configuration = Subline.Create(new List<uint>{310});
            }
        }
        [ObservableProperty]
        string cV310Configuration = Subline.Create(new List<uint>{310});        

        // ZIMO: Sound louder in CV397 (ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397) 
        [ObservableProperty]
        int zIMOFuncKeysSoundVolumeLouder;
        partial void OnZIMOFuncKeysSoundVolumeLouderChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeLouder = 0;

            }
            else
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeLouder = (byte)value;

            }
            CV397Configuration = Subline.Create(new List<uint>{397});
        }

        [ObservableProperty]
        string cV397Configuration = Subline.Create(new List<uint>{397});

        // ZIMO: Sound quieter in CV396 (ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396
        [ObservableProperty]
        int zIMOFuncKeysSoundVolumeQuieter;
        partial void OnZIMOFuncKeysSoundVolumeQuieterChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeQuieter = 0;
            }
            else
            {
                DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeQuieter = (byte)value;

            }
            CV396Configuration = Subline.Create(new List<uint>{396});
        }

        [ObservableProperty]
        string cV396Configuration = Subline.Create(new List<uint>{396});


        // ZIMO: Light suppression            
        [ObservableProperty]
        internal ObservableCollection<string> availableFunctionOutputsLightSuppress;

        // ZIMO: Light suppression on the driver's cab side 1 (forward) in
        // CV107 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X).
        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide1;
        partial void OnZIMOFuncKeyLightSuppresionCabSide1Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;

                //  We mask out the current function key number. We leave the
                //  current function output number untouched. 
                currentSetting = (byte)(currentSetting & 0xE0);

                //  Now we add the new function key number.
                currentSetting += (byte)value;

                //  We write the new value to the configuration.
                DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward = currentSetting;

            }
            CV107And109Configuration = Subline.Create(new List<uint>{107,109});  
        }
        
        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide1Output;
        partial void OnZIMOFuncKeyLightSuppresionCabSide1OutputChanged(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0x1F);

                //  Now we add the new function output number.  
                currentSetting += (byte)(value * 32);

                //  We write the new value to the configuration.
                DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward = currentSetting;
            }
            CV107And109Configuration = Subline.Create(new List<uint> { 107, 109 });
        }

        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide1Output3;
        partial void OnZIMOFuncKeyLightSuppresionCabSide1Output3Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0xF8);

                //  Now we add the new function output number.  
                currentSetting += (byte)value;

                DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward = (byte)currentSetting;
            }
            CV107And109Configuration = Subline.Create(new List<uint>{107,109});
        }

        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide1Output4;
        partial void OnZIMOFuncKeyLightSuppresionCabSide1Output4Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0xC7);

                currentSetting += (byte)(value * 8);

                //  Now we add the new function output number.  
                currentSetting += (byte)value;

                DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward = (byte)currentSetting;
            }
            CV107And109Configuration = Subline.Create(new List<uint>{107,109});
        }

        [ObservableProperty]
        string cV107And109Configuration = Subline.Create(new List<uint>{107,109});


        // ZIMO: Light suppression on the driver's cab side 2 (backward) in
        // CV108 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X).
        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide2;
        partial void OnZIMOFuncKeyLightSuppresionCabSide2Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;

                //  We mask out the current function key number. We leave the
                //  current function output number untouched. 
                currentSetting = (byte)(currentSetting & 0xE0);

                //  Now we add the new function key number.
                currentSetting += (byte)value;

                //  We write the new value to the configuration.
                DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward = currentSetting;

            }
            CV108And110Configuration = Subline.Create(new List<uint>{108,110});  
        }
        
        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide2Output;
        partial void OnZIMOFuncKeyLightSuppresionCabSide2OutputChanged(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0x1F);

                //  Now we add the new function output number.  
                currentSetting += (byte)(value * 32);

                //  We write the new value to the configuration.
                DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward = currentSetting;
            }
            CV108And110Configuration = Subline.Create(new List<uint> { 108, 110 });
        }

        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide2Output3;
        partial void OnZIMOFuncKeyLightSuppresionCabSide2Output3Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0xF8);

                //  Now we add the new function output number.  
                currentSetting += (byte)value;

                DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward = (byte)currentSetting;
            }
            CV108And110Configuration = Subline.Create(new List<uint>{108,110});
        }

        [ObservableProperty]
        int zIMOFuncKeyLightSuppresionCabSide2Output4;
        partial void OnZIMOFuncKeyLightSuppresionCabSide2Output4Changed(int value)
        {
            if (value == -1)
            {
                return;
            }
            else
            {
                byte currentSetting = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;

                //  We mask out the current function output number. We leave the
                //  current function key number untouched. 
                currentSetting = (byte)(currentSetting & 0xC7);

                currentSetting += (byte)(value * 8);

                //  Now we add the new function output number.  
                currentSetting += (byte)value;

                DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward = (byte)currentSetting;
            }
            CV108And110Configuration = Subline.Create(new List<uint>{108,110});
        }

        [ObservableProperty]
        string cV108And110Configuration = Subline.Create(new List<uint>{108,110});


        // ZIMO: Curve squeal in CV308 (ZIMO_FUNCKEY_CURVESQUEAL_CV308)
        [ObservableProperty]
        int zIMOFuncKeysCurveSqueal;
        partial void OnZIMOFuncKeysCurveSquealChanged(int value)
        {
            if (value == -1)
            {
                DecoderConfiguration.ZIMO.FuncKeyNrCurveSqueal = 0;
            }
            else
            {
                DecoderConfiguration.ZIMO.FuncKeyNrCurveSqueal = (byte)value;

            }
            CV308Configuration = Subline.Create(new List<uint>{308});
        }
        [ObservableProperty]
        string cV308Configuration = Subline.Create(new List<uint>{308});

        #endregion

        #region REGION: CONSTRUCTOR

        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public FunctionKeysViewModel()
        {
            AvailableFunctionKeys = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableFunctionKeys(true));
            AvailableFunctionOutputsLightSuppress = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableFunctionOutputs(1, 7, true));

            ZIMOInputMappingCVs = _ZIMOInputMappingCVs;

            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDecoderConfiguration();
                });
            });

            WeakReferenceMessenger.Default.Register<DecoderSpecificationUpdatedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDecoderSpecification();
                });
            });

        }

        #endregion

        // Opens a pop-up window so that the user can configure the input mapping of the function keys.
        #region REGION: COMMANDS

        /// <summary>   
        /// Reset the function output mapping in CV33 to CV46 to default values.
        /// </summary>
        [RelayCommand]
        async Task ResetOutputMappingCV33CV46()
        {
            try
            {
                //  We ask the user whether they want to delete the configuration.
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.AlertResetFunctionMapping, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }

                //  We delete the configuration.
                CV33ValueBit0 = true; CV33ValueBit1 = false; CV33ValueBit2 = false; CV33ValueBit3 = false; CV33ValueBit4 = false; CV33ValueBit5 = false; CV33ValueBit6 = false; CV33ValueBit7 = false; 
                CV34ValueBit0 = false; CV34ValueBit1 = true; CV34ValueBit2 = false; CV34ValueBit3 = false; CV34ValueBit4 = false; CV34ValueBit5 = false; CV34ValueBit6 = false; CV34ValueBit7 = false;
                CV35ValueBit0 = false; CV35ValueBit1 = false; CV35ValueBit2 = true; CV35ValueBit3 = false; CV35ValueBit4 = false; CV35ValueBit5 = false; CV35ValueBit6 = false; CV35ValueBit7 = false;
                CV36ValueBit0 = false; CV36ValueBit1 = false; CV36ValueBit2 = false; CV36ValueBit3 = true; CV36ValueBit4 = false; CV36ValueBit5 = false; CV36ValueBit6 = false; CV36ValueBit7 = false;
                CV37ValueBit0 = false; CV37ValueBit1 = true; CV37ValueBit2 = false; CV37ValueBit3 = false; CV37ValueBit4 = false; CV37ValueBit5 = false; CV37ValueBit6 = false; CV37ValueBit7 = false;
                CV38ValueBit0 = false; CV38ValueBit1 = false; CV38ValueBit2 = true; CV38ValueBit3 = false; CV38ValueBit4 = false; CV38ValueBit5 = false; CV38ValueBit6 = false; CV38ValueBit7 = false;
                CV39ValueBit0 = false; CV39ValueBit1 = false; CV39ValueBit2 = false; CV39ValueBit3 = true; CV39ValueBit4 = false; CV39ValueBit5 = false; CV39ValueBit6 = false; CV39ValueBit7 = false;
                CV40ValueBit0 = false; CV40ValueBit1 = false; CV40ValueBit2 = false; CV40ValueBit3 = false; CV40ValueBit4 = true; CV40ValueBit5 = false; CV40ValueBit6 = false; CV40ValueBit7 = false;
                CV41ValueBit0 = false; CV41ValueBit1 = false; CV41ValueBit2 = true; CV41ValueBit3 = false; CV41ValueBit4 = false; CV41ValueBit5 = false; CV41ValueBit6 = false; CV41ValueBit7 = false;
                CV42ValueBit0 = false; CV42ValueBit1 = false; CV42ValueBit2 = false; CV42ValueBit3 = true; CV42ValueBit4 = false; CV42ValueBit5 = false; CV42ValueBit6 = false; CV42ValueBit7 = false;
                CV43ValueBit0 = false; CV43ValueBit1 = false; CV43ValueBit2 = false; CV43ValueBit3 = false; CV43ValueBit4 = true; CV43ValueBit5 = false; CV43ValueBit6 = false; CV43ValueBit7 = false;
                CV44ValueBit0 = false; CV44ValueBit1 = false; CV44ValueBit2 = false; CV44ValueBit3 = false; CV44ValueBit4 = false; CV44ValueBit5 = true; CV44ValueBit6 = false; CV44ValueBit7 = false;
                CV45ValueBit0 = false; CV45ValueBit1 = false; CV45ValueBit2 = false; CV45ValueBit3 = false; CV45ValueBit4 = false; CV45ValueBit5 = false; CV45ValueBit6 = true; CV45ValueBit7 = false;
                CV46ValueBit0 = false; CV46ValueBit1 = false; CV46ValueBit2 = false; CV46ValueBit3 = false; CV46ValueBit4 = false; CV46ValueBit5 = false; CV46ValueBit6 = false; CV46ValueBit7 = true;
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }
    

        /// <summary>
        /// This functions deletes the output mapping of the function keys F1 to F12 in CV33 to CV46.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task DeleteOutputMappingCV33CV46()
        {
            try
            {
                //  We ask the user whether they want to delete the configuration.
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.AlertDeleteFunctionMapping, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }

                //  We delete the configuration.
                CV33ValueBit0 = false; CV33ValueBit1 = false; CV33ValueBit2 = false; CV33ValueBit3 = false; CV33ValueBit4 = false; CV33ValueBit5 = false; CV33ValueBit6 = false; CV33ValueBit7 = false; 
                CV34ValueBit0 = false; CV34ValueBit1 = false; CV34ValueBit2 = false; CV34ValueBit3 = false; CV34ValueBit4 = false; CV34ValueBit5 = false; CV34ValueBit6 = false; CV34ValueBit7 = false;
                CV35ValueBit0 = false; CV35ValueBit1 = false; CV35ValueBit2 = false; CV35ValueBit3 = false; CV35ValueBit4 = false; CV35ValueBit5 = false; CV35ValueBit6 = false; CV35ValueBit7 = false;
                CV36ValueBit0 = false; CV36ValueBit1 = false; CV36ValueBit2 = false; CV36ValueBit3 = false; CV36ValueBit4 = false; CV36ValueBit5 = false; CV36ValueBit6 = false; CV36ValueBit7 = false;
                CV37ValueBit0 = false; CV37ValueBit1 = false; CV37ValueBit2 = false; CV37ValueBit3 = false; CV37ValueBit4 = false; CV37ValueBit5 = false; CV37ValueBit6 = false; CV37ValueBit7 = false;
                CV38ValueBit0 = false; CV38ValueBit1 = false; CV38ValueBit2 = false; CV38ValueBit3 = false; CV38ValueBit4 = false; CV38ValueBit5 = false; CV38ValueBit6 = false; CV38ValueBit7 = false;
                CV39ValueBit0 = false; CV39ValueBit1 = false; CV39ValueBit2 = false; CV39ValueBit3 = false; CV39ValueBit4 = false; CV39ValueBit5 = false; CV39ValueBit6 = false; CV39ValueBit7 = false;
                CV40ValueBit0 = false; CV40ValueBit1 = false; CV40ValueBit2 = false; CV40ValueBit3 = false; CV40ValueBit4 = false; CV40ValueBit5 = false; CV40ValueBit6 = false; CV40ValueBit7 = false;
                CV41ValueBit0 = false; CV41ValueBit1 = false; CV41ValueBit2 = false; CV41ValueBit3 = false; CV41ValueBit4 = false; CV41ValueBit5 = false; CV41ValueBit6 = false; CV41ValueBit7 = false;
                CV42ValueBit0 = false; CV42ValueBit1 = false; CV42ValueBit2 = false; CV42ValueBit3 = false; CV42ValueBit4 = false; CV42ValueBit5 = false; CV42ValueBit6 = false; CV42ValueBit7 = false;
                CV43ValueBit0 = false; CV43ValueBit1 = false; CV43ValueBit2 = false; CV43ValueBit3 = false; CV43ValueBit4 = false; CV43ValueBit5 = false; CV43ValueBit6 = false; CV43ValueBit7 = false;
                CV44ValueBit0 = false; CV44ValueBit1 = false; CV44ValueBit2 = false; CV44ValueBit3 = false; CV44ValueBit4 = false; CV44ValueBit5 = false; CV44ValueBit6 = false; CV44ValueBit7 = false;
                CV45ValueBit0 = false; CV45ValueBit1 = false; CV45ValueBit2 = false; CV45ValueBit3 = false; CV45ValueBit4 = false; CV45ValueBit5 = false; CV45ValueBit6 = false; CV45ValueBit7 = false;
                CV46ValueBit0 = false; CV46ValueBit1 = false; CV46ValueBit2 = false; CV46ValueBit3 = false; CV46ValueBit4 = false; CV46ValueBit5 = false; CV46ValueBit6 = false; CV46ValueBit7 = false;
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        [RelayCommand]
        async Task EditInputMapping()
        {
            try
            {
                //  Check if a function key has been selected, if not create a messagebox to inform the user.
                if (SelectedInputMapping == null)
                {                    
                    await MessageBox.Show(AppResources.AlertError, AppResources.FrameFunctionKeysZIMONoMappingSelected, AppResources.OK);
                    return;
                }

                // Open a pop-up window to edit the input mapping of the selected function key.
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;
                PopUpEditInputMapping pop = new PopUpEditInputMapping(cancelTokenSource, SelectedInputMapping!);
                await Shell.Current.CurrentPage.ShowPopupAsync(pop);

                OnPropertyChanged("ZIMOInputMappingCVs");

                ZIMOInputMappingCVs = null;
                ZIMOInputMappingCVs = _ZIMOInputMappingCVs;
                WriteZIMOInputMappingListToDecoderConfiguration();
            
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Writes the current input mapping configuration to the decoder configuration.
        /// </summary>
        private void WriteZIMOInputMappingListToDecoderConfiguration()
        {
            try
            {
                if (_ZIMOInputMappingCVs == null) return;

                foreach (ZIMOInputMappingType item in _ZIMOInputMappingCVs)
                {
                    switch (item.CVNumber)
                    {
                        case 400: DecoderConfiguration.ZIMO.InputMappingF0 = item; break;
                        case 401: DecoderConfiguration.ZIMO.InputMappingF1 = item; break;
                        case 402: DecoderConfiguration.ZIMO.InputMappingF2 = item; break;
                        case 403: DecoderConfiguration.ZIMO.InputMappingF3 = item; break;
                        case 404: DecoderConfiguration.ZIMO.InputMappingF4 = item; break;
                        case 405: DecoderConfiguration.ZIMO.InputMappingF5 = item; break;
                        case 406: DecoderConfiguration.ZIMO.InputMappingF6 = item; break;
                        case 407: DecoderConfiguration.ZIMO.InputMappingF7 = item; break;
                        case 408: DecoderConfiguration.ZIMO.InputMappingF8 = item; break;
                        case 409: DecoderConfiguration.ZIMO.InputMappingF9 = item; break;
                        case 410: DecoderConfiguration.ZIMO.InputMappingF10 = item; break;
                        case 411: DecoderConfiguration.ZIMO.InputMappingF11 = item; break;
                        case 412: DecoderConfiguration.ZIMO.InputMappingF12 = item; break;
                        case 413: DecoderConfiguration.ZIMO.InputMappingF13 = item; break;
                        case 414: DecoderConfiguration.ZIMO.InputMappingF14 = item; break;
                        case 415: DecoderConfiguration.ZIMO.InputMappingF15 = item; break;
                        case 416: DecoderConfiguration.ZIMO.InputMappingF16 = item; break;
                        case 417: DecoderConfiguration.ZIMO.InputMappingF17 = item; break;
                        case 418: DecoderConfiguration.ZIMO.InputMappingF18 = item; break;
                        case 419: DecoderConfiguration.ZIMO.InputMappingF19 = item; break;
                        case 420: DecoderConfiguration.ZIMO.InputMappingF20 = item; break;
                    }
                }
            }
            catch 
            {
                return;
            }

        }

        /// <summary>
        /// Updates the ZIMO input mapping list with the current configuration variable values.
        /// </summary>
        private void UpdateZIMOInputMappingList()
        {
            if (_ZIMOInputMappingCVs == null) return;
            
            _ZIMOInputMappingCVs.Clear();

            ZIMOInputMappingType itemF0 = new ZIMOInputMappingType();
            itemF0 = DecoderConfiguration.ZIMO.InputMappingF0;
            _ZIMOInputMappingCVs.Add(itemF0);

            ZIMOInputMappingType itemF1 = new ZIMOInputMappingType();
            itemF1 = DecoderConfiguration.ZIMO.InputMappingF1;
            _ZIMOInputMappingCVs.Add(itemF1);

            ZIMOInputMappingType itemF2 = new ZIMOInputMappingType();
            itemF2 = DecoderConfiguration.ZIMO.InputMappingF2;
            _ZIMOInputMappingCVs.Add(itemF2);

            ZIMOInputMappingType itemF3 = new ZIMOInputMappingType();
            itemF3 = DecoderConfiguration.ZIMO.InputMappingF3;
            _ZIMOInputMappingCVs.Add(itemF3);

            ZIMOInputMappingType itemF4 = new ZIMOInputMappingType();
            itemF4 = DecoderConfiguration.ZIMO.InputMappingF4;
            _ZIMOInputMappingCVs.Add(itemF4);

            ZIMOInputMappingType itemF5 = new ZIMOInputMappingType();
            itemF5 = DecoderConfiguration.ZIMO.InputMappingF5;
            _ZIMOInputMappingCVs.Add(itemF5);

            ZIMOInputMappingType itemF6 = new ZIMOInputMappingType();
            itemF6 = DecoderConfiguration.ZIMO.InputMappingF6;
            _ZIMOInputMappingCVs.Add(itemF6);

            ZIMOInputMappingType itemF7 = new ZIMOInputMappingType();
            itemF7 = DecoderConfiguration.ZIMO.InputMappingF7;
            _ZIMOInputMappingCVs.Add(itemF7);

            ZIMOInputMappingType itemF8 = new ZIMOInputMappingType();
            itemF8 = DecoderConfiguration.ZIMO.InputMappingF8;
            _ZIMOInputMappingCVs.Add(itemF8);

            ZIMOInputMappingType itemF9 = new ZIMOInputMappingType();
            itemF9 = DecoderConfiguration.ZIMO.InputMappingF9;
            _ZIMOInputMappingCVs.Add(itemF9);

            ZIMOInputMappingType itemF10 = new ZIMOInputMappingType();
            itemF10 = DecoderConfiguration.ZIMO.InputMappingF10;
            _ZIMOInputMappingCVs.Add(itemF10);

            ZIMOInputMappingType itemF11 = new ZIMOInputMappingType();
            itemF11 = DecoderConfiguration.ZIMO.InputMappingF11;
            _ZIMOInputMappingCVs.Add(itemF11);

            ZIMOInputMappingType itemF12 = new ZIMOInputMappingType();
            itemF12 = DecoderConfiguration.ZIMO.InputMappingF12;
            _ZIMOInputMappingCVs.Add(itemF12);

            ZIMOInputMappingType itemF13 = new ZIMOInputMappingType();
            itemF13 = DecoderConfiguration.ZIMO.InputMappingF13;
            _ZIMOInputMappingCVs.Add(itemF13);

            ZIMOInputMappingType itemF14 = new ZIMOInputMappingType();
            itemF14 = DecoderConfiguration.ZIMO.InputMappingF14;
            _ZIMOInputMappingCVs.Add(itemF14);

            ZIMOInputMappingType itemF15 = new ZIMOInputMappingType();
            itemF15 = DecoderConfiguration.ZIMO.InputMappingF15;
            _ZIMOInputMappingCVs.Add(itemF15);

            ZIMOInputMappingType itemF16 = new ZIMOInputMappingType();
            itemF16 = DecoderConfiguration.ZIMO.InputMappingF16;
            _ZIMOInputMappingCVs.Add(itemF16);

            ZIMOInputMappingType itemF17 = new ZIMOInputMappingType();
            itemF17 = DecoderConfiguration.ZIMO.InputMappingF17;
            _ZIMOInputMappingCVs.Add(itemF17);

            ZIMOInputMappingType itemF18 = new ZIMOInputMappingType();
            itemF18 = DecoderConfiguration.ZIMO.InputMappingF18;
            _ZIMOInputMappingCVs.Add(itemF18);

            ZIMOInputMappingType itemF19 = new ZIMOInputMappingType();
            itemF19 = DecoderConfiguration.ZIMO.InputMappingF19;
            _ZIMOInputMappingCVs.Add(itemF19);

            ZIMOInputMappingType itemF20 = new ZIMOInputMappingType();
            itemF20 = DecoderConfiguration.ZIMO.InputMappingF20;
            _ZIMOInputMappingCVs.Add(itemF20);


        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            SoundFunctionKeysSupported = DeqSpecReader.AnySoundFunctionKeysSupported(DecoderSpecification.DeqSpecName);
            DriveAndMotorCharacteristicsKeysSupported = DeqSpecReader.AnyDriveAndMotorCharacteristicKeysSupported(DecoderSpecification.DeqSpecName);

            ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 = DecoderSpecification.ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156;
            RCN225_FUNCTIONKEYMAPPING_CV3346 = DecoderSpecification.RCN225_FUNCTIONKEYMAPPING_CV3346;
            ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61 = DecoderSpecification.ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61;
            DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137 = DecoderSpecification.DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137;
            DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 = DecoderSpecification.DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133;
            DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 = DecoderSpecification.DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132;
            ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 = DecoderSpecification.ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397;
            ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396  = DecoderSpecification.ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396;
            ZIMO_FUNCKEY_SOUNDALLOFF_CV310 = DecoderSpecification.ZIMO_FUNCKEY_SOUNDALLOFF_CV310;
            ZIMO_FUNCKEY_CURVESQUEAL_CV308 = DecoderSpecification.ZIMO_FUNCKEY_CURVESQUEAL_CV308;
            ZIMO_FUNCKEY_MUTE_CV313 = DecoderSpecification.ZIMO_FUNCKEY_MUTE_CV313;
            ZIMO_INPUTMAPPING_CV4XX = DecoderSpecification.ZIMO_INPUTMAPPING_CV4XX;
            ZIMO_MXFXFUNCTIONKEYMAPPING_CV3346 = DecoderSpecification.ZIMO_MXFXFUNCTIONKEYMAPPING_CV3346;
            ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X = DecoderSpecification.ZIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X;
            ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X = DecoderSpecification.ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X;
            ZIMO_FUNCKEY_SHUNTINGKEY_CV155 = DecoderSpecification.ZIMO_FUNCKEY_SHUNTINGKEY_CV155;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // We configure the descriptions of the function outputs. If user-specific names are available, these are used.
            Output0vDescription = DecoderConfiguration.UserDefinedFunctionOutputNames[0].Description == "" ? "0v" : DecoderConfiguration.UserDefinedFunctionOutputNames[0].Description;
            Output0rDescription = DecoderConfiguration.UserDefinedFunctionOutputNames[1].Description == "" ? "0r" : DecoderConfiguration.UserDefinedFunctionOutputNames[1].Description;
            Output1Description = DecoderConfiguration.UserDefinedFunctionOutputNames[2].Description == "" ? "1" : DecoderConfiguration.UserDefinedFunctionOutputNames[2].Description;
            Output2Description = DecoderConfiguration.UserDefinedFunctionOutputNames[3].Description == "" ? "2" : DecoderConfiguration.UserDefinedFunctionOutputNames[3].Description;
            Output3Description = DecoderConfiguration.UserDefinedFunctionOutputNames[4].Description == "" ? "3" : DecoderConfiguration.UserDefinedFunctionOutputNames[4].Description;
            Output4Description = DecoderConfiguration.UserDefinedFunctionOutputNames[5].Description == "" ? "4" : DecoderConfiguration.UserDefinedFunctionOutputNames[5].Description;
            Output5Description = DecoderConfiguration.UserDefinedFunctionOutputNames[6].Description == "" ? "5" : DecoderConfiguration.UserDefinedFunctionOutputNames[6].Description;
            Output6Description = DecoderConfiguration.UserDefinedFunctionOutputNames[7].Description == "" ? "6" : DecoderConfiguration.UserDefinedFunctionOutputNames[7].Description;
            Output7Description = DecoderConfiguration.UserDefinedFunctionOutputNames[8].Description == "" ? "7" : DecoderConfiguration.UserDefinedFunctionOutputNames[8].Description;
            Output8Description = DecoderConfiguration.UserDefinedFunctionOutputNames[9].Description == "" ? "8" : DecoderConfiguration.UserDefinedFunctionOutputNames[9].Description;
            Output9Description = DecoderConfiguration.UserDefinedFunctionOutputNames[10].Description == "" ? "9" : DecoderConfiguration.UserDefinedFunctionOutputNames[10].Description;
            Output10Description = DecoderConfiguration.UserDefinedFunctionOutputNames[11].Description == "" ? "10" : DecoderConfiguration.UserDefinedFunctionOutputNames[11].Description;
            Output11Description = DecoderConfiguration.UserDefinedFunctionOutputNames[12].Description == "" ? "11" : DecoderConfiguration.UserDefinedFunctionOutputNames[12].Description;
            Output12Description = DecoderConfiguration.UserDefinedFunctionOutputNames[13].Description == "" ? "12" : DecoderConfiguration.UserDefinedFunctionOutputNames[13].Description;

            //  RCN 225
            CV33Value = DecoderConfiguration.RCN225.FunctionMappingF0Forward;
            CV34Value = DecoderConfiguration.RCN225.FunctionMappingF0Backward;
            CV35Value = DecoderConfiguration.RCN225.FunctionMappingF1;
            CV36Value = DecoderConfiguration.RCN225.FunctionMappingF2;
            CV37Value = DecoderConfiguration.RCN225.FunctionMappingF3;
            CV38Value = DecoderConfiguration.RCN225.FunctionMappingF4;
            CV39Value = DecoderConfiguration.RCN225.FunctionMappingF5;
            CV40Value = DecoderConfiguration.RCN225.FunctionMappingF6;
            CV41Value = DecoderConfiguration.RCN225.FunctionMappingF7;
            CV42Value = DecoderConfiguration.RCN225.FunctionMappingF8;
            CV43Value = DecoderConfiguration.RCN225.FunctionMappingF9;
            CV44Value = DecoderConfiguration.RCN225.FunctionMappingF10;
            CV45Value = DecoderConfiguration.RCN225.FunctionMappingF11;
            CV46Value = DecoderConfiguration.RCN225.FunctionMappingF12;
            for (byte cv = 33; cv <= 46; cv++) SetBitsOfCV(cv);
            
            
            //  Döhler and Haass
            DoehlerAndHaassExtendedFunctionMappingEnabled = DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled;
            DoehlerAndHaassFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber;
            DoehlerAndHaassFuncKeysShuntingFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber;

            //  ZIMO
            ZIMOFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber;
            ZIMOExtendedFunctionMapping = DecoderConfiguration.ZIMO.ExtendedFunctionKeyMapping;
            ZIMOFuncKeysSoundVolumeLouder = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeLouder;
            ZIMOFuncKeysSoundVolumeQuieter = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeQuieter;
            ZIMOFuncKeysSoundOnOff = DecoderConfiguration.ZIMO.FuncKeyNrSoundOnOff;
            ZIMOFuncKeysCurveSqueal = DecoderConfiguration.ZIMO.FuncKeyNrCurveSqueal;

            byte tempfunctionKey = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1 = tempfunctionKey &= 0x1F;

            byte tempFunctionOutput = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output = (tempFunctionOutput &= 0xE0) >> 5;

            tempFunctionOutput = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output3 = tempFunctionOutput &= 0x07;

            tempFunctionOutput = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output4 = (tempFunctionOutput &= 0xF8) >> 3;

            byte tempfunctionKey2 = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2 = tempfunctionKey2 &= 0x1F;

            byte tempFunctionOutput2 = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output = (tempFunctionOutput2 &= 0xE0) >> 5;

            tempFunctionOutput2 = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output3 = tempFunctionOutput2 &= 0x07;

            tempFunctionOutput2 = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output4 = (tempFunctionOutput2 &= 0xF8) >> 3;

            tempfunctionKey = DecoderConfiguration.ZIMO.ShuntingKeyAndShuntingSpeed;
            ZIMOFuncKeysShuntingKeyNumber = tempfunctionKey & 0x1F;

            if (DecoderConfiguration.ZIMO.FuncKeyNrMute > 100)
            {
                ZIMOFuncKeysMute = DecoderConfiguration.ZIMO.FuncKeyNrMute - 100;
                ZIMOFuncKeysMuteInverted = true;
            }
            else
            {
                ZIMOFuncKeysMute = DecoderConfiguration.ZIMO.FuncKeyNrMute;
                ZIMOFuncKeysMuteInverted = false;
            }
            UpdateZIMOInputMappingList();
        }

        /// <summary>
        /// Sets the the bit properties of the given configuration variable number.
        /// </summary>
        /// <param name="number">The number of the desired configuration variable.</param>
        private void SetBitsOfCV(byte number)
        {
            if (number == 0) return;

            //  Use reflection to grab the property of the desired configuration variable.
            Type typeCV = this.GetType();
            PropertyInfo prop = typeCV.GetProperty("CV" + number.ToString() + "Value")!;
            if (prop == null)
            {
                Logger.PrintDevConsole("GetBitsOfCV: Unable to get the property CV" + number.ToString() + "Value");
                return;
            }
            
            //  Get the current value of the desired configuration variable.
            ushort ValueCV = (ushort)prop.GetValue(this, null)!; 

            //  Loop trough all 8 bits of the desired configuration variable.
            for (int i = 0; i <= 7; i++)
            {
                //  Grab the property of the desired bit of the configuration variable.
                string BitsPropertyName = "CV" + number.ToString() + "ValueBit" + i.ToString();
                Type type = this.GetType();
                PropertyInfo propInfo = type.GetProperty(BitsPropertyName)!;
                if (propInfo == null)
                {
                    Logger.PrintDevConsole("GetBitsOfCV: Unable to get the property " +  BitsPropertyName);
                    return;
                }

                //  Set the state of the desired bit in the configuration variable.
                Logger.PrintDevConsole("GetBitsOfCV: Setting property CV" + number.ToString() + "Value to value " + ValueCV.ToString());
                propInfo.SetValue(this, Bit.IsSet(ValueCV , i), null);
            }
        
        }
      
        #endregion

    }
}
