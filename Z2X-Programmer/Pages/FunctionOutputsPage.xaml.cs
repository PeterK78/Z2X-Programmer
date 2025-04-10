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

using System.Reflection;
using Z2XProgrammer.Helper;
using Z2XProgrammer.ViewModel;

namespace Z2XProgrammer.Pages;

[QueryProperty(nameof(SearchTarget), "SearchTarget")]
public partial class FunctionOutputsPage : ContentPage
{
	public FunctionOutputsPage(FunctionOutputsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    /// <summary>
    /// Set the SearchTarget property to a name attribute of any XAML element on the associated content page.
    /// If the SearchTarget property is set, the content page scrolls to the specified XAML element.
    /// 
    /// Note: This property is typically used by the search system. Please refer to the static SettingsSearcher class.
    /// </summary>
    public string SearchTarget
    {
        set
        {
            try
            {
                //  Check the input values. Do not process empty or null values.
                if ((value == null) || (value == "")) return;

                //  Find the XAML element by the name attribute.
                Element TargetElement = (Element)this.FindByName(value);
                if (TargetElement == null)
                {
                    Logger.PrintDevConsole("The search could not find the requested XAML element " + value + " in the XAML content page " + MethodBase.GetCurrentMethod()!.DeclaringType!.Name + " (" + MethodBase.GetCurrentMethod()!.Name + ")");
                    return;
                }

                //  Scroll to the XAML element.
                Timer timer = new Timer((object? obj) =>
                {
                    MainThread.BeginInvokeOnMainThread(() => PageScrollView.ScrollToAsync((Element)this.FindByName(value), ScrollToPosition.Start, false));
                }, null, 100, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("An error occurred while trying to scroll to the XAML element " + value + " in the XAML content page " + MethodBase.GetCurrentMethod()!.DeclaringType!.Name + " (" + MethodBase.GetCurrentMethod()!.Name + "): " + ex.Message);
            }
        }
    }
}