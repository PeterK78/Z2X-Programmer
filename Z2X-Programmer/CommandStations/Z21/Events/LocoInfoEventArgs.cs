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

namespace Z21Lib.Events
{
    /// <summary>
    /// Event arguments for LocoInfo event.
    /// </summary>
    public class LocoInfoEventArgs: EventArgs
    {
        public ushort Address { get; set; } = 0;
        public bool[] FunctionStates { get; set; } = new bool[31];
        public int SpeedSteps { get; set; } = 0;
        public int Direction { get; set; } = 0;
        public int Speed { get; set; } = 0;

        public LocoInfoEventArgs(ushort address, bool[] functionStates, int speedSteps, int direction, int speed)
        {
            Address = address;
            FunctionStates = functionStates;
            SpeedSteps = speedSteps;
            Direction = direction;
            Speed = speed;
        }

    }
}


