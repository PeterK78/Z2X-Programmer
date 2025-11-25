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
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class AddressViewModel : ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS & SEARCH

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1;

        [ObservableProperty]
        bool activityReadCVOngoing = false;

        [ObservableProperty]
        bool activityWriteVehicelAddressOngoing = false;

        #endregion

        #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool rCN225_LONGSHORTADDRESS_CV29_5;

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

        // NMRA DCC ProgrammingMode POM enabled?
        [ObservableProperty]
        bool dccNMRAProgramTrackEnabled = (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) ?  true : false;

        //  RCN225: Vehicle address CV1, CV17 and CV18 (RCN225_BASEADDRESS_CV1) 
        [ObservableProperty]
        ushort vehicleAddress;
        partial void OnVehicleAddressChanged(ushort oldValue, ushort newValue)
        {
            DecoderConfiguration.RCN225.LocomotiveAddress = newValue;
            VehicleAddressCVConfiguration = Subline.Create(new List<uint> { 1, 17, 18 });
            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
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
                //  Lets check if a valid long vehicle address has already been configured. A valid long adress is in the range between 128 and 10239.
                ushort longAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
                if ((longAddress >= NMRA.LongAddressMinimum) && (longAddress <= NMRA.LongAddressMaximum))
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
            ConsistAddressCVConfiguration = Subline.Create(new List<uint> { 19, 20 });
        }

        [ObservableProperty]
        ushort consistAddress;
        partial void OnConsistAddressChanged(ushort value)
        {
            DecoderConfiguration.RCN225.ConsistAddress = value;
            ConsistAddressCVConfiguration = Subline.Create(new List<uint> { 19, 20 });
        }

        [ObservableProperty]
        string consistAddressCVConfiguration = Subline.Create(new List<uint> { 19, 20 });

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

            WeakReferenceMessenger.Default.Register<ProgrammingModeUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetProgrammingMode();
                });
            });

        }
        #endregion 

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            ZIMO_MXFX_SECONDADDRESS_CV64 = DecoderSpecification.ZIMO_MXFX_SECONDADDRESS_CV64;
            RCN225_CONSISTADDRESS_CV19X = DecoderSpecification.RCN225_CONSISTADDRESS_CV19X;
            RCN225_LONGSHORTADDRESS_CV29_5 = DecoderSpecification.RCN225_LONGSHORTADDRESS_CV29_5;
        }

        /// <summary>
        /// The OnGetProgrammingMode handler is called when the ProgrammingModeUpdateMessage message has been received.
        /// </summary>
        private void OnGetProgrammingMode()
        {
            DccNMRAProgramTrackEnabled = (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) ?  true : false;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  RCN225

            //  RCN225: Vehicle address CV1, CV17 and CV18 (RCN225_BASEADDRESS_CV1) 
            VehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
            VehicleAddressCVConfiguration = Subline.Create(new List<uint> { 1, 17, 18 });

            //  RCN225: DCC address mode CV29.5 (RCN225_LONGSHORTADDRESS_CV29_5)
            SelectedDCCAddressModeVehicleAdr = Helper.NMRAEnumConverter.GetDCCAddressModeDescription(DecoderConfiguration.RCN225.DCCAddressModeVehicleAdr);
            SelectedDCCAddressModeVehicleAddrCVConfiguration = Subline.Create(new List<uint> { 29 });

            // RCN225: Consist address CV19 and CV20 (RCN225_CONSISTADDRESS_CV19X)
            ConsistAddress = DecoderConfiguration.RCN225.ConsistAddress;
            if (DecoderConfiguration.RCN225.ConsistAddress == 0)
            {
                ConsistAddressEnabled = false;
            }
            else
            {
                ConsistAddressEnabled = true;
            }
            ConsistAddressCVConfiguration = Subline.Create(new List<uint> { 19, 20 });

            //  ZIMO

            // ZIMO: Secondary address for function decoders CV64 (ZIMO_MXFX_SECONDADDRESS_CV64)
            SecondaryAddress = DecoderConfiguration.ZIMO.SecondaryAddress;
            SecondaryAddressCVConfiguration = Subline.Create(new List<uint> { 64, 67, 68 });

            // ZIMO: Secondary address mode for function decoders CV112 (ZIMO_MXFX_SECONDADDRESS_CV64)
            SelectedDCCAddressModeSecondaryAdr = Helper.NMRAEnumConverter.GetDCCAddressModeDescription(DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr);
            SelectedDCCAddressModeSecondaryAdrCVValues = Subline.Create(new List<uint> { 112 });

        }

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

            if (DecoderConfiguration.ZIMO.DCCAddressModeSecondaryAdr == NMRA.DCCAddressModes.Short)
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
        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Writes the vehicle address in CV1 to the decoder.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task WriteVehicleAddress()
        {
            try
            {
                // Defines the configuration variables we have to write to configure the vehicle address.
                ushort[] configurationVariables = { 1, 17, 18, 29 };

                // Is TRUE if we have written successfully a value to the decoder.
                bool WriteSuccessFull = false;

                CancellationToken cancelToken = new CancellationTokenSource().Token;

                // Ask the user if he really wanna update the vehicle address. The programming methods differ significantly,
                // which is why two different user messages are created. 
                string infoMessage = "";
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    // User message for programming on the main track.
                    infoMessage = AppResources.AlertWriteVehicleAddress1 + " " + DecoderConfiguration.RCN225Backup.LocomotiveAddress + " " + AppResources.AlertWriteVehicleAddress2 + " " + DecoderConfiguration.RCN225.LocomotiveAddress + " " + AppResources.AlertWriteVehicleAddress3;
                    infoMessage += "\n\n" + AppResources.AlertWriteVehicleAddressProgrammingMethod + " " + AppResources.DCCProgrammingModePOM + " " + AppResources.AlertWriteVehicleAddressProgrammingMethod1;
                }
                else
                {   // User message for programming on the programming track.
                    infoMessage = AppResources.AlertWriteVehicleAddressWriteProg + " " + DecoderConfiguration.RCN225.LocomotiveAddress + " " + AppResources.AlertWriteVehicleAddressWriteProg2;
                }

                if (await MessageBox.Show(AppResources.AlertInformation, infoMessage, AppResources.YES, AppResources.NO) == false)
                {
                    VehicleAddress = DecoderConfiguration.RCN225Backup.LocomotiveAddress;
                    return;
                }

                // Start the activity indicator.
                ActivityWriteVehicelAddressOngoing = true;

                //  Check if we have a valid connection to the digital system.
                if (CommandStation.Connect(cancelToken, 5000) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderDownloadError, AppResources.OK);
                    return;
                }

                //  Turn the track power ON if we are in POM mode.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    await Task.Run(() => ReadWriteDecoder.SetTrackPowerON());
                }

                // Write each configuration variable of configurationVariables to the decoder. 
                foreach (ushort cv in configurationVariables)
                {
                    // Reset our "we have written successfully to the decoder flag".
                    WriteSuccessFull = false;

                    // Write the next configuration variable to the decoder.
                    await Task.Run(() => WriteSuccessFull = ReadWriteDecoder.WriteCV(cv, DecoderConfiguration.RCN225Backup.LocomotiveAddress, DecoderConfiguration.ConfigurationVariables[cv].Value, DecoderConfiguration.ProgrammingMode, cancelToken, true));

                    if (WriteSuccessFull == true)
                    {
                        // We have written sucessfully a configuration variable - so we need to update our backup for this variables.
                        DecoderConfiguration.BackupCVs[cv].Value = DecoderConfiguration.ConfigurationVariables[cv].Value;
                        DecoderConfiguration.BackupCVs[cv].Enabled = DecoderConfiguration.ConfigurationVariables[cv].Enabled;
                    }
                    else
                    {
                        // Writing a configuration variable has failed. We display a message and exit.
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertWriteVehicleAddressError, AppResources.OK);

                        //  After writing the CV on the programming track, we must switch the track power on.
                        //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
                        if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();

                        // Stop the activity indicator.
                        ActivityWriteVehicelAddressOngoing = false;

                        return;
                    }
                }

                // Inform the application that we have set a new decoder configuration.
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                //  After writing the CV on the programming track, we must switch the track power on.
                //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();

                // Stop the activity indicator.
                ActivityWriteVehicelAddressOngoing = false;
            }
            catch (Exception)
            {
                // Stop the activity indicator.
                ActivityWriteVehicelAddressOngoing = false;
            }
        }

        /// <summary>
        /// Reads the vehicle address from the decoder. This command is only available in direct programming mode.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DetectVehicleAddress()
        {
            try
            {
                ushort[] cVValuesToRead = new ushort[] { 1, 17, 18, 29 };

                //  Check if we are in direct programming mode.
                if (DecoderConfiguration.ProgrammingMode != NMRA.DCCProgrammingModes.DirectProgrammingTrack)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.FrameAddressVehicleAddressDetectNotProgTrack, AppResources.OK);
                    return;
                }

                ActivityReadCVOngoing = true;

                CancellationToken cancelToken = new CancellationTokenSource().Token;

                //  Check if we are connected to the command station.
                if (CommandStation.Connect(cancelToken, 5000) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError, AppResources.OK);
                    ActivityReadCVOngoing = false;
                    return;
                }

                await Task.Run(() => ReadWriteDecoder.SetTrackPowerON());

                // Read each CV value from the decoder.
                bool readSuccessFull = false;
                foreach (ushort cV in cVValuesToRead)
                {
                    //  Read the next CV value from the decoder.
                    readSuccessFull = false;
                    await Task.Run(() => readSuccessFull = ReadWriteDecoder.ReadCV(cV, 0, NMRA.DCCProgrammingModes.DirectProgrammingTrack, cancelToken));

                    // If reading the CV failed, display an error message, exit programming mode, and terminate the function.
                    if (readSuccessFull == false)
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertAddressNotRead, AppResources.OK);
                        if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();
                        ActivityReadCVOngoing = false;
                        return;
                    }
                }

                await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertVehicleAddressRead + " " + DecoderConfiguration.RCN225.LocomotiveAddress, AppResources.OK);

                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                //  After reading the CV on the programming track, we must switch the track power on.
                //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();

                ActivityReadCVOngoing = false;
            }
            catch (Exception)
            {
                ActivityReadCVOngoing = false;
            }
        }

        #endregion

    }
}
