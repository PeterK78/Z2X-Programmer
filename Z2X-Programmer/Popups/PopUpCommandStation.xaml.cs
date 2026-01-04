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
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.Popups;


/// <summary>
/// This class implements a popup that shows an activity indicator. Typically this popup is used during
/// up - and download of configuration values.
/// </summary>
public partial class PopUpCommandStation : Popup
{
    CommandStationType _commandStation;
    Shell? myShell;

    #region REGION: CONSTRUCTOR

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="commandStation">The command station to be modified.</param>
    internal PopUpCommandStation(CommandStationType commandStation)
    {
        InitializeComponent();
        myShell = Application.Current!.Windows[0].Page as Shell;

        _commandStation = commandStation;

        NameEntry.Text = _commandStation.Name;
        IPAddressEntry.Text = _commandStation.IpAddress;
    }
    #endregion

    #region REGION: PRIVATE FUNCTIONS

    /// <summary>
    /// Processes the cancel button was clicked event.
    /// </summary>
    /// <param name="sender">The canncel button object.</param>
    /// <param name="e">The event arguments.</param>
    void CancelButton_Clicked(object sender, EventArgs e)
    {
        if (myShell != null) myShell.ClosePopupAsync(false);
    }

    /// <summary>
    /// Processes the OK button was clicked event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OKButton_Clicked(object sender, EventArgs e)
    {

        _commandStation.Name = NameEntry.Text.Trim();
        _commandStation.IpAddress = IPAddressEntry.Text.Trim();

        if (myShell != null) myShell.ClosePopupAsync(true);
    }   
    #endregion


}