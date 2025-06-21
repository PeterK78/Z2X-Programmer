using System.Collections;
using System.Collections.ObjectModel;
using Z2XProgrammer.DataModel;


namespace Z2XProgrammer.UserControls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Z2XFunctionKeyFunctionViewer : ContentView
{

    public static readonly BindableProperty FunctionSourceProperty =
    BindableProperty.Create(nameof(FunctionSource), typeof(ObservableCollection<String>), typeof(Z2XFunctionKeyFunctionViewer), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XFunctionKeyFunctionViewer)bindable;
        control.MainCollectionView.ItemsSource = (IList)newvalue;
    });

    public ObservableCollection<String> FunctionSource
    {
        get => (ObservableCollection<String>)GetValue(FunctionSourceProperty);
        set => SetValue(FunctionSourceProperty, value);
    }

    public Z2XFunctionKeyFunctionViewer()
    {
        InitializeComponent();
    }


}