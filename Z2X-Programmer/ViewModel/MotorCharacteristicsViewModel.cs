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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using static Z2XProgrammer.Helper.ZIMO;

namespace Z2XProgrammer.ViewModel
{
    public partial class MotorCharacteristicsViewModel : ObservableObject
    {

        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

        // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion

        #region REGION: LIMITS FOR ENTRY VALIDATION

        [ObservableProperty]
        int limitMinimumZIMOMSmotorControlReferenceVoltage = 0;

        #endregion

        #region REGION: DECODER FEATURES

        //  RCN225_SPEEDTABLE_CV29_4
        [ObservableProperty]
        bool rCN225_SPEEDTABLE_CV29_4;

        [ObservableProperty]
        bool rCN225_MEDIUMSPEED_CV6;

        [ObservableProperty]
        bool rCN225_MAXIMALSPEED_CV5;

        [ObservableProperty]
        bool rCN225_MINIMALSPEED_CV2;

        // RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X
        [ObservableProperty]
        bool rCN225_EXTENDEDSPEEDCURVEVALUES_CV67X;

        [ObservableProperty]
        bool zIMO_MXMOTORCONTROLFREQUENCY_CV9;

        [ObservableProperty]
        bool zIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57;

        [ObservableProperty]
        bool zIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57;

        [ObservableProperty]
        bool zIMO_MXMOTORCONTROLPID_CV56;

        [ObservableProperty]
        bool dOEHLERHAAS_MOTORIMPULSWIDTH_CV49;

        //  DOEHLER AND HAAS: Maximum speed in CV5 (DOEHLERANDHAASS_MAXIMALSPEED_CV5)
        [ObservableProperty]
        bool dOEHLERANDHAASS_MAXIMALSPEED_CV5;

        [ObservableProperty]
        bool pIKOSMARTDECODER_MINIMUMSPEED_CV2;

        [ObservableProperty]
        bool pIKOSMARTDECODER_MAXIMUMSPEED_CV5;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        #region Döhler & Haas

        // DÖHLER & HAAS: Motor impuls width setting in CV49 (DOEHLERHAAS_MOTORIMPULSWIDTH_CV49)
        [ObservableProperty]
        internal byte impulsWidthValue;
        partial void OnImpulsWidthValueChanged(byte value)
        {
            DecoderConfiguration.DoehlerHaas.MotorImpulsWidth = value;
            UpdateImpulsWidthTime(DecoderConfiguration.DoehlerHaas.MotorImpulsWidth);
            CV49Configuration = Subline.Create(new List<uint> { 49 });
        }

        [ObservableProperty]
        internal string impulsWidthTime = "";

        [ObservableProperty]
        string cV49Configuration = Subline.Create(new List<uint> { 49 });

        // DOEHLER AND HAAS: Maximum speed in CV5 (DOEHLERANDHAASS_MAXIMALSPEED_CV5)
        [ObservableProperty]
        internal string maximumSpeedDoehlerAndHaasValueDescription = "";

        [ObservableProperty]
        internal bool maximumSpeedDoehlerAndHaasDefaultUsed;
        partial void OnMaximumSpeedDoehlerAndHaasDefaultUsedChanged(bool value)
        {
            if (value == false)
            {
                if (DecoderConfiguration.RCN225.MaximumSpeed == 0) MaximumSpeed = 100;
                MaximumSpeedDoehlerAndHaasValueDescription = GetDoehlerAndHaasMaximumSpeedLabel();
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
            else
            {
                MaximumSpeed = 0;
                MaximumSpeedDoehlerAndHaasValueDescription = GetDoehlerAndHaasMaximumSpeedLabel();
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
        }

        [ObservableProperty]
        internal byte maximumDoehlerAndHaasSpeed;
        partial void OnMaximumDoehlerAndHaasSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MaximumSpeed = value;
            MaximumSpeedDoehlerAndHaasValueDescription = GetDoehlerAndHaasMaximumSpeedLabel();
            CV5Configuration = Subline.Create(new List<uint> { 5 });
        }

        #endregion

        #region Piko SmartDecoder

        // Piko SmartDecoder: Maximum speed in CV5 (PIKOSMARTDECODER_MAXIMUMSPEED_CV5)
        [ObservableProperty]
        internal string maximumPikoSmartDecoderSpeedValueDescription = "";

        [ObservableProperty]
        internal byte maximumPikoSmartDecoderSpeed;
        partial void OnMaximumPikoSmartDecoderSpeedChanged(byte value)
        {
            DecoderConfiguration.PikoSmartDecoderV41.MaximumSpeed = value;
            MaximumPikoSmartDecoderSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.PikoSmartDecoderV41.MaximumSpeed, 63);
            CV5Configuration = Subline.Create(new List<uint> { 5 });
        }

        // Piko SmartDecoder: Minimum speed in CV2 (PIKOSMARTDECODER_MINIMUMSPEED_CV2)
        [ObservableProperty]
        internal string minimumPikoSmartDecoderSpeedValueDescription = "";

        [ObservableProperty]
        internal byte minimumPikoSmartDecoderSpeed;
        partial void OnMinimumPikoSmartDecoderSpeedChanged(byte value)
        {
            DecoderConfiguration.PikoSmartDecoderV41.MinimumSpeed = value;
            MinimumPikoSmartDecoderSpeedValueDescription = GetMinimumSpeedLabel(DecoderConfiguration.PikoSmartDecoderV41.MinimumSpeed, 63);
            CV2Configuration = Subline.Create(new List<uint> { 2 });
        }

        #endregion

        #region RCN225

        /// <summary>
        /// RCN225: Contains the CV values of the extended speed curve (CV67 to CV94).
        /// </summary>	
        [ObservableProperty]
        internal ObservableCollection<ConfigurationVariableType>? extendedSpeedCurveValues = new ObservableCollection<ConfigurationVariableType>();

        // RCN225: Maximum speed in CV5.
        [ObservableProperty]
        internal string maximumSpeedValueDescription = "";

        [ObservableProperty]
        internal bool maximumSpeedDefaultUsed;
        partial void OnMaximumSpeedDefaultUsedChanged(bool value)
        {
            if (value == false)
            {
                if (DecoderConfiguration.RCN225.MaximumSpeed == 0) MaximumSpeed = 100;
                MaximumSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.RCN225.MaximumSpeed, 255);
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
            else
            {
                MaximumSpeed = 0;
                MaximumSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.RCN225.MaximumSpeed, 255);
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
        }

        [ObservableProperty]
        internal byte maximumSpeed;
        partial void OnMaximumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MaximumSpeed = value;
            MaximumSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.RCN225.MaximumSpeed, 255);
            CV5Configuration = Subline.Create(new List<uint> { 5 });
        }

        [ObservableProperty]
        string cV5Configuration = Subline.Create(new List<uint> { 5 });

        // RCN225: Medium speed in CV6
        [ObservableProperty]
        internal string mediumSpeedValueDescription = "";

        [ObservableProperty]
        internal byte mediumSpeed;
        partial void OnMediumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MediumSpeed = value;
            MediumSpeedValueDescription = GetMediumSpeedLabel();
            CV6Configuration = Subline.Create(new List<uint> { 6 });
        }

        [ObservableProperty]
        string cV6Configuration = Subline.Create(new List<uint> { 6 });

        // RCN225: Minimum speed in CV2
        [ObservableProperty]
        internal string minimumSpeedValueDescription = "";

        [ObservableProperty]
        internal byte minimumSpeed;
        partial void OnMinimumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MinimumSpeed = value;
            MinimumSpeedValueDescription = GetMinimumSpeedLabel(DecoderConfiguration.RCN225.MinimumSpeed, 255);
            CV2Configuration = Subline.Create(new List<uint> { 2 });
        }
        [ObservableProperty]
        string cV2Configuration = Subline.Create(new List<uint> { 2 });

        // RCN225: Speed curve selection (standard or extended) in CV29 (RCN225_SPEEDTABLE_CV29_4)
        [ObservableProperty]
        internal bool extendedSpeedCurveEnabled;
        partial void OnExtendedSpeedCurveEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.ExtendedSpeedCurveEnabled = value;
            CV29Configuration = Subline.Create(new List<uint> { 29 });
        }
        [ObservableProperty]
        string cV29Configuration = Subline.Create(new List<uint> { 29 });

        #endregion

        #region ZIMO

        // ZIMO: MX decoder motor control frequency in CV9 (ZIMO_MXMOTORCONTROLFREQUENCY_CV9)
        [ObservableProperty]
        internal bool useDefaultMotorControlFrequency;
        partial void OnUseDefaultMotorControlFrequencyChanged(bool value)
        {
            // Does the user want to use default settings (value == TRUE)) or configure the parameters themselves?
            if (value == true)
            {
                // The user would like to use the ZIMO default settings. Therefore we set the CV9 to 0.
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 0;
            }
            else
            {
                // The user would like to use the user-specific settings.
                // We use the precise default settings 55 as suggested by ZIMO.
                // CV55 equals to High frequency,medium scanning rate
                EMKRate = 5;
                EMKGap = 5;
                SelectedMotorControlFrequqencyType = ZIMOEnumConverter.GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.HighFrequency);
                IsHighFrequencySelected = true;
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 55;
            }
            CV9Configuration = Subline.Create(new List<uint> { 9 });
        }

        [ObservableProperty]
        internal int lowFrequency;
        partial void OnLowFrequencyChanged(int value)
        {
            // If the user wants to use the default settings, we set CV9 to 0.
            if (useDefaultMotorControlFrequency == true)
            {
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 0;
                return;
            }

            // Check the range of the slider, just in case ...
            if (value < 176) return;
            if (value > 255) return;
            DecoderConfiguration.ZIMO.MotorFrequencyControl = (byte)value;
            CV9Configuration = Subline.Create(new List<uint> { 9 });
        }

        [ObservableProperty]
        internal ObservableCollection<string> availableMotorControlFrequencyTypes;

        [ObservableProperty]
        internal string selectedMotorControlFrequqencyType = "";
        partial void OnSelectedMotorControlFrequqencyTypeChanged(string value)
        {
            // If the user wants to use the default settings, we set CV9 to 0.
            if (useDefaultMotorControlFrequency == true)
            {
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 0;
                return;
            }

            if (ZIMOEnumConverter.GetMotorControlFrequencyType(value) == ZIMO.MotorControlFrequencyTypes.HighFrequency)
            {
                // We use the default settings 55 for CV9 as suggested by ZIMO.
                // CV55 equals to High frequency,medium scanning rate
                IsHighFrequencySelected = true;
                EMKRate = 5;
                EMKGap = 5;
                SelectedMotorControlFrequqencyType = ZIMOEnumConverter.GetMotorControlFrequencyTypeDescription(MotorControlFrequencyTypes.HighFrequency);
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 55;
            }
            else
            {
                // We use 127 as a default settings for low frequency motors.
                IsHighFrequencySelected = false;
                LowFrequency = 127;
                DecoderConfiguration.ZIMO.MotorFrequencyControl = (byte)LowFrequency;
            }
            CV9Configuration = Subline.Create(new List<uint> { 9 });
        }

        [ObservableProperty]
        internal bool isHighFrequencySelected;

        [ObservableProperty]
        internal int eMKRate;
        partial void OnEMKRateChanged(int value)
        {
            // If the user wants to use the default settings, we set CV9 to 0.
            if (useDefaultMotorControlFrequency == true)
            {
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 0;
                return;
            }

            DecoderConfiguration.ZIMO.MotorFrequencyControl = (byte)PlaceValue.SetPlaceValues(EMKGap, EMKRate, 0);
            CV9Configuration = Subline.Create(new List<uint> { 9 });
        }

        [ObservableProperty]
        internal int eMKGap;
        partial void OnEMKGapChanged(int value)
        {
            // If the user wants to use the default settings, we set CV9 to 0.
            if (useDefaultMotorControlFrequency == true)
            {
                DecoderConfiguration.ZIMO.MotorFrequencyControl = 0;
                return;
            }

            DecoderConfiguration.ZIMO.MotorFrequencyControl = (byte)PlaceValue.SetPlaceValues(EMKGap, EMKRate, 0);
            CV9Configuration = Subline.Create(new List<uint> { 9 });
        }

        [ObservableProperty]
        string cV9Configuration = Subline.Create(new List<uint> { 9 });


        // ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57
        [ObservableProperty]
        internal bool motorControlReferenceVoltageAutomaticMode;

        [ObservableProperty]
        internal string motorControlReferenceVoltage = "";

        [ObservableProperty]
        internal byte motorControlReferenceValue;
        partial void OnMotorControlReferenceValueChanged(byte value)
        {
            if (MotorControlReferenceVoltageAutomaticMode == false)
            {
                MotorControlReferenceVoltage = (value / 10).ToString() + " V";
                DecoderConfiguration.ZIMO.MotorReferenceVoltage = value;
            }
            else
            {
                DecoderConfiguration.ZIMO.MotorReferenceVoltage = 0;
            }
            CV57Configuration = Subline.Create(new List<uint> { 57 });
        }

        // ZIMO: MS specific motor reference voltage feature in CV57 (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57)
        [ObservableProperty]
        internal bool zIMOMSMotorControlReferenceVoltageAutomaticMode;
        partial void OnZIMOMSMotorControlReferenceVoltageAutomaticModeChanged(bool value)
        {
            // We check if the user would like to the automatic configuration.
            if (value == false)
            {
                //  The user would like to use specific voltage settings. If we have already valid voltage settings
                //  (e.g. from a decoder upload), we use them. Otherwise we set a default value of 1V.
                if (DecoderConfiguration.ZIMO.MotorReferenceVoltage == 0) ZIMOMSmotorControlReferenceValue = 1;
            }
            else
            {
                ZIMOMSmotorControlReferenceValue = 0;
            }
            CV57Configuration = Subline.Create(new List<uint> { 57 });
        }

        [ObservableProperty]
        internal string zIMOMSmotorControlReferenceVoltage = "";

        [ObservableProperty]
        internal byte zIMOMSmotorControlReferenceValue;
        partial void OnZIMOMSmotorControlReferenceValueChanged(byte value)
        {
            if (value != 0)
            {
                DecoderConfiguration.ZIMO.MotorReferenceVoltage = (byte)(value + (byte)100);
                ZIMOMSmotorControlReferenceVoltage = (DecoderConfiguration.ZIMO.MotorReferenceVoltage / 10).ToString() + " V (" + DecoderConfiguration.ZIMO.MotorReferenceVoltage.ToString() + ")";
            }
            else
            {
                DecoderConfiguration.ZIMO.MotorReferenceVoltage = 0;
            }
            CV57Configuration = Subline.Create(new List<uint> { 57 });
        }

        [ObservableProperty]
        string cV57Configuration = Subline.Create(new List<uint> { 57 });


        // ZIMO: MX decoder motor control PID settings CV56
        [ObservableProperty]
        internal ObservableCollection<string> availableMotorControlPIDMotorTypes;

        [ObservableProperty]
        internal string selectedMotorControlPIDMotorType = "";
        partial void OnSelectedMotorControlPIDMotorTypeChanged(string value)
        {
            int MotorType = 0;
            if (value == ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.Normal))
            {
                MotorType = 0;
            }
            else
            {
                MotorType = 1;
            }
            DecoderConfiguration.ZIMO.MotorPIDSettings = (byte)PlaceValue.SetPlaceValues(MotorControlPIDIntegralValue, MotorControlPIDProportionalValue, MotorType);
            CV56Configuration = Subline.Create(new List<uint> { 56 });
        }

        [ObservableProperty]
        internal int motorControlPIDIntegralValue;
        partial void OnMotorControlPIDIntegralValueChanged(int value)
        {
            int MotorType = 0;
            if (SelectedMotorControlPIDMotorType == ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.Normal))
            {
                MotorType = 0;
            }
            else
            {
                MotorType = 1;
            }
            DecoderConfiguration.ZIMO.MotorPIDSettings = (byte)PlaceValue.SetPlaceValues(MotorControlPIDIntegralValue, MotorControlPIDProportionalValue, MotorType);
            CV56Configuration = Subline.Create(new List<uint> { 56 });
        }

        [ObservableProperty]
        internal int motorControlPIDProportionalValue;
        partial void OnMotorControlPIDProportionalValueChanged(int value)
        {
            int MotorType = 0;
            if (SelectedMotorControlPIDMotorType == ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.Normal))
            {
                MotorType = 0;
            }
            else
            {
                MotorType = 1;
            }
            DecoderConfiguration.ZIMO.MotorPIDSettings = (byte)PlaceValue.SetPlaceValues(MotorControlPIDIntegralValue, MotorControlPIDProportionalValue, MotorType);
            CV56Configuration = Subline.Create(new List<uint> { 56 });
        }

        [ObservableProperty]
        string cV56Configuration = Subline.Create(new List<uint> { 56 });




        #endregion

        #endregion

        #region REGION: CONSTRUCTOR

        public MotorCharacteristicsViewModel()
        {
            foreach (ConfigurationVariableType item in DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV)
            {
                ExtendedSpeedCurveValues!.Add(new ConfigurationVariableType() { Number = item.Number, Value = item.Value });
            }

            ExtendedSpeedCurveValues!.CollectionChanged += ExtendedSpeedCurveValues_CollectionChanged;

            availableMotorControlFrequencyTypes = new ObservableCollection<String>(ZIMOEnumConverter.GetAvailableMotorControlFrequencyTypes());
            availableMotorControlPIDMotorTypes = new ObservableCollection<String>(ZIMOEnumConverter.GetAvailableMotorControlPIDMotorTypes());

            OnGetDataFromDecoderSpecification();
            OnGetDecoderConfiguration();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDecoderConfiguration();
                });
            });

            WeakReferenceMessenger.Default.Register<DecoderSpecificationUpdatedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDecoderSpecification();
                });
            });

        }

        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// This event handler is called if the user changes one of the extended speed curve values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtendedSpeedCurveValues_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ExtendedSpeedCurveValues == null) return;

            ExtendedSpeedCurveType tempSpeedSettings = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues;
            foreach (ConfigurationVariableType item in ExtendedSpeedCurveValues)
            {
                if (item != null)
                {
                    ConfigurationVariableType? findCV = Array.Find(tempSpeedSettings.CV, cv => cv.Number == item.Number);
                    if (findCV != null) findCV.Value = item.Value;
                }
            }
            DecoderConfiguration.RCN225.ExtendedSpeedCurveValues = tempSpeedSettings;

        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // RCN225: Maximum speed in CV5
            MaximumSpeed = DecoderConfiguration.RCN225.MaximumSpeed;
            if ((DecoderConfiguration.RCN225.MaximumSpeed == 0) || (DecoderConfiguration.RCN225.MaximumSpeed == 1))
            {
                MaximumSpeedDefaultUsed = true;
            }
            else
            {
                MaximumSpeedDefaultUsed = false;
            }
            MaximumSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.RCN225.MaximumSpeed, 255);
            CV5Configuration = Subline.Create(new List<uint> { 5 });

            // RCN225: Minimum speed in CV2
            MinimumSpeed = DecoderConfiguration.RCN225.MinimumSpeed;
            MinimumSpeedValueDescription = GetMinimumSpeedLabel(DecoderConfiguration.RCN225.MinimumSpeed, 255);
            CV2Configuration = Subline.Create(new List<uint> { 2 });

            // RCN225: Medium speed in CV6
            MediumSpeed = DecoderConfiguration.RCN225.MediumSpeed;
            MediumSpeedValueDescription = GetMediumSpeedLabel();
            CV6Configuration = Subline.Create(new List<uint> { 6 });

            // RCN225: Speed curve selection (standard or extended) in CV29 (RCN225_SPEEDTABLE_CV29_4)
            ExtendedSpeedCurveEnabled = DecoderConfiguration.RCN225.ExtendedSpeedCurveEnabled;
            CV29Configuration = Subline.Create(new List<uint> { 29 });

            // ZIMO: MX decoder motor control frequency in CV9 (ZIMO_MXMOTORCONTROLFREQUENCY_CV9)
            if (ZIMO_MXMOTORCONTROLFREQUENCY_CV9 == true)
            {
                // Check if CV9 is set to 0 -> using default values
                if (DecoderConfiguration.ZIMO.MotorFrequencyControl == 0)
                {
                    UseDefaultMotorControlFrequency = true;
                    SelectedMotorControlFrequqencyType = ZIMOEnumConverter.GetMotorControlFrequencyTypeDescription(ZIMO.MotorControlFrequencyTypes.HighFrequency);
                }
                else
                {
                    //  In CV9 user specific values are used.
                    UseDefaultMotorControlFrequency = false;

                    //  Check if high frequency is used ..
                    if (DecoderConfiguration.ZIMO.MotorFrequencyControl < 100)
                    {
                        //  Set the type = HighFrequency
                        SelectedMotorControlFrequqencyType = ZIMOEnumConverter.GetMotorControlFrequencyTypeDescription(ZIMO.MotorControlFrequencyTypes.HighFrequency);

                        //  Set the EMK Rate. Its the tens place in CV9
                        int emkGapTemp = 0; int emkRateTemp = 0; int temp;
                        PlaceValue.GetPlaceValues(DecoderConfiguration.ZIMO.MotorFrequencyControl, out emkGapTemp, out emkRateTemp, out temp);
                        EMKRate = emkRateTemp;
                        EMKGap = emkGapTemp;
                    }
                    // ... low frequency is used.
                    else
                    {
                        SelectedMotorControlFrequqencyType = ZIMOEnumConverter.GetMotorControlFrequencyTypeDescription(ZIMO.MotorControlFrequencyTypes.LowFrequency);
                        LowFrequency = DecoderConfiguration.ZIMO.MotorFrequencyControl;
                    }
                }
                CV9Configuration = Subline.Create(new List<uint> { 9 });
            }

            // ZIMO: MX decoder motor control reference voltage in CV57 (ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57)
            if (ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 == true)
            {
                MotorControlReferenceValue = DecoderConfiguration.ZIMO.MotorReferenceVoltage;
                MotorControlReferenceVoltage = (MotorControlReferenceValue / 10).ToString() + " V";
                if (DecoderConfiguration.ZIMO.MotorReferenceVoltage == 0)
                {
                    MotorControlReferenceVoltageAutomaticMode = true;
                }
                else
                {
                    MotorControlReferenceVoltageAutomaticMode = false;
                }
                CV57Configuration = Subline.Create(new List<uint> { 57 });
            }

            // ZIMOs MS specific motor reference voltage feature in CV57 (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57)
            if (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 == true)
            {
                if (DecoderConfiguration.ZIMO.MotorReferenceVoltage < 100)
                {
                    ZIMOMSMotorControlReferenceVoltageAutomaticMode = true;
                    ZIMOMSmotorControlReferenceValue = 1;
                }
                else
                {
                    ZIMOMSmotorControlReferenceValue = (byte)(DecoderConfiguration.ZIMO.MotorReferenceVoltage - 100);
                    ZIMOMSMotorControlReferenceVoltageAutomaticMode = false;
                }
                ZIMOMSmotorControlReferenceVoltage = (DecoderConfiguration.ZIMO.MotorReferenceVoltage / 10).ToString() + " V (" + DecoderConfiguration.ZIMO.MotorReferenceVoltage.ToString() + ")";
                CV57Configuration = Subline.Create(new List<uint> { 57 });
            }

            // ZIMO: MX decoder motor control PID settings CV56
            if (DecoderConfiguration.ZIMO.MotorPIDSettings < 100)
            {
                SelectedMotorControlPIDMotorType = ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.Normal);
            }
            else
            {
                SelectedMotorControlPIDMotorType = ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.BellAnchor);
            }
            int pValue, iValue, motorType;
            PlaceValue.GetPlaceValues(DecoderConfiguration.ZIMO.MotorPIDSettings, out iValue, out pValue, out motorType);
            MotorControlPIDProportionalValue = pValue;
            MotorControlPIDIntegralValue = iValue;
            CV56Configuration = Subline.Create(new List<uint> { 56 });

            // DOEHLER AND HAAS: Maximum speed in CV5 (DOEHLERANDHAASS_MAXIMALSPEED_CV5)
            MaximumDoehlerAndHaasSpeed = DecoderConfiguration.RCN225.MaximumSpeed;
            if ((DecoderConfiguration.RCN225.MaximumSpeed == 0) || (DecoderConfiguration.RCN225.MaximumSpeed == 1))
            {
                MaximumSpeedDoehlerAndHaasDefaultUsed = true;
            }
            else
            {
                MaximumSpeedDoehlerAndHaasDefaultUsed = false;
            }
            MaximumSpeedDoehlerAndHaasValueDescription = GetDoehlerAndHaasMaximumSpeedLabel();
            CV5Configuration = Subline.Create(new List<uint> { 5 });

            // DÖHLER & HAAS: Motor impuls width setting in CV49 (DOEHLERHAAS_MOTORIMPULSWIDTH_CV49)
            ImpulsWidthValue = DecoderConfiguration.DoehlerHaas.MotorImpulsWidth;
            UpdateImpulsWidthTime(DecoderConfiguration.DoehlerHaas.MotorImpulsWidth);
            CV49Configuration = Subline.Create(new List<uint> { 49 });

            // Piko SmartDecoder V4.1: Minimum speed in CV2 (PIKOSMARTDECODER41_MINIMALSPEED_CV2)
            if (PIKOSMARTDECODER_MINIMUMSPEED_CV2 == true)
            {
                MinimumPikoSmartDecoderSpeed = DecoderConfiguration.PikoSmartDecoderV41.MinimumSpeed;
                MinimumPikoSmartDecoderSpeedValueDescription = GetMinimumSpeedLabel(DecoderConfiguration.PikoSmartDecoderV41.MinimumSpeed, 63);
                CV2Configuration = Subline.Create(new List<uint> { 2 });
            }

            // Piko SmartDecoder V4.1: Maximum speed in CV5 (PIKOSMARTDECODER41_MAXIMALSPEED_CV5)
            if (PIKOSMARTDECODER_MAXIMUMSPEED_CV5 == true)
            {
                MaximumPikoSmartDecoderSpeed = DecoderConfiguration.PikoSmartDecoderV41.MaximumSpeed;
                MaximumPikoSmartDecoderSpeedValueDescription = GetMaximumSpeedLabel(DecoderConfiguration.PikoSmartDecoderV41.MaximumSpeed, 63);
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }



        }

        /// <summary>
        /// This event handler reacts to the DecoderSpecificationUpdatedMessage message. It will fetch
        /// the supported features of the currently selected decode specification and update the local properties
        /// in this view model.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {

            //  RCN225
            RCN225_MAXIMALSPEED_CV5 = DecoderSpecification.RCN225_MAXIMALSPEED_CV5;
            RCN225_MEDIUMSPEED_CV6 = DecoderSpecification.RCN225_MEDIUMSPEED_CV6;
            RCN225_MINIMALSPEED_CV2 = DecoderSpecification.RCN225_MINIMALSPEED_CV2;
            RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X = DecoderSpecification.RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X;
            RCN225_SPEEDTABLE_CV29_4 = DecoderSpecification.RCN225_SPEEDTABLE_CV29_4;

            //  DOEHLER AND HAAS
            DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 = DecoderSpecification.DOEHLERHAAS_MOTORIMPULSWIDTH_CV49;
            DOEHLERANDHAASS_MAXIMALSPEED_CV5 = DecoderSpecification.DOEHLERANDHAASS_MAXIMALSPEED_CV5;

            //  ZIMO
            ZIMO_MXMOTORCONTROLFREQUENCY_CV9 = DecoderSpecification.ZIMO_MXMOTORCONTROLFREQUENCY_CV9;
            ZIMO_MXMOTORCONTROLPID_CV56 = DecoderSpecification.ZIMO_MXMOTORCONTROLPID_CV56;
            ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57 = DecoderSpecification.ZIMO_MXMOTORCONTROLREFERENCEVOLTAGE_CV57;
            ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 = DecoderSpecification.ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57;

            // PIKO SmartDecoder V4.1 (PIKOSMARTDECODER41_MINIMALSPEED_CV2)
            PIKOSMARTDECODER_MINIMUMSPEED_CV2 = DecoderSpecification.PIKOSMARTDECODER_MINIMALSPEED_CV2;
            PIKOSMARTDECODER_MAXIMUMSPEED_CV5 = DecoderSpecification.PIKOSMARTDECODER_MAXIMUMSPEED_CV5;

        }

        /// <summary>
        /// Returns the label for the mimum speed of CV2 (the value and a percentage value).
        /// </summary>
        /// <returns></returns>
        private string GetMinimumSpeedLabel(byte speed, byte maxValue)
        {
            return speed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(speed, maxValue) + " %)";
        }

        /// <summary>
        /// Returns the label for the medium speed.
        /// </summary>
        /// <returns></returns>
        private string GetMediumSpeedLabel()
        {
            return MediumSpeed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(MediumSpeed, 255) + " %)";
        }

        /// <summary>
        /// Returns the label for the maximum speed value of CV5 (the value and a percentage value).
        /// </summary>
        /// <param name="maxValue">The maximum value of CV5 (e.g. 255)</param>
        /// <returns>A formated string showing the value of CV5 and a percentage value.</returns>
        private string GetMaximumSpeedLabel(byte speed, byte maxValue)
        {
            return speed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(speed, maxValue) + " %)";
        }

        /// <summary>
        /// Returns the label for the maximum speed.
        /// </summary>
        /// <returns></returns>
        private string GetDoehlerAndHaasMaximumSpeedLabel()
        {
            return MaximumDoehlerAndHaasSpeed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(MaximumDoehlerAndHaasSpeed, 255) + " %)";
        }

        private void UpdateImpulsWidthTime(byte value)
        {
            switch (value)
            {
                case 0: ImpulsWidthTime = "1 ms"; break;
                case 1: ImpulsWidthTime = "2 ms"; break;
                case 2: ImpulsWidthTime = "4 ms"; break;
                case 3: ImpulsWidthTime = "8 ms"; break;
            }

        }

        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Sets a linear default curve for the extended speed curve values (CV67 to CV94).
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private void SetLinearDefaultCurve()
        {

            ExtendedSpeedCurveValues!.Clear();
            foreach (ConfigurationVariableType item in DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV)
            {
                switch (item.Number)
                {
                    case 67: item.Value = 1; break;
                    case 68: item.Value = 10; break;
                    case 69: item.Value = 20; break;
                    case 70: item.Value = 29; break;
                    case 71: item.Value = 39; break;
                    case 72: item.Value = 48; break;
                    case 73: item.Value = 57; break;
                    case 74: item.Value = 67; break;
                    case 75: item.Value = 76; break;
                    case 76: item.Value = 86; break;
                    case 77: item.Value = 95; break;
                    case 78: item.Value = 104; break;
                    case 79: item.Value = 114; break;
                    case 80: item.Value = 123; break;
                    case 81: item.Value = 133; break;
                    case 82: item.Value = 142; break;
                    case 83: item.Value = 152; break;
                    case 84: item.Value = 161; break;
                    case 85: item.Value = 170; break;
                    case 86: item.Value = 180; break;
                    case 87: item.Value = 189; break;
                    case 88: item.Value = 199; break;
                    case 89: item.Value = 208; break;
                    case 90: item.Value = 217; break;
                    case 91: item.Value = 227; break;
                    case 92: item.Value = 236; break;
                    case 93: item.Value = 246; break;
                    case 94: item.Value = 255; break;
                }

                ConfigurationVariableType newItem = new ConfigurationVariableType();
                newItem.Number = item.Number;
                newItem.Value = item.Value;
                newItem.DeqSecSupported = item.DeqSecSupported;
                newItem.Description = item.Description;
                newItem.Enabled = item.Enabled;

                ExtendedSpeedCurveValues.Add(newItem);
            }

        }

        /// <summary>
        /// Decreases the extended speed curve values (CV67 top CV94).
        /// </summary>
        [RelayCommand]
        private void DecreaseExtendedSpeedCurve()
        {
            try
            {
                ExtendedSpeedCurveValues!.Clear();
                foreach (ConfigurationVariableType item in DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV)
                {
                    if (item.Value > 0) item.Value--;

                    ConfigurationVariableType newItem = new ConfigurationVariableType();
                    newItem.Number = item.Number;
                    newItem.Value = item.Value;
                    newItem.DeqSecSupported = item.DeqSecSupported;
                    newItem.Description = item.Description;
                    newItem.Enabled = item.Enabled;

                    ExtendedSpeedCurveValues.Add(newItem);
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Increases the extended speed curve values (CV67 top CV94)
        /// </summary>
        [RelayCommand]
        private void IncreaseExtendedSpeedCurve()
        {
            ExtendedSpeedCurveValues!.Clear();
            foreach (ConfigurationVariableType item in DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV)
            {
                if (item.Value <= 254) item.Value++;

                ConfigurationVariableType newItem = new ConfigurationVariableType();
                newItem.Number = item.Number;
                newItem.Value = item.Value;
                newItem.DeqSecSupported = item.DeqSecSupported;
                newItem.Description = item.Description;
                newItem.Enabled = item.Enabled;

                ExtendedSpeedCurveValues.Add(newItem);
            }
        }

        /// <summary>
        /// Sets a curve for the extended speed curve values (CV67 to CV94) that is optimized for slow speed operation.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private void SetCurvedDefaultCurve()
        {
            ExtendedSpeedCurveValues!.Clear();
            foreach (ConfigurationVariableType item in DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV)
            {
                switch (item.Number)
                {
                    case 67: item.Value = 1; break;
                    case 68: item.Value = 3; break;
                    case 69: item.Value = 5; break;
                    case 70: item.Value = 9; break;
                    case 71: item.Value = 12; break;
                    case 72: item.Value = 16; break;
                    case 73: item.Value = 21; break;
                    case 74: item.Value = 27; break;
                    case 75: item.Value = 33; break;
                    case 76: item.Value = 39; break;
                    case 77: item.Value = 46; break;
                    case 78: item.Value = 54; break;
                    case 79: item.Value = 62; break;
                    case 80: item.Value = 71; break;
                    case 81: item.Value = 80; break;
                    case 82: item.Value = 90; break;
                    case 83: item.Value = 101; break;
                    case 84: item.Value = 112; break;
                    case 85: item.Value = 124; break;
                    case 86: item.Value = 136; break;
                    case 87: item.Value = 149; break;
                    case 88: item.Value = 162; break;
                    case 89: item.Value = 176; break;
                    case 90: item.Value = 191; break;
                    case 91: item.Value = 206; break;
                    case 92: item.Value = 222; break;
                    case 93: item.Value = 238; break;
                    case 94: item.Value = 255; break;
                }

                ConfigurationVariableType newItem = new ConfigurationVariableType();
                newItem.Number = item.Number;
                newItem.Value = item.Value;
                newItem.DeqSecSupported = item.DeqSecSupported;
                newItem.Description = item.Description;
                newItem.Enabled = item.Enabled;

                ExtendedSpeedCurveValues.Add(newItem);
            }
        }

        #endregion

    }

}
