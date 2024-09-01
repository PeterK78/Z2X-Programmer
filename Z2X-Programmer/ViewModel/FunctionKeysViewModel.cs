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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class FunctionKeysViewModel: ObservableObject
    {

        #region REGION: DECODER FEATURES

        //  RCN225 features
        [ObservableProperty]
        bool rCN225_FUNCTIONKEYMAPPING_CV3346;

        //  ZIMO features
        //  ZIMO_FUNCKEY_MUTE_CV313
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

        //   Döhler & Haass
        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCTIONKEYMAPPINGTYPE_CV137;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYDEACTIVATEACCDECTIME_CV133;

        [ObservableProperty]
        bool dOEHLERANDHAASS_FUNCKEYSHUNTING_CV132;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        #region CV33 Function Mapping Properties
        [ObservableProperty]
        ushort cV33Value;

        [ObservableProperty]
        bool cV33ValueBit0;
        partial void OnCV33ValueBit0Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 0, value);
        }

        [ObservableProperty]
        bool cV33ValueBit1;
        partial void OnCV33ValueBit1Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 1, value);
        }
        [ObservableProperty]
        bool cV33ValueBit2;
        partial void OnCV33ValueBit2Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 2, value);
        }
        [ObservableProperty]
        bool cV33ValueBit3;
        partial void OnCV33ValueBit3Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 3, value);
        }
        [ObservableProperty]
        bool cV33ValueBit4;
        partial void OnCV33ValueBit4Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 4, value);
        }
        [ObservableProperty]
        bool cV33ValueBit5;
        partial void OnCV33ValueBit5Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 5, value);
        }
        [ObservableProperty]
        bool cV33ValueBit6;
        partial void OnCV33ValueBit6Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 6, value);
        }
        [ObservableProperty]
        bool cV33ValueBit7;
        partial void OnCV33ValueBit7Changed(bool value)
        {
            CV33Value = Bit.Set((byte)CV33Value, 7, value);
        }
        #endregion

        #region CV34 Function Mapping Properties of key F0 backward
        [ObservableProperty]
        ushort cV34Value;

        [ObservableProperty]
        bool cV34ValueBit0;
        partial void OnCV34ValueBit0Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 0, value);
        }

        [ObservableProperty]
        bool cV34ValueBit1;
        partial void OnCV34ValueBit1Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 1, value);
        }
        [ObservableProperty]
        bool cV34ValueBit2;
        partial void OnCV34ValueBit2Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 2, value);
        }
        [ObservableProperty]
        bool cV34ValueBit3;
        partial void OnCV34ValueBit3Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 3, value);
        }
        [ObservableProperty]
        bool cV34ValueBit4;
        partial void OnCV34ValueBit4Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 4, value);
        }
        [ObservableProperty]
        bool cV34ValueBit5;
        partial void OnCV34ValueBit5Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 5, value);
        }
        [ObservableProperty]
        bool cV34ValueBit6;
        partial void OnCV34ValueBit6Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 6, value);
        }
        [ObservableProperty]
        bool cV34ValueBit7;
        partial void OnCV34ValueBit7Changed(bool value)
        {
            CV34Value = Bit.Set((byte)CV34Value, 7, value);
        }
        #endregion

        #region CV35 Function Mapping Properties of key F1
        [ObservableProperty]
        ushort cV35Value;

        [ObservableProperty]
        bool cV35ValueBit0;
        partial void OnCV35ValueBit0Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 0, value);
        }

        [ObservableProperty]
        bool cV35ValueBit1;
        partial void OnCV35ValueBit1Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 1, value);
        }
        [ObservableProperty]
        bool cV35ValueBit2;
        partial void OnCV35ValueBit2Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 2, value);
        }
        [ObservableProperty]
        bool cV35ValueBit3;
        partial void OnCV35ValueBit3Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 3, value);
        }
        [ObservableProperty]
        bool cV35ValueBit4;
        partial void OnCV35ValueBit4Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 4, value);
        }
        [ObservableProperty]
        bool cV35ValueBit5;
        partial void OnCV35ValueBit5Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 5, value);
        }
        [ObservableProperty]
        bool cV35ValueBit6;
        partial void OnCV35ValueBit6Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 6, value);
        }
        [ObservableProperty]
        bool cV35ValueBit7;
        partial void OnCV35ValueBit7Changed(bool value)
        {
            CV35Value = Bit.Set((byte)CV35Value, 7, value);
        }
        #endregion

        #region CV36 Function Mapping Properties of key F2
        [ObservableProperty]
        ushort cV36Value;

        [ObservableProperty]
        bool cV36ValueBit0;
        partial void OnCV36ValueBit0Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 0, value);
        }

        [ObservableProperty]
        bool cV36ValueBit1;
        partial void OnCV36ValueBit1Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 1, value);
        }
        [ObservableProperty]
        bool cV36ValueBit2;
        partial void OnCV36ValueBit2Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 2, value);
        }
        [ObservableProperty]
        bool cV36ValueBit3;
        partial void OnCV36ValueBit3Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 3, value);
        }
        [ObservableProperty]
        bool cV36ValueBit4;
        partial void OnCV36ValueBit4Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 4, value);
        }
        [ObservableProperty]
        bool cV36ValueBit5;
        partial void OnCV36ValueBit5Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 5, value);
        }
        [ObservableProperty]
        bool cV36ValueBit6;
        partial void OnCV36ValueBit6Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 6, value);
        }
        [ObservableProperty]
        bool cV36ValueBit7;
        partial void OnCV36ValueBit7Changed(bool value)
        {
            CV36Value = Bit.Set((byte)CV36Value, 7, value);
        }
        #endregion

        #region CV37 Function Mapping Properties of key F3
        [ObservableProperty]
        ushort cV37Value;

        [ObservableProperty]
        bool cV37ValueBit0;
        partial void OnCV37ValueBit0Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 0, value);
        }

        [ObservableProperty]
        bool cV37ValueBit1;
        partial void OnCV37ValueBit1Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 1, value);
        }
        [ObservableProperty]
        bool cV37ValueBit2;
        partial void OnCV37ValueBit2Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 2, value);
        }
        [ObservableProperty]
        bool cV37ValueBit3;
        partial void OnCV37ValueBit3Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 3, value);
        }
        [ObservableProperty]
        bool cV37ValueBit4;
        partial void OnCV37ValueBit4Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 4, value);
        }
        [ObservableProperty]
        bool cV37ValueBit5;
        partial void OnCV37ValueBit5Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 5, value);
        }
        [ObservableProperty]
        bool cV37ValueBit6;
        partial void OnCV37ValueBit6Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 6, value);
        }
        [ObservableProperty]
        bool cV37ValueBit7;
        partial void OnCV37ValueBit7Changed(bool value)
        {
            CV37Value = Bit.Set((byte)CV37Value, 7, value);
        }
        #endregion

        #region CV38 Function Mapping Properties of key F4
        [ObservableProperty]
        ushort cV38Value;

        [ObservableProperty]
        bool cV38ValueBit0;
        partial void OnCV38ValueBit0Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 0, value);
        }

        [ObservableProperty]
        bool cV38ValueBit1;
        partial void OnCV38ValueBit1Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 1, value);
        }
        [ObservableProperty]
        bool cV38ValueBit2;
        partial void OnCV38ValueBit2Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 2, value);
        }
        [ObservableProperty]
        bool cV38ValueBit3;
        partial void OnCV38ValueBit3Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 3, value);
        }
        [ObservableProperty]
        bool cV38ValueBit4;
        partial void OnCV38ValueBit4Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 4, value);
        }
        [ObservableProperty]
        bool cV38ValueBit5;
        partial void OnCV38ValueBit5Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 5, value);
        }
        [ObservableProperty]
        bool cV38ValueBit6;
        partial void OnCV38ValueBit6Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 6, value);
        }
        [ObservableProperty]
        bool cV38ValueBit7;
        partial void OnCV38ValueBit7Changed(bool value)
        {
            CV38Value = Bit.Set((byte)CV38Value, 7, value);
        }
        #endregion

        #region CV39 Function Mapping Properties of key F5
        [ObservableProperty]
        ushort cV39Value;

        [ObservableProperty]
        bool cV39ValueBit0;
        partial void OnCV39ValueBit0Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 0, value);
        }

        [ObservableProperty]
        bool cV39ValueBit1;
        partial void OnCV39ValueBit1Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 1, value);
        }
        [ObservableProperty]
        bool cV39ValueBit2;
        partial void OnCV39ValueBit2Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 2, value);
        }
        [ObservableProperty]
        bool cV39ValueBit3;
        partial void OnCV39ValueBit3Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 3, value);
        }
        [ObservableProperty]
        bool cV39ValueBit4;
        partial void OnCV39ValueBit4Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 4, value);
        }
        [ObservableProperty]
        bool cV39ValueBit5;
        partial void OnCV39ValueBit5Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 5, value);
        }
        [ObservableProperty]
        bool cV39ValueBit6;
        partial void OnCV39ValueBit6Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 6, value);
        }
        [ObservableProperty]
        bool cV39ValueBit7;
        partial void OnCV39ValueBit7Changed(bool value)
        {
            CV39Value = Bit.Set((byte)CV39Value, 7, value);
        }
        #endregion

        #region CV40 Function Mapping Properties of key F6
        [ObservableProperty]
        ushort cV40Value;

        [ObservableProperty]
        bool cV40ValueBit0;
        partial void OnCV40ValueBit0Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 0, value);
        }

        [ObservableProperty]
        bool cV40ValueBit1;
        partial void OnCV40ValueBit1Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 1, value);
        }
        [ObservableProperty]
        bool cV40ValueBit2;
        partial void OnCV40ValueBit2Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 2, value);
        }
        [ObservableProperty]
        bool cV40ValueBit3;
        partial void OnCV40ValueBit3Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 3, value);
        }
        [ObservableProperty]
        bool cV40ValueBit4;
        partial void OnCV40ValueBit4Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 4, value);
        }
        [ObservableProperty]
        bool cV40ValueBit5;
        partial void OnCV40ValueBit5Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 5, value);
        }
        [ObservableProperty]
        bool cV40ValueBit6;
        partial void OnCV40ValueBit6Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 6, value);
        }
        [ObservableProperty]
        bool cV40ValueBit7;
        partial void OnCV40ValueBit7Changed(bool value)
        {
            CV40Value = Bit.Set((byte)CV40Value, 7, value);
        }
        #endregion

        #region CV41 Function Mapping Properties of key F7
        [ObservableProperty]
        ushort cV41Value;

        [ObservableProperty]
        bool cV41ValueBit0;
        partial void OnCV41ValueBit0Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 0, value);
        }

        [ObservableProperty]
        bool cV41ValueBit1;
        partial void OnCV41ValueBit1Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 1, value);
        }
        [ObservableProperty]
        bool cV41ValueBit2;
        partial void OnCV41ValueBit2Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 2, value);
        }
        [ObservableProperty]
        bool cV41ValueBit3;
        partial void OnCV41ValueBit3Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 3, value);
        }
        [ObservableProperty]
        bool cV41ValueBit4;
        partial void OnCV41ValueBit4Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 4, value);
        }
        [ObservableProperty]
        bool cV41ValueBit5;
        partial void OnCV41ValueBit5Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 5, value);
        }
        [ObservableProperty]
        bool cV41ValueBit6;
        partial void OnCV41ValueBit6Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 6, value);
        }
        [ObservableProperty]
        bool cV41ValueBit7;
        partial void OnCV41ValueBit7Changed(bool value)
        {
            CV41Value = Bit.Set((byte)CV41Value, 7, value);
        }
        #endregion

        #region CV42 Function Mapping Properties of key F8
        [ObservableProperty]
        ushort cV42Value;

        [ObservableProperty]
        bool cV42ValueBit0;
        partial void OnCV42ValueBit0Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 0, value);
        }

        [ObservableProperty]
        bool cV42ValueBit1;
        partial void OnCV42ValueBit1Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 1, value);
        }
        [ObservableProperty]
        bool cV42ValueBit2;
        partial void OnCV42ValueBit2Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 2, value);
        }
        [ObservableProperty]
        bool cV42ValueBit3;
        partial void OnCV42ValueBit3Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 3, value);
        }
        [ObservableProperty]
        bool cV42ValueBit4;
        partial void OnCV42ValueBit4Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 3, value);
        }
        [ObservableProperty]
        bool cV42ValueBit5;
        partial void OnCV42ValueBit5Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 5, value);
        }
        [ObservableProperty]
        bool cV42ValueBit6;
        partial void OnCV42ValueBit6Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 6, value);
        }
        [ObservableProperty]
        bool cV42ValueBit7;
        partial void OnCV42ValueBit7Changed(bool value)
        {
            CV42Value = Bit.Set((byte)CV42Value, 7, value);
        }
        #endregion

        #region CV43 Function Mapping Properties of key F9
        [ObservableProperty]
        ushort cV43Value;

        [ObservableProperty]
        bool cV43ValueBit0;
        partial void OnCV43ValueBit0Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 0, value);
        }

        [ObservableProperty]
        bool cV43ValueBit1;
        partial void OnCV43ValueBit1Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 1, value);
        }
        [ObservableProperty]
        bool cV43ValueBit2;
        partial void OnCV43ValueBit2Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 2, value);
        }
        [ObservableProperty]
        bool cV43ValueBit3;
        partial void OnCV43ValueBit3Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 3, value);
        }
        [ObservableProperty]
        bool cV43ValueBit4;
        partial void OnCV43ValueBit4Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 4, value);
        }
        [ObservableProperty]
        bool cV43ValueBit5;
        partial void OnCV43ValueBit5Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 5, value);
        }
        [ObservableProperty]
        bool cV43ValueBit6;
        partial void OnCV43ValueBit6Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 6, value);
        }
        [ObservableProperty]
        bool cV43ValueBit7;
        partial void OnCV43ValueBit7Changed(bool value)
        {
            CV43Value = Bit.Set((byte)CV43Value, 7, value);
        }
        #endregion

        #region CV44 Function Mapping Properties of key F10
        [ObservableProperty]
        ushort cV44Value;

        [ObservableProperty]
        bool cV44ValueBit0;
        partial void OnCV44ValueBit0Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 0, value);
        }

        [ObservableProperty]
        bool cV44ValueBit1;
        partial void OnCV44ValueBit1Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 1, value);
        }
        [ObservableProperty]
        bool cV44ValueBit2;
        partial void OnCV44ValueBit2Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 2, value);
        }
        [ObservableProperty]
        bool cV44ValueBit3;
        partial void OnCV44ValueBit3Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 3, value);
        }
        [ObservableProperty]
        bool cV44ValueBit4;
        partial void OnCV44ValueBit4Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 4, value);
        }
        [ObservableProperty]
        bool cV44ValueBit5;
        partial void OnCV44ValueBit5Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 5, value);
        }
        [ObservableProperty]
        bool cV44ValueBit6;
        partial void OnCV44ValueBit6Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 6, value);
        }
        [ObservableProperty]
        bool cV44ValueBit7;
        partial void OnCV44ValueBit7Changed(bool value)
        {
            CV44Value = Bit.Set((byte)CV44Value, 7, value);
        }
        #endregion

        #region CV45 Function Mapping Properties of key F11
        [ObservableProperty]
        ushort cV45Value;

        [ObservableProperty]
        bool cV45ValueBit0;
        partial void OnCV45ValueBit0Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 0, value);
        }

        [ObservableProperty]
        bool cV45ValueBit1;
        partial void OnCV45ValueBit1Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 1, value);
        }
        [ObservableProperty]
        bool cV45ValueBit2;
        partial void OnCV45ValueBit2Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 2, value);
        }
        [ObservableProperty]
        bool cV45ValueBit3;
        partial void OnCV45ValueBit3Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 3, value);
        }
        [ObservableProperty]
        bool cV45ValueBit4;
        partial void OnCV45ValueBit4Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 4, value);
        }
        [ObservableProperty]
        bool cV45ValueBit5;
        partial void OnCV45ValueBit5Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 5, value);
        }
        [ObservableProperty]
        bool cV45ValueBit6;
        partial void OnCV45ValueBit6Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 6, value);
        }
        [ObservableProperty]
        bool cV45ValueBit7;
        partial void OnCV45ValueBit7Changed(bool value)
        {
            CV45Value = Bit.Set((byte)CV45Value, 7, value);
        }
        #endregion

        #region CV46 Function Mapping Properties of key F12
        [ObservableProperty]
        ushort cV46Value;

        [ObservableProperty]
        bool cV46ValueBit0;
        partial void OnCV46ValueBit0Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 0, value);
        }

        [ObservableProperty]
        bool cV46ValueBit1;
        partial void OnCV46ValueBit1Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 1, value);
        }
        [ObservableProperty]
        bool cV46ValueBit2;
        partial void OnCV46ValueBit2Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 2, value);
        }
        [ObservableProperty]
        bool cV46ValueBit3;
        partial void OnCV46ValueBit3Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 3, value);
        }
        [ObservableProperty]
        bool cV46ValueBit4;
        partial void OnCV46ValueBit4Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 4, value);
        }
        [ObservableProperty]
        bool cV46ValueBit5;
        partial void OnCV46ValueBit5Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 5, value);
        }
        [ObservableProperty]
        bool cV46ValueBit6;
        partial void OnCV46ValueBit6Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 6, value);
        }
        [ObservableProperty]
        bool cV46ValueBit7;
        partial void OnCV46ValueBit7Changed(bool value)
        {
            CV46Value = Bit.Set((byte)CV46Value, 7, value);
        }
        #endregion

        [ObservableProperty]
        bool dataStoreDataValid;
      
        [ObservableProperty]
        internal string zIMOFunctionMappingType;

        [ObservableProperty]
        bool doehlerAndHaassExtendedFunctionMappingEnabled;
        partial void OnDoehlerAndHaassExtendedFunctionMappingEnabledChanged(bool value)
        {
            DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled = value;
        }

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
        }

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
        }

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
        }

        //ZIMO_FUNCKEY_MUTE_CV313
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
                DecoderConfiguration.ZIMO.FuncKeyNrMute = (byte)value;

            }
        }

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

            }
        }

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
        }

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
        }

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
        }

        #endregion

        #region REGION: CONSTRUCTOR

        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public FunctionKeysViewModel()
        {

            AvailableFunctionKeys = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableFunctionKeys(true));
            ZIMOFunctionMappingType = String.Empty;

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
        }


        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  RCN 225
            CV33Value = DecoderConfiguration.RCN225.FunctionMappingF0Forward;
            GetCV33Bits();
            CV34Value = DecoderConfiguration.RCN225.FunctionMappingF0Backward;
            GetCV34Bits();
            CV35Value = DecoderConfiguration.RCN225.FunctionMappingF1;
            GetCV35Bits();
            CV36Value = DecoderConfiguration.RCN225.FunctionMappingF2;
            GetCV36Bits();
            CV37Value = DecoderConfiguration.RCN225.FunctionMappingF3;
            GetCV37Bits();
            CV38Value = DecoderConfiguration.RCN225.FunctionMappingF4;
            GetCV38Bits();
            CV39Value = DecoderConfiguration.RCN225.FunctionMappingF5;
            GetCV39Bits();
            CV40Value = DecoderConfiguration.RCN225.FunctionMappingF6;
            GetCV40Bits();
            CV41Value = DecoderConfiguration.RCN225.FunctionMappingF7;
            GetCV41Bits();
            CV42Value = DecoderConfiguration.RCN225.FunctionMappingF8;
            GetCV42Bits();
            CV43Value = DecoderConfiguration.RCN225.FunctionMappingF9;
            GetCV43Bits();
            CV44Value = DecoderConfiguration.RCN225.FunctionMappingF10;
            GetCV44Bits();
            CV45Value = DecoderConfiguration.RCN225.FunctionMappingF11;
            GetCV45Bits();
            CV46Value = DecoderConfiguration.RCN225.FunctionMappingF12;
            GetCV46Bits();

            //  Döhler and Haass
            DoehlerAndHaassExtendedFunctionMappingEnabled = DecoderConfiguration.DoehlerHaas.ExtendedFunctionKeyMappingEnabled;
            DoehlerAndHaassFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysAccDecDisableFuncKeyNumber;
            DoehlerAndHaassFuncKeysShuntingFuncKeyNumber = DecoderConfiguration.DoehlerHaas.FuncKeysShuntingFuncKeyNumber;

            //  ZIMO
            ZIMOFuncKeysAccDecDisableFuncKeyNumber = DecoderConfiguration.ZIMO.FuncKeysAccDecDisableFuncKeyNumber;
            if (DecoderConfiguration.ZIMO.FunctionKeyMappingType == 0)
            {
                ZIMOFunctionMappingType = ZIMOEnumConverter.GetMappingTypeDescription(ZIMO.FunctionMappingTypes.RCN225);
            }
            else if (DecoderConfiguration.ZIMO.FunctionKeyMappingType == 97)
            {
                ZIMOFunctionMappingType = ZIMOEnumConverter.GetMappingTypeDescription(ZIMO.FunctionMappingTypes.RCN225);
            }
            else
            {
                ZIMOFunctionMappingType = AppResources.ZIMOMappingTypeUnknown;
            }
            ZIMOFuncKeysSoundVolumeLouder = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeLouder;
            ZIMOFuncKeysSoundVolumeQuieter = DecoderConfiguration.ZIMO.FuncKeyNrSoundVolumeQuieter;
            ZIMOFuncKeysSoundOnOff = DecoderConfiguration.ZIMO.FuncKeyNrSoundOnOff;
            ZIMOFuncKeysCurveSqueal = DecoderConfiguration.ZIMO.FuncKeyNrCurveSqueal;
            ZIMOFuncKeysMute = DecoderConfiguration.ZIMO.FuncKeyNrMute;

        }

        private void GetCV33Bits()
        {
            CV33ValueBit0 = Bit.IsSet(CV33Value, 0);
            CV33ValueBit1 = Bit.IsSet(CV33Value, 1);
            CV33ValueBit2 = Bit.IsSet(CV33Value, 2);
            CV33ValueBit3 = Bit.IsSet(CV33Value, 3);
            CV33ValueBit4 = Bit.IsSet(CV33Value, 4);
            CV33ValueBit5 = Bit.IsSet(CV33Value, 5);
            CV33ValueBit6 = Bit.IsSet(CV33Value, 6);
            CV33ValueBit7 = Bit.IsSet(CV33Value, 7);
        }
      
        private void GetCV34Bits()
        {
            CV34ValueBit0 = Bit.IsSet(CV34Value, 0);
            CV34ValueBit1 = Bit.IsSet(CV34Value, 1);
            CV34ValueBit2 = Bit.IsSet(CV34Value, 2);
            CV34ValueBit3 = Bit.IsSet(CV34Value, 3);
            CV34ValueBit4 = Bit.IsSet(CV34Value, 4);
            CV34ValueBit5 = Bit.IsSet(CV34Value, 5);
            CV34ValueBit6 = Bit.IsSet(CV34Value, 6);
            CV34ValueBit7 = Bit.IsSet(CV34Value, 7);
        }

        private void GetCV35Bits()
        {
            CV35ValueBit0 = Bit.IsSet(CV35Value, 0);
            CV35ValueBit1 = Bit.IsSet(CV35Value, 1);
            CV35ValueBit2 = Bit.IsSet(CV35Value, 2);
            CV35ValueBit3 = Bit.IsSet(CV35Value, 3);
            CV35ValueBit4 = Bit.IsSet(CV35Value, 4);
            CV35ValueBit5 = Bit.IsSet(CV35Value, 5);
            CV35ValueBit6 = Bit.IsSet(CV35Value, 6);
            CV35ValueBit7 = Bit.IsSet(CV35Value, 7);
        }

        private void GetCV36Bits()
        {
            CV36ValueBit0 = Bit.IsSet(CV36Value, 0);
            CV36ValueBit1 = Bit.IsSet(CV36Value, 1);
            CV36ValueBit2 = Bit.IsSet(CV36Value, 2);
            CV36ValueBit3 = Bit.IsSet(CV36Value, 3);
            CV36ValueBit4 = Bit.IsSet(CV36Value, 4);
            CV36ValueBit5 = Bit.IsSet(CV36Value, 5);
            CV36ValueBit6 = Bit.IsSet(CV36Value, 6);
            CV36ValueBit7 = Bit.IsSet(CV36Value, 7);
        }

        private void GetCV37Bits()
        {
            CV37ValueBit0 = Bit.IsSet(CV37Value, 0);
            CV37ValueBit1 = Bit.IsSet(CV37Value, 1);
            CV37ValueBit2 = Bit.IsSet(CV37Value, 2);
            CV37ValueBit3 = Bit.IsSet(CV37Value, 3);
            CV37ValueBit4 = Bit.IsSet(CV37Value, 4);
            CV37ValueBit5 = Bit.IsSet(CV37Value, 5);
            CV37ValueBit6 = Bit.IsSet(CV37Value, 6);
            CV37ValueBit7 = Bit.IsSet(CV37Value, 7);
        }

        private void GetCV38Bits()
        {
            CV38ValueBit0 = Bit.IsSet(CV38Value, 0);
            CV38ValueBit1 = Bit.IsSet(CV38Value, 1);
            CV38ValueBit2 = Bit.IsSet(CV38Value, 2);
            CV38ValueBit3 = Bit.IsSet(CV38Value, 3);
            CV38ValueBit4 = Bit.IsSet(CV38Value, 4);
            CV38ValueBit5 = Bit.IsSet(CV38Value, 5);
            CV38ValueBit6 = Bit.IsSet(CV38Value, 6);
            CV38ValueBit7 = Bit.IsSet(CV38Value, 7);
        }

        private void GetCV39Bits()
        {
            CV39ValueBit0 = Bit.IsSet(CV39Value, 0);
            CV39ValueBit1 = Bit.IsSet(CV39Value, 1);
            CV39ValueBit2 = Bit.IsSet(CV39Value, 2);
            CV39ValueBit3 = Bit.IsSet(CV39Value, 3);
            CV39ValueBit4 = Bit.IsSet(CV39Value, 4);
            CV39ValueBit5 = Bit.IsSet(CV39Value, 5);
            CV39ValueBit6 = Bit.IsSet(CV39Value, 6);
            CV39ValueBit7 = Bit.IsSet(CV39Value, 7);
        }

        private void GetCV40Bits()
        {
            CV40ValueBit0 = Bit.IsSet(CV40Value, 0);
            CV40ValueBit1 = Bit.IsSet(CV40Value, 1);
            CV40ValueBit2 = Bit.IsSet(CV40Value, 2);
            CV40ValueBit3 = Bit.IsSet(CV40Value, 3);
            CV40ValueBit4 = Bit.IsSet(CV40Value, 4);
            CV40ValueBit5 = Bit.IsSet(CV40Value, 5);
            CV40ValueBit6 = Bit.IsSet(CV40Value, 6);
            CV40ValueBit7 = Bit.IsSet(CV40Value, 7);
        }

        private void GetCV41Bits()
        {
            CV41ValueBit0 = Bit.IsSet(CV41Value, 0);
            CV41ValueBit1 = Bit.IsSet(CV41Value, 1);
            CV41ValueBit2 = Bit.IsSet(CV41Value, 2);
            CV41ValueBit3 = Bit.IsSet(CV41Value, 3);
            CV41ValueBit4 = Bit.IsSet(CV41Value, 4);
            CV41ValueBit5 = Bit.IsSet(CV41Value, 5);
            CV41ValueBit6 = Bit.IsSet(CV41Value, 6);
            CV41ValueBit7 = Bit.IsSet(CV41Value, 7);
        }

        private void GetCV42Bits()
        {
            CV42ValueBit0 = Bit.IsSet(CV42Value, 0);
            CV42ValueBit1 = Bit.IsSet(CV42Value, 1);
            CV42ValueBit2 = Bit.IsSet(CV42Value, 2);
            CV42ValueBit3 = Bit.IsSet(CV42Value, 3);
            CV42ValueBit4 = Bit.IsSet(CV42Value, 4);
            CV42ValueBit5 = Bit.IsSet(CV42Value, 5);
            CV42ValueBit6 = Bit.IsSet(CV42Value, 6);
            CV42ValueBit7 = Bit.IsSet(CV42Value, 7);
        }

        private void GetCV43Bits()
        {
            CV43ValueBit0 = Bit.IsSet(CV43Value, 0);
            CV43ValueBit1 = Bit.IsSet(CV43Value, 1);
            CV43ValueBit2 = Bit.IsSet(CV43Value, 2);
            CV43ValueBit3 = Bit.IsSet(CV43Value, 3);
            CV43ValueBit4 = Bit.IsSet(CV43Value, 4);
            CV43ValueBit5 = Bit.IsSet(CV43Value, 5);
            CV43ValueBit6 = Bit.IsSet(CV43Value, 6);
            CV43ValueBit7 = Bit.IsSet(CV43Value, 7);
        }


        private void GetCV44Bits()
        {
            CV44ValueBit0 = Bit.IsSet(CV44Value, 0);
            CV44ValueBit1 = Bit.IsSet(CV44Value, 1);
            CV44ValueBit2 = Bit.IsSet(CV44Value, 2);
            CV44ValueBit3 = Bit.IsSet(CV44Value, 3);
            CV44ValueBit4 = Bit.IsSet(CV44Value, 4);
            CV44ValueBit5 = Bit.IsSet(CV44Value, 5);
            CV44ValueBit6 = Bit.IsSet(CV44Value, 6);
            CV44ValueBit7 = Bit.IsSet(CV44Value, 7);
        }

        private void GetCV45Bits()
        {
            CV45ValueBit0 = Bit.IsSet(CV45Value, 0);
            CV45ValueBit1 = Bit.IsSet(CV45Value, 1);
            CV45ValueBit2 = Bit.IsSet(CV45Value, 2);
            CV45ValueBit3 = Bit.IsSet(CV45Value, 3);
            CV45ValueBit4 = Bit.IsSet(CV45Value, 4);
            CV45ValueBit5 = Bit.IsSet(CV45Value, 5);
            CV45ValueBit6 = Bit.IsSet(CV45Value, 6);
            CV45ValueBit7 = Bit.IsSet(CV45Value, 7);
        }

        private void GetCV46Bits()
        {
            CV46ValueBit0 = Bit.IsSet(CV46Value, 0);
            CV46ValueBit1 = Bit.IsSet(CV46Value, 1);
            CV46ValueBit2 = Bit.IsSet(CV46Value, 2);
            CV46ValueBit3 = Bit.IsSet(CV46Value, 3);
            CV46ValueBit4 = Bit.IsSet(CV46Value, 4);
            CV46ValueBit5 = Bit.IsSet(CV46Value, 5);
            CV46ValueBit6 = Bit.IsSet(CV46Value, 6);
            CV46ValueBit7 = Bit.IsSet(CV46Value, 7);
        }

        #endregion

    }
}
