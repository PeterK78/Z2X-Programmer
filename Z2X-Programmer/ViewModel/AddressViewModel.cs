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
    public partial class AddressViewModel : ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS & SEARCH

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1;

        #endregion

        #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool rCN225_CONSISTADDRESS_CV19X;

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

        //  RCN225: Vehicle address CV1, CV17 and CV18 (RCN225_BASEADDRESS_CV1) 
        [ObservableProperty]
        ushort vehicleAddress;
        partial void OnVehicleAddressChanged(ushort oldValue, ushort newValue)
        {
            DecoderConfiguration.RCN225.LocomotiveAddress = newValue;
            VehicleAddressCVConfiguration = Subline.Create(new List<uint>{1,17,18});
            WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
        }

        [ObservableProperty]
        string vehicleAddressCVConfiguration = string.Empty;

        // RCN225: DCC address mode CV29.5 (RCN225_LONGSHORTADDRESS_CV29_5)
        [ObservableProperty]
        internal ObservableCollection<string> availableDCCAddressModesVehicleAdr;

        [ObservableProperty]
        internal string selectedDCCAddressModeVehicleAdr;
        partial void OnSelectedDCCAddressModeVehicleAdrChanged(string? oldValue, string newValue)
        {
            //  Did we get a new value? Otherwise return.
            if (newValue == null) return;

            //  Convert the description to the enum value and store it in the decoder configuration.
            DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr = NMRAEnumConverter.GetDCCAddressModeFromDescription(newValue);
            SelectedDCCAddressModeVehicleAddrCVConfiguration = Subline.Create(new List<uint> { 29 });

            //  Depending on the selected DCC address mode, set the limits for the address.
            SetGUILimits();

            //  We need to make sure that we are using valid vehicle and consist addresses.
            if (DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr == NMRA.DCCAddressModes.Extended)
            {
                //  Lets check if a valid long vehicle address has already been configured.
                if ((DecoderConfiguration.RCN225.LocomotiveAddress > NMRA.LongAddressMinimum) && (DecoderConfiguration.RCN225.LocomotiveAddress < NMRA.LongAddressMaximum))
                {
                    //  A valid long vehicle address is already available - let's use it.
                    VehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
                }
                else
                {
                    //  We do not have a valid long vehicle address - let's use the default one. 
                    VehicleAddress = NMRA.StandardLongVehicleAddress;
                }
            }
            else
            {
                //  Lets check if a valid short vehicle address has already been configured.
                if ((DecoderConfiguration.RCN225.LocomotiveAddress > NMRA.ShortAddressMinimum) && (DecoderConfiguration.RCN225.LocomotiveAddress < NMRA.ShortAddressMaximum))
                {
                    //  A valid long vehicle address is already available - let's use it.
                    VehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
                }
                else
                {
                    //  We do not have a valid long vehicle address - let's use the default one. 
                    VehicleAddress = NMRA.StandardShortVehicleAddress;
                }
            }
            
        }

        [ObservableProperty]
        string selectedDCCAddressModeVehicleAddrCVConfiguration = string.Empty;

        // RCN225: Consist address CV19 and CV20 (RCN225_CONSISTADDRESS_CV19X)
        [ObservableProperty]
        bool consistAddressEnabled;
        partial void OnConsistAddressEnabledChanged(bool value)
        {
            //  Check if we have to disable the consist address.
            if (value == false)
            {
                //  To turn of the consist address we have to set the consist address to 0.
                DecoderConfiguration.RCN225.ConsistAddress = 0;
            }
            else
            {
                //  Just in case we do not have any consist address configured, we set a new default value.
                if (DecoderConfiguration.RCN225.ConsistAddress == 0) 
                {
                    if ((DecoderConfiguration.RCN225Backup.ConsistAddress == 0) || (DecoderConfiguration.RCN225Backup.ConsistAddress > 127))
                    {
                        ConsistAddress = NMRA.StandardShortVehicleAddress;
                    }
                    else
                    {
                        ConsistAddress = DecoderConfiguration.RCN225Backup.ConsistAddress;
                    }
                }
            }
            ConsistAddressCVConfiguration = Subline.Create(new List<uint>{19, 20});
        }

        [ObservableProperty]
        ushort consistAddress;
        partial void OnConsistAddressChanged(ushort value)
        {
            DecoderConfiguration.RCN225.ConsistAddress = value;
            ConsistAddressCVConfiguration = Subline.Create(new List<uint>{19, 20});
        }

        [ObservableProperty]
        string consistAddressCVConfiguration = Subline.Create(new List<uint>{19, 20});
    
        // ZIMO: Secondary address for function decoders CV64 (ZIMO_MXFX_SECONDADDRESS_CV64)
        [ObservableProperty]
        ushort secondaryAddress;
        partial void OnSecondaryAddressChanged(ushort value)
        {
            DecoderConfiguration.ZIMO.SecondaryAddress = value;
            SecondaryAddressCVConfiguration = Subline.Create(new List<uint> { 64, 67, 68 });
            WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
        }
        [ObservableProperty]
        string secondaryAddressCVConfiguration = string.Empty;
       
        // ZIMO: Secondary address mode for function decoders CV112 (ZIMO_MXFX_SECONDADDRESS_CV64)
        [ObservableProperty]
        internal ObservableCollection<string> availableDCCAddressModesSecondaryAdr;

        [ObservableProperty]
        internal string selectedDCCAddressModeSecondaryAdr;
        partial void OnSelectedDCCAddressModeSecondaryAdrChanged(string? oldValue, string newValue)
        {
            if (newValue == null) return;
            DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr = NMRAEnumConverter.GetDCCAddressModeFromDescription(newValue);
            SelectedDCCAddressModeSecondaryAdrCVValues = Subline.Create(new List<uint> { 112 });
            SetGUILimits();
            SecondaryAddress = DecoderConfiguration.ZIMOBackup.SecondaryAddress;
        }

        [ObservableProperty]
        internal string selectedDCCAddressModeSecondaryAdrCVValues = string.Empty;


        
        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public AddressViewModel()
        {
            AvailableDCCAddressModesVehicleAdr = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCAddressModes());
            AvailableDCCAddressModesSecondaryAdr = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCAddressModes());           

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

        /// <summary>
        /// Updates the limits in the GUI.
        /// </summary>
        public void SetGUILimits()
        {
            //  Set the limits for the vehicle address, depending on the selected DCC address mode.
            if (DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr == NMRA.DCCAddressModes.Short)
            {
                LimitMinimumAddress = NMRA.ShortAddressMinimum;
                LimitMaximumAddress = NMRA.ShortAddressMaximum;
            }
            else
            {
                LimitMinimumAddress = NMRA.LongAddressMinimum;
                LimitMaximumAddress = NMRA.LongAddressMaximum;
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
            RCN225_CONSISTADDRESS_CV19X = DecoderSpecification.RCN225_CONSISTADDRESS_CV19X;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {

            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  Update the vehicle address.
            VehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;

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
