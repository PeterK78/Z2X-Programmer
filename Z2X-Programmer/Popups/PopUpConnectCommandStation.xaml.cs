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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.Popups;

public partial class PopUpConnectCommandStation : Popup
{

    CancellationTokenSource _cancelTokenSource;

    #region REGION: CONSTRUCTOR

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="tokenSource">The token to signal that the user has canceled the process.</param>
    /// <param name="textMessage">A  string that describes the process. This is displayed in the dialog.</param>
    public PopUpConnectCommandStation(CancellationTokenSource tokenSource, string textMessage)
	{
		InitializeComponent();

        ProgressDialogMessage.Text = textMessage;
        _cancelTokenSource = tokenSource;

        //WeakReferenceMessenger.Default.Register<ProgressUpdateMessagePercentage>(this, (r, m) =>
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        OnGetNewProgressInfoPercentage(m.Value);
        //    });
        //});

        //WeakReferenceMessenger.Default.Register<ProgressUpdateMessageCV>(this, (r, m) =>
        //{
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        OnGetNewProgressInfoCV(m.Value);
        //    });
        //});

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
        _cancelTokenSource?.Cancel();
    }

    ///// <summary>
    ///// Processes an ProgressUpdateMessagePercentage event.
    ///// </summary>
    ///// <param name="value">The current progress of the process as a percentage.</param>
    //internal void OnGetNewProgressInfoPercentage(int value)
    //{
    //    try
    //    {
    //        ProgressLabelPercentage.Text = value.ToString() + " %";
    //        MyProgressBar.Progress = ((double)value / (double)100);

    //    }
    //    catch (System.ObjectDisposedException)
    //    {
    //        //  Somtimes the message ProgressUpdateMessage is delayed. So it can happen,
    //        //  that this popup is already disposed. So this exception can be thrown.
    //    }

    //}

    ///// <summary>
    ///// Processes an ProgressUpdateMessageCV event.
    ///// </summary>
    ///// <param name="value">The current currently processed cv.</param>
    //internal void OnGetNewProgressInfoCV(int value)
    //{
    //    try
    //    {
    //        ProgressLabelCV.Text = "(CV" + value.ToString() + ")";
    //    }
    //    catch (System.ObjectDisposedException)
    //    {
    //        //  Somtimes the message ProgressUpdateMessage is delayed. So it can happen,
    //        //  that this popup is already disposed. So this exception can be thrown.
    //    }

    //}

    #endregion


}