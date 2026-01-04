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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// Represents a digital command station.
    /// </summary>
    public class CommandStationType: INotifyPropertyChanged
    {
        //  Private field variables.
        private string _name = string.Empty;    
        private string _ipAddress = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("digitalsystem")]
        public int DigitalSystem { get; set; } = 0;
        
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        [XmlAttribute("ipaddress")]
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged();
            }
        }
    }
}
