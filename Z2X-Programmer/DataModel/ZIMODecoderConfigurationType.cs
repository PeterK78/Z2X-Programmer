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
        /// Sets or gets the light effect for ouput 0 front in CV125.
        /// </summary>
        public LightEffects LightEffectOutput0v
        {
            get
            {
                return GetLighEffect(125);
            }
            set
            {
                SetLightEffect(125, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput 0 front in CV125.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutput0v
        {
            get
            {
                return GetLightEffectDirection(125);
            }
            set
            {
                SetLightEffectDirection(125, value);
            }
        }


        /// <summary>
        /// Sets or gets the light effect for ouput 0 rear in CV126.
        /// </summary>
        public LightEffects LightEffectOutput0r
        {
            get
            {
                return GetLighEffect(126);
            }
            set
            {
                SetLightEffect(126, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput 0 rear in CV126.
        /// </summary>  
        public LightEffectDirection LightEffectDirectionOutput0r
        {
            get
            {
               return GetLightEffectDirection(126);
            }
            set
            {
                SetLightEffectDirection(126, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA1 in CV127.
        /// </summary>
        public LightEffects LightEffectOutputFA1
        {
            get
            {
                return GetLighEffect(127);
            }
            set
            {
                SetLightEffect(127, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA1 in CV127.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA1
        {
            get
            {
               return GetLightEffectDirection(127);
            }
            set
            {
                SetLightEffectDirection(127, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA2 in CV128.
        /// </summary>
        public LightEffects LightEffectOutputFA2
        {
            get
            {
                return GetLighEffect(128);
            }
            set
            {
                SetLightEffect(128, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA2 in CV128.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA2
        {
            get
            {
               return GetLightEffectDirection(128);
            }
            set
            {
                SetLightEffectDirection(128, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA3 in CV129.
        /// </summary>
        public LightEffects LightEffectOutputFA3
        {
            get
            {
                return GetLighEffect(129);
            }
            set
            {
                SetLightEffect(129, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA3 in CV129.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA3
        {
            get
            {
               return GetLightEffectDirection(129);
            }
            set
            {
                SetLightEffectDirection(129, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA4 in CV130.
        /// </summary>
        public LightEffects LightEffectOutputFA4
        {
            get
            {
                return GetLighEffect(130);
            }
            set
            {
                SetLightEffect(130, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA4 in CV130.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA4
        {
            get
            {
               return GetLightEffectDirection(130);
            }
            set
            {
                SetLightEffectDirection(130, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA5 in CV131.
        /// </summary>
        public LightEffects LightEffectOutputFA5
        {
            get
            {
                return GetLighEffect(131);
            }
            set
            {
                SetLightEffect(131, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA5 in CV131.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA5
        {
            get
            {
               return GetLightEffectDirection(131);
            }
            set
            {
                SetLightEffectDirection(131, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA6 in CV132.
        /// </summary>
        public LightEffects LightEffectOutputFA6
        {
            get
            {
                return GetLighEffect(132);
            }
            set
            {
                SetLightEffect(132, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA6 in CV132.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA6
        {
            get
            {
               return GetLightEffectDirection(132);
            }
            set
            {
                SetLightEffectDirection(132, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA7 in CV159.
        /// </summary>
        public LightEffects LightEffectOutputFA7
        {
            get
            {
                return GetLighEffect(159);
            }
            set
            {
                SetLightEffect(159, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA7 in CV159.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA7
        {
            get
            {
               return GetLightEffectDirection(159);
            }
            set
            {
                SetLightEffectDirection(159, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect for ouput FA8 in CV160.
        /// </summary>
        public LightEffects LightEffectOutputFA8
        {
            get
            {
                return GetLighEffect(160);
            }
            set
            {
                SetLightEffect(160, value);
            }
        }

        /// <summary>
        /// Sets or gets the light effect direction for ouput FA8 in CV160.
        /// </summary>
        public LightEffectDirection LightEffectDirectionOutputFA8
        {
            get
            {
               return GetLightEffectDirection(160);
            }
            set
            {
                SetLightEffectDirection(160, value);
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F0 (CV400).
        /// </summary>
        public ZIMOInputMappingType InputMappingF0
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(0);
            }
            set
            {
                configurationVariables[400].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F1 (CV401).
        /// </summary>
        public ZIMOInputMappingType InputMappingF1
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(1);
            }
            set
            {
                configurationVariables[401].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F2 (CV402).
        /// </summary>
        public ZIMOInputMappingType InputMappingF2
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(2);
            }
            set
            {
                configurationVariables[402].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F3 (CV403).
        /// </summary>
        public ZIMOInputMappingType InputMappingF3
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(3);
            }
            set
            {
                configurationVariables[403].Value = (byte)value.InternalFunctionKeyNumber;
            }

        }

        /// <summary>
        /// Returns the input mapping for the external function key F4 (CV404).
        /// </summary>
        public ZIMOInputMappingType InputMappingF4
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(4);
            }
            set
            {
                configurationVariables[404].Value = (byte)value.InternalFunctionKeyNumber;
            }

        }

        /// <summary>
        /// Returns the input mapping for the external function key F5 (CV405).
        /// </summary>
        public ZIMOInputMappingType InputMappingF5
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(5);
            }
            set
            {
                configurationVariables[405].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F6 (CV406).
        /// </summary>
        public ZIMOInputMappingType InputMappingF6
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(6);
            }
            set
            {
                configurationVariables[406].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F7 (CV407).
        /// </summary>
        public ZIMOInputMappingType InputMappingF7
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(7);
            }
            set
            {
                configurationVariables[407].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }


        /// <summary>
        /// Returns the input mapping for the external function key F8 (CV408).
        /// </summary>
        public ZIMOInputMappingType InputMappingF8
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(8);
            }
            set
            {
                configurationVariables[408].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F9 (CV409).
        /// </summary>
        public ZIMOInputMappingType InputMappingF9
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(9);
            }
            set
            {
                configurationVariables[409].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F10 (CV410).
        /// </summary>
        public ZIMOInputMappingType InputMappingF10
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(10);
            }
            set
            {
                configurationVariables[410].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F11 (CV411).
        /// </summary>
        public ZIMOInputMappingType InputMappingF11
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(11);
            }
            set
            {
                configurationVariables[411].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F12 (CV412).
        /// </summary>
        public ZIMOInputMappingType InputMappingF12
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(12);
            }
            set
            {
                configurationVariables[412].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F13 (CV413).
        /// </summary>
        public ZIMOInputMappingType InputMappingF13
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(13);
            }
            set
            {
                configurationVariables[413].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F14 (CV414).
        /// </summary>
        public ZIMOInputMappingType InputMappingF14
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(14);
            }
            set
            {
                configurationVariables[414].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F15 (CV415).
        /// </summary>
        public ZIMOInputMappingType InputMappingF15
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(15);
            }
            set
            {
                configurationVariables[415].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F16 (CV416).
        /// </summary>
        public ZIMOInputMappingType InputMappingF16
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(16);
            }
            set
            {
                configurationVariables[416].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F17 (CV417).
        /// </summary>
        public ZIMOInputMappingType InputMappingF17
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(17);
            }
            set
            {
                configurationVariables[417].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F18 (CV418).
        /// </summary>
        public ZIMOInputMappingType InputMappingF18
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(18);
            }
            set
            {
                configurationVariables[418].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F19 (CV419).
        /// </summary>
        public ZIMOInputMappingType InputMappingF19
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(19);
            }
            set
            {
                configurationVariables[419].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// Returns the input mapping for the external function key F20 (CV420).
        /// </summary>
        public ZIMOInputMappingType InputMappingF20
        {
            get
            {
                return GetInputMappingInternalFunctionKeyNumber(20);
            }
            set
            {
                configurationVariables[420].Value = (byte)value.InternalFunctionKeyNumber;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalFunctionKeyNumber"></param>
        /// <returns></returns>
        private ZIMOInputMappingType GetInputMappingInternalFunctionKeyNumber (int externalFunctionKeyNumber)
        {
                ZIMOInputMappingType item = new ZIMOInputMappingType();
                item.ExternalFunctionKeyNumber = -1;
                item.InternalFunctionKeyNumber = -1;

                if (externalFunctionKeyNumber < 0) return item;
                if (externalFunctionKeyNumber > 28) return item;

                item.ExternalFunctionKeyNumber = externalFunctionKeyNumber;
                item.ExternalFunctionKeyDescription = "F" + externalFunctionKeyNumber;
                item.CVNumber = 400 + externalFunctionKeyNumber;
                
                // 0 = NO MAPPING -> 1:1 CONNECTION
                if (configurationVariables[externalFunctionKeyNumber+400].Value == 0)
                {
                    item.InternalFunctionKeyNumber = 0;
                }
                // 1 ... 28 => F1 ... F28, 29 ... F0
                else if ((configurationVariables[externalFunctionKeyNumber+400].Value > 0) && (configurationVariables[externalFunctionKeyNumber+400].Value < 30))
                {
                    item.InternalFunctionKeyNumber = configurationVariables[externalFunctionKeyNumber+400].Value;
                }
                // UNKNOWN CONFIGURATION
                else
                {
                    item.InternalFunctionKeyNumber = -1;
                }
                return item;

        }

        /// <summary>
        /// Sets or gets the fade in time for various light effects in CV190.
        /// </summary>  
        public byte LightEffectFadeInTime
        {
            get
            {
                return configurationVariables[190].Value;
            }
            set
            {
                configurationVariables[190].Value = value;
            }
        }

        /// <summary>
        /// Sets or gets the fade out time for various light effects in CV191.
        /// </summary>  
        public byte LightEffectFadeOutTime
        {
            get
            {
                return configurationVariables[191].Value;
            }
            set
            {
                configurationVariables[191].Value = value;
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
        /// Gets or sets the motor PID settings in CV56.
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
        /// Returns the version of the bootloader in CV248. See also BootloaderSubVersion of CV249.
        /// </summary>
        public byte BootloaderVersion
        {
            get
            {
                return configurationVariables[248].Value;
            }
        }

        /// <summary>
        /// Returns the subversion of the bootloader in CV249. See also BootloaderVersion of CV248.
        /// </summary>
        public byte BootloaderSubVersion
        {
            get
            {
                return configurationVariables[249].Value;
            }
        }

        /// <summary>
        /// The pin mode of the SUSI interface 1 (CV201)
        /// </summary>
        public SUSIPinModeType SUSIInterface1PinMode
        {
            get
            {
                switch (configurationVariables[201].Value)
                {
                    case 0: return SUSIPinModeType.SUSI;
                    case 11: return SUSIPinModeType.LogicLevelOutput;
                    case 22: return SUSIPinModeType.LogicLevelInput;
                    case 33: return SUSIPinModeType.ServoControlLine;
                    case 44: return SUSIPinModeType.SUSI;
                    case 55: return SUSIPinModeType.I2C;
                    default: return SUSIPinModeType.Unknown;
                }
            }
            set
            {
                switch (value)
                {
                    case SUSIPinModeType.SUSI: configurationVariables[201].Value = 44;break;
                    case SUSIPinModeType.LogicLevelOutput: configurationVariables[201].Value = 11;break;
                    case SUSIPinModeType.LogicLevelInput: configurationVariables[201].Value = 22;break;
                    case SUSIPinModeType.ServoControlLine: configurationVariables[201].Value = 33;break;
                    case SUSIPinModeType.I2C: configurationVariables[201].Value = 55;break;
                    case SUSIPinModeType.Unknown: configurationVariables[201].Value = 44;break;
                    default: configurationVariables[201].Value = 44;break;
                }
                return;
                
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
        /// Returns the sound project number in CV254.
        /// </summary>
        public byte SoundProjectNumber
        {
            get
            {
                return configurationVariables[254].Value;
            }
        }


        /// <summary>
        ///  Is TRUE if the extended function mapping in CV61 is activated ( = 97).
        /// </summary>
        public bool ExtendedFunctionKeyMapping
        {
            get
            {
                if (configurationVariables[61].Value == 97) return true;
                return false;
            }
            set
            {
                if (value == true) configurationVariables[61].Value = 97;
                else configurationVariables[61].Value = 0;
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
        /// The configuration of the function key to suppress the light on
        /// driver cab 1 (forward) in CV107. 
        /// </summary>
        public byte FuncKeyNrSuppressLightDriverCab1Forward
        {
            get
            {
                return configurationVariables[107].Value;
            }
            set
            {
                configurationVariables[107].Value = value;
            }
        }

        /// <summary>
        /// The configuration of the additonal function outputs
        /// in CV109 suppressed by the function key (see CV107).
        /// </summary>
        public byte AddOutputsSuppressedLightDriverCab1Forward
        {
            get
            {
                return configurationVariables[109].Value;
            }
            set
            {
                configurationVariables[109].Value = value;
            }
        }


        /// <summary>
        /// The configuration of the function key to suppress the light on
        /// driver cab 2 (backward) in CV108. 
        /// </summary>
        public byte FuncKeyNrSuppressLightDriverCab2Forward
        {
            get
            {
                return configurationVariables[108].Value;
            }
            set
            {
                configurationVariables[108].Value = value;
            }
        }

        /// <summary>
        /// The configuration of the additonal function outputs
        /// in CV110 suppressed by the function key (see CV108).
        /// </summary>
        public byte AddOutputsSuppressedLightDriverCab2Forward
        {
            get
            {
                return configurationVariables[110].Value;
            }
            set
            {
                configurationVariables[110].Value = value;
            }
        }

        /// <summary>
        /// The number of the function key (F1-F28) to turn on and off the curve squeal sound CV308.
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
        /// The number of the shunting function key (F1-F28) and the
        /// shunting speed in CV155.
        /// </summary>
        public byte ShuntingKeyAndShuntingSpeed
        {
            get
            {
                return configurationVariables[155].Value;
            }
            set
            {
                configurationVariables[155].Value = value;
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

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F0 (forward) in CV69
        /// </summary>
        public byte FunctionMappingSecondaryAddressF0Forward
        {
            get
            {
                return configurationVariables[69].Value;
            }
            set
            {
                configurationVariables[69].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F0 (backward) in CV69
        /// </summary>
        public byte FunctionMappingSecondaryAddressF0Backward
        {
            get
            {
                return configurationVariables[70].Value;
            }
            set
            {
                configurationVariables[70].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F1 in CV71
        /// </summary>
        public byte FunctionMappingSecondaryAddressF1
        {
            get
            {
                return configurationVariables[71].Value;
            }
            set
            {
                configurationVariables[71].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F2 in CV72
        /// </summary>
        public byte FunctionMappingSecondaryAddressF2
        {
            get
            {
                return configurationVariables[72].Value;
            }
            set
            {
                configurationVariables[72].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F3 in CV73
        /// </summary>
        public byte FunctionMappingSecondaryAddressF3
        {
            get
            {
                return configurationVariables[73].Value;
            }
            set
            {
                configurationVariables[73].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F4 in CV74
        /// </summary>
        public byte FunctionMappingSecondaryAddressF4
        {
            get
            {
                return configurationVariables[74].Value;
            }
            set
            {
                configurationVariables[74].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F5 in CV75
        /// </summary>
        public byte FunctionMappingSecondaryAddressF5
        {
            get
            {
                return configurationVariables[75].Value;
            }
            set
            {
                configurationVariables[75].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F6 in CV76
        /// </summary>
        public byte FunctionMappingSecondaryAddressF6
        {
            get
            {
                return configurationVariables[76].Value;
            }
            set
            {
                configurationVariables[76].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F7 in CV77
        /// </summary>
        public byte FunctionMappingSecondaryAddressF7
        {
            get
            {
                return configurationVariables[77].Value;
            }
            set
            {
                configurationVariables[77].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F8 in CV78
        /// </summary>
        public byte FunctionMappingSecondaryAddressF8
        {
            get
            {
                return configurationVariables[78].Value;
            }
            set
            {
                configurationVariables[78].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F9 in CV79
        /// </summary>
        public byte FunctionMappingSecondaryAddressF9
        {
            get
            {
                return configurationVariables[79].Value;
            }
            set
            {
                configurationVariables[79].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F10 in CV80
        /// </summary>
        public byte FunctionMappingSecondaryAddressF10
        {
            get
            {
                return configurationVariables[80].Value;
            }
            set
            {
                configurationVariables[80].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F11 in CV81
        /// </summary>
        public byte FunctionMappingSecondaryAddressF11
        {
            get
            {
                return configurationVariables[81].Value;
            }
            set
            {
                configurationVariables[81].Value = value;
            }
        }

        /// <summary>
        /// Contains the function mapping of the secondary address for the key F12 in CV82
        /// </summary>
        public byte FunctionMappingSecondaryAddressF12
        {
            get
            {
                return configurationVariables[82].Value;
            }
            set
            {
                configurationVariables[82].Value = value;
            }
        }

        /// <summary>
        /// Converts the value of a given CV number to a ZIMO light effect.
        /// </summary>
        /// <param name="cvNumber">A ZIMO light effect CV number e.g. 125</param>
        /// <returns></returns>
        internal ZIMO.LightEffects GetLighEffect(int cvNumber)
        {
            //  We grab the value of CV125 and mask out the direction bits.
            byte effect = configurationVariables[cvNumber].Value;
            effect = Bit.Set(effect, 0, false);
            effect = Bit.Set(effect, 1, false);

            //  Check the value and return the corresponding enum.
            if (effect == 0) return LightEffects.NoEffect;
            if (effect == 16) return LightEffects.SinglePulseStrobe;
            if (effect == 20) return LightEffects.DoublePulseStrobe;
            if (effect == 24) return LightEffects.RotaryBeacon;
            if (effect == 48) return LightEffects.Decoupler;
            if (effect == 52) return LightEffects.SoftStart;
            if (effect == 60) return LightEffects.FunctionOutputTurnsOffAtSpeed;
            if (effect == 88) return LightEffects.DimmingUpAndDown;
            if (effect == 92) return LightEffects.FluorescentTubeEffect;

            return LightEffects.Unknown;
        }

        /// <summary>
        /// Converts the value of a given CV number to a ZIMO light effect direction.
        /// </summary>
        /// <param name="cvNumber">A ZIMO light effect CV number e.g. 125</param>
        /// <returns></returns>
        internal LightEffectDirection GetLightEffectDirection (int cvNumber)
        {
            bool forward =  Bit.IsSet(configurationVariables[cvNumber].Value, 0);
            bool backward = Bit.IsSet(configurationVariables[cvNumber].Value, 1);

            if ((forward == false) && (backward == false)) return LightEffectDirection.DirectionIndependend;
            if ((forward == true) && (backward == false)) return LightEffectDirection.Forward;
            if ((forward == false) && (backward == true)) return LightEffectDirection.Backward;
            return LightEffectDirection.DirectionIndependend;
        }

        /// <summary>
        /// Sets the light effect direction for a given CV number.
        /// </summary>
        /// <param name="cvNumber">The desired CV number.</param>
        /// <param name="direction">The desired direciton.</param>
        internal void SetLightEffectDirection (int cvNumber, LightEffectDirection direction)
        {
            if(direction == LightEffectDirection.DirectionIndependend)
            {
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 0, false);
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 1, false);
            }
            if(direction == LightEffectDirection.Forward)
            {
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 0, true);
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 1, false);
            }
            if(direction == LightEffectDirection.Backward)
            {
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 0, false);
                configurationVariables[cvNumber].Value = Bit.Set(configurationVariables[cvNumber].Value, 1, true);
            }
        }

        /// <summary>
        /// Sets the light effect for a given CV number.
        /// </summary>
        /// <param name="cvNumber">The desired CV number.</param>
        /// <param name="effect">The desired effect.</param>
        internal void SetLightEffect(int cvNumber, ZIMO.LightEffects effect)
        {
            //  Save the current direction bits.
            LightEffectDirection currentDirection = GetLightEffectDirection(cvNumber);

            //  Set the effect bits.
            if (effect == LightEffects.NoEffect) configurationVariables[cvNumber].Value = 0;
            if (effect == LightEffects.DimmingUpAndDown) configurationVariables[cvNumber].Value = 88;
            if (effect == LightEffects.SinglePulseStrobe) configurationVariables[cvNumber].Value = 16;
            if (effect == LightEffects.DoublePulseStrobe) configurationVariables[cvNumber].Value = 22;
            if (effect == LightEffects.RotaryBeacon) configurationVariables[cvNumber].Value = 24;
            if (effect == LightEffects.Decoupler) configurationVariables[cvNumber].Value = 48;
            if (effect == LightEffects.SoftStart) configurationVariables[cvNumber].Value = 52;
            if (effect == LightEffects.FunctionOutputTurnsOffAtSpeed) configurationVariables[cvNumber].Value = 60;
            if (effect == LightEffects.FluorescentTubeEffect) configurationVariables[cvNumber].Value = 92;

            //  Restore the direction bits.
            SetLightEffectDirection(cvNumber, currentDirection);
        }
    }
}


