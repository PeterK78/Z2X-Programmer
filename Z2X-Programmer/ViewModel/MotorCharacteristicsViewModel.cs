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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;
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
        int limitMinimumMaxSpeedCV5=2;
        
        [ObservableProperty]
        int limitMinimumZIMOMSmotorControlReferenceVoltage = 1;
        
        #endregion

        #region REGION: DECODER FEATURES

        //  RCN225_SPEEDTABLE_CV29_4
        [ObservableProperty]
        bool rCN225_SPEEDTABLE_CV29_4;
      
        [ObservableProperty]
        bool rCN225_MEDIUMSPEED_CV6;

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
        #endregion

        #region REGION: PUBLIC PROPERTIES

        // RCN225: Maximum speed in CV5
        [ObservableProperty]
        internal string maximumSpeedValueDescription = "";

        [ObservableProperty]
        internal bool maximumSpeedDefaultUsed;
        partial void OnMaximumSpeedDefaultUsedChanged(bool value)
        {
            if(value == false)
            {
                LimitMinimumMaxSpeedCV5 = 2;
                MaximumSpeed = 100;
                MaximumSpeedValueDescription = GetMaximumSpeedLabel();
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
            else
            {
                LimitMinimumMaxSpeedCV5 = 0;
                MaximumSpeed = 0;
                MaximumSpeedValueDescription = GetMaximumSpeedLabel();
                CV5Configuration = Subline.Create(new List<uint> { 5 });
            }
        }

        [ObservableProperty]
        internal byte maximumSpeed;
        partial void OnMaximumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MaximumSpeed = value;
            MaximumSpeedValueDescription = GetMaximumSpeedLabel();
            CV5Configuration = Subline.Create(new List<uint> { 5 });
        }

        [ObservableProperty]
        string cV5Configuration = Subline.Create(new List<uint>{5});


        // RCN225: Medium speed in CV6
        [ObservableProperty]
        internal string mediumSpeedValueDescription = "";

        [ObservableProperty]
        internal byte mediumSpeed;
        partial void OnMediumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MediumSpeed = value;
            MediumSpeedValueDescription = GetMediumSpeedLabel();
            CV6Configuration = Subline.Create(new List<uint>{6});
        }

        [ObservableProperty]
        string cV6Configuration = Subline.Create(new List<uint>{6});


        // RCN225: Minimum speed in CV2
        [ObservableProperty]
        internal string minimumSpeedValueDescription = "";

        [ObservableProperty]
        internal byte minimumSpeed;
        partial void OnMinimumSpeedChanged(byte value)
        {
            DecoderConfiguration.RCN225.MinimumSpeed = value;
            MinimumSpeedValueDescription = GetMinimumSpeedLabel();
            CV2Configuration = Subline.Create(new List<uint>{2});
        }
        [ObservableProperty]
        string cV2Configuration = Subline.Create(new List<uint>{2});
        

        // RCN225: Speed curve selection (standard or extended) in CV29 (RCN225_SPEEDTABLE_CV29_4)
        [ObservableProperty]
        internal bool extendedSpeedCurveEnabled;
        partial void OnExtendedSpeedCurveEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.ExtendedSpeedCurveEnabled = value;
            CV29Configuration = Subline.Create(new List<uint>{29});
        }
        [ObservableProperty]
        string cV29Configuration = Subline.Create(new List<uint>{29});
        

        [ObservableProperty]
        internal byte extendedSpeedCurveValue67;
        partial void OnExtendedSpeedCurveValue67Changed(byte value)
        {
            SetExtendedSpeedCurveValue(67, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue68;
        partial void OnExtendedSpeedCurveValue68Changed(byte value)
        {
            SetExtendedSpeedCurveValue(68, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue69;
        partial void OnExtendedSpeedCurveValue69Changed(byte value)
        {
            SetExtendedSpeedCurveValue(69, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue70;
        partial void OnExtendedSpeedCurveValue70Changed(byte value)
        {
            SetExtendedSpeedCurveValue(70, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue71;
        partial void OnExtendedSpeedCurveValue71Changed(byte value)
        {
            SetExtendedSpeedCurveValue(71, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue72;
        partial void OnExtendedSpeedCurveValue72Changed(byte value)
        {
            SetExtendedSpeedCurveValue(72, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue73;
        partial void OnExtendedSpeedCurveValue73Changed(byte value)
        {
            SetExtendedSpeedCurveValue(73, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue74;
        partial void OnExtendedSpeedCurveValue74Changed(byte value)
        {
            SetExtendedSpeedCurveValue(74, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue75;
        partial void OnExtendedSpeedCurveValue75Changed(byte value)
        {
            SetExtendedSpeedCurveValue(75, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue76;
        partial void OnExtendedSpeedCurveValue76Changed(byte value)
        {
            SetExtendedSpeedCurveValue(76, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue77;
        partial void OnExtendedSpeedCurveValue77Changed(byte value)
        {
            SetExtendedSpeedCurveValue(77, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue78;
        partial void OnExtendedSpeedCurveValue78Changed(byte value)
        {
            SetExtendedSpeedCurveValue(78, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue79;
        partial void OnExtendedSpeedCurveValue79Changed(byte value)
        {
            SetExtendedSpeedCurveValue(79, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue80;
        partial void OnExtendedSpeedCurveValue80Changed(byte value)
        {
            SetExtendedSpeedCurveValue(80, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue81;
        partial void OnExtendedSpeedCurveValue81Changed(byte value)
        {
            SetExtendedSpeedCurveValue(81, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue82;
        partial void OnExtendedSpeedCurveValue82Changed(byte value)
        {
            SetExtendedSpeedCurveValue(82, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue83;
        partial void OnExtendedSpeedCurveValue83Changed(byte value)
        {
            SetExtendedSpeedCurveValue(83, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue84;
        partial void OnExtendedSpeedCurveValue84Changed(byte value)
        {
            SetExtendedSpeedCurveValue(84, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue85;
        partial void OnExtendedSpeedCurveValue85Changed(byte value)
        {
            SetExtendedSpeedCurveValue(85, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue86;
        partial void OnExtendedSpeedCurveValue86Changed(byte value)
        {
            SetExtendedSpeedCurveValue(86, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue87;
        partial void OnExtendedSpeedCurveValue87Changed(byte value)
        {
            SetExtendedSpeedCurveValue(87, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue88;
        partial void OnExtendedSpeedCurveValue88Changed(byte value)
        {
            SetExtendedSpeedCurveValue(88, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue89;
        partial void OnExtendedSpeedCurveValue89Changed(byte value)
        {
            SetExtendedSpeedCurveValue(89, value);
        }


        [ObservableProperty]
        internal byte extendedSpeedCurveValue90;
        partial void OnExtendedSpeedCurveValue90Changed(byte value)
        {
            SetExtendedSpeedCurveValue(90, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue91;
        partial void OnExtendedSpeedCurveValue91Changed(byte value)
        {
            SetExtendedSpeedCurveValue(91, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue92;
        partial void OnExtendedSpeedCurveValue92Changed(byte value)
        {
            SetExtendedSpeedCurveValue(92, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue93;
        partial void OnExtendedSpeedCurveValue93Changed(byte value)
        {
            SetExtendedSpeedCurveValue(93, value);
        }

        [ObservableProperty]
        internal byte extendedSpeedCurveValue94;
        partial void OnExtendedSpeedCurveValue94Changed(byte value)
        {
            SetExtendedSpeedCurveValue(94, value);
        }
        

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
            CV9Configuration = Subline.Create(new List<uint>{9});
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
            CV9Configuration = Subline.Create(new List<uint>{9});
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
            CV9Configuration = Subline.Create(new List<uint>{9});
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
            CV9Configuration = Subline.Create(new List<uint>{9});
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
            CV9Configuration = Subline.Create(new List<uint>{9});
        }

        [ObservableProperty]
        string cV9Configuration = Subline.Create(new List<uint>{9});


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
                DecoderConfiguration.ZIMO.MotorReferenceVoltage = 0 ;
            }
        }

        // ZIMO: MS specific motor reference voltage feature in CV57 (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57)
        [ObservableProperty]
        internal bool zIMOMSMotorControlReferenceVoltageAutomaticMode;
        partial void OnZIMOMSMotorControlReferenceVoltageAutomaticModeChanged(bool value)
        {
            if (value == false)
            {
                LimitMinimumZIMOMSmotorControlReferenceVoltage = 1;
                ZIMOMSmotorControlReferenceValue = 1;
            }
            else
            {
                LimitMinimumZIMOMSmotorControlReferenceVoltage = 0;
                ZIMOMSmotorControlReferenceValue = 0;
            }
            CV57Configuration = Subline.Create(new List<uint>{57});
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
            CV57Configuration = Subline.Create(new List<uint>{57});
        }

        [ObservableProperty]
        string cV57Configuration = Subline.Create(new List<uint>{57});


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
            if(SelectedMotorControlPIDMotorType == ZIMOEnumConverter.GetMotorControlPIDMotorTypeDescription(ZIMO.MotorControlPIDMotorTypes.Normal))
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
            CV56Configuration = Subline.Create(new List<uint>{56});
        }

        [ObservableProperty]
        string cV56Configuration = Subline.Create(new List<uint>{56});
        

        // DÖHLER & HAAS: Motor impuls width setting in CV49 (DOEHLERHAAS_MOTORIMPULSWIDTH_CV49)
        [ObservableProperty]
        internal byte impulsWidthValue;
        partial void OnImpulsWidthValueChanged(byte value)
        {
            DecoderConfiguration.DoehlerHaas.MotorImpulsWidth = value;
            UpdateImpulsWidthTime(DecoderConfiguration.DoehlerHaas.MotorImpulsWidth);
            CV49Configuration = Subline.Create(new List<uint>{49});
        }

        [ObservableProperty]
        internal string impulsWidthTime = "";

        [ObservableProperty]
        string cV49Configuration = Subline.Create(new List<uint>{49});
        

        
        #endregion
        
        #region REGION: CONSTRUCTOR
        public MotorCharacteristicsViewModel()
        {
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

        # region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Returns the label for the mimum speed.
        /// </summary>
        /// <returns></returns>
        private string GetMinimumSpeedLabel()
        {
            return MinimumSpeed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(MinimumSpeed, 255) + " %)";
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
        /// Returns the label for the maximum speed.
        /// </summary>
        /// <returns></returns>
        private string GetMaximumSpeedLabel()
        {
            return MaximumSpeed.ToString() + " (" + (int)CVByteValueToPercentage.ToDouble(MaximumSpeed, 255) + " %)";
        }


        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            ExtendedSpeedCurveEnabled = DecoderConfiguration.RCN225.ExtendedSpeedCurveEnabled;

            MaximumSpeed = DecoderConfiguration.RCN225.MaximumSpeed;
            if ((DecoderConfiguration.RCN225.MaximumSpeed == 0) || (DecoderConfiguration.RCN225.MaximumSpeed == 1))
            {
                MaximumSpeedDefaultUsed = true;                
            }
            else
            {
                MaximumSpeedDefaultUsed = false;
            }

            MinimumSpeed = DecoderConfiguration.RCN225.MinimumSpeed;
            MediumSpeed = DecoderConfiguration.RCN225.MediumSpeed;


            ImpulsWidthValue = DecoderConfiguration.DoehlerHaas.MotorImpulsWidth;
            UpdateImpulsWidthTime(DecoderConfiguration.DoehlerHaas.MotorImpulsWidth);
            ExtendedSpeedCurveValue67 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[0].Value;
            ExtendedSpeedCurveValue68 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[1].Value;
            ExtendedSpeedCurveValue69 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[2].Value;
            ExtendedSpeedCurveValue70 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[3].Value;
            ExtendedSpeedCurveValue71 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[4].Value;
            ExtendedSpeedCurveValue72 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[5].Value;
            ExtendedSpeedCurveValue73 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[6].Value;
            ExtendedSpeedCurveValue74 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[7].Value;
            ExtendedSpeedCurveValue75 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[8].Value;
            ExtendedSpeedCurveValue76 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[9].Value;
            ExtendedSpeedCurveValue77 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[10].Value;
            ExtendedSpeedCurveValue78 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[11].Value;
            ExtendedSpeedCurveValue79 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[12].Value;
            ExtendedSpeedCurveValue80 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[13].Value;
            ExtendedSpeedCurveValue81 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[14].Value;
            ExtendedSpeedCurveValue82 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[15].Value;
            ExtendedSpeedCurveValue83 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[16].Value;
            ExtendedSpeedCurveValue84 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[17].Value;
            ExtendedSpeedCurveValue85 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[18].Value;
            ExtendedSpeedCurveValue86 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[19].Value;
            ExtendedSpeedCurveValue87 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[20].Value;
            ExtendedSpeedCurveValue88 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[21].Value;
            ExtendedSpeedCurveValue89 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[22].Value;
            ExtendedSpeedCurveValue90 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[23].Value;
            ExtendedSpeedCurveValue91 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[24].Value;
            ExtendedSpeedCurveValue92 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[25].Value;
            ExtendedSpeedCurveValue93 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[26].Value;
            ExtendedSpeedCurveValue94 = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues.CV[27].Value;

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
            }

            // ZIMOs MS specific motor reference voltage feature in CV57 (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57)
            if (ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 == true)
            {
                if(DecoderConfiguration.ZIMO.MotorReferenceVoltage < 100)
                {
                    ZIMOMSMotorControlReferenceVoltageAutomaticMode = true;
                    ZIMOMSmotorControlReferenceValue = 1;
                }
                else
                {
                    ZIMOMSMotorControlReferenceVoltageAutomaticMode = false;
                    ZIMOMSmotorControlReferenceValue = (byte)(DecoderConfiguration.ZIMO.MotorReferenceVoltage - 100);
                }
                ZIMOMSmotorControlReferenceVoltage = (DecoderConfiguration.ZIMO.MotorReferenceVoltage / 10).ToString() + " V (" + DecoderConfiguration.ZIMO.MotorReferenceVoltage.ToString() + ")";
            }


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

    }

        /// <summary>
        /// This event handler reacts to the DecoderSpecificationUpdatedMessage message. It will fetch
        /// the supported features of the currently selected decode specification and update the local properties
        /// in this view model.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            RCN225_MEDIUMSPEED_CV6 = DecoderSpecification.RCN225_MEDIUMSPEED_CV6;
            DOEHLERHAAS_MOTORIMPULSWIDTH_CV49 = DecoderSpecification.DOEHLERHAAS_MOTORIMPULSWIDTH_CV49;
            RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X = DecoderSpecification.RCN225_EXTENDEDSPEEDCURVEVALUES_CV67X;
            ZIMO_MXMOTORCONTROLFREQUENCY_CV9 = DecoderSpecification.ZIMO_MOTORCONTROLFREQUENCY_CV9;
            ZIMO_MXMOTORCONTROLPID_CV56 = DecoderSpecification.ZIMO_MXMOTORCONTROLPID_CV56;
            ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57 = DecoderSpecification.ZIMO_MSMOTORCONTROLREFERENCEVOLTAGE_CV57;
            RCN225_SPEEDTABLE_CV29_4 = DecoderSpecification.RCN225_SPEEDTABLE_CV29_4;
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

        /// <summary>
        /// Sets the value of the extended speed curve configurations CVs (CV67 - CV94).
        /// </summary>
        /// <param name="cvNumber">The number of the configuration variable (67 - 94).</param>
        /// <param name="value">The value of the CV.</param>
        private bool SetExtendedSpeedCurveValue(ushort cvNumber, byte value)
        {
            //  Range check 
            if (cvNumber > 94) return false;
            if (cvNumber < 67) return false;

            ExtendedSpeedCurveType tempSpeedSettings = DecoderConfiguration.RCN225.ExtendedSpeedCurveValues;
            
            tempSpeedSettings.CV[cvNumber - 67].Value = value;

            DecoderConfiguration.RCN225.ExtendedSpeedCurveValues = tempSpeedSettings;

            return true;
        }

        ///// <summary>
        ///// Sets the limits for all GUI elements.
        ///// </summary>
        //private void SetGUILimits()
        //{
        //    LimitMaximumZIMOMSmotorControlReferenceVoltage = 155;
        //    LimitMinimumZIMOMSmotorControlReferenceVoltage = 1;
        //}

        #endregion

    }





}
