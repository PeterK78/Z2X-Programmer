# Pattern


## Slider and Switch Pattern
### Description
The slider and switch pattern is used if a CV value is to be configured
using an on-off switch and a slider. The acceleration time CV3 in the
drive properties is such a value.

### Prerequisites
1. The limits of the .NET MAUI slider (properties Maximum and Minimum) are not updated dynamically by code. The limits are set once to the maximum value range in the function SetGUILimits according to the manufacturer's documentation.
In the past, setting the limits dynamically sometimes led to unpredictable effects.

### OnGetDataFromDataStore
In the OnGetDataFromDataStore function, we must adhere to the following workflow:

1. First, we set the value property of the associated slider to the CV value read.
2. We then use the value property of the slider to decide whether we want to switch the associated switch on or off.

```
// RCN225: Acceleration rate in CV3.
AccelerationRate = DecoderConfiguration.RCN225.AccelerationRate;
if(AccelerationRate == 0)
{
    AccelerationRateEnabled = false;
}
else
{
    AccelerationRateEnabled = true;

}
AccelerationRateTime = GetAccelerationRateTimeLabel();
```

### Slider or Vaue-Property Changed Event
If the value of the configuration variable has changed (e.g. by manually setting a new value via code or by moving the slider), only a simple method is executed. The new value is transferred to the decoder configuration.

```
[ObservableProperty]
internal byte accelerationRate;
partial void OnAccelerationRateChanged(byte value)
{
    DecoderConfiguration.RCN225.AccelerationRate = value;
    AccelerationRateTime = GetAccelerationRateTimeLabel();
    CV3Configuration = Subline.Create(new List<uint>{3});
}
```

### Switch Changed Event
In the switch changed event, we must adhere to the following workflow:

1. First we check whether the user wants to use specific values or not. We recognize this by the position of the switch.
2. If the user wants to use user-specific values, we check whether user-specific values are already available. If no user-specific value is available, we use a default value according to the manufacturer's documentation.
3. If the user does not want to use user-specific values, we set the value to the off state (in this example = 0).
4. Finally we update the extended display of CV values in the GUI.

```
// RCN225: Acceleration rate in CV3.
[ObservableProperty]
internal bool accelerationRateEnabled;
partial void OnAccelerationRateEnabledChanged(bool value)
{
    //  Check if the user specific acceleration rate is enabled.
    if (value == true)
    {
        // The user would like to use the user specific acceleration rate. We check if we already have a valid value for the acceleration rate,
        // if not we set it to 2 (according to the ZIMO manual, no recommendation in the RCN225 available).
        if (DecoderConfiguration.RCN225.AccelerationRate == 0) AccelerationRate = 2;
    }
    else
    {
        //  We turn off the user specific acceleration rate.
        AccelerationRate = 0;
    }
    CV3Configuration = Subline.Create(new List<uint>{3});
}
```




