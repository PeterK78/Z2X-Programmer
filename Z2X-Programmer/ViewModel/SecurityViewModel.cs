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
    public partial class SecurityViewModel : ObservableObject
    {

        #region #region REGION: DECODER FEATURES

        [ObservableProperty]
        bool dataStoreDataValid;

        [ObservableProperty]
        bool rCN225_DECODERLOCK_CV15X;

        [ObservableProperty]
        bool zIMO_MXUPDATELOCK_CV144;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool lockWritingCVsInServiceMode;
        partial void OnLockWritingCVsInServiceModeChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockWritingCVsOnProgramTrack = value;
        }

        [ObservableProperty]
        bool lockReadingCVsInServiceMode;
        partial void OnLockReadingCVsInServiceModeChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockReadingCVsOnProgramTrack = value;
        }


        [ObservableProperty]
        bool lockWritingCVsOnMainTrack;
        partial void OnLockWritingCVsOnMainTrackChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockWritingCVsOnMainTrack = value;
        }

        [ObservableProperty]
        bool lockUpdatingDecoderFirmware;
        partial void OnLockUpdatingDecoderFirmwareChanged(bool value)
        {
            DecoderConfiguration.ZIMO.LockUpatingDecoderFirmware = value;
        }


        [ObservableProperty]
        bool playSoundWhenProgrammingCV;
        partial void OnPlaySoundWhenProgrammingCVChanged(bool value)
        {
            DecoderConfiguration.ZIMO.PlaySoundWhenProgrammingCV = value;
        }

        [ObservableProperty]
        bool decoderLockCV1516Activated;
        
        [ObservableProperty]
        byte decoderLockPasswordCV16;
        partial void OnDecoderLockPasswordCV16Changed(byte value)
        {
            DecoderConfiguration.RCN225.DecoderLockPasswordCV16 = value;
            DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
        }

        [ObservableProperty]
        byte decoderLockPasswordCV15;
        partial void OnDecoderLockPasswordCV15Changed(byte value)
        {
            DecoderConfiguration.RCN225.DecoderLockPasswordCV15 = value;
            DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
        }

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
            DecoderLockPasswordCV16 = DecoderConfiguration.RCN225.DecoderLockPasswordCV16;
            DecoderLockPasswordCV15 = DecoderConfiguration.RCN225.DecoderLockPasswordCV15;
            LockWritingCVsInServiceMode = DecoderConfiguration.ZIMO.LockWritingCVsOnProgramTrack;
            LockReadingCVsInServiceMode = DecoderConfiguration.ZIMO.LockReadingCVsOnProgramTrack;
            LockWritingCVsOnMainTrack = DecoderConfiguration.ZIMO.LockWritingCVsOnMainTrack;
            LockUpdatingDecoderFirmware = DecoderConfiguration.ZIMO.LockUpatingDecoderFirmware;
            PlaySoundWhenProgrammingCV = DecoderConfiguration.ZIMO.PlaySoundWhenProgrammingCV;
            DecoderLockCV1516Activated = DecoderLockPasswordCV16 == DecoderLockPasswordCV15 ? false : true;
        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            RCN225_DECODERLOCK_CV15X = DecoderSpecification.RCN225_DECODERLOCK_CV15X;
            ZIMO_MXUPDATELOCK_CV144 = DecoderSpecification.ZIMO_MXUPDATELOCK_CV144;
            
        }

    }
}
