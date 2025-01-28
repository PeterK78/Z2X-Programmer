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

using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;
using static Z2XProgrammer.Helper.ZIMO;

namespace Z2XProgrammer.Converter
{
    internal static class ZIMOEnumConverter
    {
        private static List<ZIMOSUSIPinModeType> _ZIMOSUSIPinModeTypes = new List<ZIMOSUSIPinModeType>();
        private static List<ZIMOMotorControlFrequencyType> _ZIMOMotorControlFrequencyTypes = new List<ZIMOMotorControlFrequencyType>();
        private static List<ZIMOMotorControlPIDMotorType> _ZIMOMotorControlPIDMotorTypes = new List<ZIMOMotorControlPIDMotorType>();
        private static List<ZIMOLightEffectType> _ZIMOLightEffects = new List<ZIMOLightEffectType>();
        private static List<ZIMOLightEffectDirectionType> _ZIMOLightEffectDirections = new List<ZIMOLightEffectDirectionType>();

        static ZIMOEnumConverter()
        {
            //  Initialize the supported ZIMO light effects in CV125x.
            InitializeZIMOLightEffects();

            ZIMOMotorControlFrequencyType lowFreq = new ZIMOMotorControlFrequencyType();
            lowFreq.Description = GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.LowFrequency);
            lowFreq.FreqType = ZIMO.MotorControlFrequencyTypes.LowFrequency;
            _ZIMOMotorControlFrequencyTypes.Add(lowFreq);

            ZIMOMotorControlFrequencyType highFreq = new ZIMOMotorControlFrequencyType();
            highFreq.Description = GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.HighFrequency);
            highFreq.FreqType = ZIMO.MotorControlFrequencyTypes.HighFrequency;
            _ZIMOMotorControlFrequencyTypes.Add(highFreq);


            ZIMOMotorControlPIDMotorType normalMotor = new ZIMOMotorControlPIDMotorType();
            normalMotor.Description = GetMotorControlPIDMotorTypeDescription(MotorControlPIDMotorTypes.Normal);
            normalMotor.MotorType = MotorControlPIDMotorTypes.Normal;
            _ZIMOMotorControlPIDMotorTypes.Add(normalMotor);

            ZIMOMotorControlPIDMotorType bellAnchorMotor = new ZIMOMotorControlPIDMotorType();
            bellAnchorMotor.Description = GetMotorControlPIDMotorTypeDescription(MotorControlPIDMotorTypes.BellAnchor);
            bellAnchorMotor.MotorType = MotorControlPIDMotorTypes.BellAnchor;
            _ZIMOMotorControlPIDMotorTypes.Add(bellAnchorMotor);
   
            //
            //  Setup the SUSI pin mode types
            //
            ZIMOSUSIPinModeType SUSIPinLogicLevelOutput = new ZIMOSUSIPinModeType();
            SUSIPinLogicLevelOutput.Description = GetSUSIInterface1PinModeDescription(SUSIPinModeType.LogicLevelOutput);
            SUSIPinLogicLevelOutput.SUSIPinMode = SUSIPinModeType.LogicLevelOutput;
            _ZIMOSUSIPinModeTypes.Add(SUSIPinLogicLevelOutput);

            ZIMOSUSIPinModeType SUSIPinLogicLevelInput = new ZIMOSUSIPinModeType();
            SUSIPinLogicLevelInput.Description = GetSUSIInterface1PinModeDescription(SUSIPinModeType.LogicLevelInput);
            SUSIPinLogicLevelInput.SUSIPinMode = SUSIPinModeType.LogicLevelInput;
            _ZIMOSUSIPinModeTypes.Add(SUSIPinLogicLevelInput);

            ZIMOSUSIPinModeType SUSIPinServo = new ZIMOSUSIPinModeType();
            SUSIPinServo.Description = GetSUSIInterface1PinModeDescription(SUSIPinModeType.ServoControlLine);
            SUSIPinServo.SUSIPinMode = SUSIPinModeType.ServoControlLine;
            _ZIMOSUSIPinModeTypes.Add(SUSIPinServo);

            ZIMOSUSIPinModeType SUSIPinSUSI = new ZIMOSUSIPinModeType();
            SUSIPinSUSI.Description = GetSUSIInterface1PinModeDescription(SUSIPinModeType.SUSI);
            SUSIPinSUSI.SUSIPinMode = SUSIPinModeType.SUSI;
            _ZIMOSUSIPinModeTypes.Add(SUSIPinSUSI);

            ZIMOSUSIPinModeType SUSIPinI2C = new ZIMOSUSIPinModeType();
            SUSIPinI2C.Description = GetSUSIInterface1PinModeDescription(SUSIPinModeType.I2C);
            SUSIPinI2C.SUSIPinMode = SUSIPinModeType.I2C;
            _ZIMOSUSIPinModeTypes.Add(SUSIPinI2C);
        }

        /// <summary>
        /// Initializes the ZIMO light effects in CV125X.
        /// It setups the light directions as well as the supported light effects.
        /// </summary>
        internal static void InitializeZIMOLightEffects()
        {
            //  
            //  Setup the three differen light effect directions
            //

            //  Independend = 0.
            ZIMOLightEffectDirectionType directionIndepend = new ZIMOLightEffectDirectionType();
            directionIndepend.Description = AppResources.LightEffectDirectionTypeIndepend;
            directionIndepend.Direction = LightEffectDirection.DirectionIndependend;
            _ZIMOLightEffectDirections.Add(directionIndepend);

            //  Forward = 1.
            ZIMOLightEffectDirectionType directionForward = new ZIMOLightEffectDirectionType();
            directionForward.Description = AppResources.LightEffectDirectionTypeForward;
            directionForward.Direction = LightEffectDirection.Forward;
            _ZIMOLightEffectDirections.Add(directionForward);

            //  Backward = 2.
            ZIMOLightEffectDirectionType directionBackward = new ZIMOLightEffectDirectionType();
            directionBackward.Description = AppResources.LightEffectDirectionTypeBackward;
            directionBackward.Direction = LightEffectDirection.Backward;
            _ZIMOLightEffectDirections.Add(directionBackward);

            //
            //  Setup the 9 supported light effects.
            //

            //  No effect = 0.
            ZIMOLightEffectType noEffect = new ZIMOLightEffectType();
            noEffect.Description = GetLightEffectDescription(LightEffects.NoEffect);
            noEffect.EffectType = LightEffects.NoEffect;
            _ZIMOLightEffects.Add(noEffect);

            //  Dimming up and down = 88.
            ZIMOLightEffectType dimUpAndDownEffect = new ZIMOLightEffectType();
            dimUpAndDownEffect.Description = GetLightEffectDescription(LightEffects.DimmingUpAndDown);
            dimUpAndDownEffect.EffectType = LightEffects.DimmingUpAndDown;
            _ZIMOLightEffects.Add(dimUpAndDownEffect);

            //  Fluorescent tube effect = 92.
            ZIMOLightEffectType fluorescentTubeEffect = new ZIMOLightEffectType();
            fluorescentTubeEffect.Description = GetLightEffectDescription(LightEffects.FluorescentTubeEffect);
            fluorescentTubeEffect.EffectType = LightEffects.FluorescentTubeEffect;
            _ZIMOLightEffects.Add(fluorescentTubeEffect);

            //  Function output turns off at speed = 60.
            ZIMOLightEffectType functionOutputTurnsOffAtSpeed = new ZIMOLightEffectType();
            functionOutputTurnsOffAtSpeed.Description = GetLightEffectDescription(LightEffects.FunctionOutputTurnsOffAtSpeed);
            functionOutputTurnsOffAtSpeed.EffectType = LightEffects.FunctionOutputTurnsOffAtSpeed;
            _ZIMOLightEffects.Add(functionOutputTurnsOffAtSpeed);

            //  Decoupler = 48.
            ZIMOLightEffectType decoupler = new ZIMOLightEffectType();
            decoupler.Description = GetLightEffectDescription(LightEffects.Decoupler);
            decoupler.EffectType = LightEffects.Decoupler;
            _ZIMOLightEffects.Add(decoupler);

            //  Double pulse strobe = 22.   
            ZIMOLightEffectType doublePulseStrobe = new ZIMOLightEffectType();
            doublePulseStrobe.Description = GetLightEffectDescription(LightEffects.DoublePulseStrobe);
            doublePulseStrobe.EffectType = LightEffects.DoublePulseStrobe;
            _ZIMOLightEffects.Add(doublePulseStrobe);

            //  Single pulse strobe = 16.
            ZIMOLightEffectType singlePulseStrobe = new ZIMOLightEffectType();
            singlePulseStrobe.Description = GetLightEffectDescription(LightEffects.SinglePulseStrobe);
            singlePulseStrobe.EffectType = LightEffects.SinglePulseStrobe;
            _ZIMOLightEffects.Add(singlePulseStrobe);

            //  Rotary beacon = 24. 
            ZIMOLightEffectType rotaryBeacon = new ZIMOLightEffectType();
            rotaryBeacon.Description = GetLightEffectDescription(LightEffects.RotaryBeacon);
            rotaryBeacon.EffectType = LightEffects.RotaryBeacon;
            _ZIMOLightEffects.Add(rotaryBeacon);

            //  Soft start = 52.
            ZIMOLightEffectType softStart = new ZIMOLightEffectType();
            softStart.Description = GetLightEffectDescription(LightEffects.SoftStart);
            softStart.EffectType = LightEffects.SoftStart;
            _ZIMOLightEffects.Add(softStart);

        }

        /// <summary>
        /// Returns the light effect direction enum for the given description.
        /// </summary>
        internal static ZIMO.LightEffectDirection GetLightEffectDirectionType (string description)
        {
            if(description == GetLightEffectDirectionDescription(LightEffectDirection.DirectionIndependend)) return LightEffectDirection.DirectionIndependend;
            if (description == GetLightEffectDirectionDescription(LightEffectDirection.Forward)) return LightEffectDirection.Forward;
            if (description == GetLightEffectDirectionDescription(LightEffectDirection.Backward)) return LightEffectDirection.Backward;
            return LightEffectDirection.DirectionIndependend;
        }

        /// <summary>
        /// Returns the light effect type for the given light effect description.
        /// </summary>
        internal static ZIMO.LightEffects GetLightEffectType (string description)
        {
            if (description == GetLightEffectDescription(LightEffects.NoEffect)) return LightEffects.NoEffect;
            if (description == GetLightEffectDescription(LightEffects.DimmingUpAndDown)) return LightEffects.DimmingUpAndDown;
            if (description == GetLightEffectDescription(LightEffects.SinglePulseStrobe)) return LightEffects.SinglePulseStrobe;
            if (description == GetLightEffectDescription(LightEffects.DoublePulseStrobe)) return LightEffects.DoublePulseStrobe;
            if (description == GetLightEffectDescription(LightEffects.RotaryBeacon)) return LightEffects.RotaryBeacon;
            if (description == GetLightEffectDescription(LightEffects.Decoupler)) return LightEffects.Decoupler;
            if (description == GetLightEffectDescription(LightEffects.SoftStart)) return LightEffects.SoftStart;
            if (description == GetLightEffectDescription(LightEffects.FunctionOutputTurnsOffAtSpeed)) return LightEffects.FunctionOutputTurnsOffAtSpeed;
            if (description == GetLightEffectDescription(LightEffects.FluorescentTubeEffect)) return LightEffects.FluorescentTubeEffect;
            if (description == GetLightEffectDescription(LightEffects.Unknown)) return LightEffects.Unknown;
            return LightEffects.NoEffect;
        }

        /// <summary>
        /// Returns a list with available light effect directions.
        /// Note: The light effect direction is used to configure ZIMO light effects in CV125.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAvailableLightEffectDirections()
        {
            List<string> EffectDirection = new List<string>();
            foreach (ZIMOLightEffectDirectionType item in _ZIMOLightEffectDirections)
            {
                if (item.Description != null) EffectDirection.Add(item.Description);
            }
            return EffectDirection;
        }

        /// <summary>
        /// Returns a list with all available light effects.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAvailableLightEffects()
        {
            List<string> Effect = new List<string>();
            foreach (ZIMOLightEffectType item in _ZIMOLightEffects)
            {
                if (item.Description != null) Effect.Add(item.Description);
            }
            return Effect;
        }

        /// <summary>
        /// Returns the description for the given light effect.
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        internal static string GetLightEffectDescription (ZIMO.LightEffects effect)
        {
            switch  (effect)
            {
                case ZIMO.LightEffects.NoEffect: return AppResources.LightEffectTypeNoEffect;
                case ZIMO.LightEffects.DimmingUpAndDown: return AppResources.LightEffectTypeDimmingUpAndDown;
                case ZIMO.LightEffects.SinglePulseStrobe: return AppResources.LightEffectTypeSinglePulseStrobe;
                case ZIMO.LightEffects.DoublePulseStrobe: return AppResources.LightEffectTypeDoublePulseStrobe;
                case ZIMO.LightEffects.RotaryBeacon: return AppResources.LightEffectTypeRotaryBeacon;
                case ZIMO.LightEffects.Decoupler: return AppResources.LightEffectTypeDecoupler;
                case ZIMO.LightEffects.SoftStart: return AppResources.LightEffectTypeSoftStart;
                case ZIMO.LightEffects.FunctionOutputTurnsOffAtSpeed: return AppResources.LightEffectTypeFunctionOutputTurnsOffAtSpeed;
                case ZIMO.LightEffects.FluorescentTubeEffect: return AppResources.LightEffectTypeFunctionOutputFluorescentTubeEffect;
                case ZIMO.LightEffects.Unknown: return AppResources.LightEffectTypeFunctionOutputUnknown;

                default: return "Unknown ZIMO light effect";
            }
        }

        /// <summary>
        /// Returns the description for the given light effect direction.
        /// </summary>
        /// <param name="direction">A ZIMO light effection direction.</param>
        /// <returns></returns>
        internal static string GetLightEffectDirectionDescription (ZIMO.LightEffectDirection direction)
        {
            switch  (direction)
            {
                case ZIMO.LightEffectDirection.DirectionIndependend: return AppResources.LightEffectDirectionTypeIndepend;
                case ZIMO.LightEffectDirection.Forward: return AppResources.LightEffectDirectionTypeForward;    
                case ZIMO.LightEffectDirection.Backward: return AppResources.LightEffectDirectionTypeBackward;
                default: return "Unknown ZIMO light effect direction.";
            }
        }


        internal static List<string> GetAvailableMotorControlPIDMotorTypes()
        {
            List<string> Names = new List<string>();
            foreach (ZIMOMotorControlPIDMotorType item in _ZIMOMotorControlPIDMotorTypes)
            {
                if (item.Description != null) { Names.Add(item.Description); }

            }
            return Names;
        }


        
        internal static ZIMO.SUSIPinModeType GetSUSIInterface1PinMode (string modeDescription)
        {
            try
            {
                if(modeDescription == AppResources.ZIMOSusiPinTypeLogicLevelOutput) return ZIMO.SUSIPinModeType.LogicLevelOutput;
                if(modeDescription == AppResources.ZIMOSusiPinTypeLogicLevelReedInput) return ZIMO.SUSIPinModeType.LogicLevelInput;
                if(modeDescription == AppResources.ZIMOSusiPinTypeServo) return ZIMO.SUSIPinModeType.ServoControlLine;
                if(modeDescription == AppResources.ZIMOSusiPinTypeSUSI) return ZIMO.SUSIPinModeType.SUSI;
                if(modeDescription == AppResources.ZIMOSusiPinTypeI2C) return ZIMO.SUSIPinModeType.I2C;
                return ZIMO.SUSIPinModeType.Unknown;                    
            }
            catch
            {
                return ZIMO.SUSIPinModeType.Unknown;                    
            }

        }

        /// <summary>
        /// Converts a SUSIPinModeType to a human readable character string.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        internal static string GetSUSIInterface1PinModeDescription (ZIMO.SUSIPinModeType mode)
        {
            try
            {
                switch (mode)
                {
                    case ZIMO.SUSIPinModeType.LogicLevelOutput: return AppResources.ZIMOSusiPinTypeLogicLevelOutput;
                    case ZIMO.SUSIPinModeType.LogicLevelInput: return AppResources.ZIMOSusiPinTypeLogicLevelReedInput;
                    case ZIMO.SUSIPinModeType.ServoControlLine: return AppResources.ZIMOSusiPinTypeServo;
                    case ZIMO.SUSIPinModeType.SUSI: return AppResources.ZIMOSusiPinTypeSUSI;
                    case ZIMO.SUSIPinModeType.I2C: return AppResources.ZIMOSusiPinTypeI2C;
                    default: return AppResources.ZIMOSusiPinTypeUnknown;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

       

        internal static string GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes type)
        {
            switch (type)
            {
                case ZIMO.MotorControlPIDMotorTypes.Normal: return AppResources.FrameMotorCharacteristicsPIDMotorTypeNormal; 
                case ZIMO.MotorControlPIDMotorTypes.BellAnchor: return AppResources.FrameMotorCharacteristicsPIDMotorTypeBellAnchor; 
                default: return "Unknown ZIMO motor control PID motor type";
            }
        }

        /// <summary>
        /// Returns a list with all available SUSI pin modes.
        /// </summary>
        /// <returns></returns>
        internal static List<string>GetAvailableSUSIPinModes()
        {
            List<string> PinModes = new List<string>();
            foreach (ZIMOSUSIPinModeType item in _ZIMOSUSIPinModeTypes)
            {
              if(item.Description != null) PinModes.Add(item.Description);
            }
            return PinModes;
        }


        internal static List<string> GetAvailableMotorControlFrequencyTypes()
        {
            List<string> Names = new List<string>();
            foreach (ZIMOMotorControlFrequencyType item in _ZIMOMotorControlFrequencyTypes)
            {
              if(item.Description != null) Names.Add(item.Description);
            }
            return Names;
        }

        internal static string GetMotorControlFrequencyTypeDescription(ZIMO.MotorControlFrequencyTypes type)
        {
            switch (type)
            {
                case ZIMO.MotorControlFrequencyTypes.LowFrequency: return AppResources.ZimoMotorControlFreqTypeLow; 
                case ZIMO.MotorControlFrequencyTypes.HighFrequency: return AppResources.ZimoMotorControlFreqTypeHigh; 
                default: return "Unknown ZIMO motor control frequency type"; 
            }
        }

        internal static ZIMO.MotorControlFrequencyTypes GetMotorControlFrequencyType (string description)
        {
            if (description == GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.HighFrequency)) return MotorControlFrequencyTypes.HighFrequency;
            return MotorControlFrequencyTypes.LowFrequency;
        }

        /// <summary>
        /// Returns the description of the selected function mapping in CV61.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetMappingTypeDescription (ZIMO.FunctionMappingTypes type)
        {
            switch (type)
            {
                case ZIMO.FunctionMappingTypes.RCN225: return AppResources.ZIMOMappingTypeRCN225;
                case ZIMO.FunctionMappingTypes.ExtendedMapping: return AppResources.ZIMOMappingTypeExtended;
                default: return "Unknown ZIMO function mapping type";
            }

        }      

        internal static ZIMO.FunctionMappingTypes GetMappingType(string description)
        {
            if (description == GetMappingTypeDescription(FunctionMappingTypes.ExtendedMapping)) return ZIMO.FunctionMappingTypes.ExtendedMapping;
            return FunctionMappingTypes.RCN225; 
        }

    }
}
