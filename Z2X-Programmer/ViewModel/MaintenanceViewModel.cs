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

using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Popups;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class MaintenanceViewModel: ObservableObject
    {
        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

        // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion

        #region REGION: DECODER FEATURES

        // ZIMO_SELFTEST_CV30;
        [ObservableProperty]
        bool zIMO_SELFTEST_CV30;

        [ObservableProperty]
        bool rCN225_DECODERRESET_CV8;

        #endregion

        #region REGION: PUBLIC PROPERTIES
        
        [ObservableProperty]
        string zIMODecoderErrorState = "";

        [ObservableProperty]
        string cV30Configuration = Subline.Create(new List<uint>{30});

        #endregion

        #region REGION: CONSTRUCTOR

        public MaintenanceViewModel()
        {
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

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // ZIMO: ZIMO_SELFTEST_CV30;
            ZIMODecoderErrorState = ZIMO.GetSelfTestResult(DecoderConfiguration.ZIMO.SelfTest);
            CV30Configuration = Subline.Create(new List<uint>{30});
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            ZIMO_SELFTEST_CV30 = DecoderSpecification.ZIMO_SELFTEST_CV30;
            RCN225_DECODERRESET_CV8 = DecoderSpecification.RCN225_DECODERRESET_CV8;
        }

        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Resets the decoder by writing value 8 to CV8.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task ResetDecoder()
        {
            try
            {

                CancellationToken cancelToken = new CancellationTokenSource().Token;

                //  The decoder reset is only allowed on the progam track.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.FrameSecurityDecoderResetNoPOMMode, AppResources.YES);
                    return;
                }

                //  Make sure that the user really wanna reset the decoder
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.FrameSecurityDecoderResetButtonMessage, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }

                if (CommandStation.Connect(cancelToken, 5000) == false) return;

                await Task.Run(() => ReadWriteDecoder.WriteCV((8), DecoderConfiguration.RCN225.LocomotiveAddress, 8, NMRA.DCCProgrammingModes.DirectProgrammingTrack, cancelToken));

                //  The decoder reset is only allowed on the progam track.
                if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.FrameSecurityDecoderResetResetPerformed, AppResources.YES, AppResources.NO);
                    return;
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        [RelayCommand]
        async Task StartZIMOSelfTest()
        {
            try
            {

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                if (CommandStation.Connect(cancelToken, 5000) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError, AppResources.OK);
                    return;
                }

                PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, "Testing ...", "");

                //  Application.Current.MainPage.ShowPopup(pop);

                Shell.Current.CurrentPage.ShowPopup(pop);

                await Task.Run(() => ReadWriteDecoder.WriteCV((30), DecoderConfiguration.RCN225.LocomotiveAddress, 255, DecoderConfiguration.ProgrammingMode, cancelToken));

                for (int i = 1; i <= 10; i++)
                {
                    Thread.Sleep(1000);
                }

                await Task.Run(() => ReadWriteDecoder.ReadCV((30), DecoderConfiguration.RCN225.LocomotiveAddress, DecoderConfiguration.ProgrammingMode, cancelToken));

                await pop.CloseAsync();

                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        [RelayCommand]
        async Task ResetErrorMemory()
        {
            try
            {

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                if (CommandStation.Connect(cancelToken, 5000) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError, AppResources.OK);
                    return;
                }

             
                await Task.Run(() => ReadWriteDecoder.WriteCV((30), DecoderConfiguration.RCN225.LocomotiveAddress, 0, DecoderConfiguration.ProgrammingMode, cancelToken));

                await Task.Run(() => ReadWriteDecoder.ReadCV((30), DecoderConfiguration.RCN225.LocomotiveAddress, DecoderConfiguration.ProgrammingMode, cancelToken));


                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }

        #endregion

    }
}
