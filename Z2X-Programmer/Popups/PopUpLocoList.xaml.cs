/*

Z2X-Programmer
Copyright (C) 2024 - 2026
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
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Popups;

/// <summary>
/// Represents a popup dialog that displays a list of locomotives and allows the user to select one or cancel the
/// operation.
/// </summary>
/// <remarks>This popup is typically used to prompt the user to choose a locomotive from a provided list. The
/// selection is communicated via a message when the user confirms their choice. If the user cancels, no selection is
/// made. The popup requires a valid CancellationTokenSource and a list of locomotives to display. This class is
/// intended to be used within the application's shell context.</remarks>
public partial class PopUpLocoList : Popup
{

    Shell? myShell;
    CancellationTokenSource _cancelTokenSource;    

    /// <summary>
    /// Initializes a new instance of the PopUpLocoList class with the specified cancellation token source, locomotive
    /// list, and data source indicator.
    /// </summary>
    /// <param name="tokenSource">A CancellationTokenSource used to signal cancellation requests for operations associated with this instance.
    /// Cannot be null.</param>
    /// <param name="locoList">A list of LocoListType objects to display in the locomotive list view. Cannot be null.</param>
    /// indicator.</param>
    public PopUpLocoList(CancellationTokenSource tokenSource, List<LocoListType> locoList, bool dataSourceIsFileSystem)
	{
		InitializeComponent();
        LocoListCollectionView.ItemsSource = locoList;
        _cancelTokenSource = tokenSource;
        myShell = Application.Current!.Windows[0].Page as Shell;
        LocoListDataSourceImage.IsVisible = !dataSourceIsFileSystem;
    }

    /// <summary>
    /// Handles the button clicked event of the cancel button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void CancelButton_Clicked(object sender, EventArgs e)
    {
        if (myShell != null) { myShell.ClosePopupAsync(false); }        
    }
    
    /// <summary>
    /// Handles the button clicked event of the OK button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OKButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (LocoListCollectionView.SelectedItem != null)
            {
                WeakReferenceMessenger.Default.Send(new LocoSelectedMessage((LocoListType)LocoListCollectionView.SelectedItem));                
                if (myShell != null) await Shell.Current.ClosePopupAsync(true);
            }
            else
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertLocoListNoLocoSelected, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            Logger.PrintDevConsole("PopUpLocoList.OKButton_Clicked: " + ex.Message);
        }
    }   
}