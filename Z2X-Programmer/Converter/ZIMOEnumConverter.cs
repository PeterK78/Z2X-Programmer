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
        private static List<ZIMOEffectType> _ZIMOEffects = new List<ZIMOEffectType>();
        private static List<ZIMOEffectDirectionType> _ZIMOLightEffectDirections = new List<ZIMOEffectDirectionType>();

        static ZIMOEnumConverter()
        {
            //  Initialize the supported ZIMO light and function effects in CV125x.
            InitializeEffects();

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

        //internal static void InitializeZIMOFunctionEffects()
        //{

        //    //   
        //    //  Setup the different ZIMO function effects.
        //    //

        //    //  No effect = 0.
        //    ZIMOFunctionEffectType noEffect = new ZIMOFunctionEffectType();
        //    noEffect.Description = GetFunctionEffectDescription(FunctionEffects.NoEffect);
        //    noEffect.EffectType = FunctionEffects.NoEffect;
        //    _ZIMOFunctionEffects.Add(noEffect);

        //    //  Decoupler = 48.
        //    ZIMOFunctionEffectType decoupler = new ZIMOFunctionEffectType();
        //    decoupler.Description = GetFunctionEffectDescription(FunctionEffects.Decoupler);
        //    decoupler.EffectType = FunctionEffects.Decoupler;
        //    _ZIMOFunctionEffects.Add(decoupler);
        //}


        /// <summary>
        /// Initializes the ZIMO light effects in CV125X.
        /// It setups the light directions as well as the supported light effects.
        /// </summary>
        internal static void InitializeEffects()
        {
            //  
            //  Setup the three different light effect directions
            //

            //  Independend = 0.
            ZIMOEffectDirectionType directionIndepend = new ZIMOEffectDirectionType();
            directionIndepend.Description = AppResources.LightEffectDirectionTypeIndepend;
            directionIndepend.Direction = EffectDirection.DirectionIndependend;
            _ZIMOLightEffectDirections.Add(directionIndepend);

            //  Forward = 1.
            ZIMOEffectDirectionType directionForward = new ZIMOEffectDirectionType();
            directionForward.Description = AppResources.LightEffectDirectionTypeForward;
            directionForward.Direction = EffectDirection.Forward;
            _ZIMOLightEffectDirections.Add(directionForward);

            //  Backward = 2.
            ZIMOEffectDirectionType directionBackward = new ZIMOEffectDirectionType();
            directionBackward.Description = AppResources.LightEffectDirectionTypeBackward;
            directionBackward.Direction = EffectDirection.Backward;
            _ZIMOLightEffectDirections.Add(directionBackward);

            //
            //  Setup the 7 supported light effects, and 1 function effect.
            //

            //  No effect = 0.
            ZIMOEffectType noEffect = new ZIMOEffectType();
            noEffect.Description = GetLightEffectDescription(ZIMOEffects.NoEffect);
            noEffect.Name = ZIMOEffects.NoEffect;
            noEffect.Category = ZIMOEffectCategory.Unknown;
            _ZIMOEffects.Add(noEffect);

            //  Dimming up and down = 88.
            ZIMOEffectType dimUpAndDownEffect = new ZIMOEffectType();
            dimUpAndDownEffect.Description = GetLightEffectDescription(ZIMOEffects.DimmingUpAndDown);
            dimUpAndDownEffect.Name = ZIMOEffects.DimmingUpAndDown;
            dimUpAndDownEffect.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(dimUpAndDownEffect);

            //  Fluorescent tube effect = 92.
            ZIMOEffectType fluorescentTubeEffect = new ZIMOEffectType();
            fluorescentTubeEffect.Description = GetLightEffectDescription(ZIMOEffects.FluorescentTubeEffect);
            fluorescentTubeEffect.Name = ZIMOEffects.FluorescentTubeEffect;
            fluorescentTubeEffect.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(fluorescentTubeEffect);

            //  Function output turns off at speed = 60.
            ZIMOEffectType functionOutputTurnsOffAtSpeed = new ZIMOEffectType();
            functionOutputTurnsOffAtSpeed.Description = GetLightEffectDescription(ZIMOEffects.FunctionOutputTurnsOffAtSpeed);
            functionOutputTurnsOffAtSpeed.Name = ZIMOEffects.FunctionOutputTurnsOffAtSpeed;
            functionOutputTurnsOffAtSpeed.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(functionOutputTurnsOffAtSpeed);

            //  Decoupler = 48.
            ZIMOEffectType decoupler = new ZIMOEffectType();
            decoupler.Description = GetLightEffectDescription(ZIMOEffects.Decoupler);
            decoupler.Name = ZIMOEffects.Decoupler;
            decoupler.Category = ZIMOEffectCategory.Function;
            _ZIMOEffects.Add(decoupler);

            //  Double pulse strobe = 22.   
            ZIMOEffectType doublePulseStrobe = new ZIMOEffectType();
            doublePulseStrobe.Description = GetLightEffectDescription(ZIMOEffects.DoublePulseStrobe);
            doublePulseStrobe.Name = ZIMOEffects.DoublePulseStrobe;
            doublePulseStrobe.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(doublePulseStrobe);

            //  Single pulse strobe = 16.
            ZIMOEffectType singlePulseStrobe = new ZIMOEffectType();
            singlePulseStrobe.Description = GetLightEffectDescription(ZIMOEffects.SinglePulseStrobe);
            singlePulseStrobe.Name = ZIMOEffects.SinglePulseStrobe;
            singlePulseStrobe.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(singlePulseStrobe);

            //  Rotary beacon = 24. 
            ZIMOEffectType rotaryBeacon = new ZIMOEffectType();
            rotaryBeacon.Description = GetLightEffectDescription(ZIMOEffects.RotaryBeacon);
            rotaryBeacon.Name = ZIMOEffects.RotaryBeacon;
            rotaryBeacon.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(rotaryBeacon);

            //  Soft start = 52.
            ZIMOEffectType softStart = new ZIMOEffectType();
            softStart.Description = GetLightEffectDescription(ZIMOEffects.SoftStart);
            softStart.Name = ZIMOEffects.SoftStart;
            softStart.Category = ZIMOEffectCategory.Light;
            _ZIMOEffects.Add(softStart);

        }

        /// <summary>
        /// Returns the light effect direction enum for the given description.
        /// </summary>
        internal static ZIMO.EffectDirection GetLightEffectDirectionType(string description)
        {
            if (description == GetLightEffectDirectionDescription(EffectDirection.DirectionIndependend)) return EffectDirection.DirectionIndependend;
            if (description == GetLightEffectDirectionDescription(EffectDirection.Forward)) return EffectDirection.Forward;
            if (description == GetLightEffectDirectionDescription(EffectDirection.Backward)) return EffectDirection.Backward;
            return EffectDirection.DirectionIndependend;
        }

        /// <summary>
        /// Returns the light effect type for the given light effect description.
        /// </summary>
        internal static ZIMO.ZIMOEffects GetLightEffectType(string description)
        {
            if (description == GetLightEffectDescription(ZIMOEffects.NoEffect)) return ZIMOEffects.NoEffect;
            if (description == GetLightEffectDescription(ZIMOEffects.DimmingUpAndDown)) return ZIMOEffects.DimmingUpAndDown;
            if (description == GetLightEffectDescription(ZIMOEffects.SinglePulseStrobe)) return ZIMOEffects.SinglePulseStrobe;
            if (description == GetLightEffectDescription(ZIMOEffects.DoublePulseStrobe)) return ZIMOEffects.DoublePulseStrobe;
            if (description == GetLightEffectDescription(ZIMOEffects.RotaryBeacon)) return ZIMOEffects.RotaryBeacon;
            if (description == GetLightEffectDescription(ZIMOEffects.Decoupler)) return ZIMOEffects.Decoupler;
            if (description == GetLightEffectDescription(ZIMOEffects.SoftStart)) return ZIMOEffects.SoftStart;
            if (description == GetLightEffectDescription(ZIMOEffects.FunctionOutputTurnsOffAtSpeed)) return ZIMOEffects.FunctionOutputTurnsOffAtSpeed;
            if (description == GetLightEffectDescription(ZIMOEffects.FluorescentTubeEffect)) return ZIMOEffects.FluorescentTubeEffect;
            if (description == GetLightEffectDescription(ZIMOEffects.Unknown)) return ZIMOEffects.Unknown;
            return ZIMOEffects.NoEffect;
        }

        /// <summary>
        /// Returns a list with available light effect directions.
        /// Note: The light effect direction is used to configure ZIMO light effects in CV125.
        /// </summary>
        /// <returns>A list with all available light effects.</returns>
        internal static List<string> GetAvailableLightEffectDirections()
        {
            List<string> EffectDirection = new List<string>();
            foreach (ZIMOEffectDirectionType item in _ZIMOLightEffectDirections)
            {
                if (item.Description != null) EffectDirection.Add(item.Description);
            }
            return EffectDirection;
        }


        /// <summary>
        /// Returns a list with all available function effects.
        /// </summary>
        /// <returns>A list with all available function effects.</returns>
        internal static List<string> GetAvailableFunctionEffects()
        {
            List<string> Effect = new List<string>();
            foreach (ZIMOEffectType item in _ZIMOEffects)
            {
                if ((item.Description != null) && (item.Category == ZIMOEffectCategory.Function))  Effect.Add(item.Description);
            }
            return Effect;
        }

        /// <summary>
        /// Returns a list with all available light effects.
        /// </summary>
        /// <returns>A list with all available function effects.</returns>
        internal static List<string> GetAvailableLightEffects()
        {
            List<string> Effect = new List<string>();
            foreach (ZIMOEffectType item in _ZIMOEffects)
            {
                if (  (item.Description != null) && ((item.Category == ZIMOEffectCategory.Light) || (item.Category == ZIMOEffectCategory.Unknown)))  Effect.Add(item.Description);
            }
            return Effect;
        }


        /// <summary>
        /// Returns a list with all available light effects.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAvailableEffects()
        {
            List<string> Effect = new List<string>();
            foreach (ZIMOEffectType item in _ZIMOEffects)
            {
                if (item.Description != null) Effect.Add(item.Description);
            }
            return Effect;
        }

        /// <summary>
        /// Returns the description for the given function effect.
        /// </summary>
        /// <param name="effect">The ZIMO function effect.</param>
        /// <returns>A description of the effect.</returns>
        internal static string GetFunctionEffectDescription(ZIMO.FunctionEffects effect)
        {
            switch (effect)
            {
                case ZIMO.FunctionEffects.NoEffect: return AppResources.ZIMOEffectTypeNoFunctionEffect;
                case ZIMO.FunctionEffects.Decoupler: return AppResources.ZIMOEffectTypeDecoupler;
                default: return "Unknown ZIMO function effect";
            }
        }


        /// <summary>
        /// Returns the description for the given light effect.
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        internal static string GetLightEffectDescription(ZIMO.ZIMOEffects effect)
        {
            switch (effect)
            {
                case ZIMO.ZIMOEffects.NoEffect: return AppResources.ZIMOEffectTypeNoEffect;
                case ZIMO.ZIMOEffects.DimmingUpAndDown: return AppResources.LightEffectTypeDimmingUpAndDown;
                case ZIMO.ZIMOEffects.SinglePulseStrobe: return AppResources.LightEffectTypeSinglePulseStrobe;
                case ZIMO.ZIMOEffects.DoublePulseStrobe: return AppResources.LightEffectTypeDoublePulseStrobe;
                case ZIMO.ZIMOEffects.RotaryBeacon: return AppResources.LightEffectTypeRotaryBeacon;
                case ZIMO.ZIMOEffects.Decoupler: return AppResources.LightEffectTypeDecoupler;
                case ZIMO.ZIMOEffects.SoftStart: return AppResources.LightEffectTypeSoftStart;
                case ZIMO.ZIMOEffects.FunctionOutputTurnsOffAtSpeed: return AppResources.LightEffectTypeFunctionOutputTurnsOffAtSpeed;
                case ZIMO.ZIMOEffects.FluorescentTubeEffect: return AppResources.LightEffectTypeFunctionOutputFluorescentTubeEffect;
                case ZIMO.ZIMOEffects.Unknown: return AppResources.LightEffectTypeFunctionOutputUnknown;

                default: return "Unknown ZIMO light effect";
            }
        }

        /// <summary>
        /// Returns the description for the given light effect direction.
        /// </summary>
        /// <param name="direction">A ZIMO light effection direction.</param>
        /// <returns></returns>
        internal static string GetLightEffectDirectionDescription(ZIMO.EffectDirection direction)
        {
            switch (direction)
            {
                case ZIMO.EffectDirection.DirectionIndependend: return AppResources.LightEffectDirectionTypeIndepend;
                case ZIMO.EffectDirection.Forward: return AppResources.LightEffectDirectionTypeForward;
                case ZIMO.EffectDirection.Backward: return AppResources.LightEffectDirectionTypeBackward;
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



        internal static ZIMO.SUSIPinModeType GetSUSIInterface1PinMode(string modeDescription)
        {
            try
            {
                if (modeDescription == AppResources.ZIMOSusiPinTypeLogicLevelOutput) return ZIMO.SUSIPinModeType.LogicLevelOutput;
                if (modeDescription == AppResources.ZIMOSusiPinTypeLogicLevelReedInput) return ZIMO.SUSIPinModeType.LogicLevelInput;
                if (modeDescription == AppResources.ZIMOSusiPinTypeServo) return ZIMO.SUSIPinModeType.ServoControlLine;
                if (modeDescription == AppResources.ZIMOSusiPinTypeSUSI) return ZIMO.SUSIPinModeType.SUSI;
                if (modeDescription == AppResources.ZIMOSusiPinTypeI2C) return ZIMO.SUSIPinModeType.I2C;
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
        internal static string GetSUSIInterface1PinModeDescription(ZIMO.SUSIPinModeType mode)
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
        internal static List<string> GetAvailableSUSIPinModes()
        {
            List<string> PinModes = new List<string>();
            foreach (ZIMOSUSIPinModeType item in _ZIMOSUSIPinModeTypes)
            {
                if (item.Description != null) PinModes.Add(item.Description);
            }
            return PinModes;
        }


        internal static List<string> GetAvailableMotorControlFrequencyTypes()
        {
            List<string> Names = new List<string>();
            foreach (ZIMOMotorControlFrequencyType item in _ZIMOMotorControlFrequencyTypes)
            {
                if (item.Description != null) Names.Add(item.Description);
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

        internal static ZIMO.MotorControlFrequencyTypes GetMotorControlFrequencyType(string description)
        {
            if (description == GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.HighFrequency)) return MotorControlFrequencyTypes.HighFrequency;
            return MotorControlFrequencyTypes.LowFrequency;
        }

        /// <summary>
        /// Returns the description of the selected function mapping in CV61.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetMappingTypeDescription(ZIMO.FunctionMappingTypes type)
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
