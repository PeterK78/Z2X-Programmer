﻿/*

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

using System.Xml.Serialization;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Model;
using Z2XProgrammer.Pages;

namespace Z2XProgrammer
{
    public partial class App : Application
    {
      

        /// <summary>
        /// The constructor of the App class.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates and takes care of the positioning and size of the main window.
        /// </summary>  
        /// <param name="activationState">The activation state of the window.</param>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // We check whether the user has already confirmed the license dialog.
            // If not, we start with the license dialog. If yes, then we start regularly with the main window.
            if (Preferences.Default.Get(AppConstants.PREFERENCES_LICENSE_KEY, AppConstants.PREFERENCES_LICENSE_DEFAULT) == "0")
            {
                // We create the main window and save the object in a field so that we can also access it later.
                GUI.MainWindow = new Window(new LicensePage());
                GUI.ResizeWindow(GUI.MainWindow,800, 600);
                GUI.CenterWindow(GUI.MainWindow);
            }
            else
            {
                // We create the main window and save the object in a field so that we can also access it later.
                GUI.MainWindow = new Window(new AppShell(new ViewModel.AppShellViewModel()));
                ResizeMainWindow(GUI.MainWindow);
                PlaceMainWindow(GUI.MainWindow);
            }

            //  We register a handler that is called when the window is destroyed.
            //  This allows us to save the current position and size of the window before exiting the program,
            //  and so other clean up stuff.
            GUI.MainWindow.Destroying += (s, e) =>
            {
                try
                { 
                    //  We save the Z2X file before we exit the program.
                    if ((DecoderConfiguration.Z2XFilePath != "") && (File.Exists(DecoderConfiguration.Z2XFilePath) == true))
                    {
                        XmlSerializer x = new XmlSerializer(typeof(Z2XProgrammerFileType));
                        if (File.Exists(DecoderConfiguration.Z2XFilePath) == true) File.Delete(DecoderConfiguration.Z2XFilePath);
                        using FileStream outputStream = System.IO.File.OpenWrite(DecoderConfiguration.Z2XFilePath);
                        using StreamWriter streamWriter = new StreamWriter(outputStream);
                        x.Serialize(streamWriter, Z2XReaderWriter.CreateZ2XProgrammerFile());
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //  Disconnect the digital command station.
                    CommandStation.Disconnect();

                    //  We check if the controller window is currently open. If so, we close the window.
                    //  Note: The position and size are saved automatically when the window object is closed (see GUI.ControllerWindow.Destroying).
                    if (GUI.ControllerWindow != null) Application.Current?.CloseWindow(GUI.ControllerWindow);

                    //  We save the position and size of the main window.
                    //  This allows us to restore them the next time we start the program.
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_MAIN_WIDTH_KEY, GUI.MainWindow.Width.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_MAIN_HEIGHT_KEY, GUI.MainWindow.Height.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_MAIN_POSX_KEY, GUI.MainWindow.X.ToString());
                    Preferences.Default.Set(AppConstants.PREFERENCES_WINDOW_MAIN_POSY_KEY, GUI.MainWindow.Y.ToString());
                }
                catch (Exception ex)
                {
                    Logger.PrintDevConsole("App.CreateWindow:" + ex.Message);
                }


            };

            return GUI.MainWindow!;
        }
    
        /// <summary>
        /// Changes the size of the window based on the user-specific settings for width and length.
        /// </summary>
        /// <param name="window"></param>
        private void ResizeMainWindow(Window window)
        {
            try
            {
                window.Width = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_WIDTH_KEY, AppConstants.PREFERENCES_WINDOW_MAINWIDTH_DEFAULT));
                window.Height = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_HEIGHT_KEY, AppConstants.PREFERENCES_WINDOW_MAIN_HEIGHT_DEFAULT));
            }
            catch (FormatException)
            {
                //  FormatExceptions usually occur when incorrect data has been saved in the user-specific settings.
                //  In this case, we will not resize the window at all.                
            }
        }

        /// <summary>
        /// Places the main window. If user-specific positions are available, these are used.
        /// If the user-specific positions are missing, the window is centered.
        /// </summary>
        /// <param name="window">The window object to center.</param>        
        private void PlaceMainWindow(Window window)
        {
            // We get the current resolution of the screen.
            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            try
            { 
                // We use the user-specific positions if they are available. Otherwise we will center the window.
                if ((Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_POSX_KEY, AppConstants.PREFERENCES_WINDOW_MAIN_POSX_DEFAULT) == "-1") && (Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_POSY_KEY, AppConstants.PREFERENCES_WINDOW_MAIN_POSY_DEFAULT)) == "-1")
                {
                    window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                    window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
                }
                else
                {
                    window.X = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_POSX_KEY, AppConstants.PREFERENCES_WINDOW_MAIN_POSX_DEFAULT));
                    window.Y = double.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_WINDOW_MAIN_POSY_KEY, AppConstants.PREFERENCES_WINDOW_MAIN_POSY_DEFAULT));
                }
            
            }
            catch (FormatException)
            {
                //  FormatExceptions usually occur when incorrect data has been saved in the user-specific settings.
                //  In this case, we will only center the window and not use any user-specific settings.
                window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
                window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
            }
        }        
    }
}
