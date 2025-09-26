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

using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.DataModel
{
    /// <summary>
    /// Contains the definition of single NMRA configuration variable (short configuration variable)
    /// </summary>
    public class ConfigurationVariableType : INotifyPropertyChanged
    {
        //  Private field variables.
        private bool _Enabled = true;
        private byte _Value = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The number of the configuration variable.
        /// </summary>
        /// 
        public int Number { get; set; }

        /// <summary>
        /// The value of the configuration variable.
        /// </summary>
        public byte Value
        {
            get => _Value;
            set
            {
                if (_Value == value) return;

                //  Setup the UndoRedo-Information.
                UndoRedoType undoRedoInfo = new UndoRedoType();
                undoRedoInfo.CVNumber = Number;
                undoRedoInfo.OldValue = _Value;
                undoRedoInfo.NewValue = value;

                _Value = value;
                OnPropertyChanged(nameof(Value));

                //  Inform the UndoRedo-Manager that a CV value has been changed.
                WeakReferenceMessenger.Default.Send(new UndoRedoMessage(undoRedoInfo));

            }
        }

        /// <summary>
        /// Enable or disables the configuration variable.
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
        /// A short description of th configuration variable.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is TRUE if the configuration variable is supported by the decoder specification.
        /// </summary>
        public bool DeqSecSupported { get; set; }

        /// <summary>
        /// Constructor of ConfigurationVariable.
        /// </summary>
        public ConfigurationVariableType()
        {
            Value = 0;
            Enabled = true;
            Description = string.Empty;
        }
    }
}
