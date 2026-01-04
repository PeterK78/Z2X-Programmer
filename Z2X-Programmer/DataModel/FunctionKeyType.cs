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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Z2XProgrammer.DataModel
{
    public partial class FunctionKeyType: INotifyPropertyChanged
    {
        #region PRIVATE FIELDS        

        /// <summary>
        /// Represents the mapping configuration for ZIMO functions.
        /// </summary>
        /// <remarks>This field holds an instance of <see cref="ZIMOFunctionMappingType"/> that defines
        /// the mapping of ZIMO functions. It is used internally to manage function mappings and is not exposed directly
        /// to external consumers.</remarks>
        private ZIMOInputMappingType _zimoFunctionMapping = new();

        private ObservableCollection<String>  _functionDescriptions = []; 

        #endregion

        #region REGION: PUBLIC DELEGATES        

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)    
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        # region PUBLIC PROPERTIES

        /// <summary>
        /// Gets or sets the ZIMO function mapping configuration.
        /// </summary>  
        public ZIMOInputMappingType ZIMOInputMapping
        {
            get => _zimoFunctionMapping;
            set
            {
                _zimoFunctionMapping = value;
                OnPropertyChanged(nameof(ZIMOInputMapping));
            }
        }

        /// <summary>
        /// Gets or sets the list of function key functions.
        /// </summary>
        public ObservableCollection<String> FunctionDescriptions
        {
            get => _functionDescriptions;
            set
            {
                _functionDescriptions = value;
                OnPropertyChanged(nameof(FunctionDescriptions));
            }
        }

        #endregion

    }
}
