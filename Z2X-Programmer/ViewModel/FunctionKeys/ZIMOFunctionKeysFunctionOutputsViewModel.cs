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


using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class ZIMOFunctionKeysFunctionOutputsViewModel: ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

         // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion

        #region DECODER FEATURES

        //  ZIMO specific function output
        [ObservableProperty]
        bool zIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 = false;
        
        #endregion

        #region REGION: PUBLIC PROPERTIES
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


        /// <summary>
        /// Is TRUE if the standard function mapping according to RCN225 is used.
        /// </summary>
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
        string cV61Configuration = Subline.Create([61]);

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

        #endregion

        # region REGION: COMSTRUCTOR

        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public ZIMOFunctionKeysFunctionOutputsViewModel()
        {
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

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61 = DecoderSpecification.ZIMO_FUNCTIONOUTPUTMAPPING_EXT_CV61;            
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

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

            //  RCN225
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

            // ZIMO: Extended function key mapping - without left-shift (ZIMO_FUNCTIONKEYMAPPINGTYPE_CV61)
            ZIMOExtendedFunctionMapping = DecoderConfiguration.ZIMO.ExtendedFunctionKeyMapping;
            CV61Configuration = Subline.Create(new List<uint> { 61 });

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

        #region REGION: COMMANDS

        /// <summary>
        /// This command opens a dialog box in which the user can enter any description for a function output.
        /// </summary>
        /// <param name="functionOutputID">The ID of the desired function output.</param>
        [RelayCommand]
        static async Task EditFunctionOutputDescription(string functionOutputID)
        {
            try
            {
                FunctionOutputType selectedFunctionOutput = DecoderConfiguration.UserDefinedFunctionOutputNames.First(p => p.ID == functionOutputID);
                if (selectedFunctionOutput == null) return;

                if ((Application.Current != null) && (Application.Current.Windows[0].Page != null))
                {
                    string Result = await Application.Current.Windows[0].Page!.DisplayPromptAsync(AppResources.FrameFunctionOutputsGetNamingTitle + " " + selectedFunctionOutput.Description, AppResources.FrameFunctionOutputsGetNamingDescription, AppResources.OK, AppResources.PopupButtonCancel, null, -1, null, selectedFunctionOutput.UserDefinedDescription);
                    if (Result != null) selectedFunctionOutput.UserDefinedDescription = Result;
                    WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                }
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("ZIMOFunctionKeysFunctionOutputsViewModel:EditFunctionOutputDescription " + ex.Message);
            }
        }

        /// <summary>
        /// Navigates one page back
        /// </summary>
        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
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

        #endregion

    }
}
