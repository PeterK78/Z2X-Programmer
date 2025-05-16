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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
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

        internal ObservableCollection<ZIMOInputMappingType>? _ZIMOInputMappingCVs= [];

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

        [ObservableProperty]
        bool zIMO_FUNCKEY_LIGHTSUPPRESIONDRIVERSCABSIDE_CV107X = false;

        [ObservableProperty]
        bool zIMO_INPUTMAPPING_CV4XX;
        
        [ObservableProperty]
        bool zIMO_FUNCKEY_MUTE_CV313;
    

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

        // ZIMO: ZIMO input mapping in CV400 - CV428 (ZIMO_INPUTMAPPING_CV4XX)
        // SelectedInputMapping contains the data for the currently selected input mapping (if the user edits a mapping).
        [ObservableProperty]
        internal ZIMOInputMappingType selectedInputMapping = new ZIMOInputMappingType();

        //  ZIMOInputMappingCVs contains all input mappings for the current decoder.
        [ObservableProperty]
        internal ObservableCollection<ZIMOInputMappingType>? zIMOInputMappingCVs= [];

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
        }

        [ObservableProperty]
        string cV156Configuration = Subline.Create([156]);

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
            CV397Configuration = Subline.Create(new List<uint>{397});
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
            CV396Configuration = Subline.Create(new List<uint>{396});
        }

        [ObservableProperty]
        string cV396Configuration = Subline.Create([396]);


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
            CV107And109Configuration = Subline.Create([107, 109]);  
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
        string cV107And109Configuration = Subline.Create([107,109]);


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
        string cV108And110Configuration = Subline.Create([108,110]);


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
       
        #region REGION: COMMANDS

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
            else if(RCN225_FUNCTIONOUTPUTMAPPING_CV3346 == true)
            {
                await Shell.Current.GoToAsync("RCN225FunctionKeysFunctionOutputsPage");
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
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
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

            //  Döhler and Haass
            DoehlerAndHaassExtendedFunctionMappingEnabled = DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled;
            DoehlerAndHaassFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber;
            DoehlerAndHaassFuncKeysShuntingFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber;

            //  ZIMO
            ZIMOFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber;
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

            UpdateZIMOInputMappingList();
        }

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
      
        #endregion

    }
}
