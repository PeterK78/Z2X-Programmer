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

using Z2XProgrammer.ViewModel;

namespace Z2XProgrammer.Pages;

[QueryProperty(nameof(SearchTarget), "SearchTarget")]
public partial class DriveCharacteristicsPage : ContentPage
{
	public DriveCharacteristicsPage(DriveCharacteristicsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    public string SearchTarget
    {
        set
        {
            if (value == null) return;

            Element TargetElement = (Element)this.FindByName(value);
            if (TargetElement == null) return;

            Timer timer = new Timer((object? obj) => {
                MainThread.BeginInvokeOnMainThread(() => PageScrollView.ScrollToAsync((Element)this.FindByName(value), ScrollToPosition.Start, false));
            }, null, 100, Timeout.Infinite);
        }
    }
}