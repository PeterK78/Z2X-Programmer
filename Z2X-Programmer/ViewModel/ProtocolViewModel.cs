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
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z21Lib.Events;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class ProtocolViewModel : ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS & SEARCH

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

        // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion

        #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool anyProtocollSettingsSupported;
      
        // ZIMO: The DCC operating mode cannot be deactivated (ZIMO_MSOPERATINGMODES_CV12)
        [ObservableProperty]
        bool zIMO_MSOPERATINGMODES_CV12;

        // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        [ObservableProperty]
        bool rCN225_OPERATINGMODES_CV12;

        [ObservableProperty]
        bool rCN225_RAILCOMENABLED_CV29_3;

        [ObservableProperty]
        bool rCN225_RAILCOMCHANNEL1BROADCAST_CV28_0;

        [ObservableProperty]
        bool rCN225_RAILCOMCHANNEL2DATA_CV28_1;

        [ObservableProperty]
        bool rCN225_ANALOGMODE_CV29_2;

        [ObservableProperty]
        bool rCN225_AUTOMATICREGISTRATION_CV28_7;

        #endregion
    
        #region REGION: PUBLIC PROPERTIES

        // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        // DCC in CV12 Bit 2
        [ObservableProperty]
        bool dccOperationEnabled;
        partial void OnDccOperationEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeDCCEnabled = value;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }

        // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        // Motorola MM in CV12 Bit 5
        [ObservableProperty]
        bool mMOperationEnabled;
        partial void OnMMOperationEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeMMEnabled = value;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }

        // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        // MFX in CV12 Bit 6
        [ObservableProperty]
        bool mFXOperationEnabled;
        partial void OnMFXOperationEnabledChanged(bool value)        
        {
            DecoderConfiguration.RCN225.OperatingModeMFXEnabled = value;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }

        // RCN225: Analog operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        // DC analog mode in CV12 Bit 0
        [ObservableProperty]
        bool analogOperationDCEnabled;
        partial void OnAnalogOperationDCEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeAnalogDCEnabled = value;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }
        
        // RCN225: Analog operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
        // AC analog mode in CV12 Bit 4
        [ObservableProperty]
        bool analogOperationACEnabled;
        partial void OnAnalogOperationACEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeAnalogACEnabled = value;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }

        [ObservableProperty]
        string cV12Configuration = string.Empty;

        // RCN225: AC mode enabled in CV29 bit 2
        [ObservableProperty]
        bool acModeEnabled;
        partial void OnAcModeEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.ACModeEnabled  = value;
            CV29Configuration = Subline.Create(new List<uint> { 29 });
        }
        [ObservableProperty]
        string cV29Configuration = string.Empty;

        // RCN225: Railcom configuration in CV29.3 (RCN225_RAILCOMENABLED_CV29_3)
        [ObservableProperty]
        bool railComEnabled;
        partial void OnRailComEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComEnabled = value;
            CV29Configuration = Subline.Create(new List<uint> { 29 });
        }

        // RCN225: Railcom channel 1 address broadcast in CV28.0 (RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0)
        [ObservableProperty]
        bool railComChannel1AdrBroadcast;
        partial void OnRailComChannel1AdrBroadcastChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComChannel1AdrBroadcast = value;
            CV28Configuration = Subline.Create(new List<uint> { 28 });
        }

        // RCN225: Railcom channel 2 in CV28            
        [ObservableProperty]
        bool railComChannel2Enabled;
        partial void OnRailComChannel2EnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComChannel2Enabled = value;
            CV28Configuration = Subline.Create(new List<uint> { 28 });
        }

        [ObservableProperty]
        string cV28Configuration = string.Empty;


        // RCN225: Automatic registration in CV28.7 (RCN225_AUTOMATICREGISTRATION_CV28_7)
        [ObservableProperty]
        bool automaticRegistrationEnabled;
        partial void OnAutomaticRegistrationEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.AutomaticRegistrationEnabled = value;
            CV28Configuration = Subline.Create(new List<uint> { 28 });
        }

        // Railcom speed received from the command station
        [ObservableProperty]
        bool railComSpeedReceived = false;

        [ObservableProperty]
        int railComSpeed = 0;

        // Railcom QOS received from the command station
        [ObservableProperty]
        bool railComQOSReceived = false;

        [ObservableProperty]
        int railComQOS = 0;

        #endregion

        # region REGION: CONSTRUCTOR
        public ProtocolViewModel()
        {
            CommandStation.OnRailComInfoReceived += OnRailComInfoReceived;

            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

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

        # region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// This event OnRailComInfoReceived is raised when the command station receives railcom data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRailComInfoReceived(object? sender, RailComInfoEventArgs e)
        {
            if (e.LocomotiveAddress == DecoderConfiguration.RCN225.LocomotiveAddress)
            {
                RailComSpeed = e.Speed;
                RailComQOS = e.QOS;
            }
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // RCN225: Railcom configuration in CV29.3 (RCN225_RAILCOMENABLED_CV29_3)
            RailComEnabled = DecoderConfiguration.RCN225.RailComEnabled;
            CV29Configuration = Subline.Create(new List<uint> { 29 });

            // RCN225: Railcom channel 1 address broadcast in CV28.0 (RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0)
            RailComChannel1AdrBroadcast = DecoderConfiguration.RCN225.RailComChannel1AdrBroadcast;
            CV28Configuration = Subline.Create(new List<uint> { 28 });

            // RCN225: Railcom channel 2 in CV28 (RCN225_RAILCOMCHANNEL2DATA_CV28_1)
            RailComChannel2Enabled = DecoderConfiguration.RCN225.RailComChannel2Enabled;
            CV28Configuration = Subline.Create(new List<uint> { 28 });

            // RCN225: AC mode enabled in CV29 bit 2 (
            AcModeEnabled = DecoderConfiguration.RCN225.ACModeEnabled;
            CV29Configuration = Subline.Create(new List<uint> { 29 });

            // RCN225: Automatic registration in CV28.7 (RCN225_AUTOMATICREGISTRATION_CV28_7)
            AutomaticRegistrationEnabled = DecoderConfiguration.RCN225.AutomaticRegistrationEnabled;
            CV28Configuration = Subline.Create(new List<uint> { 28 });

            // RCN225: Analog operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
            // DC analog mode in CV12 Bit 0
            AnalogOperationDCEnabled = DecoderConfiguration.RCN225.OperatingModeAnalogDCEnabled;
            CV12Configuration = Subline.Create(new List<uint> { 12 });

            // RCN225: Analog operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
            // AC analog mode in CV12 Bit 4
            AnalogOperationACEnabled = DecoderConfiguration.RCN225.OperatingModeAnalogACEnabled;
            CV12Configuration = Subline.Create(new List<uint>{12});

            // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
            // DCC in CV12 Bit 2
            DccOperationEnabled = DecoderConfiguration.RCN225.OperatingModeDCCEnabled;
            CV12Configuration = Subline.Create(new List<uint>{12});

            // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
            // Motorola MM in CV12 Bit 5
            MMOperationEnabled = DecoderConfiguration.RCN225.OperatingModeMMEnabled;
            CV12Configuration = Subline.Create(new List<uint>{12});

            // RCN225: Digital operating modes in CV12 (RCN225_OPERATINGMODES_CV12)
            // MFX in CV12 Bit 6
            MFXOperationEnabled = DecoderConfiguration.RCN225.OperatingModeMFXEnabled;
            CV12Configuration = Subline.Create(new List<uint>{12});
        }

        /// <summary>
        /// This event handler reacts to the DecoderSpecificationUpdatedMessage message. It will fetch
        /// the supported features of the currently selected decode specification and update the local properties
        /// in this view model.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            RCN225_RAILCOMENABLED_CV29_3 = DecoderSpecification.RCN225_RAILCOMENABLED_CV29_3;
            RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0 = DecoderSpecification.RCN225_RAILCOMCHANNEL1BROADCAST_CV28_0;
            RCN225_RAILCOMCHANNEL2DATA_CV28_1 = DecoderSpecification.RCN225_RAILCOMCHANNEL2DATA_CV28_1;
            RCN225_ANALOGMODE_CV29_2 = DecoderSpecification.RCN225_ANALOGMODE_CV29_2;
            RCN225_AUTOMATICREGISTRATION_CV28_7 = DecoderSpecification.RCN225_AUTOMATICREGISTRATION_CV28_7;
            RCN225_OPERATINGMODES_CV12 = DecoderSpecification.RCN225_OPERATINGMODES_CV12;
            ZIMO_MSOPERATINGMODES_CV12 = DecoderSpecification.ZIMO_MSOPERATINGMODES_CV12;

            AnyProtocollSettingsSupported = DeqSpecReader.AnyProtocollSettingsSupported(DecoderSpecification.DeqSpecName);
        }
        #endregion

    }
}
