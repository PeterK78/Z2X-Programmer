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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Z2XProgrammer.DataModel;

namespace Z2XProgrammer.Model
{
    /// <summary>
    /// The data model of the Z2X programmer file format (version 1.0)
    /// </summary>
    [XmlRoot("Z2XProgrammer")]
    public class Z2XProgrammerFileType
    {
        internal static string Z2X_FILEVERSION = "1.0";
        
        public Z2XProgrammerFileType()
        {
            UserDefinedNotes = String.Empty;
            UserDefindedDecoderDescription = String.Empty;
            DeqSpecName = String.Empty;
            UserDefinedImage = String.Empty;
        }

        /// <summary>
        /// The version number of the Z2X file format    
        /// </summary>
        [XmlElement(ElementName = "Z2XFileFormatVersion", Order = 1)]
        public string Z2XFileFormatVersion
        {
            get { return Z2X_FILEVERSION; }
            set { }
        }

        /// <summary>
        /// The main DCC address of the decoder
        /// </summary>
        [XmlElement(ElementName = "LocomotiveAddress", Order = 2)]
        public ushort LocomotiveAddress { get; set; }

        /// <summary>
        /// The name of the decoder specification
        /// </summary>
        [XmlElement(ElementName = "DeqSpecName", Order = 3)]
        public string DeqSpecName { get; set; }

        /// <summary>
        /// The user defined locomotive name
        /// </summary>
        [XmlElement(ElementName = "LocomotiveName", Order = 4)]
        public string UserDefindedDecoderDescription { get; set; }

        /// <summary>
        /// The user defined notes
        /// </summary>
        [XmlElement(ElementName = "Notes", Order = 5)]
        public string UserDefinedNotes { get; set; }

        /// <summary>
        /// The content of the configuration variables
        /// </summary>
        [XmlElement(ElementName = "CVs", Order = 6)]
        public  List<ConfigurationVariableType> CVs = new List<ConfigurationVariableType>();

        /// <summary>
        /// The user defined locomotive image
        /// </summary>
        [XmlElement(ElementName = "Image", Order = 7)]
        public string UserDefinedImage { get; set; }
    }
}
