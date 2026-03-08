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

using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Z2XProgrammer.UserControls;

public partial class Z2XBasicEntryWidget : ContentView
{
    // Handling the behaviors of the entry
    // Note: We use an ObservableCollection here to allow dynamic changes to the behaviors at runtime, and we handle the CollectionChanged event to update the entry's behaviors accordingly.
    public ObservableCollection<Behavior> EntryBehaviors { get; } = new ObservableCollection<Behavior>();

    // Handling the text of the entry
    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Z2XBasicEntryWidget),"",BindingMode.TwoWay, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicEntryWidget)bindable;
        control.MyEntry.Text = (string?)newvalue;
    });
    public string Text
    {
        get => (string)GetValue(TextProperty)!;
        set => SetValue(TextProperty, value);
    }

    // Handling the keyboard property of the entry
    public static readonly BindableProperty KeyboardProperty =
    BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(Z2XBasicEntryWidget), Keyboard.Default, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicEntryWidget)bindable;
        control.MyEntry.Keyboard = (Keyboard)newvalue!;
    });
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty)!;
        set => SetValue(KeyboardProperty, value);
    }

    // Handling the text of the CV value label
    public static readonly BindableProperty CVTextProperty =
    BindableProperty.Create(nameof(CVText), typeof(string), typeof(Z2XBasicEntryWidget), propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicEntryWidget)bindable;
        control.MyCVLabel.Text = (string?)newvalue;
    });
    public string CVText
    {
        get => (string)GetValue(CVTextProperty)!;
        set => SetValue(CVTextProperty, value);
    }

    // Handling the read-only state of the entry        
    public static readonly BindableProperty IsReadOnlyProperty =
    BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(Z2XBasicEntryWidget),false, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicEntryWidget)bindable;
        control.MyEntry.IsReadOnly = (bool)newvalue;
    });
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    // Handling the visibility of the CV value label
    public static readonly BindableProperty CVVisibleProperty =
    BindableProperty.Create(nameof(CVVisible), typeof(bool), typeof(Z2XBasicEntryWidget),false, propertyChanged: (bindable, oldvalue, newvalue) =>
    {
        var control = (Z2XBasicEntryWidget)bindable;
        control.MyCVLabel.IsVisible = (bool)newvalue;
    });
    public bool CVVisible
    {
        get => (bool)GetValue(CVVisibleProperty);
        set => SetValue(CVVisibleProperty, value);
    }

    // Handling the text of the title label
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText),typeof(string),typeof(Z2XBasicEntryWidget),string.Empty,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                var control = (Z2XBasicEntryWidget)bindable;
                if (control?.MyTitleLabel != null) control.MyTitleLabel.Text = (string?)newvalue;
            });
    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty)!;
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public Z2XBasicEntryWidget()
    {
        InitializeComponent();

        // Direkt initialen Zustand anwenden
        MyEntry.IsReadOnly = IsReadOnly;

        // Änderungen an EntryBehaviors übernehmen
        EntryBehaviors.CollectionChanged += EntryBehaviors_CollectionChanged;

        // Bereits vorhandene Einträge (falls XAML sie vorab gesetzt hat) übernehmen
        foreach (var b in EntryBehaviors) MyEntry.Behaviors.Add(b);
    }

    private void EntryBehaviors_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (MyEntry == null)
            return;

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            MyEntry.Behaviors.Clear();
            foreach (var b in EntryBehaviors)
                MyEntry.Behaviors.Add(b);
            return;
        }

        if (e.OldItems != null)
        {
            foreach (Behavior oldB in e.OldItems)
                MyEntry.Behaviors.Remove(oldB);
        }

        if (e.NewItems != null)
        {
            foreach (Behavior newB in e.NewItems)
                MyEntry.Behaviors.Add(newB);
        }
    }
}