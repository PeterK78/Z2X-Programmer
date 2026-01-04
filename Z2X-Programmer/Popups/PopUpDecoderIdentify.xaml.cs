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
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Popups;

public partial class PopUpDecoderIdentify : Popup
{

    ushort _vehicleAddress = 0;    
    CancellationToken _cancelToken;
    CancellationTokenSource _cancelTokenSource= new CancellationTokenSource();

    /// <summary>
    /// Constructor
    /// </summary>  
	public PopUpDecoderIdentify(ushort vehicleAddress)
	{
        List<ushort> manufacturerCVs = new List<ushort> { 3, 8 };
		InitializeComponent();
        
        ActivityIndicatorDecoderIdentify.IsRunning = false;
        ActivityIndicatorDecoderIdentify.IsVisible = false;
        
        _cancelToken = _cancelTokenSource.Token;
        _vehicleAddress = vehicleAddress;

        this.Opened += PopUpDecoderIdentify_Opened;        

    }

    /// <summary>
    /// This event handler is called, when the popup has been fully opened.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private async void PopUpDecoderIdentify_Opened(object? sender, EventArgs e)
    {
        ActivityIndicatorDecoderIdentify.IsRunning = true;
        ActivityIndicatorDecoderIdentify.IsVisible = true;

        //  In the first step, we read the standard CV 8.
        await ReadConfigurationVariables(_vehicleAddress,new List<ushort>{ 8}  , _cancelToken);

        //  In the second step, we check whether we need to read extended IDs. According to RCN-225, we must read extended IDs if
        //  the value 238 = 0xEE is present in CV8. In addition, we will also read the extended IDs if CV8 contains the value 13.
        //  This means that it is a public domain & do-it-yourself decoder.  
        if ((DecoderConfiguration.ConfigurationVariables[8].Value == 13) || (DecoderConfiguration.ConfigurationVariables[8].Value == 238))
        {
            await ReadConfigurationVariables(_vehicleAddress,new List<ushort>{ 107,108}  , _cancelToken);
        }
                
        LabelManufacturer.Text = DecoderConfiguration.RCN225.Manufacturer;

        ActivityIndicatorDecoderIdentify.IsRunning = false;
        ActivityIndicatorDecoderIdentify.IsVisible = false;

    }

    /// <summary>
    /// Handles the button clicked event of the OK button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OKButton_Clicked(object sender, EventArgs e)
    {
        _cancelTokenSource.Cancel();
        if (Application.Current!.Windows[0].Page != null) { await Application.Current!.Windows[0].Page!.ClosePopupAsync(false); }      
    }  

    /// <summary>
    /// Reads a set of configuration variables.
    /// </summary>
    /// <param name="configurationVariables">A list of configuration variables to read.</param>
    /// <param name="cancelToken">A token to cancel the operation.</param>
    /// <param name="vehicleAdress">The vehicle address.</param>
    /// <returns></returns>
    private async Task ReadConfigurationVariables(ushort vehicleAdress, List<ushort> configurationVariables, CancellationToken cancelToken)
    {
        try
        {
            //  Check if we are in direct programming mode.
            if (DecoderConfiguration.ProgrammingMode != NMRA.DCCProgrammingModes.DirectProgrammingTrack)
            {
                await MessageBox.Show(AppResources.AlertError, AppResources.FrameAddressVehicleAddressDetectNotProgTrack, AppResources.OK);
                return;
            }
            
            //  Check if we are connected to the command station.
            if (CommandStation.Connect(cancelToken, 5000) == false)
            {
                string commandStationName = Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONNAME_KEY, AppConstants.PREFERENCES_COMMANDSTATIONNAME_DEFAULT);
                string commandStationIpAddress = Preferences.Default.Get(AppConstants.PREFERENCES_COMMANDSTATIONIP_KEY, AppConstants.PREFERENCES_COMMANDSTATIONIP_DEFAULT);
                await MessageBox.Show(AppResources.AlertError, AppResources.AlertNoConnectionCentralStationError1 + " " + commandStationName + " (" + commandStationIpAddress + ") " + AppResources.AlertNoConnectionCentralStationError2,AppResources.OK);
                return;
            }

            await Task.Run(() => ReadWriteDecoder.SetTrackPowerON());

            // Read each CV value from the decoder.
            bool readSuccessFull = false;
            foreach (ushort cV in configurationVariables)
            {
                //  Read the next CV value from the decoder.
                readSuccessFull = false;
                await Task.Run(() => readSuccessFull = ReadWriteDecoder.ReadSingleCV(cV, 0, NMRA.DCCProgrammingModes.DirectProgrammingTrack, cancelToken));

                // If reading the CV failed, display an error message, exit programming mode, and terminate the function.
                if (readSuccessFull == false)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.AlertAddressNotRead, AppResources.OK);
                    if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();
                    return;
                }
            }

            //  After reading the CV on the programming track, we must switch the track power on.
            //  Switching on the track power ends the programming mode and the locomotive can be controlled again.
            if (DecoderConfiguration.ProgrammingMode == NMRA.DCCProgrammingModes.DirectProgrammingTrack) CommandStation.SetTrackPowerOn();

        }
        catch (Exception)
        {
            await MessageBox.Show(AppResources.AlertError, "Unable to read the configuration variables.", AppResources.OK);
        }
    }

}