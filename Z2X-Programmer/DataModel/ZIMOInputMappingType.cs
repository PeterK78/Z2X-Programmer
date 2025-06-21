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

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Z2XProgrammer.DataModel
{
    public partial class ZIMOInputMappingType: INotifyPropertyChanged
    {
        #region REGION: PRIVATE FIELDS    

        private int _externalFunctionKeyNumber = 0;
        private string _externalFunctionKeyDescription = string.Empty;
        private int _internalFunctionKeyNumber = 0;
        private string _internalFunctionKeyDescription = string.Empty;
        private int _cvNumber = 0;
        private byte _cvValue = 0;

        #endregion

        #region REGION: PUBLIC DELEGATES
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion region

        #region REGION: PUBLIC PROPERTIES
        public int ExternalFunctionKeyNumber
        {
            get => _externalFunctionKeyNumber;
            set
            {
                _externalFunctionKeyNumber = value;
                _externalFunctionKeyDescription = "F" + value.ToString();
                OnPropertyChanged(nameof(ExternalFunctionKeyNumber));
                OnPropertyChanged(nameof(ExternalFunctionKeyDescription));
            }
        }

        public string ExternalFunctionKeyDescription
        {
            get => _externalFunctionKeyDescription;
        }

        public int InternalFunctionKeyNumber
        {
            get => _internalFunctionKeyNumber;
            set
            {
                _internalFunctionKeyNumber = value;
                _internalFunctionKeyDescription = "F" + value.ToString();
                OnPropertyChanged(nameof(InternalFunctionKeyNumber));
                OnPropertyChanged(nameof(InternalFunctionKeyDescription));
            }
        }

        public string InternalFunctionKeyDescription
        {
            get => _internalFunctionKeyDescription;
        }

        public int CVNumber
        {
            get => _cvNumber;
            set
            {
                _cvNumber = value;
                OnPropertyChanged(nameof(CVNumber));
            }
        }

        public byte CVValue
        {
            get => _cvValue;
            set
            {
                _cvValue = value;
                OnPropertyChanged(nameof(CVValue));
            }
        }

        #endregion region
    }
}
