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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.DataModel
{

    /// <summary>
    /// Contains the data model of a decoder configuration according to RC
    /// Nn225.
    /// </summary>
    internal class RCN225DecoderConfigurationType
    {

        internal List<ConfigurationVariableType> configurationVariables = new List<ConfigurationVariableType>();

        private static ExtendedSpeedCurveType _extendedSpeedCurve = new ExtendedSpeedCurveType();

        public RCN225DecoderConfigurationType(ref List<ConfigurationVariableType> cvList)
        {
            configurationVariables = cvList;
        }

        /// <summary>
        /// The locomotive address. Depending on the configuration (Long addresses vs. short addresses) in CV1,
        /// or in CV17 and CV18.
        /// </summary>
        public ushort LocomotiveAddress
        {
            get
            {
                if (DCCAddressModeVehicleAdr == NMRA.DCCAddressModes.Short) return configurationVariables[1].Value;
                return NMRA.GetExtendedDCCAddress(configurationVariables[17].Value, configurationVariables[18].Value);
            }
            set
            {
                if (DCCAddressModeVehicleAdr == NMRA.DCCAddressModes.Short)
                {
                    configurationVariables[1].Value = (byte)value;
                    return;
                }
                byte cv17 = 0; byte cv18 = 0;
                if (NMRA.ConvertExtendedDCCAddressToCVValues(value, out cv17, out cv18) == true)
                {
                    configurationVariables[17].Value = cv17;
                    configurationVariables[18].Value = cv18;
                }
            }
        }

        /// <summary>
        /// The direction for consist mode in CV19.7.
        /// </summary>
        public bool ReverseDirectionConsistMode
        {
            get
            {
                return Bit.IsSet(configurationVariables[19].Value, 7);
            }
            set
            {
                Bit.Set(configurationVariables[19].Value,7, value);
            }
        }

        /// <summary>
        /// The consist address in CV19 and CV20.
        /// </summary>
        public ushort ConsistAddress
        {
            get
            {
                //  If CV20 is not equal to 0, we use the stanndard consist addresses
                if (configurationVariables[20].Value == 0)
                {
                    //  The highest bit stores the direction during consist operation. We remove the bit
                    //  before returning the address
                    byte tempValue = configurationVariables[19].Value;
                    return Bit.Set(tempValue, 7, false);
                }
                else
                {
                    return NMRA.GetExtendedConsistAddress(configurationVariables[19].Value, configurationVariables[20].Value);
                }   
            }
            set
            {
                //  Do we have to turn off consist addresses?
                if(value == 0)
                {
                    configurationVariables[19].Value = 0;
                    configurationVariables[20].Value = 0;
                    return;
                }

                // Consist addresses are valid between 0 and 10239.
                if (value > 10239) return;

                //  Standard consist addresses in CV19 are valid between 1 and 127. If the consist address is higher, we use CV19 and CV20
                if (value <= 127)
                {   
                    bool tempDirection = ReverseDirectionConsistMode;
                    configurationVariables[19].Value = (byte)value;
                    configurationVariables[19].Value = Bit.Set(configurationVariables[19].Value, 7, tempDirection);
                    configurationVariables[20].Value = 0;
                }
                else
                {
                    byte cv19 = 0; byte cv20 = 0;
                    if (NMRA.ConvertExtendedConsistAddressToCVValues(value, ReverseDirectionConsistMode, out cv19, out cv20) == true)
                    {
                        configurationVariables[19].Value = cv19;
                        configurationVariables[20].Value = cv20;
                    }
                }
            }
        }
       
        /// <summary>
        /// Contains the minimum speed of CV2.
        /// </summary>
        public byte MinimumSpeed
        {
            get
            {
                return configurationVariables[2].Value;
            }
            set
            {
                configurationVariables[2].Value = value;
            }
        }

        ///// <summary>
        ///// If the acceleration rate in CV3 is 0, the acceleration rate feature
        ///// is disabled in the decoder. All other values are enabling the acceleration rate
        ///// feature.
        ///// </summary>
        //public bool AccelerationRateEnabled
        //{
        //    get
        //    {
        //        if (configurationVariables[3].Value > 0) return true;
        //        return false;
        //    }
        //    set
        //    {
        //        if (value == false)
        //        {
        //            configurationVariables[3].Value = 0;
        //            return;
        //        }
        //        else
        //        {
        //            configurationVariables[3].Value = 1;
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// If the acceleration rate in CV3 is 0, the acceleration rate feature
        /// is disabled in the decoder. All other values are enabling the acceleration rate
        /// feature.
        /// </summary>
        public byte AccelerationRate
        {
            get
            {
                return configurationVariables[3].Value;
            }
            set
            {
                configurationVariables[3].Value = value;
            }

        }

        /// <summary>
        /// Contains the function mapping for the key F0 (forward) in CV33
        /// </summary>
        public byte FunctionMappingF0Forward
        {
            get
            {
                return configurationVariables[33].Value;
            }
            set
            {
                configurationVariables[33].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F0 (backward) in CV34
        /// </summary>
        public byte FunctionMappingF0Backward
        {
            get
            {
                return configurationVariables[34].Value;
            }
            set
            {
                configurationVariables[34].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F1 in CV35
        /// </summary>
        public byte FunctionMappingF1
        {
            get
            {
                return configurationVariables[35].Value;
            }
            set
            {
                configurationVariables[35].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F2 in CV36
        /// </summary>
        public byte FunctionMappingF2
        {
            get
            {
                return configurationVariables[36].Value;
            }
            set
            {
                configurationVariables[36].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F3 in CV37
        /// </summary>
        public byte FunctionMappingF3
        {
            get
            {
                return configurationVariables[37].Value;
            }
            set
            {
                configurationVariables[37].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F4 in CV38
        /// </summary>
        public byte FunctionMappingF4
        {
            get
            {
                return configurationVariables[38].Value;
            }
            set
            {
                configurationVariables[38].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F5 in CV39
        /// </summary>
        public byte FunctionMappingF5
        {
            get
            {
                return configurationVariables[39].Value;
            }
            set
            {
                configurationVariables[39].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F6 in CV40
        /// </summary>
        public byte FunctionMappingF6
        {
            get
            {
                return configurationVariables[40].Value;
            }
            set
            {
                configurationVariables[40].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F7 in CV41
        /// </summary>
        public byte FunctionMappingF7
        {
            get
            {
                return configurationVariables[41].Value;
            }
            set
            {
                configurationVariables[41].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F8 in CV42
        /// </summary>
        public byte FunctionMappingF8
        {
            get
            {
                return configurationVariables[42].Value;
            }
            set
            {
                configurationVariables[42].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F9 in CV43
        /// </summary>
        public byte FunctionMappingF9
        {
            get
            {
                return configurationVariables[43].Value;
            }
            set
            {
                configurationVariables[43].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F10 in CV44
        /// </summary>
        public byte FunctionMappingF10
        {
            get
            {
                return configurationVariables[44].Value;
            }
            set
            {
                configurationVariables[44].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F11 in CV45
        /// </summary>
        public byte FunctionMappingF11
        {
            get
            {
                return configurationVariables[45].Value;
            }
            set
            {
                configurationVariables[45].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping for the key F12 in CV46
        /// </summary>
        public byte FunctionMappingF12
        {
            get
            {
                return configurationVariables[46].Value;
            }
            set
            {
                configurationVariables[46].Value = value;
            }
        }





        /// <summary>
        /// If the deceleration rate in CV4 is 0, the deceleartion rate feature
        /// is disabled in the decoder. All other values are enabling the deceleartion rate
        /// feature.
        /// </summary>
        public byte DecelerationRate
        {
            get
            {
                return configurationVariables[4].Value;
            }
            set
            {
                configurationVariables[4].Value = value;
            }

        }

        ///// <summary>
        ///// If the deceleration rate in CV4 is 0, the deceleration rate feature
        ///// is disabled in the decoder. All other values are enabling the acceleration rate
        ///// feature.
        ///// </summary>
        //public bool DecelerationRateEnabled
        //{
        //    get
        //    {
        //        if (configurationVariables[4].Value > 0) return true;
        //        return false;
        //    }
        //    set
        //    {
        //        if (value == false)
        //        {
        //            configurationVariables[4].Value = 0;
        //            return;
        //        }
        //        else
        //        {
        //            configurationVariables[4].Value = 1;
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// Contains the minimum speed of CV5
        /// </summary>
        public byte MaximumSpeed
        {
            get
            {
                return configurationVariables[5].Value;
            }
            set
            {
                configurationVariables[5].Value = value;
            }
        }


        /// <summary>
        /// Contains the minimum speed of CV6
        /// </summary>
        public byte MediumSpeed
        {
            get
            {
                return configurationVariables[6].Value;
            }
            set
            {
                configurationVariables[6].Value = value;
            }
        }

        /// <summary>
        /// The name of the manufacturer
        /// </summary>
        public string Version
        {
            get
            {
                return configurationVariables[7].Value.ToString();
            }
        }

        /// <summary>
        /// The name of the manufacturer
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return NMRA.GetManufacturerName(configurationVariables[8].Value);
            }
        }

        /// <summary>
        /// The ID of the manufacturer
        /// </summary>
        public byte ManufacturerID
        {
            get
            {
                return configurationVariables[8].Value;
            }
        }


        /// <summary>
        /// The password for the decoder lock in CV15
        /// </summary>
        public byte DecoderLockPasswordCV15
        {
            get
            {
                return configurationVariables[15].Value;
            }
            set
            {
                configurationVariables[15].Value = value;
            }
        }

        /// <summary>
        /// The password for the decoder lock in CV16
        /// </summary>
        public  byte DecoderLockPasswordCV16
        {
            get
            {
                return configurationVariables[16].Value;
            }
            set
            {
                configurationVariables[16].Value = value;
            }
        }

        /// <summary>
        /// Returns TRUE if the RailCom address broadcast on channel 1
        /// is enabled. Or FALSE if not.
        /// </summary>
        public bool RailComChannel1AdrBroadcast
        {
            get
            {
                return Bit.IsSet(configurationVariables[28].Value, 0);
            }
            set
            {
                configurationVariables[28].Value = Bit.Set(configurationVariables[28].Value, 0, value);
            }

        }





        /// <summary>
        /// Returns TRUE if the RailCom channel 2 is enabled.
        /// Or FALSE if not.
        /// </summary>
        public bool RailComChannel2Enabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[28].Value, 1);
            }
            set
            {
                configurationVariables[28].Value = Bit.Set(configurationVariables[28].Value, 1, value);
            }

        }

        /// <summary>
        /// Returns TRUE if HLU is enabled in CV27.2.
        /// </summary>
        public bool HLUEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[27].Value, 2);
            }
            set
            {
                configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 2, value);
            }

        }

        /// <summary>
        /// Returns TRUE if the reversal of the direction in consist mode is enabled in CV19.7.
        /// </summary>
        public bool DirectionReversalConsistMode
        {
            get
            {
                return Bit.IsSet(configurationVariables[19].Value, 7);
            }
            set
            {
                configurationVariables[19].Value = Bit.Set(configurationVariables[19].Value, 7, value);
            }

        }

        /// <summary>
        /// Returns TRUE if the reversal of the direction is enabled in CV29.1.
        /// </summary>
        public bool DirectionReversal
        {
            get
            {
                return Bit.IsSet(configurationVariables[29].Value, 0);
            }
            set
            {
                configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 0, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the analog DC operation is enabled in CV12 bit 0.
        /// </summary>
        public bool OperatingModeAnalogDCEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[12].Value, 0);
            }
            set
            {
                configurationVariables[12].Value = Bit.Set(configurationVariables[12].Value, 0, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the DCC operation is enabled in CV12 bit 2.
        /// </summary>
        public bool OperatingModeDCCEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[12].Value, 2);
            }
            set
            {
                configurationVariables[12].Value = Bit.Set(configurationVariables[12].Value, 2, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the analog AC operation is enabled in CV12 bit 4.
        /// </summary>
        public bool OperatingModeAnalogACEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[12].Value, 4);
            }
            set
            {
                configurationVariables[12].Value = Bit.Set(configurationVariables[12].Value, 4, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the Märklin Motorola operation mode is enabled in CV12 bit 5.
        /// </summary>
        public bool OperatingModeMMEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[12].Value, 5);
            }
            set
            {
                configurationVariables[12].Value = Bit.Set(configurationVariables[12].Value, 5, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the Märklin MFX operation mode is enabled in CV12 bit 6.
        /// </summary>
        public bool OperatingModeMFXEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[12].Value, 6);
            }
            set
            {
                configurationVariables[12].Value = Bit.Set(configurationVariables[12].Value, 6, value);
            }
        }

        /// <summary>
        /// Returns TRUE if RailCom is activated in CV29 bit 3. Or
        /// false if not.
        /// </summary>
        public bool RailComEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[29].Value, 3);
            }
            set
            {
                configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 3, value);
            }
        }

        /// <summary>
        /// Returns TRUE if automatic registration (RCN-218 oder RailComPlus®) is enabled in CV28 bit 7. If not FALSE will be returned.
        /// </summary>
        public bool AutomaticRegistrationEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[28].Value, 7);
            }
            set
            {
                configurationVariables[28].Value = Bit.Set(configurationVariables[28].Value, 7, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the AC mode in CV29 bit 2 is enabled. If not FALSE will be returned.
        /// </summary>
        public  bool ACModeEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[29].Value, 2);
            }
            set
            {
                configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 2, value);
            }
        }

        /// <summary>
        /// Returns the selected speed step mode (14 or 28/128)
        /// Returns mode NMRA.DCCSpeedStepsMode.Step28to128 is bit 1
        /// in CV29 is set. If this bit is 0 MRA.DCCSpeedStepsMode.Step28to128
        /// </summary>
        public  NMRA.DCCSpeedStepsModes SpeedStepsMode
        {
            get
            {
                if (Bit.IsSet(configurationVariables[29].Value, 1) == true)
                {
                    return NMRA.DCCSpeedStepsModes.Step28to128;
                }
                else
                {
                    return NMRA.DCCSpeedStepsModes.Steps14;

                }
            }
            set
            {
                switch (value)
                {
                    case NMRA.DCCSpeedStepsModes.Steps14: configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 1, false); break;
                    case NMRA.DCCSpeedStepsModes.Step28to128: configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 1, true); break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// Returns the selected ABC break mode in CV27.0 and CV27.1.
        /// </summary>
        public NMRA.DCCABCBreakModes ABCBreakMode
        {            
            get
            {
                bool bit27_0 = Bit.IsSet(configurationVariables[27].Value, 0);
                bool bit27_1 = Bit.IsSet(configurationVariables[27].Value, 1);

                if ((bit27_0 == false) && (bit27_1 == false)) return NMRA.DCCABCBreakModes.Off;
                if ((bit27_0 == true) && (bit27_1 == false)) return NMRA.DCCABCBreakModes.RightTrack;
                if ((bit27_0 == false) && (bit27_1 == true)) return NMRA.DCCABCBreakModes.LeftTrack;
                return NMRA.DCCABCBreakModes.Off;
            }
            set
            {
                switch (value)
                {
                    case NMRA.DCCABCBreakModes.Off:

                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 0, false);
                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 1, false);
                        break;

                    case NMRA.DCCABCBreakModes.RightTrack:

                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 0, true);
                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 1, false);
                        break;

                    case NMRA.DCCABCBreakModes.LeftTrack:

                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 0, false);
                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 1, true);
                        break;

                    default:

                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 0, false);
                        configurationVariables[27].Value = Bit.Set(configurationVariables[27].Value, 1, false);
                        break;

                }
            }
        }
      


        

        /// <summary>
        /// Is TRUE if the additional speed parameters have been enabled in
        /// CV 29.4.
        /// </summary>
        public bool ExtendedSpeedCurveEnabled
        {
            get
            {
                return Bit.IsSet(configurationVariables[29].Value, 4);
            }
            set
            {
                configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 4, value);
            }

        }

        /// <summary>
        /// The address mode of the vehicle address (long versus short).
        /// </summary>
        internal NMRA.DCCAddressModes DCCAddressModeVehicleAdr
        {
            get
            {
                if (Bit.IsSet(configurationVariables[29].Value, 5) == true)
                {
                    return NMRA.DCCAddressModes.Extended;
                }
                return NMRA.DCCAddressModes.Short;
            }
            set
            {
                if (value == NMRA.DCCAddressModes.Extended)
                {
                    configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 5, true);
                    return;
                }
                configurationVariables[29].Value = Bit.Set(configurationVariables[29].Value, 5, false);
                return;
            }
        }

        /// <summary>
        /// Sets the CVs 67 to 94 to the values of the extended speed curve.
        /// </summary>
        public ExtendedSpeedCurveType ExtendedSpeedCurveValues
        {
            get
            {
                for (int i = 0; i <= NMRA.NumberOfExtendedSpeedSteps - 1; i++)
                {
                    _extendedSpeedCurve.CV[i].Value = configurationVariables[i + 67].Value;
                    _extendedSpeedCurve.CV[i].Number = i + 67;
                }
                return _extendedSpeedCurve;
            }
            set
            {
                _extendedSpeedCurve = value;
                for (int i = 0; i <= NMRA.NumberOfExtendedSpeedSteps - 1; i++)
                {
                    configurationVariables[i + 67].Value = _extendedSpeedCurve.CV[i].Value;
                }
            }
        }

    }

}
