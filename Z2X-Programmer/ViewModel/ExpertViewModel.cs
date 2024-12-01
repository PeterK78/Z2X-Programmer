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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class ExpertViewModel : ObservableObject
    {

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        internal ObservableCollection<ConfigurationVariableType>? configurationVariables = new ObservableCollection<ConfigurationVariableType>();

        [ObservableProperty]
        internal ConfigurationVariableType? selectedConfigurationVariable;
    
        [ObservableProperty]
        bool dataValid;

        [ObservableProperty]
        bool activityReadWriteCVOngoing = false;

        [ObservableProperty]
        bool bit0;
        partial void OnBit0Changed(bool value)
        {
            Value = Bit.Set(Value, 0, value);
        }

        [ObservableProperty]
        bool bit1;
        partial void OnBit1Changed(bool value)
        {
            Value = Bit.Set(Value, 1, value);
        }

        [ObservableProperty]
        bool bit2;
        partial void OnBit2Changed(bool value)
        {
            Value = Bit.Set(Value, 2, value);
        }

        [ObservableProperty]
        bool bit3;
        partial void OnBit3Changed(bool value)
        {
            Value = Bit.Set(Value, 3, value);
        }

        [ObservableProperty]
        bool bit4;
        partial void OnBit4Changed(bool value)
        {
            Value = Bit.Set(Value, 4, value);
        }

        [ObservableProperty]
        bool bit5;
        partial void OnBit5Changed(bool value)
        {
            Value = Bit.Set(Value, 5, value);
        }

        [ObservableProperty]
        bool bit6;
        partial void OnBit6Changed(bool value)
        {
            Value = Bit.Set(Value, 6, value);
        }

        [ObservableProperty]
        bool bit7;
        partial void OnBit7Changed(bool value)
        {
            Value = Bit.Set(Value, 7, value);
        }

        [ObservableProperty]
        byte value;
        partial void OnValueChanged(byte value)
        {
            Bit0 = Bit.IsSet(value, 0);
            Bit1 = Bit.IsSet(value, 1);
            Bit2 = Bit.IsSet(value, 2);
            Bit3 = Bit.IsSet(value, 3);
            Bit4 = Bit.IsSet(value, 4);
            Bit5 = Bit.IsSet(value, 5);
            Bit6 = Bit.IsSet(value, 6);
            Bit7 = Bit.IsSet(value, 7);
        }

        [ObservableProperty]
        ushort cvNumber;
        #endregion

        #region REGION: CONSTRUCTOR
        public ExpertViewModel()
        {
            DataValid = true;
            CvNumber = 1;
            UpdateCVList();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDecoderConfiguration();
                });
            });

            WeakReferenceMessenger.Default.Register<DecoderSpecificationUpdatedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDecoderSpecification();
                });
            });

        }
        #endregion

        #region REGION: COMMANDS
    
        //  Reads out the value and the number of the selected configuration variable and updates the GUI.
        [RelayCommand]
        private void CVSelected()
        {
            if (SelectedConfigurationVariable == null) return;
            CvNumber = (ushort)SelectedConfigurationVariable.Number;
            Value = SelectedConfigurationVariable.Value;
        }

        // This command toggles the enabled state of the selected configuration variable.
        [RelayCommand]
        private async Task EnableDisableCV()
        {
            try
            {

                //  Check if a function key has been selected.
                if (SelectedConfigurationVariable == null)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.FrameFunctionKeysZIMONoMappingSelected, AppResources.OK);
                    return;
                }
                
                // We search for the required configuration variable and toggle the Enabled property.
                ConfigurationVariableType result = DecoderConfiguration.ConfigurationVariables.Single(s => s.Number == SelectedConfigurationVariable.Number);
                result.Enabled = !result.Enabled;
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        [RelayCommand]
        async Task WriteCV()
        {
            try
            {

                if (CommandStation.Connect() == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderDownloadError, AppResources.OK);
                    return;
                }

                DataValid = false;
                ActivityReadWriteCVOngoing = true;

                //  Turn the track power ON if we are in POM mode
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    await Task.Run(() => ReadWriteDecoder.SetTrackPowerON());
                }

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                bool WriteSuccessFull = false;
    
                await Task.Run(() => WriteSuccessFull =  ReadWriteDecoder.WriteCV(CvNumber, DecoderConfiguration.RCN225.LocomotiveAddress, Value, DecoderConfiguration.ProgrammingMode, cancelToken));

                if (WriteSuccessFull == true)
                {
                    DecoderConfiguration.ConfigurationVariables[CvNumber].Value = Value;
                    DecoderConfiguration.ConfigurationVariables[CvNumber].Enabled = true;

                    DecoderConfiguration.BackupCVs[CvNumber].Value = Value;
                    DecoderConfiguration.BackupCVs[CvNumber].Enabled = true;

                    WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                    DataValid = true;
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertWriteCVError, AppResources.OK);
                }

                //  After writing the CV on the programming track, we must switch the track power on.
                //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();


                ActivityReadWriteCVOngoing = false;
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        [RelayCommand]
        async Task ReadCV()
        {
            try
            {
                if (CommandStation.Connect() == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError, AppResources.OK);
                    return;
                }

                DataValid = false;
                ActivityReadWriteCVOngoing = true;

                //  Turn the track power ON if we are in POM mode
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    await Task.Run(() => ReadWriteDecoder.SetTrackPowerON());
                }

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                bool readSuccessFull = false;
                await Task.Run(() => readSuccessFull = ReadWriteDecoder.ReadCV(CvNumber, DecoderConfiguration.RCN225.LocomotiveAddress, DecoderConfiguration.ProgrammingMode, cancelToken));

                if (readSuccessFull == true)
                {
                    Value = DecoderConfiguration.ConfigurationVariables[CvNumber].Value;
                    DecoderConfiguration.ConfigurationVariables[CvNumber].Enabled = true;

                    WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                    DataValid = true;
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertReadCVError, AppResources.OK);
                }

                //  After reading the CV on the programming track, we must switch the track power on.
                //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();

                ActivityReadWriteCVOngoing = false;

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }
        #endregion

        #region REGION: PRIVATE FUNCTIONS        

        /// <summary>
        /// Updates the list of configuration variables.
        /// </summary>
        private void UpdateCVList()
        {
            if (ConfigurationVariables == null) return;
            ConfigurationVariables.Clear();
            foreach(ConfigurationVariableType item in DecoderConfiguration.ConfigurationVariables)
            {
                if (item.DeqSecSupported == true) ConfigurationVariables.Add(item);

            }
        }
        
        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            UpdateCVList();
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            UpdateCVList();
        }

        #endregion

    }
}
