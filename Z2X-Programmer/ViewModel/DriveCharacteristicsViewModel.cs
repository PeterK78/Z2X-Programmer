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
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class DriveCharacteristicsViewModel : ObservableObject
    {

        #region REGION: LIMITS FOR ENTRY VALIDATION
        [ObservableProperty]
        int limitMinimumAccelerationRateCV3;

        [ObservableProperty]
        int limitMaximumAccelerationRateCV3;

        [ObservableProperty]
        int limitMinimumDecelerationRateCV4;

        [ObservableProperty]
        int limitMaximumDecelerationRateCV4;

        #endregion

        #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool rCN225_DIRECTION_CV29_0;

        [ObservableProperty]
        bool rCN225_CONSISTADDRESS_CV19;

        [ObservableProperty]
        bool rCN225_HLU_CV27_2;

        [ObservableProperty]
        bool rCN225_ABC_CV27_X;

        #endregion

        #region REGION: PUBLIC PROPERTIES


        //  RCN225_ABC_CV27_X
        [ObservableProperty]
        internal ObservableCollection<string> availableABCBreakModes;

        [ObservableProperty]
        internal string selectedABCBreakMode = "";
        partial void OnSelectedABCBreakModeChanged(string value)
        {
            if ((value == null) || (value == "")) return;
            DecoderConfiguration.RCN225.ABCBreakMode = NMRAEnumConverter.GetDCCABCBreakModeFromDescription(value);
        }

        [ObservableProperty]
        bool hluEnabled;
        partial void OnHluEnabledChanged(bool value)
        {
            DecoderConfiguration.RCN225.HLUEnabled = value;
        }

        [ObservableProperty]
        bool directionConsistModeReveral;
        partial void OnDirectionConsistModeReveralChanged(bool value)
        {
            DecoderConfiguration.RCN225.DirectionReversalConsistMode = value;
        }


        [ObservableProperty]
        bool directionReversal;
        partial void OnDirectionReversalChanged(bool value)
        {
            DecoderConfiguration.RCN225.DirectionReversal = value;
        }

        [ObservableProperty]
        internal ObservableCollection<string> availableSpeedStepModes;

        [ObservableProperty]
        internal string selectedSpeedStepsMode = "";
        partial void OnSelectedSpeedStepsModeChanged(string value)
        {
            if ((value == null) || (value == "")) return;
            DecoderConfiguration.RCN225.SpeedStepsMode = NMRAEnumConverter.GetDCCSpeedStepsModeFromDescription(value);
            AccelerationRateTime = GetAccelerationRateTimeLabel();
        }

        

        [ObservableProperty]
        internal bool accelerationRateEnabled;
        partial void OnAccelerationRateEnabledChanged(bool value)
        {
            if (value == true)
            {
                AccelerationRate = DecoderConfiguration.RCN225.AccelerationRate;
            }
        }

        [ObservableProperty]
        internal byte accelerationRate;
        partial void OnAccelerationRateChanged(byte value)
        {
            DecoderConfiguration.RCN225.AccelerationRate = value;
            AccelerationRateTime = GetAccelerationRateTimeLabel();
        }

        [ObservableProperty]
        internal string accelerationRateTime = "";

        [ObservableProperty]
        internal bool decelerationRateEnabled;
        partial void OnDecelerationRateEnabledChanged(bool value)
        {
            if (value == true)
            {
                DecelerationRate = DecoderConfiguration.RCN225Backup.DecelerationRate;
            }                
        }

        [ObservableProperty]
        internal byte decelerationRate;
        partial void OnDecelerationRateChanged(byte value)
        {
            DecoderConfiguration.RCN225.DecelerationRate = value;
            DecelerationRateTime = GetDecelerationRateTimeLabel();
        }

        [ObservableProperty]
        internal string decelerationRateTime = "";

        #endregion

        #region REGION: CONSTRUCTOR

        public DriveCharacteristicsViewModel()
        {
            AvailableSpeedStepModes = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableDCCSpeedStepModes());
            AvailableABCBreakModes = new ObservableCollection<string>(NMRAEnumConverter.GetAvailableDCCABCBreakModes());

            SetGUILimits();

            OnGetDataFromDataStore();
            OnGetDataFromDecoderSpecification();

                WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        OnGetDataFromDataStore();
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
        /// Sets the limits for all GUI elements.
        /// </summary>
        private void SetGUILimits()
        {
            LimitMinimumAccelerationRateCV3 = 1;
            LimitMaximumAccelerationRateCV3 = 255;

            LimitMinimumDecelerationRateCV4 = 1;
            LimitMaximumDecelerationRateCV4 = 255;
        }

        /// <summary>
        /// Returns the label for the acceleration rate.
        /// </summary>
        /// <returns></returns>
        private string GetAccelerationRateTimeLabel()
        {
            return AccelerationRate.ToString() + " (" + String.Format("{0:N2}", NMRA.CalculateAccDecRateTimes(AccelerationRate, DecoderConfiguration.RCN225.SpeedStepsMode)) + " s)";
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
            RCN225_CONSISTADDRESS_CV19 = DecoderSpecification.RCN225_CONSISTADDRESS_CV19;
            RCN225_HLU_CV27_2 = DecoderSpecification.RCN225_HLU_CV27_2;
            RCN225_ABC_CV27_X = DecoderSpecification.RCN225_ABC_CV27_X;
        }


        /// <summary>
        /// This event handler reacts to the weak DataStoreUpdatedMessage message. It will fetch
        /// the current data from the data store.
        /// </summary>
        public void OnGetDataFromDataStore()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            DirectionReversal = DecoderConfiguration.RCN225.DirectionReversal;
            HluEnabled = DecoderConfiguration.RCN225.HLUEnabled;
            SelectedSpeedStepsMode = NMRAEnumConverter.GetDCCSpeedStepModeDescription(DecoderConfiguration.RCN225.SpeedStepsMode);

            SelectedABCBreakMode = NMRAEnumConverter.GetDCCABCBreakModeDescription(DecoderConfiguration.RCN225.ABCBreakMode);

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
        }

        #endregion

        #region REGION: COMMANDS
        /// <summary>
        /// Decreases the acceleration rate of CV3 by 1.
        /// </summary>
        [RelayCommand]
        void DecreaseAccelarationRateCV3()
        {
            if (AccelerationRate > LimitMinimumAccelerationRateCV3) AccelerationRate--;
        }

        /// <summary>
        /// Increases the acceleration rate of CV3 by 1.
        /// </summary>
        [RelayCommand]
        void IncreaseAccelarationRateCV3()
        {
            if (AccelerationRate < LimitMaximumAccelerationRateCV3) AccelerationRate++;
        }

        /// <summary>
        /// Decreases the deceleration rate of CV4 by 1.
        /// </summary>
        [RelayCommand]
        void DecreaseDecelerationRateCV4()
        {
            if (DecelerationRate > LimitMinimumDecelerationRateCV4) DecelerationRate--;
        }

        /// <summary>
        /// Increases the deceleration rate of CV4 by 1.
        /// </summary>
        [RelayCommand]
        void IncreaseDecelerationRateCV4()
        {
            if (DecelerationRate < LimitMaximumDecelerationRateCV4) DecelerationRate++;
        }

        #endregion


    }
}
