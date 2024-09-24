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
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class ProtocolViewModel : ObservableObject
    {
        #region REGION: DECODER FEATURES
        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool zIMO_MSOPERATINGMODES_CV12;

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

        //  RCN225_OPERATINGMODES_CV12 Bit 0
        [ObservableProperty]
        bool analogOperationDCEnabled;
        partial void OnAnalogOperationDCEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeAnalogDCEnabled = value;
        }

        //  RCN225_OPERATINGMODES_CV12 Bit 2
        [ObservableProperty]
        bool dccOperationEnabled;
        partial void OnDccOperationEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.OperatingModeDCCEnabled = value;
        }

        //  RCN225_OPERATINGMODES_CV12 Bit 4
        [ObservableProperty]
        bool analogOperationACEnabled;
        partial void OnAnalogOperationACEnabledChanged(bool value)
        
        {
            DecoderConfiguration.RCN225.OperatingModeAnalogACEnabled = value;
        }

        //  RCN225_OPERATINGMODES_CV12 Bit 5
        [ObservableProperty]
        bool mMOperationEnabled;
        partial void OnMMOperationEnabledChanged(bool value)
        
        {
            DecoderConfiguration.RCN225.OperatingModeMMEnabled = value;
        }

        //  RCN225_OPERATINGMODES_CV12 Bit 6
        [ObservableProperty]
        bool mFXOperationEnabled;
        partial void OnMFXOperationEnabledChanged(bool value)
        
        {
            DecoderConfiguration.RCN225.OperatingModeMFXEnabled = value;
        }
    
        [ObservableProperty]
        bool railComEnabled;
        partial void OnRailComEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComEnabled = value;
        }

        [ObservableProperty]
        bool railComChannel1AdrBroadcast;
        partial void OnRailComChannel1AdrBroadcastChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComChannel1AdrBroadcast = value;
        }

        [ObservableProperty]
        bool railComChannel2Enabled;
        partial void OnRailComChannel2EnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.RailComChannel2Enabled = value;
        }

        [ObservableProperty]
        bool acModeEnabled;
        partial void OnAcModeEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.ACModeEnabled  = value;
        }

        [ObservableProperty]
        bool automaticRegistrationEnabled;
        partial void OnAutomaticRegistrationEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.AutomaticRegistrationEnabled = value;
        }

        #endregion

        # region REGION: CONSTRUCTOR
        public ProtocolViewModel()
        {
            OnGetDataFromDataStore();
            OnGetDataFromDecoderSpecification();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDataStore();
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
        /// This event handler reacts to the DataStoreUpdatedMessage message. It will fetch
        /// the current data from the data store and update the local properties in this view model.
        /// </summary>
        private void OnGetDataFromDataStore()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            RailComEnabled = DecoderConfiguration.RCN225.RailComEnabled;
            RailComChannel1AdrBroadcast = DecoderConfiguration.RCN225.RailComChannel1AdrBroadcast;
            RailComChannel2Enabled = DecoderConfiguration.RCN225.RailComChannel2Enabled;
            AcModeEnabled = DecoderConfiguration.RCN225.ACModeEnabled;
            AutomaticRegistrationEnabled = DecoderConfiguration.RCN225.AutomaticRegistrationEnabled;
            AnalogOperationDCEnabled = DecoderConfiguration.RCN225.OperatingModeAnalogDCEnabled;
            AnalogOperationACEnabled = DecoderConfiguration.RCN225.OperatingModeAnalogACEnabled;
            DccOperationEnabled = DecoderConfiguration.RCN225.OperatingModeDCCEnabled;
            MMOperationEnabled = DecoderConfiguration.RCN225.OperatingModeMMEnabled;
            MFXOperationEnabled = DecoderConfiguration.RCN225.OperatingModeMFXEnabled;
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
        }
        #endregion

    }
}
