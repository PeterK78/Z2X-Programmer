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
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{

    public partial class SoundViewModel : ObservableObject
    {

        #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool zIMO_BRAKESQUEAL_CV287;

        // ZIMO_SOUND_VOLUME_DIESELELEC_CV29X
        [ObservableProperty]
        bool zIMO_SOUND_VOLUME_DIESELELEC_CV29X;

        // ZIMO_SOUND_VOLUME_GENERIC_C266
        [ObservableProperty]
        bool zIMO_SOUND_VOLUME_GENERIC_C266;

        // ZIMO_SOUND_VOLUME_STEAM_CV27X
        [ObservableProperty]
        bool zIMO_SOUND_VOLUME_STEAM_CV27X;
        
        // ZIMO_SOUND_STARTUPDELAY_CV273
        [ObservableProperty]
        bool zIMO_SOUND_STARTUPDELAY_CV273;

        // ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285
        [ObservableProperty]
        bool zIMO_SOUND_DURATIONNOISEREDUCTION_CV285;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool anyDecoderFeatureAvailable;

        [ObservableProperty]
        bool anyZIMOSoundTimesFeaturesAvailable;

        
        [ObservableProperty]
        byte soundEMotorVolumeDependedSpeed;
        partial void OnSoundEMotorVolumeDependedSpeedChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundEMotorVolumeDependedSpeed = value;
        }

        
        [ObservableProperty]
        byte soundEMotorVolume;
        partial void OnSoundEMotorVolumeChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundEMotorVolume = value;
        }


        // ZIMO_SOUND_VOLUME_DECELERATION_CV286
        [ObservableProperty]
        byte soundVolumeDeceleration;
        partial void OnSoundVolumeDecelerationChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeDeceleration = value;
        }

        // ZIMO_SOUND_VOLUME_ACCELERATION_CV283
        [ObservableProperty]
        byte soundVolumeAcceleration;
        partial void OnSoundVolumeAccelerationChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeAcceleration = value;
        }

        // ZIMO_SOUND_VOLUME_FASTSPEEDNOLOAD_CV276
        [ObservableProperty]
        byte soundVolumeFastSpeedNoLoad;
        partial void OnSoundVolumeFastSpeedNoLoadChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeFastSpeedNoLoad = value;
        }


        // ZIMO_SOUND_VOLUME_SLOWSPEEDNOLOAD_CV275
        [ObservableProperty]
        byte soundVolumeSlowSpeedNoLoad;
        partial void OnSoundVolumeSlowSpeedNoLoadChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeSlowSpeedNoLoad = value;
        }

        // ZIMO_SOUND_STARTUPDELAY_CV273
        [ObservableProperty]
        byte soundStartUpDelay;
        partial void OnSoundStartUpDelayChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundStartUpDelay = value;
            SoundStartUpDelayText = GetSoundStartUpDelayLabel();
        }
        [ObservableProperty]
        string soundStartUpDelayText = "";

        // ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285
        [ObservableProperty]
        byte soundDurationNoiseReduction;
        partial void OnSoundDurationNoiseReductionChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundDurationNoiseReduction = value;
            SoundDurationNoiseReductionText = GetSoundDurationNoiseReductionLabel();
        }
        [ObservableProperty]
        string soundDurationNoiseReductionText = "";


        [ObservableProperty]
        int breakSquealLevel;
        partial void OnBreakSquealLevelChanged(int value)
        {
            DecoderConfiguration.ZIMO.BreakSquealLevel = (byte)value;
        }

        // ZIMO_SOUND_VOLUME_GENERIC_C266
        [ObservableProperty]
        int overallVolume;
        partial void OnOverallVolumeChanged(int value)
        {
            DecoderConfiguration.ZIMO.OverallVolume = (byte)value;
            OverallVolumeText = GetOverallVolumeText();
        }

        [ObservableProperty]
        string overallVolumeText = "";

        [ObservableProperty]
        int maximumVolumeForFunctionKeys;
        partial void OnMaximumVolumeForFunctionKeysChanged(int value)
        {
            DecoderConfiguration.ZIMO.MaximumVolumeForFuncKeysControl = (byte)value;
            float percentage = ((float)400 / (float)255) * (float)value;
            MaximumVolumeForFunctionKeysText = GetMaximumVolumeForFunctionKeysText();
        }

        [ObservableProperty]
        string maximumVolumeForFunctionKeysText = "";

        #endregion

        #region REGION: CONSTRUCTOR

        public SoundViewModel()
        {
            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

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
        /// Returns the label text for GetSoundStartUpDelay.
        /// </summary>
        /// <returns></returns>
        private string GetSoundStartUpDelayLabel()
        {
            return SoundStartUpDelay.ToString() + " (" + (SoundStartUpDelay / 10) + " s)";
        }

        /// <summary>
        /// Returns the label text for SoundDurationNoiseReduction. 
        /// </summary>
        /// <returns></returns>
        private string GetSoundDurationNoiseReductionLabel()
       {
            return SoundDurationNoiseReduction.ToString() + " (" + (SoundDurationNoiseReduction / 10) + " s)";
        }

        private string GetOverallVolumeText()
        {
            float percentage = ((float)400 / (float)255) * (float)OverallVolume;
            return OverallVolume.ToString() + " (" + string.Format("{0:N0}", percentage) + " %)";
        }

        private string GetMaximumVolumeForFunctionKeysText()
        {
            float percentage = ((float)400 / (float)255) * (float)MaximumVolumeForFunctionKeys;
            return MaximumVolumeForFunctionKeys.ToString() + " (" + string.Format("{0:N0}", percentage) + " %)";
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            OverallVolume = DecoderConfiguration.ZIMO.OverallVolume;
            OverallVolumeText = GetOverallVolumeText();
            BreakSquealLevel = DecoderConfiguration.ZIMO.BreakSquealLevel;
            SoundStartUpDelay = DecoderConfiguration.ZIMO.SoundStartUpDelay;
            SoundStartUpDelayText = GetSoundStartUpDelayLabel();
            SoundDurationNoiseReduction = DecoderConfiguration.ZIMO.SoundDurationNoiseReduction;
            SoundDurationNoiseReductionText = GetSoundDurationNoiseReductionLabel();
            SoundVolumeSlowSpeedNoLoad = DecoderConfiguration.ZIMO.SoundVolumeSlowSpeedNoLoad;
            SoundVolumeFastSpeedNoLoad = DecoderConfiguration.ZIMO.SoundVolumeFastSpeedNoLoad;
            SoundVolumeAcceleration = DecoderConfiguration.ZIMO.SoundVolumeAcceleration;
            SoundVolumeDeceleration = DecoderConfiguration.ZIMO.SoundVolumeDeceleration;
            SoundEMotorVolume = DecoderConfiguration.ZIMO.SoundEMotorVolume;
            SoundEMotorVolumeDependedSpeed = DecoderConfiguration.ZIMO.SoundEMotorVolumeDependedSpeed;
            MaximumVolumeForFunctionKeys = DecoderConfiguration.ZIMO.MaximumVolumeForFuncKeysControl;
            MaximumVolumeForFunctionKeysText = GetMaximumVolumeForFunctionKeysText();
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            ZIMO_SOUND_VOLUME_GENERIC_C266 = DecoderSpecification.ZIMO_SOUND_VOLUME_GENERIC_C266;
            ZIMO_BRAKESQUEAL_CV287 = DecoderSpecification.ZIMO_BRAKESQUEAL_CV287;
            ZIMO_SOUND_STARTUPDELAY_CV273 = DecoderSpecification.ZIMO_SOUND_STARTUPDELAY_CV273;
            ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 = DecoderSpecification.ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285;
            ZIMO_SOUND_VOLUME_STEAM_CV27X = DecoderSpecification.ZIMO_SOUND_VOLUME_STEAM_CV27X;
            ZIMO_SOUND_VOLUME_DIESELELEC_CV29X = DecoderSpecification.ZIMO_SOUND_VOLUME_DIESELELEC_CV29X;

            if  ((ZIMO_SOUND_VOLUME_GENERIC_C266 == true)   ||
                 (ZIMO_BRAKESQUEAL_CV287 == true) ||
                 (ZIMO_SOUND_STARTUPDELAY_CV273 == true) ||
                (ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 == true) ||
                (ZIMO_SOUND_VOLUME_STEAM_CV27X == true) ||
                (ZIMO_SOUND_VOLUME_DIESELELEC_CV29X == true))
            {
                AnyDecoderFeatureAvailable = true;
            }
            else
            {
                AnyDecoderFeatureAvailable = false;
            }

            if ((ZIMO_SOUND_STARTUPDELAY_CV273 == true) ||
                             (ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285 == true))
            {
                AnyZIMOSoundTimesFeaturesAvailable = true;
            }
            else
            {
                AnyZIMOSoundTimesFeaturesAvailable = false;
            }

            
        }

        #endregion

    }
}
