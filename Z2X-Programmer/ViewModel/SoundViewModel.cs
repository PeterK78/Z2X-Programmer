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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{

    public partial class SoundViewModel : ObservableObject
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

        // ZIMO_SOUND_VOLUME_FUNCKEY_CV395
        [ObservableProperty]
        bool zIMO_SOUND_VOLUME_FUNCKEY_CV395 = false;

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


        // ZIMO: Generic sound volume in CV266 (ZIMO_SOUND_VOLUME_GENERIC_C266)
        [ObservableProperty]
        int overallVolume;
        partial void OnOverallVolumeChanged(int value)
        {
            DecoderConfiguration.ZIMO.OverallVolume = (byte)value;
            OverallVolumeText = GetOverallVolumeText();
            CV266Configuration = Subline.Create(new List<uint>{266});
        }

        [ObservableProperty]
        string overallVolumeText = "";

        [ObservableProperty]
        string cV266Configuration = Subline.Create(new List<uint>{266});

        [ObservableProperty]
        int maximumVolumeForFunctionKeys;
        partial void OnMaximumVolumeForFunctionKeysChanged(int value)
        {
            DecoderConfiguration.ZIMO.MaximumVolumeForFuncKeysControl = (byte)value;
            float percentage = ((float)400 / (float)255) * (float)value;
            MaximumVolumeForFunctionKeysText = GetMaximumVolumeForFunctionKeysText();
            CV395Configuration = Subline.Create(new List<uint>{395});
        }

        [ObservableProperty]
        string maximumVolumeForFunctionKeysText = "";

        [ObservableProperty]
        string cV395Configuration = Subline.Create(new List<uint>{395});

        [ObservableProperty]
        bool anyDecoderFeatureAvailable;

        [ObservableProperty]
        bool anyZIMOSoundTimesFeaturesAvailable;

        //  ZIMO: CV298
        [ObservableProperty]
        byte soundEMotorVolumeDependedSpeed;
        partial void OnSoundEMotorVolumeDependedSpeedChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundEMotorVolumeDependedSpeed = value;
            CV298Configuration = Subline.Create(new List<uint>{298});
        }
        [ObservableProperty]
        string cV298Configuration = Subline.Create(new List<uint>{298});

        //  ZIMO: CV296
        [ObservableProperty]
        byte soundEMotorVolume;
        partial void OnSoundEMotorVolumeChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundEMotorVolume = value;
            CV296Configuration = Subline.Create(new List<uint>{296});
        }
        [ObservableProperty]
        string cV296Configuration = Subline.Create(new List<uint>{296});



        // ZIMO: ZIMO_SOUND_VOLUME_DECELERATION_CV286
        [ObservableProperty]
        byte soundVolumeDeceleration;
        partial void OnSoundVolumeDecelerationChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeDeceleration = value;
            CV286Configuration = Subline.Create(new List<uint> { 286 });
        }
        [ObservableProperty]
        string cV286Configuration = Subline.Create(new List<uint>{286});

        // ZIMO: ZIMO_SOUND_VOLUME_ACCELERATION_CV283
        [ObservableProperty]
        byte soundVolumeAcceleration;
        partial void OnSoundVolumeAccelerationChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeAcceleration = value;
            CV283Configuration = Subline.Create(new List<uint>{283});
        }
        [ObservableProperty]
        string cV283Configuration = Subline.Create(new List<uint>{283});


        // ZIMO: ZIMO_SOUND_VOLUME_FASTSPEEDNOLOAD_CV276
        [ObservableProperty]
        byte soundVolumeFastSpeedNoLoad;
        partial void OnSoundVolumeFastSpeedNoLoadChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeFastSpeedNoLoad = value;
            CV276Configuration = Subline.Create(new List<uint>{276});
        }
        [ObservableProperty]
        string cV276Configuration = Subline.Create(new List<uint>{276});


        // ZIMO: Steam sounds in CV27X (ZIMO_SOUND_VOLUME_STEAM_CV27X)
        [ObservableProperty]
        byte soundVolumeSlowSpeedNoLoad;
        partial void OnSoundVolumeSlowSpeedNoLoadChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundVolumeSlowSpeedNoLoad = value;
            CV275Configuration = Subline.Create(new List<uint>{275});
        }
        [ObservableProperty]
        string cV275Configuration = Subline.Create(new List<uint>{275});


        // ZIMO: ZIMO_SOUND_STARTUPDELAY_CV273
        [ObservableProperty]
        byte soundStartUpDelay;
        partial void OnSoundStartUpDelayChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundStartUpDelay = value;
            SoundStartUpDelayText = GetSoundStartUpDelayLabel();
            CV273Configuration = Subline.Create(new List<uint>{273});
        }
        [ObservableProperty]
        string soundStartUpDelayText = "";

        [ObservableProperty]
        string cV273Configuration = Subline.Create(new List<uint>{273});


        // ZIMO: ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285
        [ObservableProperty]
        byte soundDurationNoiseReduction;
        partial void OnSoundDurationNoiseReductionChanged(byte value)
        {
            DecoderConfiguration.ZIMO.SoundDurationNoiseReduction = value;
            SoundDurationNoiseReductionText = GetSoundDurationNoiseReductionLabel();
            CV285Configuration = Subline.Create(new List<uint> { 285 });
        }
        [ObservableProperty]
        string soundDurationNoiseReductionText = "";

        [ObservableProperty]
        string cV285Configuration = Subline.Create(new List<uint>{285});

        // ZIMO: ZIMO_BRAKESQUEAL_CV287  
        [ObservableProperty]
        int breakSquealLevel;
        partial void OnBreakSquealLevelChanged(int value)
        {
            DecoderConfiguration.ZIMO.BreakSquealLevel = (byte)value;
            CV287Configuration = Subline.Create(new List<uint> { 287 });
        }
        [ObservableProperty]
        string cV287Configuration = Subline.Create(new List<uint>{287});



       

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
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // ZIMO: Generic sound volume in CV266 (ZIMO_SOUND_VOLUME_GENERIC_C266)
            OverallVolume = DecoderConfiguration.ZIMO.OverallVolume;
            OverallVolumeText = GetOverallVolumeText();
            CV266Configuration = Subline.Create(new List<uint>{266});

            // ZIMO: ZIMO_BRAKESQUEAL_CV287  
            BreakSquealLevel = DecoderConfiguration.ZIMO.BreakSquealLevel;
            CV287Configuration = Subline.Create(new List<uint> { 287 });

            // ZIMO: ZIMO_SOUND_STARTUPDELAY_CV273
            SoundStartUpDelay = DecoderConfiguration.ZIMO.SoundStartUpDelay;
            CV273Configuration = Subline.Create(new List<uint>{273});
            SoundStartUpDelayText = GetSoundStartUpDelayLabel();

            // ZIMO: ZIMO_SOUND_DURATIONNOISEREDUCTION_CV285
            SoundDurationNoiseReduction = DecoderConfiguration.ZIMO.SoundDurationNoiseReduction;
            SoundDurationNoiseReductionText = GetSoundDurationNoiseReductionLabel();
            CV285Configuration = Subline.Create(new List<uint> { 285 });

            // ZIMO: Steam sounds in CV27X (ZIMO_SOUND_VOLUME_STEAM_CV27X)
            SoundVolumeSlowSpeedNoLoad = DecoderConfiguration.ZIMO.SoundVolumeSlowSpeedNoLoad;
            CV275Configuration = Subline.Create(new List<uint>{275});

            // ZIMO: ZIMO_SOUND_VOLUME_FASTSPEEDNOLOAD_CV276
            SoundVolumeFastSpeedNoLoad = DecoderConfiguration.ZIMO.SoundVolumeFastSpeedNoLoad;
            CV276Configuration = Subline.Create(new List<uint>{276});

            // ZIMO: ZIMO_SOUND_VOLUME_ACCELERATION_CV283
            SoundVolumeAcceleration = DecoderConfiguration.ZIMO.SoundVolumeAcceleration;
            CV283Configuration = Subline.Create(new List<uint>{283});

            // ZIMO: ZIMO_SOUND_VOLUME_DECELERATION_CV286
            SoundVolumeDeceleration = DecoderConfiguration.ZIMO.SoundVolumeDeceleration;
            CV286Configuration = Subline.Create(new List<uint> { 286 });

            //  ZIMO: CV296
            SoundEMotorVolume = DecoderConfiguration.ZIMO.SoundEMotorVolume;
            CV296Configuration = Subline.Create(new List<uint>{296});

            //  ZIMO: CV298
            SoundEMotorVolumeDependedSpeed = DecoderConfiguration.ZIMO.SoundEMotorVolumeDependedSpeed;
            CV298Configuration = Subline.Create(new List<uint> { 298 });

            // ZIMO: ZIMO_SOUND_VOLUME_DIESELELEC_CV29X
            MaximumVolumeForFunctionKeys = DecoderConfiguration.ZIMO.MaximumVolumeForFuncKeysControl;           
            MaximumVolumeForFunctionKeysText = GetMaximumVolumeForFunctionKeysText();
            CV395Configuration = Subline.Create(new List<uint> { 395 });

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
            ZIMO_SOUND_VOLUME_FUNCKEY_CV395 = DecoderSpecification.ZIMO_SOUND_VOLUME_FUNCKEY_CV395;



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

       

        #endregion

    }
}
