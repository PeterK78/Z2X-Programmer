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

using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.Text.Json;
using Z21Lib.Events;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Popups;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    /// <summary>
    /// Represents the view model for the settings page, providing properties and commands to manage application
    /// preferences, command stations, language selection, decoder specifications, and related configuration options.
    /// </summary>
    public partial class SettingsPageViewModel : ObservableObject
    {
        #region REGION: PUBLIC PROPERTIES

        /// <summary>
        /// Gets or sets the collection of available command stations.
        /// </summary>
        [ObservableProperty]
        internal ObservableCollection<CommandStationType>? commandStations = new ObservableCollection<CommandStationType>();

        /// <summary>
        /// Gets or sets the currently selected command station.
        /// </summary>
        [ObservableProperty]
        internal CommandStationType selectedCommandStation = new CommandStationType();
        partial void OnSelectedCommandStationChanged(CommandStationType? oldValue, CommandStationType newValue)
        {
            // Check if we have a valid new value, if not return.                
            if (newValue == null) return;

            // If the IP address has changed, we must disconnect the current connection.
            if ( oldValue!.IpAddress  != newValue!.IpAddress)
            { 
                WeakReferenceMessenger.Default.Send(new CommandStationUpdateMessage(newValue));
            }   

            //  Save the selected command station IP address and name to the preferences.
            Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, newValue.IpAddress);
            Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONNAME_KEY, newValue.Name);
        }

        /// <summary>
        /// The Z21 firmware version.
        /// </summary>
        [ObservableProperty]
        internal string z21FirmwareVersion = "-";

        /// <summary>
        /// The Z21 hardwware type.
        /// </summary>
        [ObservableProperty]
        internal string z21HardwareType = "-";

        [ObservableProperty]
        internal ObservableCollection<string>? availableLocoListSystems;

        [ObservableProperty]
        internal bool activityConnectingOngoing = false;

        [ObservableProperty]
        internal string selectedLocoListSystem;
        partial void OnSelectedLocoListSystemChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERNECES_LOCOLIST_SYSTEM_KEY, value);
            LocoListSystemRocrailSelected = LocoList.IsRocrail(value);
        }

        [ObservableProperty]
        internal bool locoListSystemRocrailSelected;

        [ObservableProperty]
        internal string locoListSystemIPAddress;
        partial void OnLocoListSystemIPAddressChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_LOCOLIST_IPADDRESS_KEY, value);
        }

        [ObservableProperty]
        internal string locoListSystemPort;
        partial void OnLocoListSystemPortChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_LOCOLIST_PORTNR_KEY, value);
        }

        [ObservableProperty]
        internal string locoListSystemFolder;
        partial void OnLocoListSystemFolderChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_LOCOLIST_FOLDER_KEY, value);
        }

        [ObservableProperty]
        bool enableLogging;
        partial void OnEnableLoggingChanged(bool value)
        {
            if (value == true)
            {
                Preferences.Default.Set(AppConstants.PREFERENCES_LOGGING_KEY, "1");
            }
            else
            {
                Preferences.Default.Set(AppConstants.PREFERENCES_LOGGING_KEY, "^0");
            }
        }

        [ObservableProperty]
        internal ObservableCollection<string>? availableLanguages;

        [ObservableProperty]
        internal string selectedLanguage;
        partial void OnSelectedLanguageChanged(string value)
        {
            AppCulture.SetApplicationLanguageByDescription(value);
            Preferences.Default.Set(AppConstants.PREFERENCES_LANGUAGE_KEY, AppCulture.GetLanguageKey(value).ToUpper());
        }       

        /// <summary>
        /// Set to TRUE to verify programming operations in POM.
        /// </summary>
        [ObservableProperty]
        internal bool verifyPOMWrite = true;
        partial void OnVerifyPOMWriteChanged(bool value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_VERIFYPOMWRITE_KEY, value == true ? "1" : "0");
        }

        [ObservableProperty]
        internal bool automaticDecoderDetection;
        partial void OnAutomaticDecoderDetectionChanged(bool value)
        {
            int prefValue = 1;
            if (value == true)
            {
                prefValue = 1;
            }
            else
            {
                prefValue = 0;
            }
            Preferences.Default.Set(AppConstants.PREFERENCES_AUTODECODER_DETECT_KEY, prefValue.ToString());
        }

        //  The user specific decoder specification folder.
        [ObservableProperty]
        internal string userSpecificDecoderSpecificationFolder = "";
        partial void OnUserSpecificDecoderSpecificationFolderChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_USERSPECIFICDECSPECFOLDER_KEY, value);
        }

        [ObservableProperty]
        internal bool quitOnReadError = true;
        partial void OnQuitOnReadErrorChanged(bool value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_QUITONREADERROR_KEY, value == true ? "1" : "0");
        }

        //  Additional display of CV values
        [ObservableProperty]
        internal bool additionalDisplayOfCVValues = false;
        partial void OnAdditionalDisplayOfCVValuesChanged(bool value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, value == true ? "1" : "0");
        }

        // Measurement section sensor 1 number
        [ObservableProperty]
        internal int measurementSectionSensor1Number = 1;
        partial void OnMeasurementSectionSensor1NumberChanged(int value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_KEY, value.ToString());
        }

        // Measurement section sensor 2 number
        [ObservableProperty]
        internal int measurementSectionSensor2Number = 2;
        partial void OnMeasurementSectionSensor2NumberChanged(int value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_KEY, value.ToString());
        }

        // Measurement section length in mm
        [ObservableProperty]
        internal int measurementSectionLengthInMM = 1000;
        partial void OnMeasurementSectionLengthInMMChanged(int value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_MEASUREMENTSECTION_LENGTHMM_KEY, value.ToString());
        }

        // Measurement section scale
        [ObservableProperty]
        internal int measurementSectionScale = 160;
        partial void OnMeasurementSectionScaleChanged(int value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_MEASUREMENTSECTION_SCALE_KEY, value.ToString());
        }

        #endregion

        #region REGION: CONSTRUCTOR

        public SettingsPageViewModel()
        {
            //
            //  Setup the initial content of the different GUI elements.
            //

            //  The available command stations. 
            string jsonString = Preferences.Default.Get(AppConstants.PREFERENCES_ALLCOMMANDSTATIONS_KEY, AppConstants.PREFERENCES_ALLCOMMANDSTATIONS_DEFAULT);
            CommandStations = JsonSerializer.Deserialize<ObservableCollection<CommandStationType>>(jsonString);
            if((CommandStations != null) && (CommandStations.Count > 0))        
            { 
                SelectedCommandStation = CommandStations.First(item => item.IpAddress == Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT));
                if(SelectedCommandStation != null)
                { 
                    Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, SelectedCommandStation.IpAddress);
                    Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONNAME_KEY, SelectedCommandStation.Name);
                }
            }

            //  The user specific decoder specification folder.  
            UserSpecificDecoderSpecificationFolder = FileAndFolderManagement.ApplicationFolders.UserSpecificDecSpecsFolderPath;

            //  The loco list settings.
            LocoListSystemIPAddress = LocoList.IPAddress.ToString();
            LocoListSystemPort = LocoList.PortNumber.ToString();
            LocoListSystemFolder = LocoList.Z2XFileFolder.ToString();


            AvailableLanguages = new ObservableCollection<string>(AppCulture.GetAvailableLanguagesDescriptions());
            SelectedLanguage = AppCulture.GetLanguageDescription(Preferences.Default.Get(AppConstants.PREFERENCES_LANGUAGE_KEY, AppConstants.PREFERENCES_LANGUAGE_KEY_DEFAULT));
            AvailableLocoListSystems = new ObservableCollection<string>(LocoList.GetAvailableSystems());
            if (Preferences.Default.Get(AppConstants.PREFERNECES_LOCOLIST_SYSTEM_KEY, AppConstants.PREFERNECES_LOCOLIST_SYSTEM_VALUE) == "")
            {
                SelectedLocoListSystem = LocoList.GetSystemNotAvailable();
            }
            else
            {
                SelectedLocoListSystem = Preferences.Default.Get(AppConstants.PREFERNECES_LOCOLIST_SYSTEM_KEY, AppConstants.PREFERNECES_LOCOLIST_SYSTEM_VALUE);
            }

            if (Preferences.Default.Get(AppConstants.PREFERENCES_LOGGING_KEY, AppConstants.PREFERENCES_LOGGING_DEFAULT) == "1")
            {
                EnableLogging = true;
            }
            else
            {
                EnableLogging = false;
            }

            if (int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_AUTODECODER_DETECT_KEY, AppConstants.PREFERENCES_AUTODECODER_DETECT_DEFAULT)) == 1)
            {
                AutomaticDecoderDetection = true;
            }
            else
            {
                AutomaticDecoderDetection = false;
            }

            VerifyPOMWrite = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_VERIFYPOMWRITE_KEY, AppConstants.PREFERENCES_VERIFYPOMWRITE_VALUE)) == 1 ? true : false;

            QuitOnReadError = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_QUITONREADERROR_KEY, AppConstants.PREFERENCES_QUITONREADERROR_VALUE)) == 1 ? true : false;
            AdditionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

            // Measurement section
            MeasurementSectionSensor1Number = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_DEFAULT));
            MeasurementSectionSensor2Number = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_DEFAULT));
            MeasurementSectionLengthInMM = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_LENGTHMM_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_LENGTHMM_DEFAULT));
            MeasurementSectionScale = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SCALE_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SCALE_DEFAULT));

            CommandStation.OnHardwareInfoReceived += OnHardwareInformationReceived;
            if (CommandStation.IsReachable == true) CommandStation.RequestFirmwareVersion();

        }

        #endregion

        #region REGION: COMMANDS   

        /// <summary>
        /// Deletes the currently selected command station after confirming the action with the user.
        /// </summary>
        /// <remarks>The method displays a confirmation dialog before deleting the selected command
        /// station. The deletion is only performed if the user confirms the action.</remarks>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        [RelayCommand]
        async Task DeleteCommandStation()
        {
            try
            {
                //  Check if any command station is available.
                if (CommandStations!.Count == 0)
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoCommandStationsAvailable, AppResources.OK);
                    return;
                }

                // Ask the user if they want to delete the selected station.
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.AlertDeleteCommandStationConfirmation, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }
                CommandStations!.Remove(SelectedCommandStation);

                //  Select the first digital command station (if any station is available).
                if (CommandStations.Count > 0) SelectedCommandStation = CommandStations[0];

                // Save the all digital command stations to the preferences.
                Preferences.Default.Set(AppConstants.PREFERENCES_ALLCOMMANDSTATIONS_KEY, JsonSerializer.Serialize(CommandStations));
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Displays a popup dialog to modify the currently selected command station's details.
        /// </summary>
        /// <remarks>This method presents a modal popup for editing the name and IP address of the
        /// selected command station. The operation is asynchronous and requires the main window to be available. The
        /// popup uses rounded corners for its appearance.</remarks>
        /// <returns>A task that represents the asynchronous operation of showing the modification popup. The result indicates
        /// whether the modification was confirmed or cancelled.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the shell of the main window cannot be determined.</exception>
        [RelayCommand]
        async Task ModifyCommandStation()
        {
            try
            {
                // Get the shell of the main application window.
                Shell? currentShellOfWindow0 = App.Current!.Windows[0].Page as Shell;
                if (currentShellOfWindow0 == null) throw new InvalidOperationException("The shell of main window 0 cannot be determined.");

                // Check whether a command station is selected.
                if (SelectedCommandStation == null)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoCommandStationSelected, AppResources.OK);
                    return;
                }

                // Create a copy of the selected item.
                CommandStationType tempCommandStation = new CommandStationType();
                tempCommandStation.IpAddress = SelectedCommandStation.IpAddress;
                tempCommandStation.Name = SelectedCommandStation.Name;


                // Display the popup dialog to edit the command station details.
                PopUpCommandStation popUpCommandStation = new PopUpCommandStation(tempCommandStation);
                CommunityToolkit.Maui.Core.IPopupResult<bool> response = (IPopupResult<bool>)await currentShellOfWindow0.ShowPopupAsync(popUpCommandStation, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });

                // If the user confirms the dialog, check the new values, save the new settings and activate the command station.
                if (response.Result == true)
                {
                    if (tempCommandStation.IpAddress != SelectedCommandStation.IpAddress)
                    {
                        if (CommandStations!.Any(item => item.IpAddress == tempCommandStation.IpAddress))
                        {
                            await MessageBox.Show(AppResources.AlertError, AppResources.AlertMaxCommandStationIPAddressExisting, AppResources.OK);
                            return;
                        }
                    }

                    if (tempCommandStation.Name != SelectedCommandStation.Name)
                    {
                        if (CommandStations!.Any(item => item.Name == tempCommandStation.Name))
                        {
                            await MessageBox.Show(AppResources.AlertError, AppResources.AlertMaxCommandStationNameExisting, AppResources.OK);
                            return;
                        }
                    }

                    // Update the currently selected command station with the new information.
                    SelectedCommandStation.Name = tempCommandStation.Name;
                    SelectedCommandStation.IpAddress = tempCommandStation.IpAddress;

                    // Save the all digital command stations to the preferences.
                    Preferences.Default.Set(AppConstants.PREFERENCES_ALLCOMMANDSTATIONS_KEY, JsonSerializer.Serialize(CommandStations));

                    // Set the currently selected command station active.
                    Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, SelectedCommandStation.IpAddress);

                }

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
                return;
            }
        }

        /// <summary>
        /// Displays a popup dialog to add a new command station and adds it to the collection if confirmed by the user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the shell of the main application window cannot be determined.</exception>
        [RelayCommand]
        async Task AddCommandStation()
        {
            try
            {
                Shell? currentShellOfWindow0 = App.Current!.Windows[0].Page as Shell;
                if (currentShellOfWindow0 == null) throw new InvalidOperationException("The shell of main window 0 cannot be determined.");

                // Check whether the maximum number of command stations is already reached.
                if (CommandStations!.Count >= 5)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertMaxCommandStationsReached, AppResources.OK);
                    return;
                }

                // Create an new default command station entry.
                CommandStationType newCommandStation = new CommandStationType()
                {
                    Name = "Name",
                    IpAddress = "192.168.0.111"
                };

                // Display the popup dialog to edit the command station details.
                PopUpCommandStation popUpCommandStation = new PopUpCommandStation(newCommandStation);
                CommunityToolkit.Maui.Core.IPopupResult<bool> response = (IPopupResult<bool>)await currentShellOfWindow0.ShowPopupAsync(popUpCommandStation, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    }
                });

                // If the user confirmed the dialog, add the new command station to the collection.
                if (response.Result == true)
                {
                    if (CommandStations.Any(item => item.IpAddress == newCommandStation.IpAddress))
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertMaxCommandStationIPAddressExisting, AppResources.OK);
                        return;
                    }

                    if (CommandStations.Any(item => item.Name == newCommandStation.Name))
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertMaxCommandStationNameExisting, AppResources.OK);
                        return;
                    }

                    CommandStations!.Add(newCommandStation);

                    // Save the all digital command stations to the preferences.
                    Preferences.Default.Set(AppConstants.PREFERENCES_ALLCOMMANDSTATIONS_KEY, JsonSerializer.Serialize(CommandStations));

                    //  Select the newly created command station.
                    SelectedCommandStation = CommandStations.First(item => item.IpAddress == newCommandStation.IpAddress);

                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Opens an folder picker dialog so that the user can select the user specific decoder specification folder.
        /// </summary>
        [RelayCommand]
        async Task SelectUserSpecificDeqSpecFolder()
        {
            try
            {
                FolderPickerResult result = await FolderPicker.Default.PickAsync();
                if (result.IsSuccessful)
                {
                    // Note:
                    // Sometimes the FolderPicker returns a folder which is not accessible for Z2X-Programmer.
                    // For this reason, we check access to the selected folder. If it fails we will display
                    // an error message to the user.
                    if (Directory.Exists(result.Folder.Path.ToString()) == true)
                    {
                        UserSpecificDecoderSpecificationFolder = result.Folder.Path.ToString();
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertDeqSpecFolderNotAccessible + LocoList.Z2XFileFolder, AppResources.OK);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }

        /// <summary>
        /// Checks the available decoder specifications for correctness.
        /// </summary>
        [RelayCommand]
        async Task CheckDecoderSpecifications()
        {
            try
            {
                string errorFilename = "";
                string errorMessage = "";

                //  At first we check the decoder specification files in the internal decoder specification folder.
                if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out errorFilename, out errorMessage) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDeqSpecFileNotRead + "\n\n" + errorFilename + "\n\n" + errorMessage, AppResources.OK);
                    return;
                }

                //  Now we check the decoder specification files in the user specific decoder specification folder.
                if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out errorFilename, out errorMessage) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDeqSpecFileNotRead + "\n\n" + errorFilename + "\n\n" + errorMessage, AppResources.OK);
                    return;
                }

                //  Display a message that the decoder specification files are OK.
                await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecSpecFilesOK, AppResources.OK);
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Opens an folder picker dialog so that the user can select the Z2X directory.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SelectZ2XFolder()
        {
            try
            {
                FolderPickerResult result = await FolderPicker.Default.PickAsync();
                if (result.IsSuccessful)
                {
                    // Note:
                    // Sometimes the FolderPicker returns a folder which is not accessible for Z2X-Programmer.
                    // For this reason, we check access to the selected folder. If it fails we will display
                    // an error message to the user.
                    if (Directory.Exists(result.Folder.Path.ToString()) == true)
                    {
                        LocoListSystemFolder = result.Folder.Path.ToString();
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderNotAccessible + LocoList.Z2XFileFolder, AppResources.OK);
                        return;
                    }
                }

                if (Directory.Exists(LocoList.Z2XFileFolder) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderNotExist + LocoList.Z2XFileFolder, AppResources.OK);
                    return;
                }

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Resets the application preferences (the settings) and quits the application.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task ResetZ2XProgrammer()
        {
            try
            {
                if (await MessageBox.Show(AppResources.AlertAttention, AppResources.AlertResetSettings, AppResources.YES, AppResources.NO) == false)
                {
                    return;
                }

                Preferences.Clear();

                if (Application.Current != null) Application.Current.Quit();
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// This command establishes a connection to your digital command staiton.
        /// This allows the configured IP address to be checked for correctness.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Connect()
        {
            try
            {
                // This property controls the ActivityIndicator. We set this property to TRUE so that the ActivityIndicator is displayed.
                ActivityConnectingOngoing = true;

                // So that we can check whether the new IP address is correct, we must terminate existing connections.
                CommandStation.Disconnect();

                // Now we are trying to establish a connection.
                bool ConnectSuccessFull = false;
                CancellationToken cancelToken = new CancellationTokenSource().Token;
                await Task.Run(() => ConnectSuccessFull = CommandStation.Connect(cancelToken, 5000));

                //  We hide the ActivityIndicator.
                ActivityConnectingOngoing = false;

                //  Show the result to the user.    
                if (ConnectSuccessFull == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError1 + " " + SelectedCommandStation.Name + " (" + SelectedCommandStation.IpAddress + ") " + AppResources.AlertNoConnectionCentralStationError2,AppResources.OK);
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertConnectionCommandStationOK1 + " " + SelectedCommandStation.Name + " (" + SelectedCommandStation.IpAddress + ") " + AppResources.AlertConnectionCommandStationOK2 , AppResources.OK);
                }

            }
            catch (System.FormatException)
            {
                ActivityConnectingOngoing = false;
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertWrongIPAddressFormat, AppResources.OK);
            }
            catch (Exception ex)
            {
                ActivityConnectingOngoing = false;
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        #endregion

        #region REGION: PRVIVATE FUNCTIONS

        /// <summary>
        /// This event is raised when the hardware information of the command station is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHardwareInformationReceived(object? sender, HardwareInformationEventArgs e)
        {
            Z21FirmwareVersion = e.MajorVersion.ToString() + "." + e.MinorVersion.ToString();
            Z21HardwareType = Z21HardwareDeviceTypeDescription(e.HardwareType);
        }

        /// <summary>
        /// Converts an Z21 hardware ID to a description.
        /// </summary>
        /// <param name="hardwareType">The Z21 hardware ID.</param>
        /// <returns></returns>
        private string Z21HardwareDeviceTypeDescription(uint hardwareType)
        {
            switch (hardwareType)
            {
                case 0x00000200: return AppResources.Z21DeviceTypeZ21Old;
                case 0x00000201: return AppResources.Z21DeviceTypeZ21New;
                case 0x00000202: return AppResources.Z21DeviceTypeZ21Small;
                case 0x00000204: return AppResources.Z21DeviceTypeZ21Start;
                case 0x00000205: return AppResources.Z21DeviceTypeSingleBooster;
                case 0x00000206: return AppResources.Z21DeviceTypeDualBooster;
                case 0x00000211: return AppResources.Z21DeviceTypeZ21XL;
                case 0x00000212: return AppResources.Z21DeviceTypeXLBooster;
                case 0x00000301: return AppResources.Z21DeviceTypeZ21SwitchDecoder;
                case 0x00000302: return AppResources.Z21DeviceTypeZ21SignalDecoder;
                default: return AppResources.Z21DeviceTypeUnknown + " (" + hardwareType.ToString() + ")";
            }
        }

        #endregion
    }
}
