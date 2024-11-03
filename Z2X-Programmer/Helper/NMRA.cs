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

namespace Z2XProgrammer.Helper
{
    /// <summary>
    /// This class contains definition of the NMRA (National Model Railroad Association) standard
    /// </summary>
    public static class NMRA
    {
        /// <summary>
        /// NMRA defined enumerations
        /// </summary>
        internal enum DCCProgrammingModes { DirectProgrammingTrack = 0, POMMainTrack = 1 };
        internal enum DCCAddressModes { Short = 0, Extended = 1 };
        internal enum DCCSpeedStepsModes { Steps14 = 0, Step28to128 = 1};
        internal enum DCCABCBreakModes { Off = 0, RightTrack = 1, LeftTrack=2};

        public const byte NumberOfFunctionKeys = 28;
        public const byte NumberOfExtendedSpeedSteps = 28;

        //  The standard locomotive address
        public const byte StandardLocomotiveAddress = 3;
        
        public const byte StandardCV2 = 1;
        public const byte StandardCV3 = 2;
        public const byte StandardCV4 = 1;
        public const byte StandardCV5 = 1;
        public const byte StandardCV6 = 1;

        //  The standardized manufacturer IDs
        public const byte ManufacurerID_Unknown = 0;
        public const byte ManufacturerID_Zimo = 145;
        public const byte ManufacturerID_Trix = 131;
        public const byte ManufacturerID_PIKO = 162;
        public const byte ManufacturerID_DoehlerAndHaass = 97;

        // The maximum amount of CV values.
        public const ushort MaxCVValues = 1024;

        // The maximum amount of function outputs.
        public const ushort MaxFunctionOutputs = 12;
        
        /// <summary>
        /// Converts the NMRA manufucaturer ID to a string
        /// </summary>
        /// <param name="value">Manufacturer ID</param>
        /// <returns>The manufacturer name</returns>
        public static string GetManufacturerName (byte value)
        {
            switch(value)
            {
                case ManufacturerID_Trix:               return "Trix Modelleisenbahn";
                case ManufacturerID_Zimo:               return "ZIMO Elektronik GmbH"; 
                case ManufacturerID_PIKO:               return "PIKO";
                case ManufacturerID_DoehlerAndHaass:    return "Doehler & Haass Steuerungssysteme GmbH & Co. KG";
                default:                                return  value.ToString() + " = Unknown manufacturer";
            }
        }


        /// <summary>
        /// This method calculates the acceleration and deceleration times in CV4 and CV3 according
        /// to the formula of RCN-225
        /// </summary>
        /// <param name="rate">The rate (value of CV3 or CV4)</param>
        /// <param name="mode">The DCC speed steps mode (14 or 28/128 mode)</param>
        /// <returns></returns>
        internal static double CalculateAccDecRateTimes(byte rate, DCCSpeedStepsModes mode)
        {
            //  Currently we use the ZIMO calculation formula. It does not divide the time
            //  by the currently configured speed steps.
            return ((double)rate * 0.896);


            //if (mode == NMRA.DCCSpeedStepsModes.Steps14)
            //{
            //    return ((double)rate * 0.896) / 14;
            //}
            //else
            //{
            //    return ((double)rate * 0.896) / 28;
            //}
        }

        /// <summary>
        /// Returns the long (or extended) consist address contained in CV19 and CV20
        /// </summary>
        /// <param name="cv19"></param>
        /// <param name="cv20"></param>
        /// <returns></returns>
        public static ushort GetExtendedConsistAddress (byte cv19, byte cv20)
        {
            byte tempCV19Value = Bit.Set(cv19, 7, false);
            return (ushort)(((int)cv20 * 100) + (int)tempCV19Value);
        }

        /// <summary>
        /// Returns the long (or extended) locomotive address contained in CV17 and CV18.
        /// </summary>
        /// <param name="cv17"></param>
        /// <param name="cv18"></param>
        /// <returns></returns>
        public static ushort GetExtendedDCCAddress (byte cv17, byte cv18)
        {
            ushort CV17 = cv17;
            ushort CV18 = cv18;
            int address = 0;


            if (CV17 >= 192 && CV17 <= 231 && CV18 >= 0 && CV18 <= 255)
            {
                address = ((cv17 - 192) << 8) + cv18;
                

            }
            else
            {
                address = 0;
            }
            return (ushort)address;

        }

        /// <summary>
        ///  Converts the given long (or extended) consist address to values of CV19 and CV20
        /// </summary>
        /// <param name="extendedConsistAddress"></param>
        /// <param name="cv19"></param>
        /// <param name="cv20"></param>
        /// <returns></returns>
        public static bool ConvertExtendedConsistAddressToCVValues (ushort extendedConsistAddress, bool directonReversed,    out byte cv19, out byte cv20)
        {
            int cv19temp = extendedConsistAddress % 100;
            cv19 = Bit.Set((byte)cv19temp, 7, directonReversed);
            cv20 = (byte)(extendedConsistAddress / 100);
            return true;
        }        


        /// <summary>
        /// Converts the given long or extended DCC address to values of CV17 and CV18
        /// </summary>
        /// <param name="extendedDCCAddress"></param>
        /// <param name="cv17"></param>
        /// <param name="cv18"></param>
        /// <returns></returns>
        public static bool ConvertExtendedDCCAddressToCVValues (uint extendedDCCAddress, out byte cv17, out byte cv18)
        {
            int CV17 = 0;
            int CV18 = 0;


            if (extendedDCCAddress > 0 && extendedDCCAddress <= 10239)
            {
                CV17 =((int)extendedDCCAddress >> 8) + 192;
                cv17 = (byte)CV17;
                CV18 = (int)extendedDCCAddress % 256;
                cv18 = (byte)CV18;
                return true;
                
            }
            else
            {
                cv17 = (byte)CV17;
                cv18 = (byte)CV18;   
                return false;
            }
        

        }

       



        //function calc_cv()
        //{
        //    var adresse = parseInt(document.cv.adresse.value);
        //    if (adresse > 0 && adresse <= 10239)
        //    {
        //        document.cv.cv17.value = (adresse >> 8) + 192;
        //        document.cv.cv18.value = adresse % 256;
        //    }
        //    else
        //    {
        //        document.cv.cv17.value = "Ungültige Adresse";
        //        document.cv.cv18.value = "Ungültige Adresse";
        //    }
        //}

        //function calc_adresse()
        //{
        //    var cv17 = parseInt(document.adresse.cv17.value);
        //    var cv18 = parseInt(document.adresse.cv18.value);
        //    if (cv17 >= 192 && cv17 <= 231 && cv18 >= 0 && cv18 <= 255)
        //    {
        //        var adresse = ((cv17 - 192) << 8) + cv18;
        //        document.adresse.adresse.value = adresse;
        //    }
        //    else
        //    {
        //        document.adresse.adresse.value = "Ungültige CVs";
        //    }
        //}

    }
}
