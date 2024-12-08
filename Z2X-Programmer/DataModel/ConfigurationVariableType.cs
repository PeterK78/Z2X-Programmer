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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// Contains the definition of single NMRA configuration variable (short CV)
    /// </summary>
    public class ConfigurationVariableType: INotifyPropertyChanged
    {
        private bool _Enabled = true;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)    
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The number of the CV
        /// </summary>
        /// 
        public int Number { get; set; }

        /// <summary>
        /// The value of the CV
        /// </summary>
        public byte Value {  get; set; }
        
        /// <summary>
        /// Enable or disables the CV
        /// </summary>
        public bool Enabled
        {
            get => _Enabled;
            set
            {
                _Enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }

        /// <summary>
        /// A short description of th CV
        /// </summary>
        public string Description { get; set; }


        public bool DeqSecSupported { get; set; }


        /// <summary>
        /// Constructor of ConfigurationVariable
        /// </summary>
        public ConfigurationVariableType()
        {
            Value = 0;  
            Enabled = true;
            Description= string.Empty;
        }  
    }
}
