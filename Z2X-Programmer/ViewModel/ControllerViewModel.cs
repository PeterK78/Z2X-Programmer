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
using System.Diagnostics;
using Z21Lib.Events;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Pages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class ControllerViewModel : ObservableObject
    {

        private Stopwatch stopwatch = new Stopwatch();

        #region REGION: DATASTORE & SETTINGS & SEARCH

        [ObservableProperty]
        bool externalControllerWindowAllowed = GUI.ControllerWindowShown == true ? false : true;

        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        [ObservableProperty]
        bool controllerWindowEnabled = false;

        #endregion

        #region REGION: PUBLIC PROPERTIES        

        // Measurement section speed sensor 1 image
        [ObservableProperty]
        internal ImageSource sensor1ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_24_dark.png" : "ic_fluent_circle_small_24_regular.png";

        // Measurement section speed sensor 2 image
        [ObservableProperty]
        internal ImageSource sensor2ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_24_dark.png" : "ic_fluent_circle_small_24_regular.png";

        // Measurement section sensor 1 tooltip text
        [ObservableProperty]
        internal string sensor1TooltipText = "Sensor " + Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_DEFAULT);

        // Measurement section sensor 2 tooltip text
        [ObservableProperty]
        internal string sensor2TooltipText = "Sensor " + Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_DEFAULT);

        // Measurement section busy state.
        [ObservableProperty]
        internal bool measurementSectionBusy = false;

        // Measurement section speed.
        [ObservableProperty]
        float measurementSectionSpeed = 0;

        // Vehicle address
        [ObservableProperty]
        ushort vehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;

        // RailCom Speed
        [ObservableProperty]
        ushort railComSpeed = 0;

        // RailCom QOS
        [ObservableProperty]
        ushort railComQOS = 0;

        //  Speed
        [ObservableProperty]
        ushort speed = 0;

        //  Direction Backward
        [ObservableProperty]
        bool directionBackward = false;

        //  Direction Forward
        [ObservableProperty]
        bool directionForward = false;

        //  Function F0
        [ObservableProperty]
        bool functionF0 = false;

        //  Function F1
        [ObservableProperty]
        bool functionF1 = false;

        //  Function F2
        [ObservableProperty]
        bool functionF2 = false;

        //  Function F3
        [ObservableProperty]
        bool functionF3 = false;

        //  Function F4
        [ObservableProperty]
        bool functionF4 = false;

        //  Function F5
        [ObservableProperty]
        bool functionF5 = false;

        //  Function F6
        [ObservableProperty]
        bool functionF6 = false;

        //  Function F7
        [ObservableProperty]
        bool functionF7 = false;

        //  Function F8
        [ObservableProperty]
        bool functionF8 = false;

        //  Function F9
        [ObservableProperty]
        bool functionF9 = false;

        //  Function F10
        [ObservableProperty]
        bool functionF10 = false;

        //  Function F11
        [ObservableProperty]
        bool functionF11 = false;

        //  Function F12
        [ObservableProperty]
        bool functionF12 = false;

        //  Function F13
        [ObservableProperty]
        bool functionF13 = false;

        //  Function F14
        [ObservableProperty]
        bool functionF14 = false;

        //  Function F15
        [ObservableProperty]
        bool functionF15 = false;

        //  Function F16
        [ObservableProperty]
        bool functionF16 = false;

        //  Function F17
        [ObservableProperty]
        bool functionF17 = false;

        //  Function F18
        [ObservableProperty]
        bool functionF18 = false;

        //  Function F19
        [ObservableProperty]
        bool functionF19 = false;

        //  Function F20
        [ObservableProperty]
        bool functionF20 = false;

        //  Function F21
        [ObservableProperty]
        bool functionF21 = false;

        //  Function F22
        [ObservableProperty]
        bool functionF22 = false;

        //  Function F23
        [ObservableProperty]
        bool functionF23 = false;

        //  Function F24
        [ObservableProperty]
        bool functionF24 = false;

        //  Function F25
        [ObservableProperty]
        bool functionF25 = false;

        //  Function F26
        [ObservableProperty]
        bool functionF26 = false;

        //  Function F27
        [ObservableProperty]
        bool functionF27 = false;

        //  Function F28
        [ObservableProperty]
        bool functionF28 = false;

        //  Speedsteps 14
        [ObservableProperty]
        bool speedStep14 = false;

        //  Speedsteps 28
        [ObservableProperty]
        bool speedStep28 = true;

        //  Speedsteps 128
        [ObservableProperty]
        bool speedStep128 = false;


        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public ControllerViewModel()
        {
            OnGetDecoderConfiguration();

            CommandStation.Z21.OnLocoInfoReceived += OnLocoInfoReceived;
            CommandStation.OnStatusChanged += OnCommandStationStatusChanged;
            CommandStation.OnRailComInfoReceived += OnRailComInfoReceived;
            CommandStation.OnRmBusInfoReceived += OnRailComInfoReceived;

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
        public void OnGetDecoderConfiguration()
        {
            //  Request the locomotive information for the current locomotive address.
            CommandStation.Z21.GetLocoInfo(DecoderConfiguration.RCN225.LocomotiveAddress);

            VehicleAddress = DecoderConfiguration.RCN225.LocomotiveAddress;
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            //  A different locomotive or decoder specification has been selected - we will
            //  deactivate the controller and request the information for the selected locomotive again.
            ControllerWindowEnabled = false;

            RailComSpeed = 0;

            OnGetDecoderConfiguration();
        }

        /// <summary>
        /// Sets the speed of the currently active locomotive.
        /// </summary>
        /// <param name="speed">The speed.</param>
        void SetLocoSpeed(ushort speed, int direction)
        {
            byte realSpeedSteps = 0;
            if (SpeedStep14 == true) realSpeedSteps = 14;
            if (SpeedStep28 == true) realSpeedSteps = 28;
            if (SpeedStep128 == true) realSpeedSteps = 128;

            CommandStation.Z21.SetLocoDrive(DecoderConfiguration.RCN225.LocomotiveAddress, speed, realSpeedSteps, direction);
        }

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
        /// This event OnRailComInfoReceived is raied when the command station receives RM bus data.
        /// </summary>
        private void OnRailComInfoReceived(object? sender, RmBusInfoEventArgs e)
        {
            // We read the configured sensor addresses from the preferences.
            int sensor1ConfiguredAddress = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR1NR_DEFAULT));
            int sensor2ConfiguredAddress = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SENSOR2NR_DEFAULT));

            // Check if we have received the sensor number 1, if so we start the timer.
            if ((e.State == true) && (e.FeedbackAddress == sensor1ConfiguredAddress))
            {
                MeasurementSectionBusy = true;
                Sensor1ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_green_24_dark.png" : "ic_fluent_circle_small_green_24_regular.png";

                if (stopwatch.IsRunning == false)
                {
                    stopwatch.Restart();
                }
                else
                {
                    stopwatch.Start();
                }
            }
            else if ((e.State == false) && (e.FeedbackAddress == sensor1ConfiguredAddress))
            {
                Sensor1ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_24_dark.png" : "ic_fluent_circle_small_24_regular.png";
            }

            // Check if we have received the sensor number 2, if so we stop the timer and calculate the speed.
            if ((e.State == true) && (e.FeedbackAddress == sensor2ConfiguredAddress))
            {
                Sensor2ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_green_24_dark.png" : "ic_fluent_circle_small_green_24_regular.png";

                if (stopwatch.IsRunning == true)
                {
                    MeasurementSectionBusy = false;
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds > 0)
                    {
                        //  We have to convert the speed from m/s to km/h. Therefore we multiply the speed with 3.6.
                        float measurementSectionLengthCM = (float.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_LENGTHMM_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_LENGTHMM_DEFAULT))) / 10;
                        float measurementSectionScale = float.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_MEASUREMENTSECTION_SCALE_KEY, AppConstants.PREFERENCES_MEASUREMENTSECTION_SCALE_DEFAULT));
                        MeasurementSectionSpeed = measurementSectionLengthCM * measurementSectionScale * 3600 / stopwatch.ElapsedMilliseconds / 100;
                    }
                }
            }
            else if ((e.State == false) && (e.FeedbackAddress == sensor2ConfiguredAddress))
            {
                MeasurementSectionBusy = false;
                stopwatch.Stop();
                Sensor2ImageSource = Application.Current!.RequestedTheme == AppTheme.Dark ? "ic_fluent_circle_small_24_dark.png" : "ic_fluent_circle_small_24_regular.png";
            }

        }


        /// <summary>
        /// The event OnCommandStationStatusChanged is raised when the command station switch it status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommandStationStatusChanged(object? sender, StateEventArgs e)
        {
            CommandStation.Z21.GetLocoInfo(DecoderConfiguration.RCN225.LocomotiveAddress);
        }

        /// <summary>
        /// Event handler for the OnLocoInfoReceived event of the Z21 library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocoInfoReceived(object? sender, LocoInfoEventArgs e)
        {
            try
            {
                //  If the received address is not the address of the current locomotive, we return.
                if (e.Address != DecoderConfiguration.RCN225.LocomotiveAddress) return;

                //  Update the function states.
                FunctionF0 = e.FunctionStates[0];
                FunctionF1 = e.FunctionStates[1];
                FunctionF2 = e.FunctionStates[2];
                FunctionF3 = e.FunctionStates[3];
                FunctionF4 = e.FunctionStates[4];
                FunctionF5 = e.FunctionStates[5];
                FunctionF6 = e.FunctionStates[6];
                FunctionF7 = e.FunctionStates[7];
                FunctionF8 = e.FunctionStates[8];
                FunctionF9 = e.FunctionStates[9];
                FunctionF10 = e.FunctionStates[10];
                FunctionF11 = e.FunctionStates[11];
                FunctionF12 = e.FunctionStates[12];
                FunctionF13 = e.FunctionStates[13];
                FunctionF14 = e.FunctionStates[14];
                FunctionF15 = e.FunctionStates[15];
                FunctionF16 = e.FunctionStates[16];
                FunctionF17 = e.FunctionStates[17];
                FunctionF18 = e.FunctionStates[18];
                FunctionF19 = e.FunctionStates[19];
                FunctionF20 = e.FunctionStates[20];
                FunctionF21 = e.FunctionStates[21];
                FunctionF22 = e.FunctionStates[22];
                FunctionF23 = e.FunctionStates[23];
                FunctionF24 = e.FunctionStates[24];
                FunctionF25 = e.FunctionStates[25];
                FunctionF26 = e.FunctionStates[26];
                FunctionF27 = e.FunctionStates[27];
                FunctionF28 = e.FunctionStates[28];

                //  Update the speed steps. 
                SpeedStep14 = false; SpeedStep28 = false; SpeedStep128 = false;
                if (e.MaxSpeedSteps == 14) SpeedStep14 = true;
                if (e.MaxSpeedSteps == 28) SpeedStep28 = true;
                if (e.MaxSpeedSteps == 128) SpeedStep128 = true;

                //  Update the direction.
                if (e.Direction == 1)
                {
                    DirectionForward = true;
                    DirectionBackward = false;
                }
                else
                {
                    DirectionForward = false;
                    DirectionBackward = true;
                }

                //  Grab the current speed step of the locomotive.
                Speed = (byte)e.CurrentSpeedStep;

                //  We enable the controller window.
                ControllerWindowEnabled = true;

            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("ControllerViewModel.OnLocoInfoReceived: " + ex.Message);
            }
        }

        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Stops an active calucation in the measurement section.
        /// </summary>  
        [RelayCommand]
        void StopMeasurementSection()
        {
            stopwatch.Stop();
            stopwatch.Reset();
            MeasurementSectionBusy = false;
        }

        [RelayCommand]
        void SliderDragCompleted(object obj)
        {
            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        [RelayCommand]
        void OpenInExternalWindow()
        {
            if (DeviceInfo.Platform != DevicePlatform.WinUI) return;

            //  We check, if the controller window is already opened. If so, we close the window and return.
            if (GUI.ControllerWindow != null)
            {
                Application.Current?.ActivateWindow(GUI.ControllerWindow);
                return;
            }

            //  We set the controller window shown flag to true.
            GUI.ControllerWindowShown = true;

            //  We create a new controller page and a new controller window.
            ControllerPage controllerPage = new ControllerPage(new ControllerViewModel());
            GUI.ControllerWindow = new Window(controllerPage);

            //  Configure the new window.                
            GUI.ControllerWindow.Parent = Shell.Current.CurrentPage;
            GUI.ControllerWindow.Width = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_CONTROLLER_WIDTH_KEY, AppConstants.PREFERENCES_WINDOW_CONTROLLER_WIDTH_DEFAULT));
            GUI.ControllerWindow.Height = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_CONTROLLER_HEIGHT_KEY, AppConstants.PREFERENCES_WINDOW_CONTROLLER_HEIGHT_DEFAULT));
            GUI.ControllerWindow.X = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSX_KEY, AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSX_DEFAULT));
            GUI.ControllerWindow.Y = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSY_KEY, AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSY_DEFAULT));
            GUI.ControllerWindow.MinimumHeight = double.Parse(AppConstants.PREFERENCES_WINDOW_CONTROLLER_HEIGHT_DEFAULT);
            GUI.ControllerWindow.MinimumWidth = double.Parse(AppConstants.PREFERENCES_WINDOW_CONTROLLER_WIDTH_DEFAULT);


            Shell.Current.GoToAsync("ControllerPage");

            //  Open the new controller window
            App.Current?.OpenWindow(GUI.ControllerWindow);

            //  We register a handler that is called when the window is destroyed.
            //  This allows us to save the current position and size of the window before exiting the program.
            GUI.ControllerWindow.Destroying += (s, e) =>
            {
                try
                {
                    //  We save the position and size of the main window.
                    //  This allows us to restore them the next time we start the program.
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_CONTROLLER_WIDTH_KEY, GUI.ControllerWindow.Width.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_CONTROLLER_HEIGHT_KEY, GUI.ControllerWindow.Height.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSX_KEY, GUI.ControllerWindow.X.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_CONTROLLER_POSY_KEY, GUI.ControllerWindow.Y.ToString());

                    //  We set the controller window object to null.
                    GUI.ControllerWindow = null;
                }
                catch (Exception ex)
                {
                    Logger.PrintDevConsole("App.CreateWindow:" + ex.Message);
                }
            };
        }

        [RelayCommand]
        void IncreaseSpeed()
        {
            //   The maximum values oare taken from the Z21 protocol description.
            //   Please refer to the chapter “LAN_X_SET_LOCO_DRIVE” for more information.
            if (SpeedStep14 == true && Speed < 14) Speed++;
            if (SpeedStep28 == true && Speed < 28) Speed++;
            if (SpeedStep128 == true && Speed < 126) Speed++;

            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        [RelayCommand]
        void DecreaseSpeed()
        {
            if (Speed > 0) Speed--;
            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        [RelayCommand]
        void SetDirectionForward()
        {
            DirectionForward = true;
            DirectionBackward = false;
            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        [RelayCommand]
        void SetDirectionBackward()
        {
            DirectionForward = false;
            DirectionBackward = true;
            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        /// <summary>
        /// Stops the locomotive.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        void StopLoco()
        {
            Speed = 0;
            SetLocoSpeed(Speed, (DirectionForward == true) ? 1 : 0);
        }

        /// <summary>
        /// Activates a loco function.
        /// </summary>
        /// <param name="functionNumber">The function number to be activated.</param>
        [RelayCommand]
        async Task ActivateLocoFunction(string functionNumber)
        {
            try
            {
                //  We connect to the command station. If we are not able to connect,
                //  we show an error message and return.
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken cancelToken = cancelTokenSource.Token;
                if (CommandStation.Connect(cancelToken, 2000) == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertDigitalCommandStationNoReachablePart1 + " " + Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT) + " " + AppResources.AlertDigitalCommandStationNoReachablePart2, AppResources.OK);
                    return;
                }

                if (Byte.TryParse(functionNumber, out byte fnNumber) == true)
                {
                    if (fnNumber > 31) throw new ArgumentException("Function number is out of range.");

                    CommandStation.Z21.SetLocoFunction(DecoderConfiguration.RCN225.LocomotiveAddress, Z21Lib.Enums.SwitchType.Toggle, fnNumber);
                }
                else
                {
                    throw new ArgumentException("Function number is not a byte.");
                }
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("ControllerViewModel.ActivateLocoFunction: " + ex.Message);
            }
        }

        #endregion
    }

}
