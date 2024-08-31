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
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.Popups;

public partial class PopUpActivityIndicator : Popup
{

    CancellationTokenSource _cancelTokenSource;

	public PopUpActivityIndicator(CancellationTokenSource tokenSource, string textMessage)
	{
		InitializeComponent();

        ProgressDialogMessage.Text = textMessage;
        ProgressLabelCV.Text = "0 %";
        MyProgressBar.Progress = 0;
        
        _cancelTokenSource = tokenSource;

        WeakReferenceMessenger.Default.Register<ProgressUpdateMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                OnGetNewProgressInfo(m.Value);
            });
        });
     
    }

    void CancelButton_Clicked(object sender, EventArgs e)
    {
        _cancelTokenSource?.Cancel();
    }

    internal void OnGetNewProgressInfo(int value)
    {
        try
        {
            ProgressLabelCV.Text = value.ToString() + " %";
            MyProgressBar.Progress = ((double)value / (double)100);

        }
        catch (System.ObjectDisposedException)
        {
            //  Somtimes the message ProgressUpdateMessage is delayed. So it can happen,
            //  that this popup is already disposed. So this exception can be thrown.
        }

    }
 
}