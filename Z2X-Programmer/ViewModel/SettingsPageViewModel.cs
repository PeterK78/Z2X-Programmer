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

using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class SettingsPageViewModel : ObservableObject
    {

        #region REGION: PUBLIC PROPERTIES

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

        [ObservableProperty]
        internal string z21IPAddress;
        partial void OnZ21IPAddressChanged(string value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, value);
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

        [ObservableProperty]
        internal string decSpecFolder;

        [ObservableProperty]
        internal bool quitOnReadError = true;
        partial void OnQuitOnReadErrorChanged(bool value)
        {
            Preferences.Default.Set(AppConstants.PREFERENCES_QUITONREADERROR_KEY, value == true ? "1" : "0");
        }

        #endregion

        #region REGION: CONSTRUCTOR

        public SettingsPageViewModel()

        {
            //
            //  Setup the content of the GUI elements
            //
            Z21IPAddress = Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT);
            DecSpecFolder = FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath;
            LocoListSystemIPAddress = LocoList.IPAddress.ToString();
            LocoListSystemPort = LocoList.PortNumber.ToString();
            LocoListSystemFolder = LocoList.Folder.ToString();


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
            
            if (int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_AUTODECODER_DETECT_KEY, AppConstants.PREFERENCES_AUTODECODER_DETECT_DEFAULT)) ==  1 )
            {
                AutomaticDecoderDetection = true;
            }
            else
            {
                AutomaticDecoderDetection = false;
            }

            QuitOnReadError = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_QUITONREADERROR_KEY, AppConstants.PREFERENCES_QUITONREADERROR_VALUE)) == 1 ? true : false;

        }

        #endregion

        #region REGION: COMMANDS        

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
                    if(Directory.Exists(result.Folder.Path.ToString()) == true)
                    {
                        LocoListSystemFolder = result.Folder.Path.ToString();
                    }
                    else
                    {
                        await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderNotAccessible + LocoList.Folder, AppResources.OK);
                        return;
                    }
                }

                if (Directory.Exists(LocoList.Folder) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListZ2XFolderNotExist + LocoList.Folder, AppResources.OK);
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

        [RelayCommand]
        async Task CheckDecoderSpecifications()
        {
            try
            {

                if (DeqSpecReader.CheckDecSpecsFormatValid(FileAndFolderManagement.ApplicationFolders.DecSpecsFolderPath, out string errorFilename, out string errorMessage) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDeqSpecFileNotRead + "\n\n" + errorFilename + "\n\n" + errorMessage, AppResources.OK);
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecSpecFilesOK, AppResources.OK);
                }
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

                if (ConnectSuccessFull == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError, AppResources.OK);
                }
                else
                {
                    await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertNoConnectionCentralStationOK, AppResources.OK);
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

    }
}
