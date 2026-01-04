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

namespace Z2XProgrammer.DataModel
{
    public class FunctionKeyFunctionType: INotifyPropertyChanged
    {
        #region PRIVATE FIELDS        

        private  string _Descrption = string.Empty; 
        
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)    
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        # region PUBLIC PROPERTIES

        public FunctionKeyFunctionType(string description)
        {
            _Descrption = description;
        }

        /// <summary>
        /// Gets or sets the ZIMO function mapping configuration.
        /// </summary>  
        public string Description
        {
            get => _Descrption;
            set
            {
                _Descrption = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        #endregion

    }
}
