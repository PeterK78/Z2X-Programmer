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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Popups;

public partial class PopUpLocoList : Popup
{

    CancellationTokenSource _cancelTokenSource;

	public PopUpLocoList(CancellationTokenSource tokenSource, List<LocoListType> locoList)
	{
		InitializeComponent();

        LocoListCollectionView.ItemsSource = locoList;

        _cancelTokenSource = tokenSource;
     
    }

    /// <summary>
    /// Handles the button clicked event of the cancel button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    
    /// <summary>
    /// Handles the button clicked event of the OK button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OKButton_Clicked(object sender, EventArgs e)
    {
        if (LocoListCollectionView.SelectedItem != null)
        {
            WeakReferenceMessenger.Default.Send(new LocoSelectedMessage((LocoListType)LocoListCollectionView.SelectedItem));
            this.Close();
        }
        else
        {
            if ((Application.Current != null) && (Application.Current.MainPage != null))
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, AppResources.AlertLocoListNoLocoSelected, AppResources.OK);
            }
        }
    }   
}