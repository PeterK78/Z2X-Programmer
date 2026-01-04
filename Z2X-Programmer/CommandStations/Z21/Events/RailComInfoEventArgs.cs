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

namespace Z21Lib.Events
{
    /// <summary>
    /// Event arguments RailComInfoEventArgs.
    /// </summary>
    public class RailComInfoEventArgs: EventArgs
    {
        public ushort LocomotiveAddress { get; set; } = 0;

        /// <summary>
        /// The current speed reported by RailCom. -1 means that the decoder does not support
        /// RailCom speed reporting.
        /// </summary>
        public short Speed { get; set; } = 0;

        /// <summary>
        /// The current signal quality reported by RailCom. -1 means that the decoder does not
        /// support RailCom signal quality reporting. 0 = Perfect Quality.
        /// </summary>
        public short QOS { get; set; } = 0;

        public ushort TransmitErrors { get; set; } = 0;    

        public RailComInfoEventArgs(ushort locomotiveAddress, short speed, short qos, ushort transmitErrors)
        {
            LocomotiveAddress = locomotiveAddress;
            Speed = speed;
            QOS = qos;
            TransmitErrors = transmitErrors;
        }

    }
}


