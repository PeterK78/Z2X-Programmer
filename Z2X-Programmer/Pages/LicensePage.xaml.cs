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

public partial class LicensePage : ContentPage
{

    /// <summary>
    /// Ctor
    /// </summary>
    public LicensePage()
    {
        InitializeComponent();
       // LicenseLabel.Text = License.Text;    
    }

    /// <summary>
    /// Handles the click event of the OK button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OKButton_Clicked(object sender, EventArgs e)
    {
        Preferences.Default.Set(AppConstants.PREFERENCES_LICENSE_KEY, "1");
        if(App.Current != null) App.Current.MainPage = new AppShell(new ViewModel.AppShellViewModel());
    }

    /// <summary>
    /// Handles the click event of the NO button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void NOButton_Clicked(object sender, EventArgs e)
    {
        if(App.Current != null) App.Current.Quit();
    }

    /// <summary>
    /// Handles the click event of the Licencse button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    async void LicenseButton_Clicked(object sender, EventArgs e)
    {
        
        await Browser.OpenAsync("https://github.com/PeterK78/Z2X-Programmer?tab=GPL-3.0-1-ov-file");
    }

    /// <summary>
    /// Handles the checked event of the checbox CheckboxAccept
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnCheckboxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {             
        OKButton.IsVisible = e.Value;
    }

}