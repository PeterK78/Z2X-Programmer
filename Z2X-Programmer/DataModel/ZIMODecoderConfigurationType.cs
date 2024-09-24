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
using static Z2XProgrammer.Helper.ZIMO;

namespace Z2XProgrammer.DataModel
{
    internal class ZIMODecoderConfigurationType
    {
        internal List<ConfigurationVariableType> configurationVariables = new List<ConfigurationVariableType>();

        public ZIMODecoderConfigurationType(ref List<ConfigurationVariableType> cvList)
        {
            configurationVariables = cvList;
        }

        /// <summary>
        /// Returns TRUE if the decoder plays a sound when writing
        /// to a CV (CV144, Bit 4).
        /// </summary>
        public bool PlaySoundWhenProgrammingCV
        {
            get
            {
                return Bit.IsSet(configurationVariables[144].Value, 4);

            }
            set
            {
                configurationVariables[144].Value = Bit.Set(configurationVariables[144].Value, 4, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the update of the decoder firmware is
        /// locked in the controller (CV144 - Bit 7).
        /// </summary>
        public bool LockUpatingDecoderFirmware
        {
            get
            {
                return Bit.IsSet(configurationVariables[144].Value, 7);

            }
            set
            {
                configurationVariables[144].Value = Bit.Set(configurationVariables[144].Value, 7, value);
            }
        }


        /// <summary>
        /// Returns TRUE if the writing to CVs on the main track is
        /// locked in the controller (CV144 - Bit 3).
        /// </summary>
        public bool LockWritingCVsOnMainTrack
        {
            get
            {
                return Bit.IsSet(configurationVariables[144].Value, 3);

            }
            set
            {
                configurationVariables[144].Value = Bit.Set(configurationVariables[144].Value, 3, value);
            }
        }


        

        /// <summary>
        /// The maximum volume if the volume is controlled by the function key in CV397.
        /// </summary>
        public byte MaximumVolumeForFuncKeysControl
        {
            get
            {
                return configurationVariables[395].Value;
            }
            set
            {
                configurationVariables[395].Value = value;
            }
        }

        /// <summary>
        /// The overall sound volume in CV266
        /// </summary>
        public byte OverallVolume
        {
            get
            {
                return configurationVariables[266].Value;
            }
            set
            {
                configurationVariables[266].Value = value;
            }
        }


        /// <summary>
        /// The duration of the noise reduction during deceleration in CV 285
        /// </summary>
        public byte SoundDurationNoiseReduction
        {
            get
            {
                return configurationVariables[285].Value;
            }
            set
            {
                configurationVariables[285].Value = value;
            }
        }

        /// <summary>
        /// The volume of the sound at low speed without a load in CV 275
        /// </summary>
        public byte SoundVolumeSlowSpeedNoLoad
        {
            get
            {
                return configurationVariables[275].Value;
            }
            set
            {
                configurationVariables[275].Value = value;
            }
        }


        /// <summary>
        /// The volume of the sound of electro motors dependended of the speed in CV298
        /// </summary>
        public byte SoundEMotorVolumeDependedSpeed
        {
            get
            {
                return configurationVariables[298].Value;
            }
            set
            {
                configurationVariables[298].Value = value;
            }
        }

        /// <summary>
        /// The volume of the sound of electro motors in CV296
        /// </summary>
        public byte SoundEMotorVolume
        {
            get
            {
                return configurationVariables[296].Value;
            }
            set
            {
                configurationVariables[296].Value = value;
            }
        }


        /// <summary>
        /// The volume of the sound during deceleartion in CV286
        /// </summary>
        public byte SoundVolumeDeceleration
        {
            get
            {
                return configurationVariables[286].Value;
            }
            set
            {
                configurationVariables[286].Value = value;
            }
        }

        /// <summary>
        /// The volume of the sound during acceleration in CV283
        /// </summary>
        public byte SoundVolumeAcceleration
        {
            get
            {
                return configurationVariables[283].Value;
            }
            set
            {
                configurationVariables[283].Value = value;
            }
        }

        

        /// <summary>
        /// The volume of the sound at fast speed without a load in CV 276
        /// </summary>
        public byte SoundVolumeFastSpeedNoLoad
        {
            get
            {
                return configurationVariables[276].Value;
            }
            set
            {
                configurationVariables[276].Value = value;
            }
        }


        /// <summary>
        /// The start up delay of the sound in CV 273
        /// </summary>
        public byte SoundStartUpDelay
        {
            get
            {
                return configurationVariables[273].Value;
            }
            set
            {
                configurationVariables[273].Value = value;
            }
        }

        /// <summary>
        /// The overall sound volume in CV287
        /// </summary>
        public byte BreakSquealLevel
        {
            get
            {
                return configurationVariables[287].Value;
            }
            set
            {
                configurationVariables[287].Value = value;
            }
        }


        /// <summary>
        /// Returns TRUE if the writing to CVs on the programming track is
        /// locked in the controller (CV144 - Bit 6).
        /// </summary>
        public bool LockWritingCVsOnProgramTrack
        {
            get
            {
                return Bit.IsSet(configurationVariables[144].Value, 6);

            }
            set
            {
                configurationVariables[144].Value = Bit.Set(configurationVariables[144].Value, 6, value);
            }
        }

        /// <summary>
        /// Returns TRUE if the rading to CVs on the programming track is
        /// locked in the controller (CV144 - Bit 5).
        /// </summary>
        public bool LockReadingCVsOnProgramTrack
        {
            get
            {
                return Bit.IsSet(configurationVariables[144].Value, 5);

            }
            set
            {
                configurationVariables[144].Value = Bit.Set(configurationVariables[144].Value, 5, value);
            }
        }


        /// <summary>
        /// Sets or gets the light effect for ouput 0 front.
        /// </summary>
        public LightEffects LightEffectOutput0v
        {
            get
            {

                if (configurationVariables[125].Value == 0) return LightEffects.NoEffect;
                if (configurationVariables[125].Value == 88) return LightEffects.DimmingUpAndDown;
                return LightEffects.NoEffect;
            }
            set
            {
                if (value == LightEffects.NoEffect) configurationVariables[125].Value = 0;
                if (value == LightEffects.DimmingUpAndDown) configurationVariables[125].Value = 88;
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput 0 front.
        /// </summary>
        public LightEffects LightEffectOutput0r
        {
            get
            {

                if (configurationVariables[126].Value == 0) return LightEffects.NoEffect;
                if (configurationVariables[126].Value == 88) return LightEffects.DimmingUpAndDown;
                return LightEffects.NoEffect;
            }
            set
            {
                if (value == LightEffects.NoEffect) configurationVariables[126].Value = 0;
                if (value == LightEffects.DimmingUpAndDown) configurationVariables[126].Value = 88;
            }
        }



        /// <summary>
        /// Sets or gets the dimming value (brightness) for all functions outputs in CV60.
        /// </summary>
        public byte DimmingFunctionOutputMasterValue
        {
            get
            {
                return configurationVariables[60].Value;
            }
            set
            {
                configurationVariables[60].Value = value;
            }
        }

        /// <summary>
        /// Sets or gets selft test function in CV30.
        /// </summary>
        public byte SelfTest
        {
            get
            {
                return configurationVariables[30].Value;
            }
            set
            {
                configurationVariables[30].Value = value;
            }
        }

        /// <summary>
        /// Enables or disables dimming for the function outputs FA0 to FA06 in CV114.
        /// </summary>
        public byte DimmingFunctionFA0FA06OutputsEnabled
        {
            get
            {
                return configurationVariables[114].Value;
            }
            set
            {
                configurationVariables[114].Value = value;
            }
        }

        /// <summary>
        /// Enables or disables dimming for the function outputs FA0 to FA06 in CV114.
        /// </summary>
        public byte DimmingFunctionFA7FA12OutputsEnabled
        {
            get
            {
                return configurationVariables[152].Value;
            }
            set
            {
                configurationVariables[152].Value = value;
            }
        }



        /// <summary>
        /// Gets or sets the motor PID settings in CV56
        /// </summary>
        public byte MotorPIDSettings
        {
            get
            {
                return configurationVariables[56].Value;
            }
            set
            {
                configurationVariables[56].Value = value;
            }
        }

    
        /// <summary>
        /// Gets or sets the motor reference voltage in CV57
        /// 
        /// Note: The behavior of CV57 has changed between ZIMO MX and MS decoders.
        /// </summary>
        public byte MotorReferenceVoltage
        {
            get
            {
                return configurationVariables[57].Value;
            }
            set
            {
                configurationVariables[57].Value = value;
            }

        }


        /// <summary>
        /// Gets or sets the motor control frequency in CV9
        /// </summary>
        public byte MotorFrequencyControl
        {
            get
            {
                return configurationVariables[9].Value;
            }
            set
            {
                configurationVariables[9].Value = value;
            }

        }


        /// <summary>
        /// Returns the ZIMO specific software version of a decoder (CV7 + CV65)
        /// </summary>
        public string SoftwareVersion
        {
            get
            {
                return configurationVariables[7].Value.ToString() + "." + configurationVariables[65].Value.ToString();
            }

        }

        /// <summary>
        /// Returns the ZIMO specific decoder type (CV250)
        /// </summary>
        public byte DecoderType
        {
            get
            {
                return configurationVariables[250].Value;
            }
        }

        /// <summary>
        /// Returns the ZIMO specific boot loader version (CV248, CV249)
        /// </summary>
        public string BootloaderVersion
        {
            get
            {
                return configurationVariables[248].Value.ToString() + "." + configurationVariables[249].Value.ToString();
            }
        }

        /// <summary>
        /// Returns the ZIMO specific decoder ID (CV250, CV251, CV252, CV253)
        /// </summary>
        public string DecoderID
        {
            get
            {
                return configurationVariables[250].Value.ToString() + "." + configurationVariables[251].Value.ToString() + "." + configurationVariables[252].Value.ToString() + "." + configurationVariables[253].Value.ToString();
            }
        }


        /// <summary>
        ///  The type of the function mapping in CV61
        /// </summary>
        public byte FunctionKeyMappingType
        {
            get
            {
                return configurationVariables[61].Value;
            }
            set
            {
                configurationVariables[61].Value = value;
            }
        }

        /// <summary>
        /// The number of the function key (F1-F28) to set turn on and off the sound in CV310
        /// </summary>
        public byte FuncKeyNrSoundOnOff
        {
            get
            {
                return configurationVariables[310].Value;
            }
            set
            {
                configurationVariables[310].Value = value;
            }

        }

        /// <summary>
        /// The number of the function key (F1-F28) to mute the sound in CV313.
        /// </summary>
        public byte FuncKeyNrMute
        {
            get
            {
                return configurationVariables[313].Value;
            }
            set
            {
                configurationVariables[313].Value = value;
            }

        }

        /// <summary>
        /// The number of the function key (F1-F28) to set the sound volume louder in CV397
        /// </summary>
        public byte FuncKeyNrSoundVolumeLouder
        {
            get
            {
                return configurationVariables[397].Value;
            }
            set
            {
                configurationVariables[397].Value = value;
            }

        }

        /// <summary>
        /// The number of the function key (F1-F28) to turn on and off the curve squeal sound CV308
        /// </summary>
        public byte FuncKeyNrCurveSqueal
        {
            get
            {
                return configurationVariables[308].Value;
            }
            set
            {
                configurationVariables[308].Value = value;
            }

        }

        /// <summary>
        /// The number of the function key (F1-F28) to set the sound volume quieter in CV396
        /// </summary>
        public byte FuncKeyNrSoundVolumeQuieter
        {
            get
            {
                return configurationVariables[396].Value;
            }
            set
            {
                configurationVariables[396].Value = value;
            }

        }

        /// <summary>
        /// The number of the function key (F1-F28) to disable the accerleration and deceleration times
        /// in CV156.
        /// </summary>
        public byte FuncKeysAccDecDisableFuncKeyNumber
        {
            get
            {
                return configurationVariables[156].Value;
            }
            set
            {
                configurationVariables[156].Value = value;
            }
        }

        /// <summary>
        /// The address mode of the secondary address (long versus short).
        /// Configured in CV 112 bit 5.
        /// </summary>
        internal NMRA.DCCAddressModes DCCAddressModeSecondaryAdr
        {
            get
            {
                if (Bit.IsSet(configurationVariables[112].Value, 5) == true)
                {
                    return NMRA.DCCAddressModes.Extended;
                }
                return NMRA.DCCAddressModes.Short;
            }
            set
            {
                if (value == NMRA.DCCAddressModes.Extended)
                {
                    configurationVariables[112].Value = Bit.Set(configurationVariables[112].Value, 5, true);
                    return;
                }
                configurationVariables[112].Value = Bit.Set(configurationVariables[112].Value, 5, false);
                return;
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
        /// The secondary address. Used by ZIMO FX decoders.
        /// </summary>
        public ushort SecondaryAddress
        {
            get
            {
                if (DCCAddressModeSecondaryAdr == NMRA.DCCAddressModes.Short) return configurationVariables[64].Value;
                return NMRA.GetExtendedDCCAddress(configurationVariables[67].Value, configurationVariables[68].Value);
            }
            set
            {
                if (DCCAddressModeSecondaryAdr == NMRA.DCCAddressModes.Short)
                {
                    configurationVariables[64].Value = (byte)value;
                    return;
                }
                byte cv67 = 0; byte cv68 = 0;
                if (NMRA.ConvertExtendedDCCAddressToCVValues(value, out cv67, out cv68) == true)
                {
                    configurationVariables[67].Value = cv67;
                    configurationVariables[68].Value = cv68;
                }
            }
        }

    }
}


