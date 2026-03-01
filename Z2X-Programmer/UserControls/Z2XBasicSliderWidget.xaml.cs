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

namespace Z2XProgrammer.UserControls;

public partial class Z2XBasicSliderWidget : ContentView
{
    // Handling the value of the slider
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(Z2XBasicSliderWidget), 0.0, defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldv, newv) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MySlider != null)
                {
                    control.MySlider.Value = (double)newv;
                    control.MySliderLabel.Text = control.GetSliderValueText((double)newv);
                }
            });

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    // Handling the minimum value of the slider
    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum),typeof(double),typeof(Z2XBasicSliderWidget),0.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MySlider != null) control.MySlider.Minimum = (double)newvalue;
            });
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    // Handling the maximum value of the slider
    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum),typeof(double),typeof(Z2XBasicSliderWidget),1.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MySlider != null) control.MySlider.Maximum = (double)newvalue;
            });
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    // Handling the tooltip of the slider
    public static readonly BindableProperty SliderTooltipTextProperty =
        BindableProperty.Create(nameof(SliderTooltipText),typeof(string),typeof(Z2XBasicSliderWidget),string.Empty,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MySlider != null) ToolTipProperties.SetText(control.MySlider, (string)newvalue);
            });
    public string SliderTooltipText
    {
        get => (string)GetValue(SliderTooltipTextProperty);
        set => SetValue(SliderTooltipTextProperty, value);
    }

    // Handling the text of the title label
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText),typeof(string),typeof(Z2XBasicSliderWidget),string.Empty,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MyTitleLabel != null) control.MyTitleLabel.Text = (string?)newvalue;
            });
    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty)!;
        set => SetValue(TitleTextProperty, value);
    }

    // Handling the text of the description label
    public static readonly BindableProperty DescriptionTextProperty =
        BindableProperty.Create(nameof(DescriptionText),typeof(string),typeof(Z2XBasicSliderWidget),string.Empty,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MyTitleLabel != null) control.MyDecriptionLabel.Text = (string?)newvalue;
            });
    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty)!;
        set => SetValue(DescriptionTextProperty, value);
    }

    // Handling the text of the CV value label
    public static readonly BindableProperty CVTextProperty =
        BindableProperty.Create(nameof(CVText),typeof(string),typeof(Z2XBasicSliderWidget),string.Empty,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MyCVLabel != null) control.MyCVLabel.Text = (string?)newvalue;
            });
    public string CVText
    {
        get => (string)GetValue(CVTextProperty)!;
        set => SetValue(CVTextProperty, value);
    }

    // Handling the visibility of the CV value label
    public static readonly BindableProperty CVVisibleProperty =
        BindableProperty.Create(nameof(CVVisible),typeof(bool),typeof(Z2XBasicSliderWidget),false,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MyCVLabel != null) control.MyCVLabel.IsVisible = (bool)newvalue;
            });
    public bool CVVisible
    {
        get => (bool)GetValue(CVVisibleProperty);
        set => SetValue(CVVisibleProperty, value);
    }

    //  Handling the visibility of the heat indicator
    public static readonly BindableProperty HeatIndicatorIsVisibleProperty =
        BindableProperty.Create(nameof(HeatIndicatorIsVisible),typeof(bool),typeof(Z2XBasicSliderWidget),false,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
                if (control?.MyHeatIndicator != null) control.MyHeatIndicator.IsVisible = (bool)newvalue;
            });
    public bool HeatIndicatorIsVisible
    {
        get => (bool)GetValue(HeatIndicatorIsVisibleProperty);
        set => SetValue(HeatIndicatorIsVisibleProperty, value);
    }

    // Handling the scaling of the percent label
    public static readonly BindableProperty PercentMinimumProperty =
        BindableProperty.Create(nameof(PercentMinimum), typeof(double), typeof(Z2XBasicSliderWidget), 0.0, defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldv, newv) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
            });

    public double PercentMinimum
    {
        get => (double)GetValue(PercentMinimumProperty);
        set => SetValue(PercentMinimumProperty, value);
    }

        // Handling the scaling of the percent label
    public static readonly BindableProperty PercentMaximumProperty =
        BindableProperty.Create(nameof(PercentMaximum), typeof(double), typeof(Z2XBasicSliderWidget), 100.0, defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldv, newv) =>
            {
                var control = (Z2XBasicSliderWidget)bindable;
            });

    public double PercentMaximum
    {
        get => (double)GetValue(PercentMaximumProperty);
        set => SetValue(PercentMaximumProperty, value);
    }



    /// <summary>
    /// Constructor
    /// </summary>
    public Z2XBasicSliderWidget()
    {
        InitializeComponent();
        if (MySliderLabel != null) MySliderLabel.Text = GetSliderValueText(Convert.ToDouble(Value));
        if (MyHeatIndicator != null) MyHeatIndicator.IsVisible = false;
    }

    private string GetSliderValueText(double value)
    {
        if (value == 0) return "0 (0 %)";
        float percentage = ((float)100 / ((float)Maximum - (float)Minimum)) * (float)value;

        double scaledValue = (value - Minimum) * (PercentMaximum - PercentMinimum) / (Maximum - Minimum) + PercentMinimum;


        return value.ToString("F0") + " (" + string.Format("{0:N0}", scaledValue) + " %)";
    }

    private void MySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Value = e.NewValue;
    }
}