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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// This class describes the properties of a function output.
    /// </summary>
    public class FunctionOutputType: INotifyPropertyChanged
    {
        //  Private field variables.
        private string _UserDefinedDescription = string.Empty;
        private string _Description = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)    
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// An unique ID to identify the function output.
        /// </summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// A description of the function output defined by the user.
        /// </summary>
        public string UserDefinedDescription
        {
            get => _UserDefinedDescription;
            set
            {
                _UserDefinedDescription = value;
                OnPropertyChanged(nameof(UserDefinedDescription));
            }
        }

        /// <summary>
        /// A description of the function output.
        /// </summary>
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                 OnPropertyChanged(nameof(Description));
            }
        }


    }
}
