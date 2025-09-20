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

Credits to Jakob-Eichberger z21Client https://github.com/Jakob-Eichberger/z21Client

*/

using System.Net.Sockets;
using System.Net;
using System.Timers;
using Z21Lib.Enums;
using Z21Lib.Events;
using Z21Lib.Model;
using Z2XProgrammer.Helper;

namespace Z21Lib
{
    //  This class contains a client implementation of the Z21 protocol of Roco/Fleischmann.
    public sealed class Client
    {
        #region REGION: PRIVATE FIELDS

        //  Z21 CentralState states.
        private const byte csEmergencyStop = 0x01;
        private const byte csTrackVoltageOff = 0x02;
        private const byte csShortCircuit = 0x04;
        private const byte csProgrammingModeActive = 0x20;

        //  The UDP port (default according to the protocol is 21105).
        private const int _udpPort = 21105;

        //  The IP address of the command station.
        private IPAddress _ipAddress = default!;

        //  A flag to signalize the that the command station is reachable per ping.
        private bool _reachable = false;

        //  A private UDP client object .
        private UdpClient _udpClient = new();

        // _pingClientTimer is used ping the Z21 command station in 5 seconds interval.
        private System.Timers.Timer _pingZ21Timer = new System.Timers.Timer() { AutoReset = true, Enabled = false, Interval = new TimeSpan(0, 0, 5).TotalMilliseconds, };

        //  According to the Z21 protocol specification we have to communicate at least each minute with the Z21. We use the timer _renewZ21SubscriptionTimer.
        private System.Timers.Timer _renewZ21SubscriptionTimer = new System.Timers.Timer() { AutoReset = true, Enabled = false, Interval = new TimeSpan(0, 0, 50).TotalMilliseconds, };

        //  RM bus sensor states.
        private bool[] RMBusSensorStates = new bool[256];

        #endregion

        #region REGION: PUBLIC DELEGATES

        // Will be called if a CV values has been programmed.
        public event EventHandler<ProgramEventArgs> OnProgramResultReceived = default!;

        // Will be called if we receive locomotive info from the Z21.
        public event EventHandler<LocoInfoEventArgs> OnLocoInfoReceived = default!;

        // Will be called if the status of the command station has been changed (e.g. track power, programming mode etc.).
        public event EventHandler<StateEventArgs> OnStatusChanged = default!;

        // Will be called if we receive railcom data.
        public event EventHandler<RailComInfoEventArgs> OnRailComInfoReceived = default!;

        // Will be called if we receive rmbus data.
        public event EventHandler<RmBusInfoEventArgs> OnRmBusInfoReceived = default!;

        // Will be called if we receive the hardware information of the Z21.
        public event EventHandler<HardwareInformationEventArgs> OnHardwareInfoReceived = default!;

        /// <summary>
        /// OnReachabilityChanged is raised when the reachability to the Z21 has changed.    
        /// </summary>
        public event EventHandler<bool> OnReachabilityChanged = default!;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        /// <summary>
        /// The IP address of the Z21
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
            }
        }

        /// <summary>
        /// True if the Z21 command station is reachable via ICMP ping.
        /// </summary>
        /// <returns>Returns true if the Z21 is reachable.</returns>
        public bool IsReachable
        {
            get
            {
                return _reachable;
            }
            set
            {
                var clientReachabletemp = _reachable;
                _reachable = value;

                if (!clientReachabletemp && value)
                {
                    Logger.PrintDevConsole("ClientReachable - Z21 is reachable via ping");
                    ConfigureBroadCast();
                    GetOperatingMode();
                    OnReachabilityChanged?.Invoke(this, _reachable);
                    _renewZ21SubscriptionTimer.Enabled = true;
                }
                else if (clientReachabletemp && !value)
                {
                    Logger.PrintDevConsole("ClientReachable - Z21 is unreachable via ping");
                    OnReachabilityChanged?.Invoke(this, _reachable);
                    _renewZ21SubscriptionTimer.Enabled = false;
                }
            }
        }

        #endregion

        #region REGION: PUBLIC FUNCTIONS

        /// <summary>
        /// Change the speed and direction of a locomotive.
        /// </summary>
        /// <param name="vehicleAddress">The locomotive address.</param>
        /// <param name="speedStep">The speed setting according to the Z21 protocoll section "LAN_X_SET_LOCO_DRIVE".</param>
        /// <param name="maxNumberOfSpeedSteps">14, 28, 128 (depending on the set rail format in the Z21).</param>
        /// <param name="direction">1 = Forward, 0 = Backward.</param>
        public void SetLocoDrive(ushort vehicleAddress, int speedStep, byte maxNumberOfSpeedSteps, int direction)
        {
            Logger.PrintDevConsole("Z21Lib:SetLocoDrive (LAN_X_SET_LOCO_DRIVE) address:" + vehicleAddress + " new speedstep:" + speedStep + " maxSpeedStep:" + maxNumberOfSpeedSteps + " direction:" + direction);

            //  DB0: Configure the DCC speed step mode.           
            byte DB0 = 0;
            switch (maxNumberOfSpeedSteps)
            {
                case 14:
                    DB0 = 0x10;
                    break;
                case 28:
                    DB0 = 0x12;
                    break;
                case 128:
                    DB0 = 0x13;
                    break;
                default:
                    Logger.PrintDevConsole("Z21Lib:SetLocoDrive (LAN_X_SET_LOCO_DRIVE) wrond speed steps = " + maxNumberOfSpeedSteps);
                    return;
            }

            // DB3: Configure the speed step and direction.
            byte DB3 = 0;

            //  Set the direction bit.
            if (direction == 1) DB3 = 128;

            // We now add the speed step information to DB3. 
            switch (maxNumberOfSpeedSteps)
            {
                case 14:
                    DB3 = (byte)(DB3 + DCC14SpeedStep2SpeedCoding(speedStep));
                    break;
                case 28:
                    DB3 = (byte)(DB3 + DCC28SpeedStep2SpeedCoding(speedStep));
                    break;
                case 128:
                    DB3 = (byte)(DB3 + DCC128SpeedStep2SpeedCoding(speedStep));
                    break;
            }

            byte[] bytes = new byte[10];
            bytes[0] = 0x0A;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE4;
            bytes[5] = DB0;
            bytes[6] = MSB(vehicleAddress);
            bytes[7] = LSB(vehicleAddress);
            bytes[8] = DB3;
            bytes[9] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7] ^ bytes[8]);

            Sending(bytes);

        }

        /// <summary>
        /// The following command can be used to poll the status of a locomotive. At the same time,
        /// the client also "subscribes" to the locomotive information for this locomotive address.
        /// </summary>
        /// <param name="locomotiveAddress">The address of the locomotive.</param>
        public void GetLocoInfo(ushort locomotiveAddress)
        {
            Logger.PrintDevConsole("Z21Lib:GetLocoInfo (LAN_X_GET_LOCO_INFO) address:" + locomotiveAddress);

            byte[] bytes = new byte[9];
            bytes[0] = 0x09;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE3;
            bytes[5] = 0xF0;
            bytes[6] = MSB(locomotiveAddress);
            bytes[7] = LSB(locomotiveAddress);
            bytes[8] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7]);

            Sending(bytes);

        }

        /// <summary>
        /// Setting a locomotive function (e.g. F0, F1, etc.).
        /// 
        /// Z21 commando: LAN_X_SET_LOCO_FUNCTION
        /// 
        /// 
        /// </summary>
        /// <param name="locomotiveAddress">The address of the locomotive.</param>
        /// <param name="switchType">The switch type (Off, On, Toggle).</param>
        /// <param name="functionNumber">The number of the function (0, 1 .. 31).</param>
        public void SetLocoFunction(ushort locomotiveAddress, SwitchType switchType, byte functionNumber)
        {
            Logger.PrintDevConsole("Z21Lib:SetLocoFunction (LAN_X_SET_LOCO_FUNCTION) address:" + locomotiveAddress + " switchType:" + switchType + " function number:" + functionNumber);

            if (functionNumber > 31) return;

            //   Setup the DB3 byte according to the protocoll specification.
            byte dB3 = functionNumber;
            switch (switchType)
            {
                case SwitchType.Off:
                    dB3 = Bit.Set(dB3, 7, false);
                    dB3 = Bit.Set(dB3, 6, false);
                    break;
                case SwitchType.On:
                    dB3 = Bit.Set(dB3, 7, false);
                    dB3 = Bit.Set(dB3, 6, true);
                    break;
                case SwitchType.Toggle:
                    dB3 = Bit.Set(dB3, 7, true);
                    dB3 = Bit.Set(dB3, 6, false);
                    break;
            }

            byte[] bytes = new byte[10];
            bytes[0] = 0x0A;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE4;
            bytes[5] = 0xF8;
            bytes[6] = MSB(locomotiveAddress);
            bytes[7] = LSB(locomotiveAddress);
            bytes[8] = dB3;
            bytes[9] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7] ^ bytes[8]);

            Sending(bytes);

        }

        /// <summary>
        /// Logging off the client from the Z21.
        /// 
        /// Z21 commando: LAN_LOGOFF
        /// 
        /// </summary>
        public void LogOff()
        {
            Logger.PrintDevConsole("Z21Lib:LogOff (LAN_LOGOFF)");

            byte[] bytes = new byte[4];
            bytes[0] = 0x04;
            bytes[1] = 0;
            bytes[2] = 0x30;
            bytes[3] = 0;

            Sending(bytes);
        }

        /// <summary>
        /// The Z21 status can be requested with this command (LAN_X_GET_STATUS)
        /// 
        /// Z21 commando: LAN_X_GET_STATUS
        /// </summary>
        public void GetOperatingMode()
        {
            Logger.PrintDevConsole("Z21Lib:GetOperatingMode (LAN_X_GET_STATUS)");

            byte[] bytes = new byte[7];
            bytes[0] = 0x07;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0x21;
            bytes[5] = 0x24;
            bytes[6] = 0x05;

            Sending(bytes);
        }

        /// <summary>
        /// Requests the hardware information of the Z21.
        /// </summary>
        public void GetHardwareInformation()
        {
            Logger.PrintDevConsole("Z21Lib:GetHardwareInformation (LAN_GET_HWINFO)");

            byte[] bytes = new byte[4];
            bytes[0] = 0x04;
            bytes[1] = 0;
            bytes[2] = 0x1A;
            bytes[3] = 0;

            Sending(bytes);

        }

        /// <summary>
        /// Setting the broadcast flags in the Z21. These flags are set per client (i.e. per IP + port number)
        /// and must be set again at the next logon.
        ///
        /// Z21 commando: LAN_SET_BROADCASTFLAGS
        /// 
        /// </summary>
        public void ConfigureBroadCast()
        {
            Logger.PrintDevConsole("Z21Lib:ConfigureBroadCast (LAN_SET_BROADCASTFLAGS)");

            // We activate the following bits:
            //
            // Bit 0: We are able to receive the following broadcasts:
            //
            // * LAN_X_BC_TRACK_POWER_OFF
            // * LAN_X_BC_TRACK_POWER_ON
            // * LAN_X_BC_PROGRAMMING_MODE
            // * LAN_X_BC_TRACK_SHORT_CIRCUIT
            // * LAN_X_BC_STOPPED
            // * LAN_X_LOCO_INFO
            //
            // Bit 1: Enables LAN_RMBUS_DATACHANGED
            // Bit 2: Enables LAN_RAILCOM_DATACHANGED
            // 
            int broadCastFlags = 0b00000000000000000000000000000111;



            byte[] bytes = new byte[8];
            bytes[0] = 0x08;
            bytes[1] = 0;
            bytes[2] = 0x50;
            bytes[3] = 0;

            //  The flags have to be noted in "little endian" notation (the smallest part is the front)
            var flags = BitConverter.GetBytes(broadCastFlags);
            bytes[4] = flags[0];
            bytes[5] = flags[1];
            bytes[6] = flags[2];
            bytes[7] = flags[3];

            Sending(bytes);

        }

        /// <summary>
        /// Read a CV in direct mode from the progamming track
        ///
        /// Z21 commando: LAN_X_CV_READ
        /// 
        /// </summary>
        public bool ReadCVProgramTrack(ushort cvNumber)
        {
            Logger.PrintDevConsole("Z21Lib:ReadCVProgramTrack (LAN_X_CV_READ) from CV number " + cvNumber + " in direct mode.");

            cvNumber--;

            byte[] bytes = new byte[9];
            bytes[0] = 0x09;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0x23;
            bytes[5] = 0x11;
            bytes[6] = MSB(cvNumber);
            bytes[7] = LSB(cvNumber);
            bytes[8] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7]);

            Sending(bytes);

            return true;

        }

        /// <summary>
        /// Write a CV in direct mode
        /// </summary>
        /// <param name="cvNumber">The number of the configuration variable</param>
        /// <param name="value">The new value</param>
        public bool WriteCVProgramTrack(ushort cvNumber, byte value)
        {
            Logger.PrintDevConsole("Z21Lib:WriteCVProgramTrack (LAN_X_CV_WRITE) cv:" + cvNumber + " value:" + value);

            cvNumber--;

            byte[] bytes = new byte[10];
            bytes[0] = 0x0A;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0x24;
            bytes[5] = 0x12;
            bytes[6] = MSB(cvNumber);
            bytes[7] = LSB(cvNumber);
            bytes[8] = value;
            bytes[9] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7] ^ bytes[8]);

            Sending(bytes);

            return true;

        }

        /// <summary>
        /// Read a CV in POM mode from the main track
        ///
        /// Z21 commando: LAN_X_CV_READ
        /// 
        /// </summary>
        public bool ReadCVPOM(ushort cvNumber, ushort locomotiveAddress)
        {
            Logger.PrintDevConsole("Z21Lib:ReadCV in POM mode (LAN_X_CV_POM_READ_BYTE) locomotiveAddress:" + locomotiveAddress.ToString() + " cv:" + cvNumber.ToString());

            cvNumber--;

            byte[] bytes = new byte[12];
            bytes[0] = 0x0C;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE6;
            bytes[5] = 0x30;
            bytes[6] = MSB(locomotiveAddress);
            bytes[7] = LSB(locomotiveAddress);
            bytes[8] = (byte)(MSB(cvNumber) + (ushort)0xE4);
            bytes[9] = LSB(cvNumber);
            bytes[10] = 0;
            bytes[11] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7] ^ bytes[8] ^ bytes[9] ^ bytes[10]);

            Sending(bytes);

            return true;

        }

        /// <summary>
        /// This command is used to switch on the track voltage or to end the emergency stop or programming mode.
        /// is ended.
        ///
        /// Z21 commando: LAN_X_SET_TRACK_POWER_ON
        /// 
        /// </summary>
        public void SetTrackPowerOn()
        {

            Logger.PrintDevConsole("Z21Lib:SetTrackPowerOn (LAN_X_SET_TRACK_POWER_ON)");

            byte[] bytes = new byte[7];
            bytes[0] = 0x07;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0x21;
            bytes[5] = 0x81;
            bytes[6] = (byte)(bytes[4] ^ bytes[5]);

            Sending(bytes);

        }

        /// <summary>
        /// This command is used to switch track voltage off.
        ///
        /// Z21 commando: LAN_X_SET_TRACK_POWER_OFF
        /// 
        /// </summary>
        public void SetTrackPowerOff()
        {

            Logger.PrintDevConsole("Z21Lib:SetTrackPowerOff (LAN_X_SET_TRACK_POWER_OFF)");

            byte[] bytes = new byte[7];
            bytes[0] = 0x07;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0x21;
            bytes[5] = 0x80;
            bytes[6] = (byte)(bytes[4] ^ bytes[5]);

            Sending(bytes);

        }

        /// <summary>
        /// Connects to the Z21 command station
        /// </summary>
        /// <param name="Z21IpAddress">The ip address of the Z21 command station</param>
        /// <exception cref="NullReferenceException"></exception>
        public void Connect(IPAddress Z21IpAddress)
        {
            //  Check if the target ip address is not null                
            if (Z21IpAddress is null) throw new NullReferenceException($"Z21Lib:Z21IPAddress must not be null");

            //  Check if the UDP client has been closed before
            if (_udpClient.Client == null)
            {
                _udpClient = new();
            }

            if (OperatingSystem.IsWindows())
            {
                Logger.PrintDevConsole("Z21Lib:Z21 Client: Allowing NAT traversal");
                _udpClient.AllowNatTraversal(true);
            }
            IPAddress = Z21IpAddress;

            _udpClient.Connect(IPAddress, _udpPort);

            Logger.PrintDevConsole($"Z21Lib:UPD connection to {IPAddress}:{_udpPort} established.");

            //  Start receiving data from the UDP socket. Use the function ReceivingRawZ21Data
            //  for parsing the incoming data
            _udpClient.BeginReceive(new AsyncCallback(ReceivingRawZ21Data), null);

            //  According to the Z21 protocol specification, each client has to communicate at
            //  least each minute with the command station.
            //
            //  INFO: This timer will be enabled if IsReachable will be set to TRUE.
            //
            _renewZ21SubscriptionTimer.Elapsed += (a, b) => GetOperatingMode();

            //  Send a PING to the Z21 and check if it's reachable
            _ = Task.Run(async () => IsReachable = await PingAsync());

            //  To monitor the connection to the Z21 we will send all 5 seconds a ping to the Z21.
            _pingZ21Timer.Elapsed += Z21PingTimerEllapsed;
            _pingZ21Timer.Enabled = true;

        }

        /// <summary>
        /// Disconnects from the Z21 command station.
        /// </summary>
        public void Disconnect()
        {

            //  We stop the cyclic ping timer and remove the event handler.
            _pingZ21Timer.Enabled = false;
            _pingZ21Timer.Elapsed -= Z21PingTimerEllapsed;

            //  We stop the heart beat subscription timer and remove the event handler.
            _renewZ21SubscriptionTimer.Enabled = false;
            _renewZ21SubscriptionTimer.Elapsed -= (a, b) => GetOperatingMode();

            // We are properly logging out of the Z2X network.
            LogOff();

            // We are closing the UDP socket.
            _udpClient.Close();

            IsReachable = false;

        }

        /// <summary>
        /// Write a byte to a CV in POM mode.
        /// </summary>
        /// <param name="cvNumber">The number of the configuration variable</param>
        /// <param name="locomotiveAddress">The address of the locomotive</param>
        /// <param name="value">The new value</param>
        public bool WriteCVBytePOM(ushort cvNumber, ushort locomotiveAddress, byte value)
        {
            Logger.PrintDevConsole("Z21Lib:WriteCVBytePOM (LAN_X_CV_POM_WRITE_BYTE) locomotiveAddress:" + locomotiveAddress.ToString() + " cv nr.:" + cvNumber + " value:" + value);

            if (locomotiveAddress == 0) return false;

            cvNumber--;

            byte[] bytes = new byte[12];
            bytes[0] = 0x0C;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE6;
            bytes[5] = 0x30;
            bytes[6] = MSB(locomotiveAddress);
            bytes[7] = LSB(locomotiveAddress);
            bytes[8] = (byte)(MSB(cvNumber) + (ushort)0xEC);
            bytes[9] = LSB(cvNumber);
            bytes[10] = value;
            bytes[11] = (byte)(bytes[4] ^ bytes[5] ^ bytes[6] ^ bytes[7] ^ bytes[8] ^ bytes[9] ^ bytes[10]);

            Sending(bytes);

            return true;

        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// This function checks if the emergency stop is active.
        /// </summary>
        /// <param name="speedCoding">The curren speed value encoded according to Z21 protocoll.</param>
        /// <param name="maxSpeedSteps">The maximum speed steps supported by the decoder.</param>
        /// <returns></returns>
        private bool EStopActive(int speedCoding, int maxSpeedSteps)
        {
            switch (maxSpeedSteps)
            {
                case 14: if (speedCoding == 0x01) return true; break;
                case 28: if ((speedCoding == 0x01) || (speedCoding == 0x11)) return true; break;
                case 128: if (speedCoding == 0x01) return true; break;
                default: return false;
            }
            return false;
        }


        /// <summary>
        /// Convert the given DCC128 speed coding to the DCC128 speed step (0 - 128).
        /// </summary>
        /// <param name="speedCoding">The encoded DCC128 speed step.</param>
        /// <returns></returns>
        private byte DCC128SpeedCoding2SpeedStep(int speedCoding)
        {
            byte speedStep = 0;
            if ((speedCoding == 0) || (speedCoding == 1))
            {
                speedStep = 0;
            }
            else
            {
                speedStep = (byte)(speedCoding - 1);
            }
            return speedStep;
        }

        /// <summary>
        /// This function converts the DCC128 speed step (0 -128) to the Z21 speed coding. The conversion
        /// is according to 'Fahrstufen-Codierung bei „DCC 128“' from the Z21 protocol specification.
        /// </summary>
        /// <param name="speedStep">The DCC128 speed step (0-128)</param>
        /// <returns></returns>
        private byte DCC128SpeedStep2SpeedCoding(int speedStep)
        {
            if (speedStep == 0) return 0;
            return (byte)(speedStep + 1);
        }


        /// <summary>
        /// Convert the given DCC14 speed coding to the DCC14 speed step (0 - 14).
        /// </summary>
        /// <param name="speedCoding">The encoded DCC28 speed step.</param>
        /// <returns></returns>
        private byte DCC14SpeedCoding2SpeedStep(byte speedCoding)
        {
            switch (speedCoding)
            {

                case 0x00: return 0;
                case 0x02: return 1;
                case 0x03: return 2;
                case 0x04: return 3;
                case 0x05: return 4;
                case 0x06: return 5;
                case 0x07: return 6;
                case 0x08: return 7;
                case 0x09: return 8;
                case 0x0A: return 9;
                case 0x0B: return 10;
                case 0x0C: return 11;
                case 0x0D: return 12;
                case 0x0E: return 13;
                case 0x0F: return 14;
                default: return 0x00;
            }

        }

        /// <summary>
        /// This function converts the DCC14 speed step (0 - 14) to the Z21 speed coding. The conversion
        /// is according to 'Fahrstufen-Codierung bei „DCC 14“' from the Z21 protocol specification.
        /// </summary>
        /// <param name="speedStep">The DCC14 speed step (0-14)</param>
        /// <returns></returns>
        private byte DCC14SpeedStep2SpeedCoding(int speedStep)
        {
            switch (speedStep)
            {

                case 0: return 0x00;
                case 1: return 0x02;
                case 2: return 0x03;
                case 3: return 0x04;
                case 4: return 0x05;
                case 5: return 0x06;
                case 6: return 0x07;
                case 7: return 0x08;
                case 8: return 0x09;
                case 9: return 0x0A;
                case 10: return 0x0B;
                case 11: return 0x0C;
                case 12: return 0x0D;
                case 13: return 0x0E;
                case 14: return 0x0F;
                default: return 0x00;
            }

        }

        /// <summary>
        /// Convert the given DCC28 speed coding to the DCC28 speed step (0 - 28).
        /// </summary>
        /// <param name="speedCoding">The encoded DCC28 speed step.</param>
        /// <returns></returns>
        private byte DCC28SpeedCoding2SpeedStep(byte speedCoding)
        {
            switch (speedCoding)
            {
                case 0x00: return 0;
                case 0x01: return 0;
                case 0x02: return 1;
                case 0x12: return 2;
                case 0x03: return 3;
                case 0x13: return 4;
                case 0x04: return 5;
                case 0x14: return 6;
                case 0x05: return 7;
                case 0x15: return 8;
                case 0x06: return 9;
                case 0x16: return 10;
                case 0x07: return 11;
                case 0x17: return 12;
                case 0x08: return 13;
                case 0x18: return 14;
                case 0x09: return 15;
                case 0x19: return 16;
                case 0x0A: return 17;
                case 0x1A: return 18;
                case 0x0B: return 19;
                case 0x1B: return 20;
                case 0x0C: return 21;
                case 0x1C: return 22;
                case 0x0D: return 23;
                case 0x1D: return 24;
                case 0x0E: return 25;
                case 0x1E: return 26;
                case 0x0F: return 27;
                case 0x1F: return 28;
                default: return 0x00;
            }
        }


        /// <summary>
        /// This function converts the DCC28 speed step (0 - 28) to the Z21 speed coding. The conversion
        /// is according to 'Fahrstufen-Codierung bei „DCC 28“' from the Z21 protocol specification.
        /// </summary>
        /// <param name="speedStep">The DCC28 speed step (0-28)</param>
        /// <returns></returns>
        private byte DCC28SpeedStep2SpeedCoding(int speedStep)
        {
            switch (speedStep)
            {

                case 0: return 0x00;
                case 1: return 0x02;
                case 2: return 0x12;
                case 3: return 0x03;
                case 4: return 0x13;
                case 5: return 0x04;
                case 6: return 0x14;
                case 7: return 0x05;
                case 8: return 0x15;
                case 9: return 0x06;
                case 10: return 0x16;
                case 11: return 0x07;
                case 12: return 0x17;
                case 13: return 0x08;
                case 14: return 0x18;
                case 15: return 0x09;
                case 16: return 0x19;
                case 17: return 0x0A;
                case 18: return 0x1A;
                case 19: return 0x0B;
                case 20: return 0x1B;
                case 21: return 0x0C;
                case 22: return 0x1C;
                case 23: return 0x0D;
                case 24: return 0x1D;
                case 25: return 0x0E;
                case 26: return 0x1E;
                case 27: return 0x0F;
                case 28: return 0x1F;
                default: return 0x00;
            }

        }


        /// <summary>
        /// Receives the raw data bytes from the UDP socket.
        /// </summary>
        /// <param name="res"></param>
        private void ReceivingRawZ21Data(IAsyncResult res)
        {
            try
            {
                IPEndPoint RemoteIpEndPoint = null!;
                byte[] received = _udpClient.EndReceive(res, ref RemoteIpEndPoint!);
                _udpClient.BeginReceive(new AsyncCallback(ReceivingRawZ21Data), null);

                //OnReceive?.Invoke(this, new DataEventArgs(received));
                //Logger.PrintDevConsole($"Z21Lib:ReceivingRawZ21Data  {BitConverter.ToString(received)}");

                ParseRawZ21Data(received);
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("Z21Lib:Error while receiving data (" + ex.Message + ")");
            }
        }

        /// <summary>
        /// Sends a ping to the Z21. If we receive a pong, the function returns true. 
        /// </summary>
        /// <returns>Returns true if the client is reachable. False if an error occurs. </returns>
        private async Task<bool> PingAsync()
        {
            var ping = new System.Net.NetworkInformation.Ping();
            var result = await ping.SendPingAsync(IPAddress);

            bool ReturnValue = result.Status == System.Net.NetworkInformation.IPStatus.Success;

            //Logger.PrintDevConsole($"Z21Lib:PingAsync to " + IPAddress.ToString() + " was successfull = " + ReturnValue.ToString());

            return ReturnValue;

        }

        /// <summary>
        /// Returns the MSB of a cv number
        /// </summary>
        /// <param name="cvNumber"></param>
        /// <returns></returns>
        private byte MSB(ushort cvNumber)
        {
            return Convert.ToByte(cvNumber >> 8);
        }

        /// <summary>
        /// Returns the LSB of a cv number
        /// </summary>
        /// <param name="cvNumber"></param>
        /// <returns></returns>
        private byte LSB(ushort cvNumber)
        {
            //  We set the highest 8 bits to 0 and convert to byte
            return Convert.ToByte(cvNumber &= 0xFF);
        }


        /// <summary>
        /// Sends the given bytes to the UDP client.
        /// </summary>
        /// <param name="bytes"></param>
        private async void Sending(byte[] bytes)
        {
            try
            {
                //Logger.PrintDevConsole($"Z21Lib:Sending {BitConverter.ToString(bytes)}");
                await _udpClient.SendAsync(bytes, bytes?.GetLength(0) ?? 0);
            }
            catch
            {
                // Somtimes the UDP client is already disposed
            }
        }

        /// <summary>
        /// PingClient_Elapsed is a private function which is called when the cyclic ping timer is raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Z21PingTimerEllapsed(object? sender, ElapsedEventArgs e)
        {
            //Logger.PrintDevConsole("Z21Lib:Z21PingTimerEllapsed");

            _pingZ21Timer.Enabled = false;
            try
            {

                IsReachable = await PingAsync();
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("Z21Lib:Error while pinging client (" + ex.Message + ")");
            }
            finally
            {
                _pingZ21Timer.Enabled = true;
            }
        }

        /// <summary>
        /// This functions parses the received raw data and converts it to a Z21 data record
        /// </summary>
        /// <param name="bytes"></param>
        private void ParseRawZ21Data(byte[] bytes)
        {
            //Logger.PrintDevConsole($"Z21Lib:ParseRawZ21Data : {BitConverter.ToString(bytes)}");

            if (bytes == null) return;
            int z = 0;
            int max = bytes.GetLength(0);
            while (z < max)
            {
                int length = bytes[z];
                if (length > 3 & z + length <= max)
                {
                    byte[] einzelbytes = new byte[length];
                    Array.Copy(bytes, z, einzelbytes, 0, length);
                    EvaluateZ21DataRecord(einzelbytes);
                    z += length;
                }
                else
                {
                    z = max;
                    Logger.PrintDevConsole($"Z21Lib:Bad telegram " + bytes.ToString());
                }
            }
        }

        /// <summary>
        /// Parses the data record bytes from the Z21 command station according to the Z21 LAN protocol.
        /// </summary>
        /// <param name="receivedBytes"></param>
        private void EvaluateZ21DataRecord(byte[] receivedBytes)
        {
            //Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response data record {BitConverter.ToString(receivedBytes)}");

            //  Check the Header
            switch (receivedBytes[2])
            {

                case 0x40:

                    //  Check the X-Header
                    switch (receivedBytes[4])
                    {


                        //  LAN_X_UNKNOWN_COMMAND
                        //  LAN_X_BC_TRACK_SHORT_CIRCUIT
                        //  LAN_X_BC_PROGRAMMING_MODE
                        //  LAN_X_BC_TRACK_POWER_ON
                        //  LAN_X_BC_TRACK_POWER_OFF
                        case 0x61:

                            // Check DB 0
                            switch (receivedBytes[5])
                            {
                                case 0x00:
                                    // LAN_X_BC_TRACK_POWER_OFF
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_TRACK_POWER_OFF");
                                    OnStatusChanged.Invoke(this, new StateEventArgs(TrackPower.OFF));
                                    break;
                                case 0x01:
                                    // LAN_X_BC_TRACK_POWER_ON
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_TRACK_POWER_ON");
                                    OnStatusChanged.Invoke(this, new StateEventArgs(TrackPower.ON));
                                    break;

                                case 0x02:
                                    // LAN_X_BC_PROGRAMMING_MODE
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_PROGRAMMING_MODE");
                                    OnStatusChanged.Invoke(this, new StateEventArgs(TrackPower.Programing));
                                    break;

                                case 0x08:
                                    // LAN_X_BC_TRACK_SHORT_CIRCUIT
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_TRACK_SHORT_CIRCUIT");
                                    OnStatusChanged.Invoke(this, new StateEventArgs(TrackPower.Short));
                                    break;

                                case 0x12:
                                    // LAN_X_CV_NACK_SC
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_TRACK_SHORT_CIRCUIT");
                                    OnProgramResultReceived?.Invoke(this, new ProgramEventArgs(new DCCConfigurationVariable(0, 0), false));
                                    break;


                                case 0x13:
                                    // LAN_X_CV_NACK
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_CV_NACK");
                                    OnProgramResultReceived?.Invoke(this, new ProgramEventArgs(new DCCConfigurationVariable(0, 0), false));
                                    break;

                                default:
                                    // UNKNOWN COMMAND        
                                    Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response - Unknown DB0 value");
                                    break;
                            }
                            break;

                        //  LAN_X_STATUS_CHANGED
                        case 0x62:
                            Logger.PrintDevConsole("Z21Lib:Evaluation - LAN_X_STATUS_CHANGED");
                            OnStatusChanged?.Invoke(this, new StateEventArgs(GetCentralStateData(receivedBytes)));
                            break;

                        //  LAN_X_CV_RESULT
                        case 0x64:

                            //Check DB 0
                            switch (receivedBytes[5])
                            {
                                // LAN_X_CV_RESULT
                                case 0x14:
                                    Logger.PrintDevConsole($"Z21Lib:Evaluation - LAN_X_CV_RESULT (value={receivedBytes[8].ToString()})");
                                    OnProgramResultReceived?.Invoke(this, new ProgramEventArgs(new DCCConfigurationVariable(ConvertAdress(receivedBytes[6], receivedBytes[7]), receivedBytes[8]), true));
                                    break;

                            }
                            break;

                        // LAN_X_BC_STOPPED
                        case 0x81:

                            //  Check DB 0
                            switch (receivedBytes[5])
                            {
                                // LAN_X_BC_STOPPED
                                case 0x00:
                                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_BC_STOPPED");
                                    OnStatusChanged.Invoke(this, new StateEventArgs(TrackPower.OFF));
                                    break;
                            }
                            break;

                        //  LAN_X_LOCO_INFO
                        case 0xEF:

                            byte DB3 = receivedBytes[8];

                            // Parsing DB0 and DB1 (Byte 5 + 6)
                            ushort locomotiveAddress = (ushort)((receivedBytes[5] << 8) + receivedBytes[6]);

                            // Parsing DB2 (Byte 7)
                            int maxSpeedSteps = 0;
                            switch (receivedBytes[7] & 0x7)
                            {
                                case 0: maxSpeedSteps = 14; break;
                                case 2: maxSpeedSteps = 28; break;
                                case 4: maxSpeedSteps = 128; break;
                                default: maxSpeedSteps = 0; break;
                            }

                            // Parsing DB3 (Byte 8) - Direction. 
                            int direction = Bit.IsSet(receivedBytes[8], 7) == true ? 1 : 0;


                            // Parsing DB3 (Byte 8) - Emergency stop active.
                            bool eStopActive = EStopActive((byte)(DB3 & 0x7F), maxSpeedSteps);

                            // Parsing DB3 (Byte 8) - Speed step information. 
                            byte currentSpeedStep = 0;
                            if (eStopActive == false)
                            {
                                switch (maxSpeedSteps)
                                {
                                    case 14:
                                        currentSpeedStep = (byte)DCC14SpeedCoding2SpeedStep((byte)(DB3 & 0x7F));
                                        break;
                                    case 28:
                                        currentSpeedStep = (byte)DCC28SpeedCoding2SpeedStep((byte)(DB3 & 0x7F));
                                        break;
                                    case 128:
                                        currentSpeedStep = (byte)DCC128SpeedCoding2SpeedStep((DB3 & 0x7F));
                                        break;
                                }
                            }

                            // Parsing DB4.
                            bool[] functionStates = new bool[31];
                            functionStates[0] = (Bit.IsSet(receivedBytes[9], 4));
                            functionStates[4] = (Bit.IsSet(receivedBytes[9], 3));
                            functionStates[3] = (Bit.IsSet(receivedBytes[9], 2));
                            functionStates[2] = (Bit.IsSet(receivedBytes[9], 1));
                            functionStates[1] = (Bit.IsSet(receivedBytes[9], 0));

                            //  Parsing DB5.
                            functionStates[5] = (Bit.IsSet(receivedBytes[10], 0));
                            functionStates[6] = (Bit.IsSet(receivedBytes[10], 1));
                            functionStates[7] = (Bit.IsSet(receivedBytes[10], 2));
                            functionStates[8] = (Bit.IsSet(receivedBytes[10], 3));
                            functionStates[9] = (Bit.IsSet(receivedBytes[10], 4));
                            functionStates[10] = (Bit.IsSet(receivedBytes[10], 5));
                            functionStates[11] = (Bit.IsSet(receivedBytes[10], 6));
                            functionStates[12] = (Bit.IsSet(receivedBytes[10], 7));

                            //  Parsing DB6.
                            functionStates[13] = (Bit.IsSet(receivedBytes[11], 0));
                            functionStates[14] = (Bit.IsSet(receivedBytes[11], 1));
                            functionStates[15] = (Bit.IsSet(receivedBytes[11], 2));
                            functionStates[16] = (Bit.IsSet(receivedBytes[11], 3));
                            functionStates[17] = (Bit.IsSet(receivedBytes[11], 4));
                            functionStates[18] = (Bit.IsSet(receivedBytes[11], 5));
                            functionStates[19] = (Bit.IsSet(receivedBytes[11], 6));
                            functionStates[20] = (Bit.IsSet(receivedBytes[11], 7));

                            //  Parsing DB7.
                            functionStates[21] = (Bit.IsSet(receivedBytes[12], 0));
                            functionStates[22] = (Bit.IsSet(receivedBytes[12], 1));
                            functionStates[23] = (Bit.IsSet(receivedBytes[12], 2));
                            functionStates[24] = (Bit.IsSet(receivedBytes[12], 3));
                            functionStates[25] = (Bit.IsSet(receivedBytes[12], 4));
                            functionStates[26] = (Bit.IsSet(receivedBytes[12], 5));
                            functionStates[27] = (Bit.IsSet(receivedBytes[12], 6));
                            functionStates[28] = (Bit.IsSet(receivedBytes[12], 7));

                            //  Parsing DB8.
                            functionStates[21] = (Bit.IsSet(receivedBytes[12], 0));
                            functionStates[22] = (Bit.IsSet(receivedBytes[12], 1));
                            functionStates[23] = (Bit.IsSet(receivedBytes[12], 2));
                            functionStates[24] = (Bit.IsSet(receivedBytes[12], 3));
                            functionStates[25] = (Bit.IsSet(receivedBytes[12], 4));
                            functionStates[26] = (Bit.IsSet(receivedBytes[12], 5));
                            functionStates[27] = (Bit.IsSet(receivedBytes[12], 6));
                            functionStates[28] = (Bit.IsSet(receivedBytes[12], 7));

                            OnLocoInfoReceived?.Invoke(this, new LocoInfoEventArgs(locomotiveAddress, functionStates, maxSpeedSteps, direction, currentSpeedStep, (currentSpeedStep == 0) ? true : false, eStopActive));

                            //Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response (LAN_X_LOCO_INFO) locoAddress:" + locomotiveAddress + " DB3:" + DB3 + " speedSteps:" + maxSpeedSteps + " currentSpeedStep:" + currentSpeedStep + " direction:" + direction);

                            break;
                    }

                    break;

                case 0x80:

                    //  LAN_RMBUS_DATACHANGED
                    byte groupIndex = receivedBytes[4];
                    byte feedbackStatus0 = receivedBytes[5];
                    byte feedbackStatus1 = receivedBytes[6];
                    byte feedbackStatus2 = receivedBytes[7];
                    byte feedbackStatus3 = receivedBytes[8];
                    byte feedbackStatus4 = receivedBytes[9];
                    byte feedbackStatus5 = receivedBytes[10];
                    byte feedbackStatus6 = receivedBytes[11];
                    byte feedbackStatus7 = receivedBytes[12];
                    byte feedbackStatus8 = receivedBytes[13];
                    byte feedbackStatus9 = receivedBytes[14];

                    for (int i = 5; i <= 14; i++) ParseRMBusData(groupIndex, receivedBytes[i], i - 4);

                    break;

                case 0x88:

                    // LAN_RAILCOM_DATACHANGED
                    ushort locoAddress = (ushort)((receivedBytes[5] << 8) + receivedBytes[4]);
                    byte railComOption = receivedBytes[13];
                    byte railComSpeed = receivedBytes[14];
                    byte railComQOS = receivedBytes[15];

                    OnRailComInfoReceived?.Invoke(this, new RailComInfoEventArgs(locoAddress, railComSpeed, railComQOS));

                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response (LAN_RAILCOM_DATACHANGED) locoAddress:" + locoAddress + " railComOption=" + railComOption.ToString() + " railComSpeed=" + railComSpeed.ToString() + " railComQOS=" + railComQOS.ToString());

                    break;


                case 0x1A:

                    //  LAN_GET_HWINFO
                    uint majorVersion = receivedBytes[9];
                    uint minorVersion = uint.Parse(receivedBytes[8].ToString("X"));
                    uint hardwareType = Convert.ToUInt32((receivedBytes[5] * 100 + receivedBytes[4]).ToString(), 16);

                    OnHardwareInfoReceived?.Invoke(this, new HardwareInformationEventArgs(majorVersion, minorVersion, hardwareType));

                    Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response (LAN_GET_HWINFO) majorVersion=" + majorVersion + " minorVersion=" + minorVersion + " hardwareType=" + hardwareType);

                    break;

                default:
                    Logger.PrintDevConsole($"Z21Lib:Unknown telegram " + receivedBytes.ToString());
                    break;
            }
        }

        /// <summary>
        /// Parses the RMBus feedback status bytes and raises an event for each changed feedback address.
        /// </summary>
        /// <param name="groupIndex">The group index byte according to the Z21 lan protocoll.</param>
        /// <param name="feedbackStatus">The content of a feedback status according to the Z21 lan protocoll.</param>
        /// <param name="feedbackStatusIndex">The number of the feedback status byte (1-10).</param>
        private void ParseRMBusData(byte groupIndex, byte feedbackStatus, int feedbackStatusIndex)
        {
            //  Loop through all 8 bits of the feedback status byte.
            for (int i = 0; i < 8; i++)
            {
                // Calculate the feedback input address.
                int feedbackAddress = (groupIndex * 80) + (i + 1) + (8 * (feedbackStatusIndex - 1));

                // Check is a feedback input bit set?
                if ((Bit.IsSet(feedbackStatus, i) == true) && (RMBusSensorStates[feedbackAddress] == false))
                {
                    RMBusSensorStates[feedbackAddress] = true;
                    OnRmBusInfoReceived?.Invoke(this, new RmBusInfoEventArgs(feedbackAddress, true));
                    Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response (LAN_RMBUS_DATACHANGED) feedbackAddress=" + feedbackAddress.ToString() + " state=TRUE " + "groupIndex=" + groupIndex.ToString() + " feedbackStatus=" + feedbackStatus.ToString() + " feedbackStatusIndex=" + feedbackStatusIndex.ToString());
                }
                else if ((Bit.IsSet(feedbackStatus, i) == false) && (RMBusSensorStates[feedbackAddress] == true))
                {
                    RMBusSensorStates[feedbackAddress] = false;
                    OnRmBusInfoReceived?.Invoke(this, new RmBusInfoEventArgs(feedbackAddress, false));
                    Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response (LAN_RMBUS_DATACHANGED) feedbackAddress=" + feedbackAddress.ToString() + " state=FALSE " + " groupIndex=" + groupIndex.ToString() + " feedbackStatus=" + feedbackStatus.ToString() + " feedbackStatusIndex=" + feedbackStatusIndex.ToString());
                }

            }

        }

        /// <summary>
        /// Converts the CVAdr_MSB and CVAdr_LSB of the Z2X protocol to a valid configuration variable number.
        /// </summary>
        /// <param name="cvAdr_MSB">CVAdr_MSB</param>
        /// <param name="cvAdr_LSB">CVAdr_LSB</param>
        /// <returns></returns>
        private ushort ConvertAdress(byte cvAdr_MSB, byte cvAdr_LSB)
        {
            int MSB = (int)cvAdr_MSB << 8;
            return (ushort)(MSB + (ushort)+cvAdr_LSB + 1);
        }


        /// <summary>
        /// Converts the content of LAN_X_STATUS_CHANGED to a TrackPower structure.
        /// </summary>
        /// <param name="received"></param>
        /// <returns></returns>
        private TrackPower GetCentralStateData(byte[] received)
        {
            Logger.PrintDevConsole($"Z21Lib:GetCentralStateData - data received: {BitConverter.ToString(received)}");

            TrackPower state = TrackPower.ON;

            bool isEmergencyStop = (received[6] & csEmergencyStop) == csEmergencyStop;
            bool isTrackVoltageOff = (received[6] & csTrackVoltageOff) == csTrackVoltageOff;
            bool isShortCircuit = (received[6] & csShortCircuit) == csShortCircuit;
            bool isProgrammingModeActive = (received[6] & csProgrammingModeActive) == csProgrammingModeActive;

            if (isEmergencyStop || isTrackVoltageOff)
            {
                state = TrackPower.OFF;
                Logger.PrintDevConsole("Z21Lib:GetCentralStateData - Track power = OFF");
            }
            else if (isShortCircuit)
            {
                state = TrackPower.Short;
                Logger.PrintDevConsole("Z21Lib:GetCentralStateData - Track power = Short");
            }
            else if (isProgrammingModeActive)
            {
                state = TrackPower.Programing;
                Logger.PrintDevConsole("Z21Lib:GetCentralStateData - Track power = Programming");
            }
            return state;

        }

        #endregion
    }

}
