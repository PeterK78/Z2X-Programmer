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
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;
using Z2XProgrammer.Popups;
using CommunityToolkit.Maui.Storage;
using Z2XProgrammer.Model;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.Messages;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Converter;
using Z21Lib.Events;
using Color = Microsoft.Maui.Graphics.Color;


namespace Z2XProgrammer.ViewModel
{
    public partial class AppShellViewModel: ObservableObject
    {

        #region REGION: PRIVATE FIELDS

        private bool _commandStationReachable = false;
        private bool _commandStationStatusBlinking = false;

        #endregion 

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        string applicationTitle = "Z2X-Programmer";

        [ObservableProperty]
        Color commandStationConnectionStateColor = Color.FromRgb(0, 0, 0);

        [ObservableProperty]
        string commandStationState;

        [ObservableProperty]
        internal ImageSource locomotiveImageSource;

        [ObservableProperty]
        internal string locomotiveAddress = "";

        [ObservableProperty]
        internal string locomotiveDescription = "";


        [ObservableProperty]
        internal ObservableCollection<string>?availableDecSpecs;

        [ObservableProperty]
        internal string selectedDecSpeq;
        partial void OnSelectedDecSpeqChanged(string? oldValue, string newValue)
        {
            DecoderSpecification.DeqSpecName = newValue;
        }

        [ObservableProperty]
        private ObservableCollection<String>? availableProgrammingModes;

        [ObservableProperty]
        internal string selectedProgrammingMode;
        partial void OnSelectedProgrammingModeChanged(string? oldValue, string newValue)
        {
            DecoderConfiguration.ProgrammingMode = CommandStation.GetProgrammingModeFromDescription(newValue);
            int mode = (int)DecoderConfiguration.ProgrammingMode;
            Preferences.Default.Set(AppConstants.PREFERENCES_PROGRAMMINGMODE_KEY,mode.ToString());
        }

        #endregion

        #region REGION: CONSTRUCTOR   

        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public AppShellViewModel()
        {
             AvailableProgrammingModes = new ObservableCollection<String>(CommandStation.GetAvailableProgrammingModeNames());
         
            int mode = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_PROGRAMMINGMODE_KEY, AppConstants.PREFERENCES_PROGRAMMINGMODE_DEFAULT));
            SelectedProgrammingMode = CommandStation.GetProgrammingModeDescription((NMRA.DCCProgrammingModes)mode);

            if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out string errorFilename, out string errorMessage) == false)
            {
                AvailableDecSpecs = new ObservableCollection<string>(new List<string>());
                SelectedDecSpeq = "";
            }
            else
            {
                AvailableDecSpecs = new ObservableCollection<string>(DeqSpecReader.GetAvailableDecSpecs(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath));
                SelectedDecSpeq = DeqSpecReader.GetDefaultDecSpecName();
            }

            locomotiveImageSource = "badgeicon.png";
            LocomotiveDescription = "Z2X-Programmer";
            LocomotiveAddress = "-";
            commandStationState = AppResources.CommandStationStateNotConnected;

            CommandStation.OnStatusChanged += OnCommandStationStatusChanged;
            CommandStation.OnReachabilityChanged += OnCommandStationReachabilityChanged;

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

        /// <summary>
        /// Create a blank new decoder configuration
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task New()
        {
            try
            {

                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    if (await Application.Current.MainPage.DisplayAlert(AppResources.AlertAttention, AppResources.AlertNewFile, AppResources.YES, AppResources.NO) == false)
                    {
                        return;
                    }
                }

                DecoderConfiguration.Init();
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                DecoderSpecification.DeqSpecName = DeqSpecReader.GetDefaultDecSpecName();
                WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }
        }


        /// <summary>
        /// Connects to the command station
        /// </summary>
        [RelayCommand]
        async Task ConnectCommandStation()
        {
            try
            {
                CommandStation.Connect();
            }
            catch (System.FormatException)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
                }
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }
        }

        /// <summary>
        /// A FilePicker dialog opens and the user can select a Z2X file. The selected Z2X file is then loaded.
        /// </summary>
        [RelayCommand]
        async Task OpenZ2XFile()
        {
            PickOptions options;

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                options = new()
                {
                    PickerTitle = AppResources.FilePickerOpenZ2XFileTitle,
                };

            }
            else
            {
                //  Define the custom file type "Z2X". The configuration is platform specific
                var customFileType = new FilePickerFileType(
                            new Dictionary<DevicePlatform, IEnumerable<string>>
                            {
                            { DevicePlatform.WinUI, new[] { ".z2x" } },
                            { DevicePlatform.Android, new[] { "application/z2x" } },
                            });

                options = new()
                {
                    PickerTitle = AppResources.FilePickerOpenZ2XFileTitle,
                    FileTypes = customFileType
                };
            }

            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("z2x", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        using var stream = await result.OpenReadAsync();
                        Z2XReaderWriter.ReadFile(stream);

                        DecoderConfiguration.IsValid = true;

                        WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                        WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
                    }
                    else
                    {
                        if ((Application.Current != null) && (Application.Current.MainPage != null))
                        {
                            await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertNoZ2XFileType, AppResources.OK);
                        }
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                //  We do not have the permission to read the Z2X file. The user needs to grant permission to Z2X-Programmer.
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertNoReadAccessToFilesAndFolders, AppResources.OK);
                }
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertZ2XFileNotRead + ex.Message, AppResources.OK);
                }
            }

        }

        [RelayCommand]
        async Task SaveZ2XFile()
        {
            CancellationToken cancelToken = new CancellationToken();
            XmlSerializer x = new XmlSerializer(typeof(Z2XProgrammerFileType));
            using MemoryStream outputStream = new MemoryStream();

            try
            {
                x.Serialize(outputStream, Z2XReaderWriter.CreateZ2XProgrammerFile());
                var fileSaveResult = await FileSaver.Default.SaveAsync(Z2XReaderWriter.GetZ2XStandardFileName(), outputStream, cancelToken);
                if (fileSaveResult.IsSuccessful == false)
                {
                    if ((Application.Current != null) && (Application.Current.MainPage != null))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertZ2XFileNotSaved, AppResources.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertZ2XFileNotSaved + " (Exception message: " + ex.Message + ").", AppResources.OK);
                }

            }
        }


        /// <summary>
        /// This commando uploads the configuration from the locomitive decoder into the data store
        /// </summary>
        [RelayCommand]
        async Task UploadDecoder()
        {

            Logger.PrintDevConsole("AppShellViewModel: Enter UploadDecoder)");

            //  Setup the cancellation token
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;

            //  Setup the popup window
            PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, AppResources.PopUpMessageUploadDecoder);

            try
            {

                //  Check the locomotive address
                if ((DecoderConfiguration.RCN225.LocomotiveAddress == 0) && (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack))
                {
                    if ((Application.Current != null) && (Application.Current.MainPage != null))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertLocomotiveAddressNotZero, AppResources.OK);
                    }
                    return;
                }


                Shell.Current.CurrentPage.ShowPopup(pop);

                var progress = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessage(value));
                });

                bool success = await Task.Run(() => ReadWriteDecoder.UploadDecoderData(cancelToken, DecoderConfiguration.RCN225.LocomotiveAddress, DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode, progress));
                await pop.CloseAsync();

                //  Display an error message and return if the upload has failed
                if (success == false)
                {
                    if ((Application.Current != null) && (Application.Current.MainPage != null))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertDecoderUploadError, AppResources.OK);
                    }
                    return;
                }
                else
                {
                    if ((Application.Current != null) && (Application.Current.MainPage != null))
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.AlertInformation, AppResources.AlertDecoderUploadSuccess, AppResources.OK);
                    }
                }

                DecoderConfiguration.IsValid = true;
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
            }
            catch (FormatException)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
                }
                DecoderConfiguration.IsValid = false;
                if (pop != null) await pop.CloseAsync();

            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
                DecoderConfiguration.IsValid = false;
                if (pop != null) await pop.CloseAsync();
            }
        }

        /// <summary>
        /// Downloads the modified settings to the decoder.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task DownloadDecoder()
        {
            try
            {

                if (Application.Current == null) return;
                if (Application.Current.MainPage == null) return;

                List<int> ModifiedConfigVariables = ReadWriteDecoder.GetModifiedConfigurationVariables(DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode);
                if (ModifiedConfigVariables.Count > 0)

                {
                    string ModifiedCVValues = String.Empty;
                    for (int i = 0; i <= ModifiedConfigVariables.Count - 1; i++)
                    {
                        ModifiedCVValues += "CV" + ModifiedConfigVariables[i].ToString() + ", ";
                    }
                    ModifiedCVValues = ModifiedCVValues.Remove(ModifiedCVValues.Length - 2, 2);

                    if (await Application.Current.MainPage.DisplayAlert(AppResources.AlertAttention, AppResources.DownloadNewSettingsYesNo + ModifiedCVValues, AppResources.YES, AppResources.NO) == false)
                    {
                        return;
                    }
                }
                else
                {
                    if (await Application.Current.MainPage.DisplayAlert(AppResources.AlertAttention, AppResources.DownloadNewSettingsYesNoSimple, AppResources.YES, AppResources.NO) == false)
                    {
                        return;
                    }
                }

                //  Check the locomotive address
                if (DecoderConfiguration.RCN225.LocomotiveAddress == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertLocomotiveAddressNotZero, AppResources.OK);
                    return;
                }

                //  Setup the cancellation token
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, AppResources.PopUpMessageDownloadDecoder);

                Shell.Current.CurrentPage.ShowPopup(pop);

                var progress = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessage(value));
                });

                bool success = await Task.Run(() => ReadWriteDecoder.DownloadDecoderData(cancelToken, DecoderConfiguration.RCN225.LocomotiveAddress, DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode, progress));

                await pop.CloseAsync();

                if (success == true)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertInformation, AppResources.AlertDecoderDonwloadSuccessfull, AppResources.OK);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertDecoderDownloadError, AppResources.OK);

                }
            }
            catch (FormatException)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
                }
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }

        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Starts the blinking of the command station status label.
        /// </summary>
        private async void StartBlinkingCommandStateLabel()
        {
            if (_commandStationStatusBlinking == true) return;

            _commandStationStatusBlinking = true;
            while (_commandStationStatusBlinking)
            {
                await Task.Delay(100);
                CommandStationConnectionStateColor = Color.FromRgb(255, 255, 255);
                await Task.Delay(100);
                CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206);
            }
        }


        /// <summary>
        /// Stops the blinking of the command station status label.
        /// </summary>
        private void StopBlinkingCommandStateLabel()
        {
            _commandStationStatusBlinking = false;
        }

        /// <summary>
        /// The event OnCommandStationStatusChanged is raised when the command station switch it status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandStationStatusChanged(object? sender, StateEventArgs e)
        {
            
            switch (e.TrackPower)
            {
                case Z21Lib.Enums.TrackPower.Short:
                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.Short");
                    CommandStationState = AppResources.CommandStationStateShortCircuitMode;
                    CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206);   // BLUE
                    StopBlinkingCommandStateLabel();
                    break;

                case Z21Lib.Enums.TrackPower.ON:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.ON");
                    CommandStationState = AppResources.CommandStationStateNormalMode;
                    CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206);   // BLUE
                    StopBlinkingCommandStateLabel();
                    break;

                case Z21Lib.Enums.TrackPower.OFF:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.OFF");
                    CommandStationState = AppResources.CommandStationStateStopMode;
                    break;

                case Z21Lib.Enums.TrackPower.Programing:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.Programing");
                    CommandStationState = AppResources.CommandStationStateProgrammingMode;
                    CommandStationConnectionStateColor = Color.FromRgb(27, 135, 85); // GREEN
                    StopBlinkingCommandStateLabel();
                    break;
            }
            
        }

        /// <summary>
        /// The event OnCommandStationReachabilityChanged is raised when the reachability by a ping is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandStationReachabilityChanged(object? sender, bool e)
        {
            _commandStationReachable = e;
            if (e == false)
            {
                CommandStationState = AppResources.CommandStationStateNotConnected;
            }
        }


        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            SelectedDecSpeq = DecoderSpecification.DeqSpecName;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            if ((DecoderConfiguration.UserDefindedImage != null) && (DecoderConfiguration.UserDefindedImage != ""))
            {
                LocomotiveImageSource = Base64StringToImage.ConvertBase64String2ImageSource(DecoderConfiguration.UserDefindedImage);
            }
            else
            {
                LocomotiveImageSource = "badgeicon.png";
            }

            if (DataStoreDataValid == true)
            {
                LocomotiveDescription = DecoderConfiguration.UserDefindedDecoderDescription;
                LocomotiveAddress = DecoderConfiguration.RCN225.LocomotiveAddress.ToString();
            }
            else
            {
                LocomotiveDescription = "Z2X-Programmer";
                LocomotiveAddress = "-";
            }

        }





        #endregion

    }
}
