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

using System.Collections;
using System.Collections.ObjectModel;

namespace Z2XProgrammer.UserControls;

public partial class Z2XBasicPickerWidget : ContentView
{
    // Handling the source of the picker       
    public static readonly BindableProperty ItemsSourceProperty =
    BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<string>), typeof(Z2XBasicPickerWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        control.MyPicker.ItemsSource = (IList)newvalue!;
    });
    public ObservableCollection<string> ItemsSource
    {
        get => (ObservableCollection<string>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    // Handling the text of the description label
    public static readonly BindableProperty DescriptionTextProperty =
    BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(Z2XBasicPickerWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        control.MyDescriptionLabel.Text = (string?)newvalue;
    });
    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty)!;
        set => SetValue(DescriptionTextProperty, value);
    }

    // Handling the text of the CV value label
    public static readonly BindableProperty CVTextProperty =
    BindableProperty.Create(nameof(CVText), typeof(string), typeof(Z2XBasicPickerWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        control.MyCVLabel.Text = (string?)newvalue;
    });
    public string CVText
    {
        get => (string)GetValue(CVTextProperty)!;
        set => SetValue(CVTextProperty, value);
    }

    // Handling the visibility of the CV value label
    public static readonly BindableProperty CVVisibleProperty =
    BindableProperty.Create(nameof(CVVisible), typeof(bool), typeof(Z2XBasicPickerWidget),false, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        control.MyCVLabel.IsVisible = (bool)newvalue;
    });
    public bool CVVisible
    {
        get => (bool)GetValue(CVVisibleProperty);
        set => SetValue(CVVisibleProperty, value);
    }

    // Handling the selected item of the picker
    public static readonly BindableProperty SelectedItemProperty =
    BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(Z2XBasicPickerWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        control.MyPicker.SelectedItem = newvalue;
    });

    public string SelectedItem
    {
        get => (string)GetValue(SelectedItemProperty)!;
        set => SetValue(SelectedItemProperty, value);
    }

    // Handling the selected index of the picker
    public static readonly BindableProperty SelectedIndexProperty =
    BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(Z2XBasicPickerWidget), -1, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicPickerWidget)bindable;
        if(control.ItemsSource == null) throw new InvalidOperationException("ItemsSource must be set before setting SelectedIndex.");   
        control.MyPicker.SelectedIndex = (int)newvalue;
    });

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public Z2XBasicPickerWidget()
    {
        InitializeComponent();
    }

    private void MyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        SelectedIndex = picker.SelectedIndex;
        if (SelectedIndex != -1)
        {
            SelectedItem = (string)picker.ItemsSource[SelectedIndex]!;
        }
    }

}