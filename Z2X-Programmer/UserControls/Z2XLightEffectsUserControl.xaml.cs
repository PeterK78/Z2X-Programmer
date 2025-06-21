using System.Collections;
using System.Collections.ObjectModel;

namespace Z2XProgrammer.UserControls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Z2XLightEffectsUserControl : ContentView
{
    public static readonly BindableProperty FunctionOutputAvailableProperty =
    BindableProperty.Create(nameof(FunctionOutputAvailable), typeof(bool), typeof(Z2XLightEffectsUserControl),propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XLightEffectsUserControl)bindable;
        bool visible = (bool)newvalue;

        if (visible == true)
        {
            control.PickerEffect.IsVisible = true;
            control.PickerDirection.IsVisible = true;
            control.LabelFunctionOutputNotAvailable.IsVisible = false;
        }
        else
        {
            control.PickerEffect.IsVisible = false;
            control.PickerDirection.IsVisible = false;
            control.LabelFunctionOutputNotAvailable.IsVisible = true;
        }
    });


    public static readonly BindableProperty CVValueProperty =
        BindableProperty.Create(nameof(CVValue), typeof(string), typeof(Z2XLightEffectsUserControl), propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.LabelConfigurationVariable.Text = (string)newvalue;
        });

    public static readonly BindableProperty ItemsSourceEffectsProperty =
        BindableProperty.Create(nameof(ItemsSourceEffects), typeof(ObservableCollection<String>), typeof(Z2XLightEffectsUserControl), propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.PickerEffect.ItemsSource = (IList)newvalue;
        });

    public static readonly BindableProperty ItemsSourceDirectionProperty =
        BindableProperty.Create(nameof(ItemsSourceDirection), typeof(IList), typeof(Z2XLightEffectsUserControl), propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.PickerDirection.ItemsSource = (IList)newvalue;
        });



    public static readonly BindableProperty SelectedItemEffectProperty =
        BindableProperty.Create(nameof(SelectedItemEffect), typeof(object), typeof(Z2XLightEffectsUserControl), propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.PickerEffect.SelectedItem = newvalue;
        });

    public static readonly BindableProperty SelectedItemDirectionProperty =
        BindableProperty.Create(nameof(SelectedItemDirection), typeof(object), typeof(Z2XLightEffectsUserControl), propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.PickerDirection.SelectedItem = newvalue;
        });

    public static readonly BindableProperty AdditionalCVValuesVisibleProperty =
        BindableProperty.Create(nameof(AdditionalCVValuesVisible), typeof(bool), typeof(Z2XLightEffectsUserControl), false, propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            var control = (Z2XLightEffectsUserControl)bindable;
            control.LabelConfigurationVariable.IsVisible = (bool)newvalue;
        });


    public Z2XLightEffectsUserControl()
    {
        InitializeComponent();
    }

    public string CVValue
    {
        get => (string)GetValue(CVValueProperty);
        set => SetValue(CVValueProperty, value);
    }

    public ObservableCollection<string> ItemsSourceEffects
    {
        get => (ObservableCollection<string>)GetValue(ItemsSourceEffectsProperty);
        set => SetValue(ItemsSourceEffectsProperty, value);
    }

    public IList ItemsSourceDirection
    {
        get => (IList)GetValue(ItemsSourceDirectionProperty);
        set => SetValue(ItemsSourceDirectionProperty, value);
    }

    public bool FunctionOutputAvailable
    {
        get => (bool)GetValue(FunctionOutputAvailableProperty);
        set => SetValue(FunctionOutputAvailableProperty, value);
    }

    public object SelectedItemEffect
    {
        get => (object)GetValue(SelectedItemEffectProperty);
        set => SetValue(SelectedItemEffectProperty, value);
    }

    public object SelectedItemDirection
    {
        get => (object)GetValue(SelectedItemDirectionProperty);
        set => SetValue(SelectedItemDirectionProperty, value);
    }

    public bool AdditionalCVValuesVisible
    {
        get => (bool)GetValue(AdditionalCVValuesVisibleProperty);
        set => SetValue(AdditionalCVValuesVisibleProperty, value);
    }

    private void PickerEffect_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            SelectedItemEffect = (string)picker.ItemsSource[selectedIndex]!;
        }
    }

    private void PickerDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            SelectedItemDirection = (string)picker.ItemsSource[selectedIndex]!;
        }
    }
}