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

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.Traincontroller
{
    /// <summary>
    /// Contains the implementation of the Rocrail train controller software. The Rocrail
    /// RCP protocol is used to communicate with the Rocrail server.
    /// 
    /// See the RCP documentation:
    /// https://wiki.rocrail.net/doku.php?id=develop:cs-protocol-en
    /// </summary>
    internal static class Rocrail
    {
        /// <summary>
        /// Connects to the Rocrail server and calls the locomotive list data by
        /// using the Rocrail Client Protocol (RCP).
        /// </summary>
        /// <param name="ipAdress">The ip address of the Rocrail server.</param>
        /// <param name="port">The port number of the Rocrail server.</param>
        internal async static Task<List<LocoListType>> GetLocomotiveList(IPAddress ipAdress, int port)
        {
            Socket _client = null!;

            try
            {
                //  Before we start communicating, we check whether the Rocrail server is available.
                //  The default timeout of the PingAsync method is 5 seconds.
                if (await PingAsync(ipAdress) == false) return [];

                //  We set up a TCPIP socket for communication with the Rocrail server.
                IPEndPoint ipEndPoint = new(ipAdress, port);
                _client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                await _client.ConnectAsync(ipEndPoint);

                // Now we create the RCP message and send it to the Rocrail server.
                // RCP uses the UTF-8 format, so we must ensure that our message is converted to this format before sending.
                _ = await _client.SendAsync(Encoding.UTF8.GetBytes(CreateRCPMessage("model", "<model cmd=\"lclist\"/>")), SocketFlags.None);

                // We are now waiting for the RCP LCLIST response. We simply wait until we receive the closing XML tag </lclist>.
                // To avoid an infinite loop, we use a 5 second timeout.
                byte[] RCPReceiveBuffer = new byte[1024];
                string MessageBuffer = "";

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (MessageBuffer.Contains("</lclist>") == false)
                {
                    Array.Clear(RCPReceiveBuffer);
                    int NumberOfBytesReceived = await _client.ReceiveAsync(RCPReceiveBuffer, SocketFlags.None);
                    MessageBuffer += Encoding.UTF8.GetString(RCPReceiveBuffer, 0, NumberOfBytesReceived);
                    if (sw.ElapsedMilliseconds > 5000) throw new TimeoutException();
                }
                _client.Close();

                //  Now we mask out the LCLIST response.
                string LCLISTMessage = MessageBuffer.Substring(MessageBuffer.IndexOf("<lclist>"), MessageBuffer.IndexOf("</lclist>") - MessageBuffer.IndexOf("<lclist>") + "</lclist>".Length);
                
                //  Finally, we parse the XML string and convert it to a LocoListType.
                return ConvertXElementToLocolist(XElement.Parse(LCLISTMessage, LoadOptions.PreserveWhitespace));
            }
            catch (Exception ex) 
            {
                if(_client != null) _client.Close();
                Logger.PrintDevConsole("Rocrail:GetLocomotiveList: " + ex.Message);
                return []; 
            }   
        }

        /// <summary>
        /// Converts an XElement object to a LocoListType list.
        /// </summary>
        /// <param name="lclist">The lclist XElement object.</param>
        internal static List<LocoListType> ConvertXElementToLocolist(XElement lclist)
        {
            try
            {
                List<LocoListType> locoList = [];

                foreach (XElement element in lclist.Elements())
                {
                    LocoListType entry = new LocoListType();

                    if (element.Attribute("addr") != null) entry.LocomotiveAddress = ushort.Parse(element.Attribute("addr")!.Value);
                    if (element.Attribute("id") != null) entry.UserDefindedDecoderDescription = element.Attribute("id")!.Value;

                    locoList.Add(entry);
                }
                return locoList;
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("Rocrail:ConvertXElementToLocolist: " + ex.Message);
                return [];
            }

        }

        /// <summary>
        /// Sends a ping to the specified IP address. If we receive a pong, the function returns true. 
        /// </summary>
        /// <returns>Returns true if the client is reachable. False if an error occurs. </returns>
        internal static async Task<bool> PingAsync(IPAddress ipAdress)
        {
            var ping = new System.Net.NetworkInformation.Ping();
            var result = await ping.SendPingAsync(ipAdress);
            return result.Status == System.Net.NetworkInformation.IPStatus.Success;
        }

        /// <summary>
        /// Creates a Rocrail RCP message.
        /// </summary>
        /// <param name="xmlType">The RCP XML type.</param>
        /// <param name="xmlMsg">The RCP XML message.</param>
        /// <returns></returns>
        internal static string CreateRCPMessage(string xmlType, string xmlMsg)
        {
            return "<xmlh><xml size=\"" + xmlMsg.Length + "\" name=\"" + xmlType + "\"/></xmlh>" + xmlMsg ;
        }
      
    }
}
