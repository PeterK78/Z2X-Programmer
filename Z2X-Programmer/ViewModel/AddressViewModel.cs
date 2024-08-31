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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{

    [QueryProperty(nameof(SearchTarget), "SearchTarget")]
    public partial class AddressViewModel : ObservableObject
    {

        private string localSearchTarget = "";

        #region REGION: DECODER FEATURES

        public string SearchTarget
        {
            get => localSearchTarget;
            set
            {
                localSearchTarget = value;
            }
        }

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool rCN225_CONSISTADDRESS_CV19;

        [ObservableProperty]
        bool zIMO_MXFX_SECONDADDRESS_CV64;
        #endregion

        #region REGION: LIMITS FOR ENTRY VALIDATION
        [ObservableProperty]
        int limitMinimumAddress;

        [ObservableProperty]
        int limitMaximumAddress;

        [ObservableProperty]
        int limitZimoSecondAddressMinimum;

        [ObservableProperty]
        int limitZimoSecondAddressMaximum;


        #endregion

        #region REGION: PUBLIC PROPERTIES
        [ObservableProperty]
        bool consistAddressEnabled;
        partial void OnConsistAddressEnabledChanged(bool value)
        {
            if (value == false)
            {
                DecoderConfiguration.RCN225.ConsistAddress = 0;
                return;
            }
            else
            {
                if ((DecoderConfiguration.RCN225Backup.ConsistAddress == 0)  || (DecoderConfiguration.RCN225Backup.ConsistAddress > 127))
                {
                    ConsistAddress = 1;
                }
                else
                {
                    ConsistAddress = DecoderConfiguration.RCN225Backup.ConsistAddress;
                }
            }
        }

        [ObservableProperty]
        ushort consistAddress;
        partial void OnConsistAddressChanged(ushort value)
        {
            DecoderConfiguration.RCN225.ConsistAddress = value;
        }

        [ObservableProperty]
        ushort locomotiveAddress;
        partial void OnLocomotiveAddressChanged(ushort oldValue, ushort newValue)
        {
            DecoderConfiguration.RCN225.LocomotiveAddress = newValue;
            Preferences.Default.Set(AppConstants.PREFERENCES_LOCOMOTIVEADDRESS_KEY, DecoderConfiguration.RCN225.LocomotiveAddress.ToString());
            WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
        }

        [ObservableProperty]
        ushort secondaryAddress;
        partial void OnSecondaryAddressChanged(ushort value)
        {
            DecoderConfiguration.ZIMO.SecondaryAddress = value;
            WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
        }

        [ObservableProperty]
        internal ObservableCollection<string> availableDCCAddressModesVehicleAdr;

        [ObservableProperty]
        internal ObservableCollection<string> availableDCCAddressModesSecondaryAdr;

        [ObservableProperty]
        internal string selectedDCCAddressModeSecondaryAdr;
        partial void OnSelectedDCCAddressModeSecondaryAdrChanged(string? oldValue, string newValue)
        {
            if (newValue == null) return;
            DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr = NMRAEnumConverter.GetDCCAddressModeFromDescription(newValue);
            SetGUILimits();
            SecondaryAddress = DecoderConfiguration.ZIMOBackup.SecondaryAddress;
        }

        [ObservableProperty]
        internal string selectedDCCAddressModeVehicleAdr;
        partial void OnSelectedDCCAddressModeVehicleAdrChanged(string? oldValue, string newValue)
        {
            if (newValue == null) return;
            DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr = NMRAEnumConverter.GetDCCAddressModeFromDescription(newValue);
            SetGUILimits();
            LocomotiveAddress = DecoderConfiguration.RCN225Backup.LocomotiveAddress;
            ConsistAddress = DecoderConfiguration.RCN225Backup.ConsistAddress;
            
        }
        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public AddressViewModel()
        {
            AvailableDCCAddressModesVehicleAdr = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCAddressModes());
            AvailableDCCAddressModesSecondaryAdr = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCAddressModes());

            LocomotiveAddress = ushort.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_LOCOMOTIVEADDRESS_KEY, AppConstants.PREFERENCES_LOCOMOTIVEADDRESS_DEFAULT));

            SelectedDCCAddressModeVehicleAdr = NMRAEnumConverter.GetDCCAddressModeDescription(NMRA.DCCAddressModes.Short);
            SelectedDCCAddressModeSecondaryAdr = NMRAEnumConverter.GetDCCAddressModeDescription(NMRA.DCCAddressModes.Short);
            
            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

            SetGUILimits();

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

        #region REGION: PRIVATE FUNCTIONS

        public void SetGUILimits()
        {
            if(DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr == NMRA.DCCAddressModes.Short)
            {
                LimitMinimumAddress = 1;
                LimitMaximumAddress = 127;
            }
            else
            {
                LimitMinimumAddress = 128;
                LimitMaximumAddress = 10239;
            }
    
            if(DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr == NMRA.DCCAddressModes.Short)
            {
                LimitZimoSecondAddressMinimum = 1;
                LimitZimoSecondAddressMaximum = 127;
            }
            else
            {
                LimitZimoSecondAddressMinimum = 128;
                LimitZimoSecondAddressMaximum = 10239;
            }
            
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            ZIMO_MXFX_SECONDADDRESS_CV64 = DecoderSpecification.ZIMO_MXFX_SECONDADDRESS_CV64;
            RCN225_CONSISTADDRESS_CV19 = DecoderSpecification.RCN225_CONSISTADDRESS_CV19;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {

            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  Update the vehicle address
            LocomotiveAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
            SelectedDCCAddressModeVehicleAdr = Helper.NMRAEnumConverter.GetDCCAddressModeDescription(DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr);

            //  Update the ZIMO specific secondary address
            SecondaryAddress = DecoderConfiguration.ZIMO.SecondaryAddress;
            SelectedDCCAddressModeSecondaryAdr = Helper.NMRAEnumConverter.GetDCCAddressModeDescription(DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr);

            ConsistAddress = DecoderConfiguration.RCN225.ConsistAddress;
            if(DecoderConfiguration.RCN225.ConsistAddress == 0)
            {
                ConsistAddressEnabled = false;

            }
            else
            {
                ConsistAddressEnabled = true;
            } 

        }
        #endregion
        
    }
}
