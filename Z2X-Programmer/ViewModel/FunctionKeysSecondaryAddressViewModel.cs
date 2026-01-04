/*

Z2X-Programmer
Copyright (C) 2024 - 2026
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
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    /// <summary>
    /// Implementation of the view model for the function keys page of the secondary address.
    /// </summary>
    public partial class FunctionKeysSecondaryAddressViewModel: ObservableObject
    {

        #region REGION: DECODER FEATURES
        
        [ObservableProperty]
        bool zIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        #region CV69 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F0 (FORWARD)        
        [ObservableProperty]
        ushort cV69Value;
   
        [ObservableProperty]
        bool cV69ValueBit0;
        partial void OnCV69ValueBit0Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }

        [ObservableProperty]
        bool cV69ValueBit1;
        partial void OnCV69ValueBit1Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit2;
        partial void OnCV69ValueBit2Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit3;
        partial void OnCV69ValueBit3Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit4;
        partial void OnCV69ValueBit4Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit5;
        partial void OnCV69ValueBit5Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit6;
        partial void OnCV69ValueBit6Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;
        }
        [ObservableProperty]
        bool cV69ValueBit7;
        partial void OnCV69ValueBit7Changed(bool value)
        {
           CV69Value = Bit.Set((byte)CV69Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward = (byte)CV69Value;     
        }
        #endregion

        #region CV70 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F0 (BACKWARD)
        [ObservableProperty]
        ushort cV70Value;

        [ObservableProperty]
        bool cV70ValueBit0;
        partial void OnCV70ValueBit0Changed(bool value)
        {    
           CV70Value = Bit.Set((byte)CV70Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }

        [ObservableProperty]
        bool cV70ValueBit1;
        partial void OnCV70ValueBit1Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }    
        [ObservableProperty]
        bool cV70ValueBit2;
        partial void OnCV70ValueBit2Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        [ObservableProperty]
        bool cV70ValueBit3;
        partial void OnCV70ValueBit3Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        [ObservableProperty]
        bool cV70ValueBit4;
        partial void OnCV70ValueBit4Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        [ObservableProperty]
        bool cV70ValueBit5;
        partial void OnCV70ValueBit5Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        [ObservableProperty]
        bool cV70ValueBit6;
        partial void OnCV70ValueBit6Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        [ObservableProperty]
        bool cV70ValueBit7;
        partial void OnCV70ValueBit7Changed(bool value)
        {
           CV70Value = Bit.Set((byte)CV70Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward = (byte)CV70Value;
        }
        #endregion

        #region CV71 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F1
       [ObservableProperty]
       ushort cV71Value;

       [ObservableProperty]
       bool cV71ValueBit0;
       partial void OnCV71ValueBit0Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }

       [ObservableProperty]
       bool cV71ValueBit1;
       partial void OnCV71ValueBit1Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit2;
       partial void OnCV71ValueBit2Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit3;
       partial void OnCV71ValueBit3Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit4;
       partial void OnCV71ValueBit4Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit5;
       partial void OnCV71ValueBit5Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit6;
       partial void OnCV71ValueBit6Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       [ObservableProperty]
       bool cV71ValueBit7;
       partial void OnCV71ValueBit7Changed(bool value)
       {
           CV71Value = Bit.Set((byte)CV71Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1 = (byte)CV71Value;
       }
       #endregion

        #region CV72 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F2
       [ObservableProperty]
       ushort cV72Value;

       [ObservableProperty]
       bool cV72ValueBit0;
       partial void OnCV72ValueBit0Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }

       [ObservableProperty]
       bool cV72ValueBit1;
       partial void OnCV72ValueBit1Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit2;
       partial void OnCV72ValueBit2Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit3;
       partial void OnCV72ValueBit3Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit4;
       partial void OnCV72ValueBit4Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit5;
       partial void OnCV72ValueBit5Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit6;
       partial void OnCV72ValueBit6Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       [ObservableProperty]
       bool cV72ValueBit7;
       partial void OnCV72ValueBit7Changed(bool value)
       {
           CV72Value = Bit.Set((byte)CV72Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2 = (byte)CV72Value;
       }
       #endregion

        #region CV73 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F3
       [ObservableProperty]
       ushort cV73Value;

       [ObservableProperty]
       bool cV73ValueBit0;
       partial void OnCV73ValueBit0Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }

       [ObservableProperty]
       bool cV73ValueBit1;
       partial void OnCV73ValueBit1Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit2;
       partial void OnCV73ValueBit2Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit3;
       partial void OnCV73ValueBit3Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit4;
       partial void OnCV73ValueBit4Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit5;
       partial void OnCV73ValueBit5Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit6;
       partial void OnCV73ValueBit6Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       [ObservableProperty]
       bool cV73ValueBit7;
       partial void OnCV73ValueBit7Changed(bool value)
       {
           CV73Value = Bit.Set((byte)CV73Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3 = (byte) CV73Value;
       }
       #endregion

        #region CV74 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F4
       [ObservableProperty]
       ushort cV74Value;

       [ObservableProperty]
       bool cV74ValueBit0;
       partial void OnCV74ValueBit0Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }

       [ObservableProperty]
       bool cV74ValueBit1;
       partial void OnCV74ValueBit1Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit2;
       partial void OnCV74ValueBit2Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit3;
       partial void OnCV74ValueBit3Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit4;
       partial void OnCV74ValueBit4Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit5;
       partial void OnCV74ValueBit5Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit6;
       partial void OnCV74ValueBit6Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       [ObservableProperty]
       bool cV74ValueBit7;
       partial void OnCV74ValueBit7Changed(bool value)
       {
           CV74Value = Bit.Set((byte)CV74Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4 = (byte) CV74Value;
       }
       #endregion

        #region CV75 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F5
       [ObservableProperty]
       ushort cV75Value;

       [ObservableProperty]
       bool cV75ValueBit0;
       partial void OnCV75ValueBit0Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }

       [ObservableProperty]
       bool cV75ValueBit1;
       partial void OnCV75ValueBit1Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit2;
       partial void OnCV75ValueBit2Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit3;
       partial void OnCV75ValueBit3Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit4;
       partial void OnCV75ValueBit4Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit5;
       partial void OnCV75ValueBit5Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit6;
       partial void OnCV75ValueBit6Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       [ObservableProperty]
       bool cV75ValueBit7;
       partial void OnCV75ValueBit7Changed(bool value)
       {
           CV75Value = Bit.Set((byte)CV75Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5 =(byte) CV75Value;
       }
       #endregion

        #region CV76 FUNCTION OUT MAPPING OF KEY F6
       [ObservableProperty]
       ushort cV76Value;

       [ObservableProperty]
       bool cV76ValueBit0;
       partial void OnCV76ValueBit0Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }

       [ObservableProperty]
       bool cV76ValueBit1;
       partial void OnCV76ValueBit1Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit2;
       partial void OnCV76ValueBit2Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit3;
       partial void OnCV76ValueBit3Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit4;
       partial void OnCV76ValueBit4Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit5;
       partial void OnCV76ValueBit5Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit6;
       partial void OnCV76ValueBit6Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       [ObservableProperty]
       bool cV76ValueBit7;
       partial void OnCV76ValueBit7Changed(bool value)
       {
           CV76Value = Bit.Set((byte)CV76Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6 = (byte)CV76Value;
       }
       #endregion

        #region CV77 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F7
       [ObservableProperty]
       ushort cV77Value;

       [ObservableProperty]
       bool cV77ValueBit0;
       partial void OnCV77ValueBit0Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }

       [ObservableProperty]
       bool cV77ValueBit1;
       partial void OnCV77ValueBit1Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit2;
       partial void OnCV77ValueBit2Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit3;
       partial void OnCV77ValueBit3Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit4;
       partial void OnCV77ValueBit4Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit5;
       partial void OnCV77ValueBit5Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit6;
       partial void OnCV77ValueBit6Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;
       }
       [ObservableProperty]
       bool cV77ValueBit7;
       partial void OnCV77ValueBit7Changed(bool value)
       {
           CV77Value = Bit.Set((byte)CV77Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7 = (byte)CV77Value;        
       }
       #endregion

        #region CV78 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F8
       [ObservableProperty]
       ushort cV78Value;

       [ObservableProperty]
       bool cV78ValueBit0;
       partial void OnCV78ValueBit0Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;
       }

       [ObservableProperty]
       bool cV78ValueBit1;
       partial void OnCV78ValueBit1Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit2;
       partial void OnCV78ValueBit2Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit3;
       partial void OnCV78ValueBit3Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit4;
       partial void OnCV78ValueBit4Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit5;
       partial void OnCV78ValueBit5Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit6;
       partial void OnCV78ValueBit6Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       [ObservableProperty]
       bool cV78ValueBit7;
       partial void OnCV78ValueBit7Changed(bool value)
       {
           CV78Value = Bit.Set((byte)CV78Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8 =(byte)CV78Value;

       }
       #endregion

        #region CV79 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F9
       [ObservableProperty]
       ushort cV79Value;

       [ObservableProperty]
       bool cV79ValueBit0;
       partial void OnCV79ValueBit0Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }

       [ObservableProperty]
       bool cV79ValueBit1;
       partial void OnCV79ValueBit1Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit2;
       partial void OnCV79ValueBit2Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit3;
       partial void OnCV79ValueBit3Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit4;
       partial void OnCV79ValueBit4Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit5;
       partial void OnCV79ValueBit5Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit6;
       partial void OnCV79ValueBit6Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       [ObservableProperty]
       bool cV79ValueBit7;
       partial void OnCV79ValueBit7Changed(bool value)
       {
           CV79Value = Bit.Set((byte)CV79Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9 = (byte)CV79Value;
       }
       #endregion

        #region CV80 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F10
       [ObservableProperty]
       ushort cV80Value;

       [ObservableProperty]
       bool cV80ValueBit0;
       partial void OnCV80ValueBit0Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }

       [ObservableProperty]
       bool cV80ValueBit1;
       partial void OnCV80ValueBit1Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit2;
       partial void OnCV80ValueBit2Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit3;
       partial void OnCV80ValueBit3Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit4;
       partial void OnCV80ValueBit4Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit5;
       partial void OnCV80ValueBit5Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit6;
       partial void OnCV80ValueBit6Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       [ObservableProperty]
       bool cV80ValueBit7;
       partial void OnCV80ValueBit7Changed(bool value)
       {
           CV80Value = Bit.Set((byte)CV80Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10 = (byte) CV80Value;
       }
       #endregion

        #region CV81 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F11
       [ObservableProperty]
       ushort cV81Value;

       [ObservableProperty]
       bool cV81ValueBit0;
       partial void OnCV81ValueBit0Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }

       [ObservableProperty]
       bool cV81ValueBit1;
       partial void OnCV81ValueBit1Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit2;
       partial void OnCV81ValueBit2Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit3;
       partial void OnCV81ValueBit3Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit4;
       partial void OnCV81ValueBit4Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit5;
       partial void OnCV81ValueBit5Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit6;
       partial void OnCV81ValueBit6Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       [ObservableProperty]
       bool cV81ValueBit7;
       partial void OnCV81ValueBit7Changed(bool value)
       {
           CV81Value = Bit.Set((byte)CV81Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11= (byte) CV81Value;
       }
       #endregion

        #region CV82 SECONDARY ADDRESS FUNCTION OUTPUT MAPPING OF KEY F12
       [ObservableProperty]
       ushort cV82Value;

       [ObservableProperty]
       bool cV82ValueBit0;
       partial void OnCV82ValueBit0Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 0, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }

       [ObservableProperty]
       bool cV82ValueBit1;
       partial void OnCV82ValueBit1Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 1, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit2;
       partial void OnCV82ValueBit2Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 2, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit3;
       partial void OnCV82ValueBit3Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 3, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit4;
       partial void OnCV82ValueBit4Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 4, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit5;
       partial void OnCV82ValueBit5Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 5, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit6;
       partial void OnCV82ValueBit6Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 6, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
       [ObservableProperty]
       bool cV82ValueBit7;
       partial void OnCV82ValueBit7Changed(bool value)
       {
           CV82Value = Bit.Set((byte)CV82Value, 7, value);
           DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12 = (byte)CV82Value;
       }
        #endregion


        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public FunctionKeysSecondaryAddressViewModel()
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
            ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X = DecoderSpecification.ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // Set the bits for the configuration variables used by the decoder
            // feature ZIMO_FUNCTIONMAPPING_SECONDARYADDR_CV69X.
            CV69Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Forward;
            CV70Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF0Backward;
            CV71Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF1;
            CV72Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF2;
            CV73Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF3;
            CV74Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4;
            CV74Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF4;
            CV75Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF5;
            CV76Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF6;
            CV77Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF7;
            CV78Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF8;
            CV79Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF9;
            CV80Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF10;
            CV81Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF11;
            CV82Value = DecoderConfiguration.ZIMO.FunctionMappingSecondaryAddressF12;  
            for (byte cv = 69; cv <= 82; cv++)
            {                
                SetBitsOfCV(cv);
            }
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
