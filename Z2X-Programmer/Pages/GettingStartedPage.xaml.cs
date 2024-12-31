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

using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.Pages;

public partial class GettingStartedPage : ContentPage
{

    /// <summary>
    /// Ctor
    /// </summary>
    public GettingStartedPage()
    {
        InitializeComponent();   
    }

    /// <summary>
    /// Handles the click event of the OK button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OKButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set(AppConstants.PREFERENCES_LICENSE_KEY, "1");
        if(App.Current != null)
        {
            App.Current.Windows[0].Page = new AppShell(new ViewModel.AppShellViewModel());
            App.Current.Windows[0].Width = 1024;
            App.Current.Windows[0].Height = 600;
            CenterWindow(App.Current.Windows[0]);
        }
    }
    /// <summary>
    /// Centers the given window.
    /// </summary>
    /// <param name="window">The window object to center.</param>    
    private void CenterWindow(Window window)
    {

        // We get the current resolution of the screen.
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

        try
        {
            window.X = (displayInfo.Width / displayInfo.Density - window.Width) / 2;
            window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;
        }
        catch { }
    }
}