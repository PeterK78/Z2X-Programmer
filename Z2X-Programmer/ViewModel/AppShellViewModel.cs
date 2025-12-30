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
using Colors = Z2XProgrammer.Helper.Colors;
using System.ComponentModel;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.Shapes;
using CommunityToolkit.Maui;


namespace Z2XProgrammer.ViewModel
{
    /// <summary>
    /// ViewModel for the main application shell.
    /// </summary>
    public partial class AppShellViewModel : ObservableObject
    {

        #region REGION: PRIVATE FIELDS

        //  Stores the current connection state of the digital command station.
        private bool _commandStationReachable = false;

        //  Stores the current state of the status LED.
        private bool _commandStationStatusBlinking = false;

        //  Stores the current state of the digital command station.
        private Z21Lib.Enums.TrackPower _commandStationOperationMode = Z21Lib.Enums.TrackPower.Unknown;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool undoAvailable = false;
        partial void OnUndoAvailableChanged(bool value)
        {
            ApplicationTitle = GUI.GetWindowTitle(System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath), UndoAvailable);
        }

        [ObservableProperty]
        bool redoAvailable = false;

        [ObservableProperty]
        bool connectingOngoing = false;

        [ObservableProperty]
        bool activityReadWriteCVOngoing = false;

        [ObservableProperty]
        string applicationTitle = "Z2X-Programmer";

        [ObservableProperty]
        Color commandStationConnectionStateColor = Colors.GetColor("ButtonText_Light", "ButtonText_Dark");

        [ObservableProperty]
        string commandStationState;

        [ObservableProperty]
        internal ImageSource locomotiveImageSource;

        [ObservableProperty]
        internal string locomotiveAddress = "";

        [ObservableProperty]
        internal string locomotiveDescription = "";


        [ObservableProperty]
        internal ObservableCollection<string>? availableDecSpecs;

        [ObservableProperty]
        internal string selectedDecSpeq = string.Empty;
        partial void OnSelectedDecSpeqChanged(string? oldValue, string newValue)
        {
            if (oldValue != newValue) SwitchDecoderSpecification(newValue);
        }

        [ObservableProperty]
        internal string selectedDecSpecNotes = string.Empty;

        [ObservableProperty]
        private ObservableCollection<String>? availableProgrammingModes;

        [ObservableProperty]
        internal string selectedProgrammingMode;
        partial void OnSelectedProgrammingModeChanged(string? oldValue, string newValue)
        {
            DecoderConfiguration.ProgrammingMode = CommandStation.GetProgrammingModeFromDescription(newValue);
            int mode = (int)DecoderConfiguration.ProgrammingMode;
            Preferences.Default.Set(AppConstants.PREFERENCES_PROGRAMMINGMODE_KEY, mode.ToString());
            WeakReferenceMessenger.Default.Send(new ProgrammingModeUpdateMessage(true));
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


            //  We create a list with available decoder speficication files.
            string errorFilename = "";
            string errorMessage = "";
            AvailableDecSpecs = new ObservableCollection<string>(new List<string>());
            SelectedDecSpeq = "";

            //  At first we add the internal decoder specification files.
            if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out errorFilename, out errorMessage) == true)
            {
                foreach (string item in DeqSpecReader.GetAvailableDecSpecs(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath)) AvailableDecSpecs.Add(item);
                SelectedDecSpeq = DeqSpecReader.GetDefaultDecSpecName();
            }
            //  Now we add the user specific decoder specification files.
            if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out errorFilename, out errorMessage) == true)
            {
                foreach (string item in DeqSpecReader.GetAvailableDecSpecs(FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath)) AvailableDecSpecs.Add(item);
                SelectedDecSpeq = DeqSpecReader.GetDefaultDecSpecName();
            }


            SelectedDecSpecNotes = DeqSpecReader.GetDecSpecNotes(SelectedDecSpeq, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT));

            LocomotiveImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_locomotive_list_64_dark.png" : "ic_fluent_locomotive_list_64_regular.png";
            LocomotiveDescription = AppResources.LocoListTitle;
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

            // We register to receive the DecoderSpecificationUpdatedMessage message.
            WeakReferenceMessenger.Default.Register<DecoderSpecificationUpdatedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDecoderSpecification(m.Value);
                });
            });

            WeakReferenceMessenger.Default.Register<LocoSelectedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnLocoSelectedMessage(m.Value);
                });
            });

            WeakReferenceMessenger.Default.Register<SomethingChangedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnSomethingChangedMessage();
                });
            });

            //  Setup the UndoRedo manager. 
            UndoRedoManager.Reset();
            UndoRedoManager.PropertyChanged += OnUndoRedoManagerPropertyChanged;

        }

        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Opens a popup to identify the used decoder.
        /// </summary>
        [RelayCommand]
        static async Task IdentifyDecoder()
        {
            Shell? currentShellOfWindow0 = App.Current!.Windows[0].Page as Shell;
            if (currentShellOfWindow0 == null) throw new InvalidOperationException("The shell of main window 0 cannot be determined.");

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;
            PopUpDecoderIdentify popUpDecoderIdent = new PopUpDecoderIdentify(DecoderConfiguration.RCN225.VehicleAddress);
            IPopupResult<bool> response = (IPopupResult<bool>)await currentShellOfWindow0.ShowPopupAsync(popUpDecoderIdent, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(12)
                }
            });

        }

        /// <summary>
        /// Closes the controller window.
        /// </summary>
        [RelayCommand]
        void CloseControllerWindow()
        {
            if (GUI.ControllerWindow != null)
            {
                Application.Current?.CloseWindow(GUI.ControllerWindow);
            }
        }

        /// <summary>
        /// Displays the locomotive list. Depending on the user settings, either the list from the Traincontroller
        /// software or the list from the file system is displayed.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task GetLocomotiveList()
        {
            try
            {
                List<LocoListType> locoList = new List<LocoListType>();

                // The shell is required to open a pop-up. Since we are in a multi-window environment,
                // we must determine the shell of the first window.
                Shell? currentShellOfWindow0 = App.Current!.Windows[0].Page as Shell;
                if (currentShellOfWindow0 == null) throw new InvalidOperationException("The shell of main window 0 cannot be determined.");

                //  Check if the Z2X files folder is available.
                if (LocoList.Z2XFileFolder == "")
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderEmpty, AppResources.OK);
                    return;
                }

                if (Directory.Exists(LocoList.Z2XFileFolder) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderNotExist + LocoList.Z2XFileFolder, AppResources.OK);
                    return;
                }

                ActivityReadWriteCVOngoing = true;

                locoList = await Task.Run(() => LocoList.GetLocomotiveList());

                ActivityReadWriteCVOngoing = false;

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;
                PopUpLocoList pop = new PopUpLocoList(cancelTokenSource, locoList);

                // Workaround:
                // The .NET MAUI pop-ups currently have problems with multi-window applications. For this reason,
                // it is important that we bring the main window to the foreground before closing a pop-up. Otherwise the
                // the current shell can not be found by ClosePopupAsync.
                App.Current.ActivateWindow(App.Current.Windows[0]);

                IPopupResult<bool> response = (IPopupResult<bool>)await currentShellOfWindow0.ShowPopupAsync(pop, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });

                if (response.Result == true) await Shell.Current.GoToAsync("//AddressPage");
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListNotReachable + ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Create a blank new decoder configuration project.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task New()
        {
            try
            {
                //  We inform the user whether they would like to create a new project.
                //  If the user declines, we terminate the function.
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.AlertNewFile, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }

                //  We initialize the decoder configuration.
                //  We then inform the application that this has changed.
                DecoderConfiguration.Init(NMRA.StandardShortVehicleAddress, "");
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                //  Finally, we set the DecoderConfiguration to our standard DecoderConfiguration.
                //  We then inform the application that this has changed.
                DecoderSpecification.DeqSpecName = DeqSpecReader.GetDefaultDecSpecName();
                DecoderConfiguration.SetDecoderSpecification(DecoderSpecification.DeqSpecName);
                DecoderConfiguration.EnableAllCVsSupportedByDecSpec(DecoderSpecification.DeqSpecName);
                DecoderConfiguration.Z2XFilePath = "";
                WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));

                //  We set the application title to the name of the Z2X file.
                ApplicationTitle = GUI.GetWindowTitle("", false);

                UndoRedoManager.Reset();

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// This command is executed when the operating status button is pressed.
        /// If the digital command station cannot be reached, a new connection is established.
        /// </summary>
        [RelayCommand]
        async Task ConnectCommandStation()
        {
            try
            {
                //  If the digital command center is already reachable, we return.
                if (CommandStation.IsReachable == false)
                {
                    // This property controls the ActivityIndicator
                    // We set this property to True so that it is displayed.
                    ConnectingOngoing = true;

                    //  Setup the cancellation token
                    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                    CancellationToken cancelToken = cancelTokenSource.Token;

                    //  We are now trying to establish a new connection to the digital command station.                    
                    bool ConnectionSuccess = false;
                    await Task.Run(() => ConnectionSuccess = CommandStation.Connect(cancelToken, 5000));

                    //  We hide the ActivityIndicator.
                    ConnectingOngoing = false;

                    // We will inform the user if we are unable to establish a connection.
                    if (ConnectionSuccess == false) await MessageBox.Show(AppResources.AlertError, AppResources.AlertDigitalCommandStationNoReachablePart1 + " " + Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT) + " " + AppResources.AlertDigitalCommandStationNoReachablePart2, AppResources.OK);
                }
                else
                {
                    //  The command station is already connected. Therefore we just toggle the track power.
                    if (_commandStationOperationMode == Z21Lib.Enums.TrackPower.OFF)
                    {
                        CommandStation.Z21.SetTrackPowerOn();
                    }
                    else
                    {
                        CommandStation.Z21.SetTrackPowerOff();
                    }
                }
            }
            catch (System.FormatException)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Undos the last configuration variable change.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Undo()
        {
            await Task.Run(() => UndoRedoManager.UndoLastCVChange());
            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
        }

        /// <summary>
        /// Redos the undo.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Redo()
        {
            await Task.Run(() => UndoRedoManager.RedoLastUndo());
            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
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

                        //  We set property BackupDataFromDecoderIsValid to FALSE to signal that the backup data was loaded from a Z2X file.    
                        DecoderConfiguration.BackupDataFromDecoderIsValid = false;

                        //  We set the path to the Z2X file.
                        DecoderConfiguration.Z2XFilePath = result.FullPath;

                        //  We set the application title to the name of the Z2X file.
                        ApplicationTitle = GUI.GetWindowTitle(System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath), false);

                        WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                        WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(false));

                        UndoRedoManager.Reset();

                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoZ2XFileType, AppResources.OK);
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                //  We do not have the permission to read the Z2X file. The user needs to grant permission to Z2X-Programmer.
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoReadAccessToFilesAndFolders, AppResources.OK);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertZ2XFileNotRead + ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Saves the data to the current Z2X file.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SaveZ2XFile()
        {
            try
            {
                //  If we do not have a Z2X file path, we open the SaveAsZ2XFile dialog.
                if (DecoderConfiguration.Z2XFilePath == "")
                {
                    await SaveAsZ2XFile();
                    return;
                }

                XmlSerializer x = new XmlSerializer(typeof(Z2XProgrammerFileType));
                try
                {
                    if (File.Exists(DecoderConfiguration.Z2XFilePath) == true) File.Delete(DecoderConfiguration.Z2XFilePath);
                    using FileStream outputStream = System.IO.File.OpenWrite(DecoderConfiguration.Z2XFilePath);
                    using StreamWriter streamWriter = new StreamWriter(outputStream);
                    x.Serialize(streamWriter, Z2XReaderWriter.CreateZ2XProgrammerFile());
                    streamWriter.Flush();
                    streamWriter.Close();

                    //  We set the application title to the name of the Z2X file - and remove the asterisk.
                    ApplicationTitle = GUI.GetWindowTitle(System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath), false);

                }

                catch (Exception ex)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertZ2XFileNotSaved + " (Exception message: " + ex.Message + ").", AppResources.OK);
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Opens a FilePicker dialog and saves the data to a Z2X file.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SaveAsZ2XFile()
        {
            if (DataStoreDataValid == false) return;

            CancellationToken cancelToken = new CancellationToken();
            XmlSerializer x = new XmlSerializer(typeof(Z2XProgrammerFileType));
            using MemoryStream outputStream = new MemoryStream();

            try
            {
                x.Serialize(outputStream, Z2XReaderWriter.CreateZ2XProgrammerFile());
                var fileSaveResult = await FileSaver.Default.SaveAsync(Z2XReaderWriter.GetZ2XStandardFileName(), outputStream, cancelToken);
                if (fileSaveResult.IsSuccessful == true)
                {
                    if (fileSaveResult.FilePath != null)
                    {
                        //  Update the new Z2X project file path.
                        DecoderConfiguration.Z2XFilePath = fileSaveResult.FilePath;

                        //  We set the application title to the name of the Z2X file.
                        ApplicationTitle = GUI.GetWindowTitle(System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath), false);

                        return;
                    }
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertZ2XFileNotSaved, AppResources.OK);
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertZ2XFileNotSaved + " (Exception message: " + ex.Message + ").", AppResources.OK);
            }
        }

        /// <summary>
        /// Opens the extendend toolbar menu.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task OpenExtendedMenuItemPopup()
        {
            try
            {

                //  Create a collection of menu items.
                List<string> menuItems = new List<string>();
                menuItems.Add(AppResources.ExtendedMenuItemSaveAs);
                menuItems.Add(AppResources.ExtendedMenuItemRedo);
                menuItems.Add(AppResources.ExtendedMenuItemReportProblem);

                //  Present the the action sheet to the user.
                if (Application.Current == null) return;
                string action = await Application.Current.Windows[0].Page!.DisplayActionSheetAsync(AppResources.ExtendedMenuItemTitle, null, null, menuItems.ToArray());

                //  Execute the selected action.
                if (action == AppResources.ExtendedMenuItemSaveAs)
                {
                    if (DataStoreDataValid == true)
                    {
                        await SaveAsZ2XFile();
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoDataToSave, AppResources.OK);
                    }
                }
                else if (action == AppResources.ExtendedMenuItemRedo)
                {
                    if (RedoAvailable == true)
                    {
                        await Redo();
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoRedoAvailable, AppResources.OK);
                    }
                }
                else if (action == AppResources.ExtendedMenuItemReportProblem)
                {
                    await Browser.OpenAsync("https://github.com/PeterK78/Z2X-Programmer/issues");
                }


            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// This commando uploads the configuration from the decoder.
        /// </summary>
        [RelayCommand]
        async Task UploadDecoder()
        {

            Logger.PrintDevConsole("AppShellViewModel: Enter UploadDecoder)");

            int CurrentlyUploadedCV = 0;

            // The shell is required to open a pop-up. Since we are in a multi-window environment,
            // we must determine the shell of the first window.
            Shell? currentShellOfWindow0 = App.Current!.Windows[0].Page as Shell;
            if (currentShellOfWindow0 == null) throw new InvalidOperationException("The shell of  main window 0 cannot be determined.");

            //  Setup the cancellation token.
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;

            //  Setup the popup indicator remark. We will show an info, if one or more configuration variables 
            // supported by the selected decoder specification are currently be disabled.
            string note = string.Empty;
            if (DecoderConfiguration.AllSupportedCVsEnabled() == false) { note = AppResources.AlertSomeCVsAreDisabledUpload; }


            //  Setup the popup window.
            PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, AppResources.PopUpMessageUploadDecoder, note, DecoderConfiguration.ProgrammingMode);

            try
            {

                //  Check the locomotive address
                if ((DecoderConfiguration.RCN225.VehicleAddress == 0) && (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack))
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocomotiveAddressNotZero, AppResources.OK);
                }

                //  Before we start uploading the data from the decoder, we check whether we can establish a connection to the digital command center.
                //  If the digital command center is actually not reachable, we try to create a connection.
                if (CommandStation.Z21.IsReachable == false)
                {
                    PopUpConnectCommandStation popupConnectCommandSation = new PopUpConnectCommandStation(cancelTokenSource, AppResources.InfoConnectionToDigitalCommandStation + Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT) + ").");
                    currentShellOfWindow0.ShowPopup(popupConnectCommandSation, new PopupOptions
                    {
                        Shape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(12)
                        }
                    });

                    bool ConnectSuccess = await Task.Run(() => CommandStation.Connect(cancelToken, 10000));
                    await popupConnectCommandSation.CloseAsync();
                    if (ConnectSuccess == false)
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertDigitalCommandStationNoReachablePart1 + " " + Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT) + " " + AppResources.AlertDigitalCommandStationNoReachablePart2, AppResources.OK);
                        return;
                    }
                }

                currentShellOfWindow0.ShowPopup(pop, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });

                // Initializes the ProgressPercentage Progress object with the specified callback.
                // This means that percentage changes can be received and forwarded during the upload. 
                var ProgressPercentage = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessagePercentage(value));
                });

                // Initializes the ProgressCV Progress object with the specified callback.
                // This means that the currently processed configuration variable can be received and forwarded during the upload. 
                var ProgressCV = new Progress<int>(value =>
                {
                    CurrentlyUploadedCV = value;
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessageCV(value));
                });

                bool success = await Task.Run(() => ReadWriteDecoder.UploadDecoderData(cancelToken, DecoderConfiguration.RCN225.VehicleAddress, DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode, ProgressPercentage, ProgressCV));

                // Workaround:
                // The .NET MAUI pop-ups currently have problems with multi-window applications. For this reason,
                // it is important that we bring the main window to the foreground before closing a pop-up. Otherwise the
                // the current shell can not be found by ClosePopupAsync.
                App.Current.ActivateWindow(App.Current.Windows[0]);
                await currentShellOfWindow0.ClosePopupAsync(pop);

                //  Display an error message and return if the upload has failed.
                if (success == false)
                {
                    //  Depending on the programming mode we display an error message.
                    if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.POMMainTrack)
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderUploadErrorPart1 + " CV" + CurrentlyUploadedCV.ToString() + ".\n\n" + AppResources.AlertDecoderUploadErrorPart2POM, AppResources.OK);
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderUploadErrorPart1 + " CV" + CurrentlyUploadedCV.ToString() + ".\n\n" + AppResources.AlertDecoderUploadErrorPart2Direct, AppResources.OK);
                    }
                    return;
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecoderUploadSuccess, AppResources.OK);
                }

                // We set property BackupDataFromDecoderIsValid to TRUE to signal that the backup data was loaded directly from the decoder.
                DecoderConfiguration.BackupDataFromDecoderIsValid = true;

                DecoderConfiguration.IsValid = true;

                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                UndoRedoManager.Reset();
            }
            catch (FormatException)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
                DecoderConfiguration.IsValid = false;
                if (pop != null) await pop.CloseAsync();

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
                DecoderConfiguration.IsValid = false;
                if (pop != null) await pop.CloseAsync();
            }
        }

        /// <summary>
        /// Downloads the modified settings to the decoder.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task DownloadDiffDecoder()
        {
            try
            {
                if (Application.Current == null) return;

                //  We create a list of configuration values for which the current value is different from the backup value.
                string ModifiedCVValues = String.Empty;
                List<int> ModifiedConfigVariables = ReadWriteDecoder.GetModifiedConfigurationVariables(DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode);

                //  We do not write the configuration variables CV1, CV17 and CV18. These configuration variables are for configuring the vehicle address.
                ModifiedConfigVariables.Remove(1);
                ModifiedConfigVariables.Remove(17);
                ModifiedConfigVariables.Remove(18);

                if (ModifiedConfigVariables.Count == 0)
                {
                    //  No modified values were found.
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoModifiedValuesFound, AppResources.OK);
                    return;
                }


                //  Check if we have valid locomotive address.
                if (DecoderConfiguration.RCN225.VehicleAddress == 0)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocomotiveAddressNotZero, AppResources.OK);
                    return;
                }

                ////  We show the user which variables are changed. We then ask whether they want to download these values
                ////  - if so, we start the download. Otherwise we return.
                //CommunityToolkit.Maui.Sample.Views.Popups.ReturnResultPopup x = new CommunityToolkit.Maui.Sample.Views.Popups.ReturnResultPopup(ModifiedConfigVariables, AppResources.DownloadNewSettingsYesNo,  AppResources.DownloadDataTitle, "ic_fluent_arrow_download_diff_24_regular.png", DataStore.DecoderConfiguration.BackupDataFromDecoderIsValid);;
                //IPopupResult<bool> pipResult = await Shell.Current.CurrentPage.ShowPopupAsync<bool>(x); 


                PopUpDownloadData popupDownloadData = new PopUpDownloadData(ModifiedConfigVariables, AppResources.DownloadNewSettingsYesNo, AppResources.DownloadDataTitle, "ic_fluent_arrow_download_diff_24_regular.png", DataStore.DecoderConfiguration.BackupDataFromDecoderIsValid);
                IPopupResult<string> popUpResult = await Shell.Current.CurrentPage.ShowPopupAsync<string>(popupDownloadData, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });
                if ((popUpResult != null) && (popUpResult.Result != "OK")) return;

                //  Setup the cancellation token.
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                //  Setup the popup indicator remark. We will show an info, if one or more configuration variables 
                // supported by the selected decoder specification are currently be disabled.
                string note = string.Empty;
                if (DecoderConfiguration.AllSupportedCVsEnabled() == false) { note = AppResources.AlertSomeCVsAreDisabledUpload; }

                PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, AppResources.PopUpMessageDownloadDecoder, note, DecoderConfiguration.ProgrammingMode);

                Shell.Current.CurrentPage.ShowPopup(pop, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });

                var ProgressPercentage = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessagePercentage(value));
                });

                var ProgressCV = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessageCV(value));
                });

                // Check whether programming processes need to be verified via POM.
                bool verifyPOM = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_VERIFYPOMWRITE_KEY, AppConstants.PREFERENCES_VERIFYPOMWRITE_VALUE)) == 1 ? true : false;

                bool success = await Task.Run(() => ReadWriteDecoder.DownloadDecoderData(cancelToken, DecoderConfiguration.RCN225.VehicleAddress, DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode, ProgressPercentage, false, ProgressCV, ModifiedConfigVariables, verifyPOM));

                await pop.CloseAsync();

                if (success == true)
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecoderDonwloadSuccessfull, AppResources.OK);
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderDownloadError, AppResources.OK);

                }
            }
            catch (FormatException)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
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

                //  We create a list with configuration variables which can be safely written by the given decoder specification.
                string ModifiedCVValues = String.Empty;
                List<int> ListOfWritableConfigVariables = ReadWriteDecoder.GetAllWritableConfigurationVariables(DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode);

                //  We do not write the configuration variables CV1, CV17 and CV18. These configuration variables are for configuring the vehicle address.
                ListOfWritableConfigVariables.Remove(1);
                ListOfWritableConfigVariables.Remove(17);
                ListOfWritableConfigVariables.Remove(18);

                if (ListOfWritableConfigVariables.Count == 0)
                {
                    //  Something happed - we did not find any valid CV value.
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoModifiedValuesFound, AppResources.OK);
                    return;
                }

                //  We show the user which variables are changed. We then ask whether they want to download these values
                //  - if so, we start the download. Otherwise we return.
                PopUpDownloadData popupDownloadData = new PopUpDownloadData(ListOfWritableConfigVariables, AppResources.DownloadAllSettingsYesNo, AppResources.DownloadDataTitle, "ic_fluent_arrow_download_24_regular.png", DataStore.DecoderConfiguration.BackupDataFromDecoderIsValid);
                IPopupResult<string> popUpResult = await Shell.Current.CurrentPage.ShowPopupAsync<string>(popupDownloadData);
                if ((popUpResult != null) && (popUpResult.Result != "OK")) return;

                //  Check the locomotive address.
                if (DecoderConfiguration.RCN225.VehicleAddress == 0)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocomotiveAddressNotZero, AppResources.OK);
                    return;
                }

                //  Setup the cancellation token.
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;

                //  Setup the popup indicator remark. We will show an info, if one or more configuration variables 
                // supported by the selected decoder specification are currently be disabled.
                string note = string.Empty;
                if (DecoderConfiguration.AllSupportedCVsEnabled() == false) { note = AppResources.AlertSomeCVsAreDisabledUpload; }
                ;

                PopUpActivityIndicator pop = new PopUpActivityIndicator(cancelTokenSource, AppResources.PopUpMessageDownloadDecoder, note, DecoderConfiguration.ProgrammingMode);

                Shell.Current.CurrentPage.ShowPopup(pop);

                var ProgressPercentage = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessagePercentage(value));
                });

                var ProgressCV = new Progress<int>(value =>
                {
                    WeakReferenceMessenger.Default.Send(new ProgressUpdateMessageCV(value));
                });

                // Check whether programming processes need to be verified via POM.
                bool verifyPOM = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_VERIFYPOMWRITE_KEY, AppConstants.PREFERENCES_VERIFYPOMWRITE_VALUE)) == 1 ? true : false;

                bool success = await Task.Run(() => ReadWriteDecoder.DownloadDecoderData(cancelToken, DecoderConfiguration.RCN225.VehicleAddress, DecoderSpecification.DeqSpecName, DecoderConfiguration.ProgrammingMode, ProgressPercentage, true, ProgressCV, ListOfWritableConfigVariables, verifyPOM));

                await pop.CloseAsync();

                if (success == true)
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecoderDonwloadSuccessfull, AppResources.OK);
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDecoderDownloadError, AppResources.OK);

                }
            }
            catch (FormatException)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Receives the UndoRedoManager property changes and updates the Undo and Redo buttons.
        /// </summary>
        private void OnUndoRedoManagerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UndoAvailable = UndoRedoManager.UndoAvailable;
            RedoAvailable = UndoRedoManager.RedoAvailable;
        }


        /// <summary>
        /// Activates the selected decoder specification.
        /// </summary>
        /// <param name="decSpecName">The name of the decoder specification.</param>
        private void SwitchDecoderSpecification(string decSpecName)
        {
            //  First, the new decoder specification is set. Afterwards we read the the notes from the decoder specification.
            DecoderSpecification.DeqSpecName = decSpecName;
            SelectedDecSpecNotes = DeqSpecReader.GetDecSpecNotes(decSpecName, FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT));

            //  In the second step, we configure the configuration variables.
            //  First we mark all CVs to see if they are supported by the selected decoder
            //  specification, then we activate all supported CVs.
            DecoderConfiguration.SetDecoderSpecification(SelectedDecSpeq);

            //  We enable configuration variables of the selected decoder specification.                
            DecoderConfiguration.EnableAllCVsSupportedByDecSpec(SelectedDecSpeq);

            //  Inform the application that a new decoder specification has been selected.
            WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));

        }

        /// <summary>
        /// Starts the blue blinking of the command station status label.
        /// </summary>
        private async void StartBlinkingCommandStateLabel()
        {
            if (_commandStationStatusBlinking == true) return;

            _commandStationStatusBlinking = true;
            while (_commandStationStatusBlinking)
            {
                await Task.Delay(100);
                CommandStationConnectionStateColor = Colors.GetColor("ButtonText_Light", "ButtonText_Dark");
                await Task.Delay(100);
                CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206); // BLUE
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
                    _commandStationOperationMode = Z21Lib.Enums.TrackPower.Short;
                    break;

                case Z21Lib.Enums.TrackPower.ON:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.ON");
                    CommandStationState = AppResources.CommandStationStateNormalMode;
                    CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206);   // BLUE
                    StopBlinkingCommandStateLabel();
                    _commandStationOperationMode = Z21Lib.Enums.TrackPower.ON;
                    break;

                case Z21Lib.Enums.TrackPower.OFF:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.OFF");
                    CommandStationState = AppResources.CommandStationStateStopMode;
                    CommandStationConnectionStateColor = Color.FromRgb(33, 130, 206);   // BLUE
                    StartBlinkingCommandStateLabel();
                    _commandStationOperationMode = Z21Lib.Enums.TrackPower.OFF;
                    break;

                case Z21Lib.Enums.TrackPower.Programing:

                    Logger.PrintDevConsole("AppShellViewModel:OnCommandStationStatusChanged - TrackPower.Programing");
                    CommandStationState = AppResources.CommandStationStateProgrammingMode;
                    CommandStationConnectionStateColor = Color.FromRgb(27, 135, 85); // GREEN
                    StopBlinkingCommandStateLabel();
                    _commandStationOperationMode = Z21Lib.Enums.TrackPower.Programing;
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
        private void OnGetDataFromDecoderSpecification(bool updateGUI)
        {
            if (updateGUI)
            {
                if (SelectedDecSpeq != DecoderSpecification.DeqSpecName) SelectedDecSpeq = DecoderSpecification.DeqSpecName;
            }
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // We update the locomotive image. If we do not find a valid locomotive image, we use the default image ( => locomotive list icon).
            if ((DecoderConfiguration.UserDefindedImage != null) && (DecoderConfiguration.UserDefindedImage != ""))
            {
                LocomotiveImageSource = Base64StringToImage.ConvertBase64String2ImageSource(DecoderConfiguration.UserDefindedImage);
            }
            else
            {
                LocomotiveImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_locomotive_list_64_dark.png" : "ic_fluent_locomotive_list_64_regular.png";
            }

            if (DataStoreDataValid == true)
            {
                LocomotiveDescription = DecoderConfiguration.UserDefindedDecoderDescription;
                LocomotiveAddress = DecoderConfiguration.RCN225.VehicleAddress.ToString();
            }
            else
            {
                LocomotiveDescription = AppResources.LocoListTitle;
                LocomotiveAddress = DecoderConfiguration.RCN225.VehicleAddress.ToString();
            }

        }

        /// <summary>
        ///  The OnSomethingChangedMessage message handler is called when the SomethingChangedMessage
        ///  message has been received.
        /// </summary>        
        internal void OnSomethingChangedMessage()
        {
            ApplicationTitle = GUI.GetWindowTitle(System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath), true);
        }

        /// <summary>
        /// The OnLocoSelectedMessage message handler is called when the LocoSelectedMessage
        /// message has been received. This message is typically sent by the PopUp PopUpLocoList
        /// when the OK button is pressed.
        /// </summary>
        /// <param name="locomotive">Contains the data of the selected locomotive.</param>
        internal void OnLocoSelectedMessage(LocoListType locomotive)
        {
            try
            {
                // We check if we have received a valid Z2X file path. If not, we try to find a matching Z2X file in
                // the Z2X file directory.
                if (File.Exists(locomotive.FilePath) == false)
                {
                    //  Check if we have a matching Z2X file. We search the Z2X file directory.
                    //  We search until we have found the corresponding Z2X file.
                    string[] fileEntries = Directory.GetFiles(LocoList.Z2XFileFolder);
                    foreach (string fileEntry in fileEntries)
                    {
                        using (Stream fs = File.OpenRead(fileEntry))
                        {
                            Z2XProgrammerFileType myFile = new Z2XProgrammerFileType();
                            var mySerializer = new XmlSerializer(typeof(Z2XProgrammerFileType));

                            // Call the Deserialize method and cast to the object type.
                            myFile = (Z2XProgrammerFileType)mySerializer.Deserialize(fs)!;

                            if (myFile.LocomotiveAddress == locomotive.LocomotiveAddress)
                            {
                                locomotive.FilePath = fileEntry;
                                break;
                            }

                        }
                    }
                }

                //  If we have found a Z2X file, we load it. Otherwise we create a new Z2X project.
                if (File.Exists(locomotive.FilePath))
                {
                    using (Stream fs = File.OpenRead(locomotive.FilePath))
                    {
                        Z2XReaderWriter.ReadFile(fs);
                        DecoderConfiguration.IsValid = true;

                        //  We set property BackupDataFromDecoderIsValid to FALSE to signal that the backup data was loaded from a Z2X file.    
                        DecoderConfiguration.BackupDataFromDecoderIsValid = false;

                        //  We set the path to the Z2X file.
                        DecoderConfiguration.Z2XFilePath = locomotive.FilePath;

                        //  We set the application title to the name of the Z2X file.
                        ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(DecoderConfiguration.Z2XFilePath);

                        WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                        WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));

                        UndoRedoManager.Reset();
                    }
                }
                else
                {
                    DecoderConfiguration.Init(locomotive.LocomotiveAddress, locomotive.UserDefindedDecoderDescription);
                    WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                    DecoderSpecification.DeqSpecName = DeqSpecReader.GetDefaultDecSpecName();
                    WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
                }

                //Shell.Current.GoToAsync("//AddressPage");


            }
            catch (System.ObjectDisposedException)
            {
                //  Somtimes the message ProgressUpdateMessage is delayed. So it can happen,
                //  that this popup is already disposed. So this exception can be thrown.
            }
            catch (FileNotFoundException ex)
            {
                _ = MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }

        #endregion

    }
}
