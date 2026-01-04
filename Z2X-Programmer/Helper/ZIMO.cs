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

using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Helper
{
    /// <summary>
    /// This class contains all the constants and enums for the ZIMO decoder.
    /// </summary>
    internal static class ZIMO
    {

        internal enum MotorControlFrequencyTypes { HighFrequency = 0, LowFrequency = 1 };
        internal enum MotorControlPIDMotorTypes { Normal  = 0, BellAnchor = 1 };
        internal enum FunctionMappingTypes { RCN225 = 0, ExtendedMapping = 1};
        internal enum ZIMOEffects { Unknown = 252, NoEffect = 0, DimmingUpAndDown = 88, SinglePulseStrobe = 16, DoublePulseStrobe = 22, RotaryBeacon = 24, Decoupler=48, SoftStart = 52, FunctionOutputTurnsOffAtSpeed = 60, FluorescentTubeEffect = 92 };
        internal enum EffectDirection { DirectionIndependend = 0, Forward = 1, Backward = 2};
        internal enum FunctionEffects { Unknown = 252, NoEffect = 0, Decoupler = 48 };
        internal enum ZIMOEffectCategory { Unknown = 0, Light = 1, Function = 2 };


        // The differet modes of the SUSI function output pins. Defined in CV201 and CV202.
        internal enum SUSIPinModeType { Unknown = 0, LogicLevelOutput = 1, LogicLevelInput = 2, ServoControlLine = 3, SUSI = 4, I2C = 5 };

        internal static string GetSelfTestResult (byte value)
        {
            switch (value)
            {
                case 0: return AppResources.ZIMOErrorCode0;
                case 1: return AppResources.ZIMOErrorCode1; 
                case 2: return AppResources.ZIMOErrorCode2;
                case 3: return AppResources.ZIMOErrorCode3;
                case 4: return AppResources.ZIMOErrorCode4;
                case 5: return AppResources.ZIMOErrorCode5;
                case 6: return AppResources.ZIMOErrorCode6;
                case 17: return AppResources.ZIMOErrorCode17;
                case 36: return AppResources.ZIMOErrorCode36;
                case 49: return AppResources.ZIMOErrorCode49;
                case 50: return AppResources.ZIMOErrorCode50;
                case 51: return AppResources.ZIMOErrorCode51;
                default: return  value + " " + AppResources.ZIMOErrorCodeUnknown;
            }
        }

        /// <summary>
        /// Returns TRUE if the ZIMO bootloader version supports the "Fail-Safe" feature.
        /// According to the instructions of the Roco Maintenance Tool, ZIMO MS bootloaders only support a "Fail-Safe" mode from version 2.2.
        /// </summary>
        /// <param name="bootloaderVersion"></param>
        /// <param name="bootloaderSubversion"></param>
        /// <returns></returns>
        public static bool IsBootloaderVersionFailSafe(byte bootloaderVersion, byte bootloaderSubversion)
        {
            if (bootloaderVersion < 2) return false;
            if (bootloaderVersion > 2) return true;
            if (bootloaderSubversion < 2) return false;
            return true;
        }

    }
}
