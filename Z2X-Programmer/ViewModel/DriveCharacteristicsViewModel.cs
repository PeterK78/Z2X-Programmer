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
using Z21Lib.Events;
using Z2XProgrammer.Communication;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class DriveCharacteristicsViewModel : ObservableObject
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

        #region REGION: DECODER FEATURES

        //  RCN225: Drive direction in CV29.0 (RCN225_DIRECTION_CV29_0)
        [ObservableProperty]
        bool rCN225_DIRECTION_CV29_0;

        [ObservableProperty]
        bool rCN225_CONSISTADDRESS_CV19X;

        [ObservableProperty]
        bool rCN225_HLU_CV27_2;

        [ObservableProperty]
        bool rCN225_ABC_CV27_X;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        //  RCN225: Drive direction in CV29.0 (RCN225_DIRECTION_CV29_0)
        [ObservableProperty]
        bool directionReversal;
        partial void OnDirectionReversalChanged(bool value)
        {
            DecoderConfiguration.RCN225.DirectionReversal = value;
            CV29Configuration = Subline.Create(new List<uint>{29});
        }
        [ObservableProperty]
        string cV29Configuration = Subline.Create(new List<uint>{29});

        //  RCN225: Drive direction for consist mode in CV19 (RCN225_CONSISTADDRESS_CV19X)
        [ObservableProperty]
        bool directionConsistModeReveral;
        partial void OnDirectionConsistModeReveralChanged(bool value)
        {
            DecoderConfiguration.RCN225.DirectionReversalConsistMode = value;
            CV19Configuration = Subline.Create(new List<uint> { 19 });  
        }        
        [ObservableProperty]
        string cV19Configuration = Subline.Create(new List<uint>{19});

        //  RCN225: Speed steps mode in CV29.0 (RCN225_SPEEDSTEPSMODE_CV29_0)
        [ObservableProperty]
        internal ObservableCollection<string> availableSpeedStepModes;
        
        [ObservableProperty]
        internal string selectedSpeedStepsMode = "";
        partial void OnSelectedSpeedStepsModeChanged(string value)
        {
            if ((value == null) || (value == "")) return;
            DecoderConfiguration.RCN225.SpeedStepsMode = NMRAEnumConverter.GetDCCSpeedStepsModeFromDescription(value);
            AccelerationRateTime = GetAccelerationRateTimeLabel();
            CV29Configuration = Subline.Create(new List<uint> { 29 });
        }

        [ObservableProperty]
        internal string currentlySelectedSpeedSteps = "";


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

        [ObservableProperty]
        internal byte accelerationRate;
        partial void OnAccelerationRateChanged(byte value)
        {
            DecoderConfiguration.RCN225.AccelerationRate = value;
            AccelerationRateTime = GetAccelerationRateTimeLabel();
            CV3Configuration = Subline.Create(new List<uint>{3});
        }

        [ObservableProperty]
        internal string accelerationRateTime = "";

        [ObservableProperty]
        string cV3Configuration = Subline.Create(new List<uint>{3});

        // RCN225: Decleration rate CV4.
        [ObservableProperty]
        internal bool decelerationRateEnabled;
        partial void OnDecelerationRateEnabledChanged(bool value)
        {
            //  Check if the deceleration rate is enabled.
            if (value == true)
            {
                // The user would like to use the user specific deceleration rate. We check if we already have a valid value for the deceleration rate,
                // if not we set it to 1 (according to the ZIMO manual, no recommendation in the RCN225 available). 
                if (DecoderConfiguration.RCN225.DecelerationRate == 0) DecelerationRate = 1;
            }   
            else
            {
                //  We turn off the deceleration rate.
                DecelerationRate = 0;
            }
            CV4Configuration = Subline.Create(new List<uint> { 4 });
        }

        [ObservableProperty]
        internal byte decelerationRate;
        partial void OnDecelerationRateChanged(byte value)
        {
            DecoderConfiguration.RCN225.DecelerationRate = value;
            DecelerationRateTime = GetDecelerationRateTimeLabel();
            CV4Configuration = Subline.Create(new List<uint> { 4 });
        }

        [ObservableProperty]
        internal string decelerationRateTime = "";

        [ObservableProperty]
        string cV4Configuration = Subline.Create(new List<uint>{4});


        // RCN225: ABC breaking track function in CV27 (RCN225_ABC_CV27_X)
        [ObservableProperty]
        internal ObservableCollection<string> availableABCBreakModes;

        [ObservableProperty]
        internal string selectedABCBreakMode = "";
        partial void OnSelectedABCBreakModeChanged(string value)
        {
            if ((value == null) || (value == "")) return;
            DecoderConfiguration.RCN225.ABCBreakMode = NMRAEnumConverter.GetDCCABCBreakModeFromDescription(value);
            CV27Configuration = Subline.Create(new List<uint> { 27 });
        }

        [ObservableProperty]
        string cV27Configuration = Subline.Create(new List<uint>{27});

        // RCN225: HLU function in CV27 (RCN225_HLU_CV27_2)
        [ObservableProperty]
        bool hluEnabled;
        partial void OnHluEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.HLUEnabled = value;
            CV27Configuration = Subline.Create(new List<uint>{27});
        }

        #endregion

        #region REGION: CONSTRUCTOR

        public DriveCharacteristicsViewModel()
        {
            AvailableSpeedStepModes = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCSpeedStepModes());
            AvailableABCBreakModes = new ObservableCollection<string>(NMRAEnumConverter.GetAvailableDCCABCBreakModes());

            CommandStation.Z21.OnLocoInfoReceived += OnLocoInfoReceived; 

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
        /// Event handler for the OnLocoInfoReceived event of the Z21 library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocoInfoReceived(object? sender, LocoInfoEventArgs e)
        {
            try
            {
                CurrentlySelectedSpeedSteps = e.MaxSpeedSteps.ToString();
            }
            catch (Exception ex)
            {
                Logger.PrintDevConsole("DriveCharacteristicsViewModel.OnLocoInfoReceived: " + ex.Message);
            }
        }

        /// <summary>
        /// Returns the label for the acceleration rate.
        /// </summary>
        /// <returns></returns>
        private string GetAccelerationRateTimeLabel()
        {
            return AccelerationRate.ToString() + " (" + String.Format("{0:N1}", NMRA.CalculateAccDecRateTimes(AccelerationRate, DecoderConfiguration.RCN225.SpeedStepsMode)) + " s)";
        }

        /// <summary>
        /// Returns the label for the deceleration rate.
        /// </summary>
        /// <returns></returns>
        private string GetDecelerationRateTimeLabel()
        {
            return DecelerationRate.ToString() + " (" + String.Format("{0:N1}", NMRA.CalculateAccDecRateTimes(DecelerationRate, DecoderConfiguration.RCN225.SpeedStepsMode)) + " s)";
        }


        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        public void OnGetDataFromDecoderSpecification()
        {
            RCN225_DIRECTION_CV29_0 = DecoderSpecification.RCN225_DIRECTION_CV29_0;
            RCN225_CONSISTADDRESS_CV19X = DecoderSpecification.RCN225_CONSISTADDRESS_CV19X;
            RCN225_HLU_CV27_2 = DecoderSpecification.RCN225_HLU_CV27_2;
            RCN225_ABC_CV27_X = DecoderSpecification.RCN225_ABC_CV27_X;
        }


        
        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  RCN225: Speed steps mode in CV29.0 (RCN225_SPEEDSTEPSMODE_CV29_0)
            SelectedSpeedStepsMode = NMRAEnumConverter.GetDCCSpeedStepModeDescription(DecoderConfiguration.RCN225.SpeedStepsMode);
            CV29Configuration = Subline.Create(new List<uint> { 29 });

            // RCN225: Acceleration rate in CV3
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
            CV3Configuration = Subline.Create(new List<uint>{3});

            // RCN225: Decleration rate CV4.
            DecelerationRate = DecoderConfiguration.RCN225.DecelerationRate;
            if(DecelerationRate == 0)
            {
                DecelerationRateEnabled = false;
            }
            else
            {
                DecelerationRateEnabled = true;
            }
            DecelerationRateTime = GetDecelerationRateTimeLabel();
            CV4Configuration = Subline.Create(new List<uint> { 4 });

            //  RCN225: Drive direction in CV29.0 (RCN225_DIRECTION_CV29_0)
            if (RCN225_DIRECTION_CV29_0 == true)
            {
                DirectionReversal = DecoderConfiguration.RCN225.DirectionReversal;
                CV29Configuration = Subline.Create(new List<uint>{29});
            }

            // RCN225: HLU function in CV27 (RCN225_HLU_CV27_2)
            if (RCN225_HLU_CV27_2 == true)
            {
                HluEnabled = DecoderConfiguration.RCN225.HLUEnabled;
                CV27Configuration = Subline.Create(new List<uint> { 27 });
            }

            // RCN225: ABC breaking track function in CV27 (RCN225_ABC_CV27_X)
            if (RCN225_ABC_CV27_X == true)
            {
                SelectedABCBreakMode = NMRAEnumConverter.GetDCCABCBreakModeDescription(DecoderConfiguration.RCN225.ABCBreakMode);
                CV27Configuration = Subline.Create(new List<uint> { 27 });
            }

            //  RCN225: Drive direction for consist mode in CV19 (RCN225_CONSISTADDRESS_CV19X)
            if (RCN225_CONSISTADDRESS_CV19X == true)
            {
                DirectionConsistModeReveral = DecoderConfiguration.RCN225.DirectionReversalConsistMode;
                CV19Configuration = Subline.Create(new List<uint> { 19 }); 
            }

        }

        #endregion

        #region REGION: COMMANDS
        /// <summary>
        /// Decreases the acceleration rate of CV3 by 1.
        /// </summary>
        [RelayCommand]
        void DecreaseAccelarationRateCV3()
        {
            if (AccelerationRate > 0) AccelerationRate--;
        }

        /// <summary>
        /// Increases the acceleration rate of CV3 by 1.
        /// </summary>
        [RelayCommand]
        void IncreaseAccelarationRateCV3()
        {
            if (AccelerationRate < 255) AccelerationRate++;
        }

        /// <summary>
        /// Decreases the deceleration rate of CV4 by 1.
        /// </summary>
        [RelayCommand]
        void DecreaseDecelerationRateCV4()
        {
            if (DecelerationRate > 0) DecelerationRate--;
        }

        /// <summary>
        /// Increases the deceleration rate of CV4 by 1.
        /// </summary>
        [RelayCommand]
        void IncreaseDecelerationRateCV4()
        {
            if (DecelerationRate < 255) DecelerationRate++;
        }

        #endregion

    }
}
