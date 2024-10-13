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

using System.Globalization;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Pages;

namespace Z2XProgrammer
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            
            // Check if the user has already agreed to the license.
            if (Preferences.Default.Get(AppConstants.PREFERENCES_LICENSE_KEY, AppConstants.PREFERENCES_LICENSE_DEFAULT) == "0")
            {
                MainPage = new LicensePage();
            }
            else
            {
                MainPage = new AppShell(new ViewModel.AppShellViewModel());
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _window = base.CreateWindow(activationState);

            if (_window != null)
            {

                // Get display size
                var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

                string width = Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWWIDTH_KEY, AppConstants.PREFERENCES_WINDOWWIDTH_DEFAULT);
                string height = Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWHEIGHT_KEY, AppConstants.PREFERENCES_WINDOWHEIGHT_DEFAULT);

                _window.Width = double.Parse(width);
                _window.Height = double.Parse(height);

                // Center the window
                if ((Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWPOSX_KEY, AppConstants.PREFERENCES_WINDOWPOSX_DEFAULT) == "-1") && (Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWPOSY_KEY, AppConstants.PREFERENCES_WINDOWPOSY_DEFAULT)) == "-1")
                {
                    _window.X = (displayInfo.Width / displayInfo.Density - _window.Width) / 2;
                    _window.Y = (displayInfo.Height / displayInfo.Density - _window.Height) / 2;
                }
                else
                {
                    _window.X = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWPOSX_KEY, AppConstants.PREFERENCES_WINDOWPOSX_DEFAULT));
                    _window.Y = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOWPOSY_KEY, AppConstants.PREFERENCES_WINDOWPOSY_DEFAULT));
                }


                _window.Destroying += (s, e) =>
                {
                    //  Disconnect the digital command station.
                    CommandStation.Disconnect();

                    int width = (int)_window.Width;
                    int height = (int)_window.Height;
                    int x = (int)_window.X;
                    int y = (int)_window.Y;

                    //  Update the window dimensions
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOWWIDTH_KEY, width.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOWHEIGHT_KEY, height.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOWPOSX_KEY, x.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOWPOSY_KEY, y.ToString());
                };
            }
            return _window!;
        }
    }
}
