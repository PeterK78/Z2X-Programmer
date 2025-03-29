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

        //  Z21 CentralState states
        private const byte csEmergencyStop = 0x01;
        private const byte csTrackVoltageOff = 0x02;
        private const byte csShortCircuit  = 0x04;
        private const byte csProgrammingModeActive = 0x20;

        //  The UDP port (default according to the protocol is 21105)
        private const int _udpPort = 21105;

        #region REGION: PRIVATE FIELDS

        //  The IP address of the command station
        private IPAddress _ipAddress = default!;

        //  A flag to signalize the that the command station is reachable per ping
        private bool _reachable = false;

        //  A private UDP client object 
        private UdpClient _udpClient = new();

        // _pingClientTimer is used ping the Z21 command station in 5 seconds interval.
        private System.Timers.Timer _pingZ21Timer = new System.Timers.Timer() { AutoReset = true, Enabled = false, Interval = new TimeSpan(0, 0, 5).TotalMilliseconds, };

        //  According to the Z21 protocol specification we have to communicate at least each minute with the Z21. We use the timer _renewZ21SubscriptionTimer.
        private System.Timers.Timer _renewZ21SubscriptionTimer  = new System.Timers.Timer() { AutoReset = true, Enabled = false, Interval = new TimeSpan(0, 0, 50).TotalMilliseconds, };
        
        #endregion

        #region REGION: PUBLIC DELEGATES

        //  Will be called if a CV values has been programmed.
        public event EventHandler<ProgramEventArgs> OnProgramResultReceived = default!;

        //  Will be called if we receive locomotive info from the Z21.
        public event EventHandler<LocoInfoEventArgs> OnLocoInfoReceived = default!;

        //  Will be called if the status of the command station has been changed (e.g. track power, programming mode etc.).
        public event EventHandler<StateEventArgs> OnStatusChanged = default!;

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

        #region REGION: PRIVATE FUNCTIONS

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
                Logger.PrintDevConsole($"Z21Lib:ReceivingRawZ21Data  {BitConverter.ToString(received)}");

                ParseRawZ21Data(received);
            }
            catch (Exception ex)
            {
                 Logger.PrintDevConsole("Z21Lib:Error while receiving data (" + ex.Message + ")");
            }
        }

        #endregion

        #region REGION: PUBLIC FUNCTIONS




        /// <summary>
        /// Change the speed and direction of a locomotive.
        /// </summary>
        /// <param name="locomotiveAddress">The locomotive address.</param>
        /// <param name="speed">The speed setting according to the Z21 protocoll section "LAN_X_SET_LOCO_DRIVE".</param>
        /// <param name="realSpeedSteps">The real speed steps (according to the Z21).</param>
        /// <param name="direction">1 = Forward, 0 = Backward.</param>
        public void SetLocoDrive(ushort locomotiveAddress, int speed, byte realSpeedSteps, int direction)
        {
            Logger.PrintDevConsole("Z21Lib:SetLocoDrive (LAN_X_SET_LOCO_DRIVE) address:" + locomotiveAddress + " speed: " + speed + " realSpeedSteps: " + realSpeedSteps + " direction: " + direction);

            //  We must prevent the speedstep from being set to 1. This would trigger an emergency stop. 
            if (speed > 0) speed++;

            //  Create the DB0 settings and a limit the speed.           
            byte DB0 = 0;
            switch (realSpeedSteps)
            {
                case 14:
                    DB0 = 0x10;
                    if (speed > 15) speed = 15;
                    break;
                case 28:
                    DB0 = 0x12;
                    if (speed > 31) speed = 31;
                    break;
                case 128:
                    DB0 = 0x13;
                    if (speed > 127) speed = 127;
                    break;
                default:
                    Logger.PrintDevConsole("Z21Lib:SetLocoDrive (LAN_X_SET_LOCO_DRIVE) wrond speed steps = " + realSpeedSteps);
                    return;
            }

            byte DB3 = 0;
            if (direction == 1) DB3 = 128;
            DB3 = (byte)(DB3 + speed);                


            byte[] bytes = new byte[10];
            bytes[0] = 0x0A;
            bytes[1] = 0;
            bytes[2] = 0x40;
            bytes[3] = 0;
            bytes[4] = 0xE4;
            bytes[5] = DB0;
            bytes[6] = MSB(locomotiveAddress);
            bytes[7] = LSB(locomotiveAddress);
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
                case SwitchType.Off:            dB3 = Bit.Set(dB3, 7, false);
                                                dB3 = Bit.Set(dB3, 6, false);
                                                break;      
                case SwitchType.On:             dB3 = Bit.Set(dB3, 7, false);
                                                dB3 = Bit.Set(dB3, 6, true);
                                                break;
                case SwitchType.Toggle:         dB3 = Bit.Set(dB3, 7, true);
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
        /// Setting the broadcast flags in the Z21. These flags are set per client (i.e. per IP + port number)
        /// and must be set again at the next logon.
        ///
        /// Z21 commando: LAN_SET_BROADCASTFLAGS
        /// 
        /// </summary>
        public void ConfigureBroadCast()
        {
            Logger.PrintDevConsole("Z21Lib:ConfigureBroadCast (LAN_SET_BROADCASTFLAGS)");

            //  
            // We are setting only the first bit to 1 (0x00000001). So we are able to receive
            // the following broadcasts:
            //
            // * LAN_X_BC_TRACK_POWER_OFF
            // * LAN_X_BC_TRACK_POWER_ON
            // * LAN_X_BC_PROGRAMMING_MODE
            // * LAN_X_BC_TRACK_SHORT_CIRCUIT
            // * LAN_X_BC_STOPPED
            // * LAN_X_LOCO_INFO
            byte[] bytes = new byte[8];
            bytes[0] = 0x08;
            bytes[1] = 0;
            bytes[2] = 0x50;
            bytes[3] = 0;

            //  The flags have to be noted in "little endian" notation (the smallest part is the front)
            var flags = BitConverter.GetBytes(0x00000001);
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
                Logger.PrintDevConsole($"Z21Lib:Sending {BitConverter.ToString(bytes)}");
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
            Logger.PrintDevConsole($"Z21Lib:ParseRawZ21Data : {BitConverter.ToString(bytes)}");

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
            Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response data record {BitConverter.ToString(receivedBytes)}");

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
                                    OnProgramResultReceived?.Invoke(this, new ProgramEventArgs(new DCCConfigurationVariable(0,0), false));
                                    break;          

                                default:
                                    // UNKNOWN COMMAND        
                                    Logger.PrintDevConsole($"Z21Lib:EvaluateZ21Response - Unknown DB0 value");
                                    break;
                            }
                            break;

                        //  LAN_X_STATUS_CHANGED
                        case 0x62:
                            Logger. PrintDevConsole("Z21Lib:Evaluation - LAN_X_STATUS_CHANGED");
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

                            Logger.PrintDevConsole("Z21Lib:EvaluateZ21Response LAN_X_LOCO_INFO");

                            // Parsing DB0 and DB1.
                            ushort locomotiveAddress = (ushort)((receivedBytes[5] << 8) + receivedBytes[6]);

                            // Parsing DB2.
                            int speedSteps = 0;
                            switch (receivedBytes[7] & 0x7)
                            {
                                case 0: speedSteps = 14; break;
                                case 2: speedSteps = 28; break;     
                                case 4: speedSteps = 128; break;
                                default: speedSteps = 0; break;
                            }

                            // Parsing DB3.
                            int direction = Bit.IsSet(receivedBytes[8], 7) == true ? 1 : 0;
                            int speed = receivedBytes[8] & 0x127;                               

                            // Parsing DB4.
                            bool[] functionStates  = new bool[31];
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

                            OnLocoInfoReceived?.Invoke(this, new LocoInfoEventArgs(locomotiveAddress, functionStates, speedSteps, direction, speed));

                            break;
                    }

                    break;

                default:
                    Logger.PrintDevConsole($"Z21Lib:Unknown telegram " + receivedBytes.ToString());
                    break;
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
