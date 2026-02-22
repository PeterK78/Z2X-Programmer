using System;
using System.Collections;
using System.Collections.ObjectModel;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.UserControls;

/// <summary>
/// Represents a simple widget with a picker control that allows users to select an item from a list, with optional description
/// and value labels.
/// </summary>
/// <remarks>This control provides bindable properties for the item source, description text, value label text,
/// and visibility. It also exposes properties for tracking the selected item and index, enabling integration with data
/// binding scenarios in XAML-based applications.</remarks>
public partial class Z2XBasicSwitchWidget : ContentView
{

    // Handling the IsToggled property of the switch
    public static readonly BindableProperty IsToggledProperty =
    BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(Z2XBasicSwitchWidget),false, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicSwitchWidget)bindable;
        control.MySwitch.IsToggled = (bool)newvalue;
        control.MySwitchLabel.Text = (bool)newvalue ? AppResources.SwitchStateOn : AppResources.SwitchStateOff;
    });
    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    // Handling the text of the description label
    public static readonly BindableProperty DescriptionTextProperty =
    BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(Z2XBasicSwitchWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicSwitchWidget)bindable;
        control.MyDescriptionLabel.Text = (string?)newvalue;
    });
    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty)!;
        set => SetValue(DescriptionTextProperty, value);
    }

    // Handling the text of the CV value label
    public static readonly BindableProperty CVTextProperty =
    BindableProperty.Create(nameof(CVText), typeof(string), typeof(Z2XBasicSwitchWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicSwitchWidget)bindable;
        control.MyCVLabel.Text = (string?)newvalue;
    });
    public string CVText
    {
        get => (string)GetValue(CVTextProperty)!;
        set => SetValue(CVTextProperty, value);
    }

    // Handling the visibility of the CV value label
    public static readonly BindableProperty CVVisibleProperty =
    BindableProperty.Create(nameof(CVVisible), typeof(bool), typeof(Z2XBasicSwitchWidget),false, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicSwitchWidget)bindable;
        control.MyCVLabel.IsVisible = (bool)newvalue;
    });
    public bool CVVisible
    {
        get => (bool)GetValue(CVVisibleProperty);
        set => SetValue(CVVisibleProperty, value);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public Z2XBasicSwitchWidget()
    {
        InitializeComponent();
        MySwitchLabel.Text = (bool)IsToggled ? AppResources.SwitchStateOn : AppResources.SwitchStateOff;
    }

    private void MySwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var picker = (Switch)sender;
        IsToggled = picker.IsToggled;
    }
}