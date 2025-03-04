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
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class SecurityViewModel : ObservableObject
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

        #region #region REGION: DECODER FEATURES
    
        [ObservableProperty]
        bool anyDecoderFeatureAvailable;

        [ObservableProperty]
        bool rCN225_DECODERLOCK_CV15X;

        [ObservableProperty]
        bool zIMO_MXUPDATELOCK_CV144;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        // ZIMO: Zimo specific update configuration in CV144 for MX decoder (ZIMO_MXUPDATELOCK_CV144)
        [ObservableProperty]
        bool lockWritingCVsInServiceMode;
        partial void OnLockWritingCVsInServiceModeChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockWritingCVsOnProgramTrack = value;
            CV144Configuration = Subline.Create(new List<uint>{144});
        }

        [ObservableProperty]
        bool lockReadingCVsInServiceMode;
        partial void OnLockReadingCVsInServiceModeChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockReadingCVsOnProgramTrack = value;
            CV144Configuration = Subline.Create(new List<uint>{144});
        }

        [ObservableProperty]
        bool lockWritingCVsOnMainTrack;
        partial void OnLockWritingCVsOnMainTrackChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockWritingCVsOnMainTrack = value;
            CV144Configuration = Subline.Create(new List<uint>{144});
        }

        [ObservableProperty]
        bool lockUpdatingDecoderFirmware;
        partial void OnLockUpdatingDecoderFirmwareChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockUpatingDecoderFirmware = value;
            CV144Configuration = Subline.Create(new List<uint> { 144 });
        }

        [ObservableProperty]
        bool playSoundWhenProgrammingCV;
        partial void OnPlaySoundWhenProgrammingCVChanged(bool value)
        {
            DecoderConfiguration.ZIMO.PlaySoundWhenProgrammingCV = value;
            CV144Configuration = Subline.Create(new List<uint>{144});
        }

        [ObservableProperty]
        string cV144Configuration = Subline.Create(new List<uint>{144});

        // RCN225: Decoder lock configuration in CV15 and CV16 (RCN225_DECODERLOCK_CV15X)
        [ObservableProperty]
        bool decoderLockCV1516Activated;
        
        [ObservableProperty]
        byte decoderLockPasswordCV16;
        partial void OnDecoderLockPasswordCV16Changed(byte value)
        {
            DecoderConfiguration.RCN225.DecoderLockPasswordCV16 = value;
            DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
            CV15Configuration = Subline.Create(new List<uint> {15});
            CV16Configuration = Subline.Create(new List<uint>{16});
        }

        [ObservableProperty]
        byte decoderLockPasswordCV15;
        partial void OnDecoderLockPasswordCV15Changed(byte value)
        {
            DecoderConfiguration.RCN225.DecoderLockPasswordCV15 = value;
            DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
            CV15Configuration = Subline.Create(new List<uint> {15});
            CV16Configuration = Subline.Create(new List<uint>{16});
        }

        [ObservableProperty]
        string cV15Configuration = Subline.Create(new List<uint> {15});

        [ObservableProperty]
        string cV16Configuration = Subline.Create(new List<uint>{16});


        #endregion

        #region REGION: LIMITS FOR ENTRY VALIDATION

        [ObservableProperty]
        int limitMinimumCV16 = 0;

        [ObservableProperty]
        int limitMaximumCV16 = 239;

        [ObservableProperty]
        int limitMinimumCV15 = 0;

        [ObservableProperty]
        int limitMaximumCV15 = 255;

        #endregion

        #region REGION: CONSTRUCTOR
        public SecurityViewModel()
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

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            
            if(RCN225_DECODERLOCK_CV15X == true)
            {
                DecoderLockPasswordCV16 = DecoderConfiguration.RCN225.DecoderLockPasswordCV16;
                DecoderLockPasswordCV15 = DecoderConfiguration.RCN225.DecoderLockPasswordCV15;
                DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
            }

            if(ZIMO_MXUPDATELOCK_CV144 == true)
            {
                LockWritingCVsInServiceMode = DecoderConfiguration.ZIMO.LockWritingCVsOnProgramTrack;
                LockReadingCVsInServiceMode = DecoderConfiguration.ZIMO.LockReadingCVsOnProgramTrack;
                LockWritingCVsOnMainTrack = DecoderConfiguration.ZIMO.LockWritingCVsOnMainTrack;
                LockUpdatingDecoderFirmware = DecoderConfiguration.ZIMO.LockUpatingDecoderFirmware;
                PlaySoundWhenProgrammingCV = DecoderConfiguration.ZIMO.PlaySoundWhenProgrammingCV;
            }
            
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            RCN225_DECODERLOCK_CV15X = DecoderSpecification.RCN225_DECODERLOCK_CV15X;
            ZIMO_MXUPDATELOCK_CV144 = DecoderSpecification.ZIMO_MXUPDATELOCK_CV144;

            if ((RCN225_DECODERLOCK_CV15X == true) || (ZIMO_MXUPDATELOCK_CV144 == true))
            {
                AnyDecoderFeatureAvailable = true;
            }
            else
            {
                AnyDecoderFeatureAvailable = false;
            }
            
        }

    }
}
