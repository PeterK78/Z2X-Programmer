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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Popups;

// This pop-up shows the user a list of configuration variables.
// It is typically displayed before downloading new data to a decoder.
public partial class PopUpDownloadData : Popup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PopUpDownloadData"/> class.
    /// </summary>
    /// <param name="configurationVariables">A list of CV numbers which are written to the decoder.</param>
    /// <param name="descriptionText">Any description text to be displayed in the dialog.</param>
    /// <param name="titleText">The title of the dialog.</param>
    /// <param name="titleIconSource">The ressource name of the dialog icon.</param>
	public PopUpDownloadData(List<int> configurationVariables, string descriptionText, string titleText, string titleIconSource, bool DataFromDecoderValid)
    {
        InitializeComponent();

        if (configurationVariables == null) return;

        DecriptionTextLabel.Text = descriptionText;
        TitelTextLabel.Text = titleText;

        //  Setup the icon.
        if ((titleIconSource != null) && (titleIconSource != ""))
        { 
            TitelIcon.Source = titleIconSource;
            TitelIcon.IsVisible = true;
        }
        else
        {

            TitelIcon.IsVisible = false;
        }

        //  If the data in the backup is not from the decoder, show a warning.
        if(!DataFromDecoderValid)
        {
            NoteTextLabel.Text = AppResources.AlertDataInBackupAreNotFromDecoder;
            NoteTextLabel.IsVisible = true;
        }
        else
        {
            NoteTextLabel.IsVisible = false;
        }


        List<ConfigurationVariableDownloadInfo> listOfCVariables = new List<ConfigurationVariableDownloadInfo>();
        foreach(int variable in configurationVariables)
        {
            ConfigurationVariableDownloadInfo item = new ConfigurationVariableDownloadInfo();
            item.Number = variable;
            foreach (ConfigurationVariableType cv in DecoderConfiguration.ConfigurationVariables)
            {
                if (cv.Number == variable)
                {
                    item.ValueZ2X = cv.Value;
                    item.Enabled = cv.Enabled;
                    item.Description = cv.Description;
                    continue;
                }
            }
            foreach (ConfigurationVariableType cvBackup in DecoderConfiguration.BackupCVs)
            {
                if (cvBackup.Number == variable)
                {
                    item.ValueDecoder = cvBackup.Value;
                    continue;
                }
            }
            listOfCVariables.Add(item); 
        }
        ConfigurationVariablesCollectionView.ItemsSource = listOfCVariables;
     
    }

    /// <summary>
    /// Handles the button clicked event of the cancel button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Close(false);
    }

    
    /// <summary>
    /// Handles the button clicked event of the OK button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OKButton_Clicked(object sender, EventArgs e)
    {
        this.Close(true);
    }   
}