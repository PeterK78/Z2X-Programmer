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

using CommunityToolkit.Maui.Extensions;
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
    public partial class FunctionKeysViewModel : ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

        // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1;

        // AnyFunctionOnThisPageSupported is TRUE if the current decoder specification supports any function on this page.
        [ObservableProperty]
        bool anyFunctionOnThisPageSupported;

        #endregion

        #region REGION: DECODER FEATURES

        //  Any sound function keys supported?
        [ObservableProperty]
        bool soundFunctionKeysSupported = false;

        //  Any drive- an motor characeteristics related keys supported?
        [ObservableProperty]
        bool driveAndMotorCharacteristicsKeysSupported = false;

        // Any function output mapping functions supported?
        [ObservableProperty]
        bool functionOuputMappingSupported = false;

        #region Döhler & Haass

        //  Döhler & Haass: The extended function output mapping in CV137
        //[ObservableProperty]
        //bool dOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137 = false;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYSHUNTING_CV132;

        #endregion

        #region RCN225

        //  RCN225: Standard function key mapping in CV33 to CV46
        [ObservableProperty]
        bool rCN225_FUNCTIONOUTPUTMAPPING_CV3346 = false;

        #endregion

        #region ZIMO

        //  ZIMO: The standard and extended function output mapping (CV61)
        [ObservableProperty]
        bool zIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61;

        // ZIMO: ABV key in CV156 (ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156)
        [ObservableProperty]
        bool zIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156;

        //  ZIMO: Shunting function key in CV155 (ZIMO_FUNCKEY_SHUNTINGKEY_CV155)
        [ObservableProperty]
        bool zIMO_FUNCKEY_SHUNTINGKEY_CV155 = false;

        // ZIMO: Light suppression on the driver's cab side in CV107, CV108, CV109 and CV110 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X)
        [ObservableProperty]
        bool zIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X = false;

        [ObservableProperty]
        bool zIMO_INPUTMAPPING_CV4XX;

        [ObservableProperty]
        bool zIMO_FUNCKEY_MUTE_CV313;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396;

        [ObservableProperty]
        bool zIMO_FUNCKEY_SOUNDALLOFF_CV310;

        [ObservableProperty]
        bool zIMO_FUNCKEY_CURVESQUEAL_CV308;

        //  ZIMO: Function keys for high beam and dimming in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        [ObservableProperty]
        bool zIMO_FUNCKEY_HIGHBEAMDIPPEDBEAM_CV119X = false;

        //  ZIMO: Function mapping in CV33 - CV46 for FX decoder
        [ObservableProperty]
        bool zIMO_MXFXFUNCTIONKEYMAPPING_CV3346 = false;

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

        #endregion

        #region REGION: PUBLIC PROPERTIES      

        [ObservableProperty]
        public ObservableCollection<FunctionKeyType> functionKeys = new(DecoderConfiguration.FunctionKeys);

        // ZIMO: ZIMO input mapping in CV400 - CV428 (ZIMO_INPUTMAPPING_CV4XX)
        // SelectedInputMapping contains the data for the currently selected input mapping (if the user edits a mapping).
        [ObservableProperty]
        internal FunctionKeyType selectedInputMapping = new();

        //  ZIMOInputMappingCVs contains all input mappings for the current decoder.
        [ObservableProperty]
        internal ObservableCollection<ZIMOInputMappingType>? zIMOInputMappingCVs = [];

        //  ZIMO: Function keys for high beam and dimming in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        #region ZIMO: Function keys for high beam and dipped beam in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
        [ObservableProperty]
        ushort cV119Value;
        partial void OnCV119ValueChanged(ushort value)
        {
            if ((value > 0) && (value != 128))
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(true,true,6, AppResources.FrameFunctionKeysHighBeamDimmingF6FuncKeyDesc);
            }
            else
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(false,true,-1, AppResources.FrameFunctionKeysHighBeamDimmingF6FuncKeyDesc);
            }
        }


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
        partial void OnCV120ValueChanged(ushort value)
        {
            if ((value > 0) && (value != 128))
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(true,true,7, AppResources.FrameFunctionKeysHighBeamDimmingF7FuncKeyDesc);
            }
            else
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(false,true,-1, AppResources.FrameFunctionKeysHighBeamDimmingF7FuncKeyDesc);
            }
        }

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

        // Döhler & Haass: Function key mapping type in CV137 (DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137)
        [ObservableProperty]
        bool doehlerAndHaassExtendedFunctionMappingEnabled;

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
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysShuntingKeyNumber != 0,true, ZIMOFuncKeysShuntingKeyNumber, AppResources.FrameFunctionKeysShuntingKeyDesc);
        }

        [ObservableProperty]
        string cV155Configuration = Subline.Create([155]);

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
            DecoderConfiguration.SetFunctionKeyFunctionDescription(value != 0,true, value, AppResources.FrameFunctionKeysDeactivateAccDecTimeFuncKeyDesc);
        }

        [ObservableProperty]
        string cV156Configuration = Subline.Create([156]);

        // Döhler & Haass: Shunting key in CV132 (DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132)
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
        string cV132Configuration = Subline.Create([132]);

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
        string cV133Configuration = Subline.Create([133]);

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
        string cV313Configuration = Subline.Create([313]);

        [ObservableProperty]
        bool zIMOFuncKeysMuteInverted;
        partial void OnZIMOFuncKeysMuteInvertedChanged(bool value)
        {
            //  If the Mute key is disabled we will not turn on the inversion.
            if (DecoderConfiguration.ZIMO.FuncKeyNrMute == 0) return;

            //  Do we need to turn on the inversion?
            if (value == true)
            {
                //  Check if we have already enabled the inversion. If yes, just return.
                if (DecoderConfiguration.ZIMO.FuncKeyNrMute >= 101) return;

                //  We enable the inversion by adding 100 to the currently configured function key nunber.                    
                DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)(DecoderConfiguration.ZIMO.FuncKeyNrMute + 100);
            }
            else
            {
                //  Check if we already have disabled the inversion. If yes, just return.
                if (DecoderConfiguration.ZIMO.FuncKeyNrMute < 100) return;

                //  We disable the inversion by writing just the number of the function key.
                DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)ZIMOFuncKeysMute;
            }
            CV313Configuration = Subline.Create(new List<uint> { 313 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysMute != 0,true, ZIMOFuncKeysMute, AppResources.FrameFunctionKeysSoundMuteDesc);        
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
                CV310Configuration = Subline.Create(new List<uint> { 310 });
            }
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysSoundOnOff != 0,true, ZIMOFuncKeysSoundOnOff, AppResources.FrameFunctionKeysSoundOnOff);        
        }
        [ObservableProperty]
        string cV310Configuration = Subline.Create([310]);

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
            CV397Configuration = Subline.Create(new List<uint> { 397 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysSoundVolumeLouder != 0,true, ZIMOFuncKeysSoundVolumeLouder, AppResources.FrameFunctionKeysSoundLouderDesc);
        }

        [ObservableProperty]
        string cV397Configuration = Subline.Create([397]);

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
            CV396Configuration = Subline.Create(new List<uint> { 396 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(zIMOFuncKeysSoundVolumeQuieter != 0,true, zIMOFuncKeysSoundVolumeQuieter, AppResources.FrameFunctionKeysSoundQuieterDesc);
        }

        [ObservableProperty]
        string cV396Configuration = Subline.Create([396]);


        // ZIMO: Light suppression            
        [ObservableProperty]
        internal ObservableCollection<string> availableFunctionOutputsLightSuppress;

        // ZIMO: Light suppression on the driver's cab side 1 (forward) in CV107 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X)
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
             CV107And109Configuration = Subline.Create([107, 109]);
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeyLightSuppresionCabSide1 !=0, true, ZIMOFuncKeyLightSuppresionCabSide1, AppResources.FrameFunctionKeysLightSuppressionDriverCabCab1Desc);
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
            CV107And109Configuration = Subline.Create([107, 109]);
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
            CV107And109Configuration = Subline.Create([107, 109]);
        }

        [ObservableProperty]
        string cV107And109Configuration = Subline.Create([107, 109]);


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
            CV108And110Configuration = Subline.Create([108, 110]);
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeyLightSuppresionCabSide2 != 0,true, ZIMOFuncKeyLightSuppresionCabSide2, AppResources.FrameFunctionKeysLightSuppressionDriverCabCab2Desc);
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
            CV108And110Configuration = Subline.Create([108, 110]);
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
            CV108And110Configuration = Subline.Create([108, 110]);
        }

        [ObservableProperty]
        string cV108And110Configuration = Subline.Create([108, 110]);


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
            CV308Configuration = Subline.Create(new List<uint> { 308 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysCurveSqueal != 0,true, ZIMOFuncKeysCurveSqueal, AppResources.FrameFunctionKeysSoundCurveSquealOnOff);        
        }
        [ObservableProperty]
        string cV308Configuration = Subline.Create([308]);

        #endregion

        #region REGION: CONSTRUCTOR

        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public FunctionKeysViewModel()
        {
            AvailableFunctionKeys = [.. NMRAEnumConverter.GetAvailableFunctionKeys(true)];
            AvailableFunctionOutputsLightSuppress = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableFunctionOutputs(1, 7, true));

            Shell.Current.Navigated += (sender, e) =>
            {
                if(e.Current.Location.ToString() == "//FunctionKeysPage")
                {
                    UpdateFunctionKeysOverview();
                }
            };

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

        #region REGION: COMMANDS

        /// <summary>
        /// Updates the function keys overview.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        void UpdateFunctionKeysOverview()
        {
            GetFunctionOutputMappingDescriptions();
        }

        /// <summary>
        /// This command opens the appropriate page for configuring the function outputs. 
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToFunctionOutputsPage()
        {
            //  Do we have to open the ZIMO specific page?
            if (ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 == true)
            {
                await Shell.Current.GoToAsync("ZIMOFunctionKeysFunctionOutputsPage");
            }
            //  Do we have to open the Döhler & Haass specific page?
            else if (DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137 == true)
            {
                await Shell.Current.GoToAsync("DoehlerAndHaassFunctionKeysFunctionOutputsPage");
            }
            //  Do we have to open the RCN225 specific page?
            else if (RCN225_FUNCTIONOUTPUTMAPPING_CV3346 == true)
            {
                await Shell.Current.GoToAsync("RCN225FunctionKeysFunctionOutputsPage");
            }

        }

        /// <summary>
        /// Resets the input mapping of all function keys to the default values.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task ResetInputMapping()
        {
            try
            {
                foreach (FunctionKeyType item in DecoderConfiguration.FunctionKeys)
                {
                    //  Reset the ZIMO input mapping of the function key.
                    item.ZIMOInputMapping.ExternalFunctionKeyNumber = 0;
                    item.ZIMOInputMapping.CVValue = 0;
                }

                //  Update the configuration variables.
                WriteZIMOInputMappingListToDecoderConfiguration();
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }   

        /// <summary>
        /// // Opens a pop-up window so that the user can configure the input mapping of the function keys.
        /// </summary>
        /// <returns></returns>
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
                CancellationTokenSource cancelTokenSource = new();
                CancellationToken cancelToken = cancelTokenSource.Token;
                PopUpEditInputMapping pop = new(cancelTokenSource, SelectedInputMapping.ZIMOInputMapping!);
                await Shell.Current.CurrentPage.ShowPopupAsync(pop);

                // Update the decoder configuration with the new settings.
                DecoderConfiguration.FunctionKeys[SelectedInputMapping.ZIMOInputMapping.InternalFunctionKeyNumber].ZIMOInputMapping = SelectedInputMapping.ZIMOInputMapping;

                //  Update the configuration variables.
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
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            // We check if the current decoder supports any function keys, any sound function keys, any drive and motor characteristics keys or any function output mapping.
            AnyFunctionOnThisPageSupported = DeqSpecReader.AnyFunctionKeySupported(DecoderSpecification.DeqSpecName);
            SoundFunctionKeysSupported = DeqSpecReader.AnySoundFunctionKeysSupported(DecoderSpecification.DeqSpecName);
            DriveAndMotorCharacteristicsKeysSupported = DeqSpecReader.AnyDriveAndMotorCharacteristicKeysSupported(DecoderSpecification.DeqSpecName);
            FunctionOuputMappingSupported = DeqSpecReader.AnyFunctionOuputMappingSupported(DecoderSpecification.DeqSpecName);

            //  Function output configuration in CV33 to CV64
            RCN225_FUNCTIONOUTPUTMAPPING_CV3346 = DecoderSpecification.RCN225_FUNCTIONOUTPUTMAPPING_CV3346;
            ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 = DecoderSpecification.ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61;
            DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137 = DecoderSpecification.DOEHLERANDHAASS_FUNCTIONOUTPUTMAPPING_EXT_CV137;

            ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156 = DecoderSpecification.ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156;
            DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133 = DecoderSpecification.DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133;
            DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132 = DecoderSpecification.DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132;
            ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397 = DecoderSpecification.ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397;
            ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396 = DecoderSpecification.ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396;
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

            //  We setup the function keys overview
            UpdateTheInputMappingList();

            // We configure the descriptions of the function outputs. If user-specific names are available, these are used.
            Output0vDescription = DecoderConfiguration.UserDefinedFunctionOutputNames[0].UserDefinedDescription == "" ? "0v" : DecoderConfiguration.UserDefinedFunctionOutputNames[0].UserDefinedDescription;
            Output0rDescription = DecoderConfiguration.UserDefinedFunctionOutputNames[1].UserDefinedDescription == "" ? "0r" : DecoderConfiguration.UserDefinedFunctionOutputNames[1].UserDefinedDescription;
            Output1Description = DecoderConfiguration.UserDefinedFunctionOutputNames[2].UserDefinedDescription == "" ? "1" : DecoderConfiguration.UserDefinedFunctionOutputNames[2].UserDefinedDescription;
            Output2Description = DecoderConfiguration.UserDefinedFunctionOutputNames[3].UserDefinedDescription == "" ? "2" : DecoderConfiguration.UserDefinedFunctionOutputNames[3].UserDefinedDescription;
            Output3Description = DecoderConfiguration.UserDefinedFunctionOutputNames[4].UserDefinedDescription == "" ? "3" : DecoderConfiguration.UserDefinedFunctionOutputNames[4].UserDefinedDescription;
            Output4Description = DecoderConfiguration.UserDefinedFunctionOutputNames[5].UserDefinedDescription == "" ? "4" : DecoderConfiguration.UserDefinedFunctionOutputNames[5].UserDefinedDescription;
            Output5Description = DecoderConfiguration.UserDefinedFunctionOutputNames[6].UserDefinedDescription == "" ? "5" : DecoderConfiguration.UserDefinedFunctionOutputNames[6].UserDefinedDescription;
            Output6Description = DecoderConfiguration.UserDefinedFunctionOutputNames[7].UserDefinedDescription == "" ? "6" : DecoderConfiguration.UserDefinedFunctionOutputNames[7].UserDefinedDescription;
            Output7Description = DecoderConfiguration.UserDefinedFunctionOutputNames[8].UserDefinedDescription == "" ? "7" : DecoderConfiguration.UserDefinedFunctionOutputNames[8].UserDefinedDescription;
            Output8Description = DecoderConfiguration.UserDefinedFunctionOutputNames[9].UserDefinedDescription == "" ? "8" : DecoderConfiguration.UserDefinedFunctionOutputNames[9].UserDefinedDescription;
            Output9Description = DecoderConfiguration.UserDefinedFunctionOutputNames[10].UserDefinedDescription == "" ? "9" : DecoderConfiguration.UserDefinedFunctionOutputNames[10].UserDefinedDescription;
            Output10Description = DecoderConfiguration.UserDefinedFunctionOutputNames[11].UserDefinedDescription == "" ? "10" : DecoderConfiguration.UserDefinedFunctionOutputNames[11].UserDefinedDescription;
            Output11Description = DecoderConfiguration.UserDefinedFunctionOutputNames[12].UserDefinedDescription == "" ? "11" : DecoderConfiguration.UserDefinedFunctionOutputNames[12].UserDefinedDescription;
            Output12Description = DecoderConfiguration.UserDefinedFunctionOutputNames[13].UserDefinedDescription == "" ? "12" : DecoderConfiguration.UserDefinedFunctionOutputNames[13].UserDefinedDescription;

            GetFunctionOutputMappingDescriptions();

            //  ZIMO: Function keys for high beam and dimming in CV119 and CV120 (ZIMO_FUNCKEY_HIGHBEAMDIMMING_CV119X)
            CV119Value = DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam;
            CV119ValueBit0 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 0);
            CV119ValueBit1 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 1);
            CV119ValueBit2 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 2);
            CV119ValueBit3 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 3);
            CV119ValueBit4 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 4);
            CV119ValueBit5 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 5); 
            CV119ValueBit6 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 6);
            CV119ValueBit7 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF6HighDippedBeam, 7);
            if (CV119Value > 0)
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(true,true, 6, AppResources.FrameFunctionKeysHighBeamDimmingF6FuncKeyDesc);
            }
            else
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(false,true, -1, AppResources.FrameFunctionKeysHighBeamDimmingF6FuncKeyDesc);
            }

            CV120Value = DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam;
            CV120ValueBit0 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 0);
            CV120ValueBit1 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 1);
            CV120ValueBit2 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 2);
            CV120ValueBit3 = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 3);
            CV120ValueBit4  = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 4);
            CV120ValueBit5  = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 5);
            CV120ValueBit6  = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 6);
            CV120ValueBit7  = Bit.IsSet(DecoderConfiguration.RCN225.FunctionKeyF7HighDippedBeam, 7);
            if (CV120Value > 0)
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(true,true, 7, AppResources.FrameFunctionKeysHighBeamDimmingF7FuncKeyDesc);
            }
            else
            {
                DecoderConfiguration.SetFunctionKeyFunctionDescription(false,true,-1, AppResources.FrameFunctionKeysHighBeamDimmingF7FuncKeyDesc);
            }

            // ZIMO: ABV key in CV156 (ZIMO_FUNCKEYDEACTIVATEACCDECTIME_CV156)
            ZIMOFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber;
            CV156Configuration = Subline.Create([156]);
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysAccDecDisableFuncKeyNumber != 0, true, ZIMOFuncKeysAccDecDisableFuncKeyNumber, AppResources.FrameFunctionKeysDeactivateAccDecTimeFuncKeyDesc);

            //  ZIMO: Shunting function key in CV155 (ZIMO_FUNCKEY_SHUNTINGKEY_CV155)
            byte currentShuntinKeyNumber = DecoderConfiguration.ZIMO.ShuntingKeyAndShuntingSpeed;
            ZIMOFuncKeysShuntingKeyNumber = currentShuntinKeyNumber & 0x1F;
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysShuntingKeyNumber != 0,true, ZIMOFuncKeysShuntingKeyNumber, AppResources.FrameFunctionKeysShuntingKeyDesc);        

            // ZIMO: Sound louder in CV397 (ZIMO_FUNCKEY_SOUNDVOLUMELOUDER_CV397)
            ZIMOFuncKeysSoundVolumeLouder = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeLouder;
            CV397Configuration = Subline.Create(new List<uint> { 397 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysSoundVolumeLouder != 0,true, ZIMOFuncKeysSoundVolumeLouder, AppResources.FrameFunctionKeysSoundLouderDesc);        

            // ZIMO: Sound quieter in CV396 (ZIMO_FUNCKEY_SOUNDVOLUMEQUIETER_CV396
            ZIMOFuncKeysSoundVolumeQuieter = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeQuieter;
            CV396Configuration = Subline.Create(new List<uint> { 396 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysSoundVolumeQuieter != 0,true, ZIMOFuncKeysSoundVolumeQuieter, AppResources.FrameFunctionKeysSoundQuieterDesc);

            // ZIMO: Sound on and off CV310 (ZIMO_FUNCKEY_SOUNDALLOFF_CV310)
            ZIMOFuncKeysSoundOnOff = DecoderConfiguration.ZIMO.FuncKeyNrSoundOnOff;
            CV310Configuration = Subline.Create(new List<uint> { 310 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysSoundOnOff != 0,true, ZIMOFuncKeysSoundOnOff, AppResources.FrameFunctionKeysSoundOnOff);        

            // ZIMO: Curve squeal in CV308 (ZIMO_FUNCKEY_CURVESQUEAL_CV308)
            ZIMOFuncKeysCurveSqueal = DecoderConfiguration.ZIMO.FuncKeyNrCurveSqueal;
            CV308Configuration = Subline.Create(new List<uint> { 308 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysCurveSqueal != 0,true, ZIMOFuncKeysCurveSqueal, AppResources.FrameFunctionKeysSoundCurveSquealOnOff);        

            // ZIMO: Light suppression on the driver's cab side 1 (forward) in CV107 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X)
            byte currentLightSuppressKeyCab1Forward = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1 = currentLightSuppressKeyCab1Forward &= 0x1F;
            CV107And109Configuration = Subline.Create([107, 109]);
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeyLightSuppresionCabSide1 != 0, true, ZIMOFuncKeyLightSuppresionCabSide1, AppResources.FrameFunctionKeysLightSuppressionDriverCabCab1Desc);

            byte tempFunctionOutput = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output = (tempFunctionOutput &= 0xE0) >> 5;

            tempFunctionOutput = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output3 = tempFunctionOutput &= 0x07;

            tempFunctionOutput = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab1Forward;
            ZIMOFuncKeyLightSuppresionCabSide1Output4 = (tempFunctionOutput &= 0xF8) >> 3;

            // ZIMO: Light suppression on the driver's cab side 2 (backward) in CV108 (ZIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X).
            byte tempfunctionKey2 = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2 = tempfunctionKey2 &= 0x1F;
            CV108And110Configuration = Subline.Create([108, 110]);
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeyLightSuppresionCabSide2 != 0, true, ZIMOFuncKeyLightSuppresionCabSide2, AppResources.FrameFunctionKeysLightSuppressionDriverCabCab2Desc);

            byte tempFunctionOutput2 = DecoderConfiguration.ZIMO.FuncKeyNrSuppressLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output = (tempFunctionOutput2 &= 0xE0) >> 5;

            tempFunctionOutput2 = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output3 = tempFunctionOutput2 &= 0x07;

            tempFunctionOutput2 = DecoderConfiguration.ZIMO.AddOutputsSuppressedLightDriverCab2Forward;
            ZIMOFuncKeyLightSuppresionCabSide2Output4 = (tempFunctionOutput2 &= 0xF8) >> 3;

            //  ZIMO: Sound mute in CV313 (ZIMO_FUNCKEY_MUTE_CV313)
            if (DecoderConfiguration.ZIMO.FuncKeyNrMute > 100)
            {
                ZIMOFuncKeysMuteInverted = true;
                ZIMOFuncKeysMute = DecoderConfiguration.ZIMO.FuncKeyNrMute - 100;
            }
            else
            {
                ZIMOFuncKeysMuteInverted = false;
                ZIMOFuncKeysMute = DecoderConfiguration.ZIMO.FuncKeyNrMute;
            }
            CV313Configuration = Subline.Create(new List<uint> { 313 });
            DecoderConfiguration.SetFunctionKeyFunctionDescription(ZIMOFuncKeysMute != 0,true, ZIMOFuncKeysMute, AppResources.FrameFunctionKeysSoundMuteDesc);        

            // Döhler & Haass: Function key mapping type in CV137 (DOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137)
            DoehlerAndHaassExtendedFunctionMappingEnabled = DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled;

            // Döhler & Haass: ABV key in CV133 (DOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133)
            DoehlerAndHaassFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber;
            CV133Configuration = Subline.Create(new List<uint> { 133 });

            // Döhler & Haass: Shunting key in CV132 (DOEHLERANDHAASS_FUNCKEYSHUNTING_CV132)
            DoehlerAndHaassFuncKeysShuntingFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber;
            CV132Configuration = Subline.Create(new List<uint> { 132 });

        }

        /// <summary>
        /// Writes the current input mapping configuration to the decoder configuration.
        /// </summary>
        private void WriteZIMOInputMappingListToDecoderConfiguration()
        {
            try
            {
                foreach (FunctionKeyType item in DecoderConfiguration.FunctionKeys)
                {
                    switch (item.ZIMOInputMapping.CVNumber)
                    {
                        case 400: DecoderConfiguration.ZIMO.InputMappingF0 = item.ZIMOInputMapping; break;
                        case 401: DecoderConfiguration.ZIMO.InputMappingF1 = item.ZIMOInputMapping; break;
                        case 402: DecoderConfiguration.ZIMO.InputMappingF2 = item.ZIMOInputMapping; break;
                        case 403: DecoderConfiguration.ZIMO.InputMappingF3 = item.ZIMOInputMapping; break;
                        case 404: DecoderConfiguration.ZIMO.InputMappingF4 = item.ZIMOInputMapping; break;
                        case 405: DecoderConfiguration.ZIMO.InputMappingF5 = item.ZIMOInputMapping; break;
                        case 406: DecoderConfiguration.ZIMO.InputMappingF6 = item.ZIMOInputMapping; break;
                        case 407: DecoderConfiguration.ZIMO.InputMappingF7 = item.ZIMOInputMapping; break;
                        case 408: DecoderConfiguration.ZIMO.InputMappingF8 = item.ZIMOInputMapping; break;
                        case 409: DecoderConfiguration.ZIMO.InputMappingF9 = item.ZIMOInputMapping; break;
                        case 410: DecoderConfiguration.ZIMO.InputMappingF10 = item.ZIMOInputMapping; break;
                        case 411: DecoderConfiguration.ZIMO.InputMappingF11 = item.ZIMOInputMapping; break;
                        case 412: DecoderConfiguration.ZIMO.InputMappingF12 = item.ZIMOInputMapping; break;
                        case 413: DecoderConfiguration.ZIMO.InputMappingF13 = item.ZIMOInputMapping; break;
                        case 414: DecoderConfiguration.ZIMO.InputMappingF14 = item.ZIMOInputMapping; break;
                        case 415: DecoderConfiguration.ZIMO.InputMappingF15 = item.ZIMOInputMapping; break;
                        case 416: DecoderConfiguration.ZIMO.InputMappingF16 = item.ZIMOInputMapping; break;
                        case 417: DecoderConfiguration.ZIMO.InputMappingF17 = item.ZIMOInputMapping; break;
                        case 418: DecoderConfiguration.ZIMO.InputMappingF18 = item.ZIMOInputMapping; break;
                        case 419: DecoderConfiguration.ZIMO.InputMappingF19 = item.ZIMOInputMapping; break;
                        case 420: DecoderConfiguration.ZIMO.InputMappingF20 = item.ZIMOInputMapping; break;
                    }
                }
            }
            catch
            {
                return;
            }

        }

        /// <summary>
        /// Updates the input mapping list with the current configuration of the decoder.
        /// Note:
        /// Currently only the ZIMO input mapping is supported.
        /// </summary>
        private void UpdateTheInputMappingList()
        {
            for (int i = 0; i < NMRA.NumberOfFunctionKeys; i++)
            {
                Type type = DecoderConfiguration.ZIMO.GetType();
                PropertyInfo propInfo = type.GetProperty("InputMappingF" + i.ToString())!;
                DecoderConfiguration.FunctionKeys[i].ZIMOInputMapping = propInfo.GetValue(DecoderConfiguration.ZIMO) as ZIMOInputMappingType ?? new ZIMOInputMappingType();
            }
        }

        /// <summary>
        /// Grabs the function descriptions for the function outputs from the decoder configuration.    
        /// </summary>
        private void GetFunctionOutputMappingDescriptions()
        {
            // We loop trough all function 12 keys (function mapping currently supports F0 (forward and backward) to F12).
            for (int functionKey = 0; functionKey <= 13; functionKey++)
            {
                // For each function key we loop trough all function outputs.
                for (int functionOutputNumber = 0; functionOutputNumber < 8; functionOutputNumber++)
                {
                    //In the first step, we get the configuration of the selected function key.
                    Type decoderConfigType = DecoderConfiguration.RCN225.GetType();
                    string cvPropertyNameOfFunctionKey = string.Empty;
                    if (functionKey == 0)
                    {
                        cvPropertyNameOfFunctionKey = "FunctionMappingF0Forward";
                    }
                    else if (functionKey == 1)
                    {
                        cvPropertyNameOfFunctionKey = "FunctionMappingF0Backward";
                    }
                    else
                    {
                        cvPropertyNameOfFunctionKey = "FunctionMappingF" + (functionKey - 1).ToString();
                    }
                    byte functionKeyConfiguration = (byte) decoderConfigType.GetProperty(cvPropertyNameOfFunctionKey)!.GetValue(DecoderConfiguration.RCN225)!;

                    // In the second step, we get the description of the current function output number.   
                    Z2XProgrammer.Resources.Strings.AppResources appResources = new Z2XProgrammer.Resources.Strings.AppResources(); 
                    Type type = appResources.GetType();
                    string appResourceStringID = string.Empty;

                    // Depending on the mapping method (RCN225, ZIMO etc., we have to consider the left shift of the outputs.
                    if(DecoderConfiguration.ZIMO.ExtendedFunctionKeyMapping == true)
                    {
                        //  ZIMO extended function key mapping.

                        if (functionOutputNumber == 0) 
                        {
                            appResourceStringID = "FrameFunctionKeysOutput0vDesc"; // Special case for 0v
                        }
                        else if (functionOutputNumber == 1) 
                        {
                            appResourceStringID = "FrameFunctionKeysOutput0rDesc"; // Special case for 0r
                        }
                        else
                        {
                            appResourceStringID = "FrameFunctionKeysOutput" + (functionOutputNumber -1).ToString() + "Desc";
                        }
                    }
                    else
                    {
                        // RCN225 function key mapping.
                        if ((functionKey >= 0) && (functionKey <= 3)) 
                        {
                            if (functionOutputNumber == 0) 
                            {
                                appResourceStringID = "FrameFunctionKeysOutput0vDesc"; // Special case for 0v
                            }
                            else if (functionOutputNumber == 1) 
                            {
                                appResourceStringID = "FrameFunctionKeysOutput0rDesc"; // Special case for 0r
                            }
                            else
                            {
                                appResourceStringID = "FrameFunctionKeysOutput" + (functionOutputNumber-1).ToString() + "Desc";
                            }
                        }
                        else if((functionKey >= 4) && (functionKey <= 7))
                        {
                            appResourceStringID = "FrameFunctionKeysOutput" + (functionOutputNumber + 2).ToString() + "Desc";
                        }
                        else if((functionKey >= 8) && (functionKey <= 13))
                        {
                            appResourceStringID = "FrameFunctionKeysOutput" + (functionOutputNumber + 5).ToString() + "Desc";
                        }
                    }


                    int functionKeyNumber = 0;
                    if ((functionKey == 0)  || (functionKey == 1)) // Special case for 0v and 0r
                    {
                        functionKeyNumber = 0;
                    }   
                    else
                    {
                        functionKeyNumber = functionKey - 1; // Adjust for 0v and 0r
                    }
                    
                    // We set the function key function description.
                    PropertyInfo propInfo = type.GetProperty(appResourceStringID)!;
                    string functionDescription = (string)propInfo.GetValue(appResources)!;

                    if(DecoderConfiguration.UserDefinedFunctionOutputNames[functionOutputNumber].UserDefinedDescription != "")
                    {
                        //  If the user has defined a description for the function output, we use it.
                        functionDescription += ": " + DecoderConfiguration.UserDefinedFunctionOutputNames[functionOutputNumber].UserDefinedDescription;
                    }

                    if (functionKey == 0)
                    {
                        functionDescription += " " + AppResources.FrameFunctionKeysDescriptionDirectionForward;
                    }
                    else if (functionKey == 1)
                    {
                        functionDescription += " " + AppResources.FrameFunctionKeysDescriptionDirectionBackward;
                    }
                   

                    if (Bit.IsSet(functionKeyConfiguration, functionOutputNumber) == true)
                    {
                        // We set the function key function description to the current function output number and description.      
                        DecoderConfiguration.SetFunctionKeyFunctionDescription(true,false, functionKeyNumber, functionDescription); 
                    }
                    else
                    {
                        // We set the function key function description to empty string.
                        DecoderConfiguration.SetFunctionKeyFunctionDescription(false, false, functionKeyNumber, functionDescription);
                    }

                }
            }
        }

        #endregion

    }
}

