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

using System.Xml.Serialization;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// Represents a NMRA manufacturer entry.
    /// </summary>
    public class NMRAManufacturerType
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("extendedId")]
        public int? ExtendedId { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [XmlAttribute("country")]
        public string Country { get; set; } = string.Empty;

        [XmlAttribute("url")]
        public string Url { get; set; } = string.Empty;

        [XmlAttribute("decoderDBLink")]
        public string DecoderDBLink { get; set; } = string.Empty;

    }
}
