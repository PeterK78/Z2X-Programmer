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
using Z2XProgrammer.DataModel;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.Popups;

public partial class PopUpEditInputMapping : Popup
{

    ZIMOInputMappingType _Mapping;

    CancellationTokenSource _cancelTokenSource;

	public PopUpEditInputMapping(CancellationTokenSource tokenSource, ZIMOInputMappingType mapping)
	{
		InitializeComponent();
        _cancelTokenSource = tokenSource;
        _Mapping = mapping;

        ExternalFunctionKeyLabel.Text = _Mapping.ExternalFunctionKeyDescription;
        InternalFunctionKeyMapping.ItemsSource = GetAvailableInputMappingTypes();

        if (_Mapping.InternalFunctionKeyNumber == 0)
        {
            InternalFunctionKeyMapping.SelectedIndex = 0;
        }
        else if (_Mapping.InternalFunctionKeyNumber == 29)
        {
            InternalFunctionKeyMapping.SelectedIndex = 1;
        }
        else
        {
            InternalFunctionKeyMapping.SelectedIndex = _Mapping.InternalFunctionKeyNumber + 1;
        }
    }

    /// <summary>   
    /// Returns a list with available input mappings.
    /// </summary>
    /// <returns></returns>
    private List<string> GetAvailableInputMappingTypes()
    { 
        List<string> Mappings = new List<string>();

        Mappings.Add(AppResources.ZIMOInputMappingDirectMapping);
        Mappings.Add("F0");
        Mappings.Add("F1");
        Mappings.Add("F2");
        Mappings.Add("F3");
        Mappings.Add("F4");
        Mappings.Add("F5");
        Mappings.Add("F6");
        Mappings.Add("F7");
        Mappings.Add("F8");
        Mappings.Add("F9");
        Mappings.Add("F10");
        Mappings.Add("F11");
        Mappings.Add("F12");
        Mappings.Add("F13");
        Mappings.Add("F14");
        Mappings.Add("F15");
        Mappings.Add("F16");
        Mappings.Add("F17");
        Mappings.Add("F18");
        Mappings.Add("F19");
        Mappings.Add("F20");

        return Mappings;

    }

    /// <summary>
    /// Handles the button clicked event of the cancel button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void CancelButtonClicked(object sender, EventArgs e)
    {
        this.Close();
    }

    
    /// <summary>
    /// Handles the button clicked event of the OK button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OKButtonClicked(object sender, EventArgs e)
    {
        if(InternalFunctionKeyMapping.SelectedIndex == 0)
        {
            _Mapping.InternalFunctionKeyNumber = 0;
        }
        else if(InternalFunctionKeyMapping.SelectedIndex == 1)
        {
            _Mapping.InternalFunctionKeyNumber = 29;
        }
        else
        {
            _Mapping.InternalFunctionKeyNumber = InternalFunctionKeyMapping.SelectedIndex - 1;
        }

        this.Close();   
    }   
  
}