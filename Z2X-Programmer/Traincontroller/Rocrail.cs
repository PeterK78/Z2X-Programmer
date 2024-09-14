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

using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.Traincontroller
{
    /// <summary>
    /// Contains the implementation of the Rocrail train controller software.
    /// </summary>
    internal static class Rocrail
    {
        /// <summary>
        /// Connects to the Rocrail server and calls the locomotive list data.
        /// </summary>
        /// <param name="ipAdress">The ip address of the Rocrail server.</param>
        /// <param name="port">The port number of the Rocrail server.</param>
        internal async static Task<List<LocoListType>> GetLocomotiveList(IPAddress ipAdress, int port)
        {
            try
            {
                List<LocoListType> LocoList = new List<LocoListType>();

                IPEndPoint ipEndPoint = new(ipAdress, port);

                using Socket _client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                await _client.ConnectAsync(ipEndPoint);

                // Send message <model cmd="lclist"/>
                string MessageX = "<model cmd=\"lclist\"/>";
                string Message = AddXMLHeader("model", MessageX);

                var messageBytes = Encoding.UTF8.GetBytes(Message);
                _ = await _client.SendAsync(messageBytes, SocketFlags.None);

                bool LcListFound = false;
                string RocrailMessage = "";
                string MessageBuffer = "";
                string CharactersReceived = "";

                while (LcListFound == false)
                {

                    byte[] RawByteBuffer = new byte[1024];
                    int BytesReceived = await _client.ReceiveAsync(RawByteBuffer, SocketFlags.None);
                    CharactersReceived = Encoding.UTF8.GetString(RawByteBuffer, 0, BytesReceived);
                    MessageBuffer += CharactersReceived;

                    while (BytesReceived >= RawByteBuffer.Length)
                    {
                        BytesReceived = await _client.ReceiveAsync(RawByteBuffer, SocketFlags.None);
                        CharactersReceived = Encoding.UTF8.GetString(RawByteBuffer, 0, BytesReceived);
                        MessageBuffer += CharactersReceived;
                    }

                    
                    while ((IsMessageAvailbelInBuffer(MessageBuffer) == true) && (LcListFound == false))
                    {
                        Logger.PrintDevConsole($"Rocrail: Trying to parse " + RocrailMessage);

                        RocrailMessage = GetNextMessageFromBuffer(MessageBuffer);
                        MessageBuffer = RemoveMessageFromBuffer(MessageBuffer);
                        
                        //  Check if we have got the lclist response message
                        if (RocrailMessage.Contains("<lclist>"))
                        {
                            XElement LcListElements = XElement.Parse(RocrailMessage, LoadOptions.PreserveWhitespace);
                            if (LcListElements.Name == "lclist")
                            {
                                LocoList = ProcessLclist(LcListElements);
                                LcListFound = true;
                            }
                        }
                    }

                }

                _client.Close();

                return LocoList;
            }
            catch (Exception ex) 
            {
                string msg = ex.Message;
                return null;
            }   
        }

        /// <summary>
        /// Process the Rocrail locomotive list.
        /// </summary>
        /// <param name="lclist">The lclist XML element of Rocrail.</param>
        internal static List<LocoListType> ProcessLclist(XElement lclist)
        {

            List<LocoListType> locoList = new List<LocoListType>();

            foreach (XElement element in lclist.Elements())
            {
                LocoListType entry = new LocoListType();
                if (element.Attribute("addr") != null) entry.LocomotiveAddress = ushort.Parse(element.Attribute("addr")!.Value);
                if(element.Attribute("id") != null) entry.UserDefindedDecoderDescription = element.Attribute("id")!.Value;

                locoList.Add(entry);
            }
            return locoList;

        }

        /// <summary>
        /// Adds the Rocrail XML header to the Rocrail XML message.
        /// </summary>
        /// <param name="xmlType">The Rocrail XML type.</param>
        /// <param name="xmlMsg">The Rocrail XML message.</param>
        /// <returns></returns>
        internal static string AddXMLHeader(string xmlType, string xmlMsg)
        {
            string buffer = string.Empty;
            buffer = "<xmlh><xml size=\"" + xmlMsg.Length + "\" name=\"" + xmlType + "\"/></xmlh>" + xmlMsg ;
            return buffer;
        }

        /// <summary>
        /// Returns the XML header of a Rocrail server message.
        /// </summary>
        /// <param name="message">A Rocrail server message.</param>
        internal static string GetXMLHeader( string message)
        {
            int Start = message.IndexOf("</xmlh>");
            message = message.Substring(0,Start + 7);
            return message;
        }

        /// <summary>
        /// Returns the size of the XML message - defined in the XML header.
        /// </summary>
        /// <param name="header">A string which contains the XML header.</param>
        /// <returns></returns>
        internal static int GetMessageSize (string header)
        {
            int Start = header.IndexOf("size=\"");
            int End = header.IndexOf("\"/>", Start + 1);
            string Size = header.Substring(Start +6, End - Start - 6);            
            return int.Parse(Size);
        }

        /// <summary>
        /// Checks if a valid Rocrail XML message is available in the message buffer.
        /// </summary>
        /// <param name="buffer">The Rocrail message buffer.</param>
        /// <returns></returns>
        internal static bool IsMessageAvailbelInBuffer( string buffer)
        {
            //  Check whether at least the required characters for the header are present.
            if (buffer.Length <= 70)
            {
                Logger.PrintDevConsole($"Rocrail: Buffer too small small for header.");
                return false;
            }

            //  Check if we have at least on XML header present in the buffer.
            if (buffer.IndexOf("</xmlh>") < 0)
            {
                Logger.PrintDevConsole($"Rocrail: XML header not present.");
                return false;
            }

            string XMLHeader = GetXMLHeader(buffer);
            int XMLMessageLength = GetMessageSize(buffer);

            //  Check whether the buffer contains at least the characters required for this message.
            if (buffer.Length < XMLMessageLength + XMLHeader.Length)
            {
                Logger.PrintDevConsole($"Rocrail: Buffer too smal for message.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the next Rocrail XML message from the buffer.
        /// </summary>
        /// <param name="buffer">The Rocrail message buffer.</param>
        /// <returns></returns>
        internal static string GetNextMessageFromBuffer(string buffer)
        {
            string XMLHeader = GetXMLHeader(buffer);
            buffer = buffer.Remove(0, XMLHeader.Length);

            int XMLLength = GetMessageSize(XMLHeader);

            //int HiddenCharaceters = CountHiddenCharacters(buffer);

            int len = buffer.Length;
            
            if( len < XMLLength)
            {
                // Längen problem
                int x = 0;
                x++;
    
            }
            string Message = buffer.Substring(0, XMLLength);

            int nullchar = Message.IndexOf('\0');
            Message = Message.Substring(0, nullchar - 1);

            return Message; 
        
        }

        /// <summary>
        /// Removes the first Rocrail XML message from the buffer.
        /// </summary>
        /// <param name="buffer">The Rocrail message buffer.</param>
        /// <returns></returns>
        internal static string RemoveMessageFromBuffer(string buffer)
        {
            string XMLHeader = GetXMLHeader(buffer);
            buffer = buffer.Remove(0, XMLHeader.Length);
            int XMLLength = GetMessageSize(XMLHeader);
            return buffer.Remove(0, XMLLength);
        }
    }
}
