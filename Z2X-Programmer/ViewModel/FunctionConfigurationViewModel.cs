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
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;
using static Z2XProgrammer.Helper.ZIMO;

namespace Z2XProgrammer.ViewModel
{
    public partial class FunctionConfigurationViewModel : ObservableObject
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
        #region ZIMO

        [ObservableProperty]
        bool anyFunctionSettingsSupported;

        //  ZIMO: ZIMO_ELECTRIC_UNCOUPLER_CV115X
        [ObservableProperty]
        internal ObservableCollection<string> availableDecouplerSettings;

        [ObservableProperty]
        internal ObservableCollection<string> availableFunctionOutputsDecoupler;

        [ObservableProperty]
        bool zIMO_ELECTRIC_UNCOUPLER_CV115X;

        #endregion
        #endregion

        #region REGION: PUBLIC PROPERTIES   

        //  ZIMO: ZIMO_ELECTRIC_UNCOUPLER_CV115X
        #region FUNCTION OUTPUT DESCRIPTION

        [ObservableProperty]
        string output1Description = string.Empty;

        [ObservableProperty]
        string output2Description = string.Empty;

        [ObservableProperty]
        string output3Description = string.Empty;

        [ObservableProperty]
        string output4Description = string.Empty;

        [ObservableProperty]
        string output5Description = string.Empty;

        [ObservableProperty]
        string output6Description = string.Empty;

        [ObservableProperty]
        string output7Description = string.Empty;

        [ObservableProperty]
        string output8Description = string.Empty;

        #endregion

        #region FUNCTION OUTPUTS AVAILABLE

        [ObservableProperty]
        bool zIMOFunctionOutput1Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput2Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput3Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput4Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput5Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput6Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput7Enabled;

        [ObservableProperty]
        bool zIMOFunctionOutput8Enabled;

        #endregion

        [ObservableProperty]
        string zIMOUncouplerSettingOutput1 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput1Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput1 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA1 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA1 = ZIMO.ZIMOEffects.NoEffect;
            }
            
            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput2 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput2Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput2 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA2 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA2 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));                

        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput3 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput3Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput3 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA3 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA3 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput4 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput4Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput4 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA4 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA4 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput5 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput5Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput5 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA5 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA5 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput6 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput6Changed(string value)
        {

            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput6 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }


            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA6 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA6 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput7 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput7Changed(string value)
        {
            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput7 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA7 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA7 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

        }

        [ObservableProperty]
        string zIMOUncouplerSettingOutput8 = AppResources.ZIMODecouplerEffectDisabled;
        partial void OnZIMOUncouplerSettingOutput8Changed(string value)
        {

            if ((value == AppResources.ZIMODecouplerEffectEnabled) && (ActiveDecouplerFunctionOutputs() > 2))
            {
                ShowInfoMessage();
                ZIMOUncouplerSettingOutput8 = AppResources.ZIMODecouplerEffectDisabled;
                return;
            }

            if (value == AppResources.ZIMODecouplerEffectEnabled)
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA8 = ZIMO.ZIMOEffects.Decoupler;
            }
            else
            {
                DecoderConfiguration.ZIMO.LightEffectOutputFA8 = ZIMO.ZIMOEffects.NoEffect;
            }

            WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

        }

        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public FunctionConfigurationViewModel()
        {

            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

            AvailableFunctionOutputsDecoupler = new ObservableCollection<String>(NMRAEnumConverter.GetAvailableFunctionOutputs(1, 8, true));
            AvailableDecouplerSettings = new ObservableCollection<String>(GetAvailableFunctionStates());

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
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            AnyFunctionSettingsSupported = DeqSpecReader.AnyFunctionSettingsSupported(DecoderSpecification.DeqSpecName);

            //  ZIMO
            ZIMO_ELECTRIC_UNCOUPLER_CV115X = DecoderSpecification.ZIMO_ELECTRIC_UNCOUPLER_CV115X;

        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            //  ZIMO: Electric uncoupler configuration in CV115x (ZIMO_ELECTRIC_UNCOUPLER_CV115X)
            Output1Description = DecoderConfiguration.UserDefinedFunctionOutputNames[2].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[2].Description;
            Output2Description = DecoderConfiguration.UserDefinedFunctionOutputNames[3].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[3].Description;
            Output3Description = DecoderConfiguration.UserDefinedFunctionOutputNames[4].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[4].Description;
            Output4Description = DecoderConfiguration.UserDefinedFunctionOutputNames[5].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[5].Description;
            Output5Description = DecoderConfiguration.UserDefinedFunctionOutputNames[6].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[6].Description;
            Output6Description = DecoderConfiguration.UserDefinedFunctionOutputNames[7].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[7].Description;
            Output7Description = DecoderConfiguration.UserDefinedFunctionOutputNames[8].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[8].Description;
            Output8Description = DecoderConfiguration.UserDefinedFunctionOutputNames[9].Description == "" ? "-" : DecoderConfiguration.UserDefinedFunctionOutputNames[9].Description;

            ZIMOFunctionOutput1Enabled = IsFunctionOutputAvailable(1);
            ZIMOFunctionOutput2Enabled = IsFunctionOutputAvailable(2);
            ZIMOFunctionOutput3Enabled = IsFunctionOutputAvailable(3);
            ZIMOFunctionOutput4Enabled = IsFunctionOutputAvailable(4);
            ZIMOFunctionOutput5Enabled = IsFunctionOutputAvailable(5);
            ZIMOFunctionOutput6Enabled = IsFunctionOutputAvailable(6);
            ZIMOFunctionOutput7Enabled = IsFunctionOutputAvailable(7);
            ZIMOFunctionOutput8Enabled = IsFunctionOutputAvailable(8);

            ZIMOUncouplerSettingOutput1 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA1);
            ZIMOUncouplerSettingOutput2 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA2);
            ZIMOUncouplerSettingOutput3 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA3);
            ZIMOUncouplerSettingOutput4 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA4);
            ZIMOUncouplerSettingOutput5 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA5);
            ZIMOUncouplerSettingOutput6 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA6);
            ZIMOUncouplerSettingOutput7 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA7);
            ZIMOUncouplerSettingOutput8 = IsDecouplerEnabled(DecoderConfiguration.ZIMO.LightEffectOutputFA8);

        }

        /// <summary>
        /// Returns a string with the decoupler function state. The function state is "Enabled" or "Disabled".
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        internal static string IsDecouplerEnabled(ZIMOEffects effect)
        {
            if (effect == ZIMOEffects.Decoupler)
            {
                return AppResources.ZIMODecouplerEffectEnabled;
            }
            else
            {
                return AppResources.ZIMODecouplerEffectDisabled;
            }
        }

        /// <summary>
        /// Returns TRUE if the function output is available. The function output is available if the function effect is "NoEffect" or "Decoupler".
        /// </summary>
        /// <param name="functionOutputNumber">A function number between 1 and 8. Only function numbers between 1 and 8 are supporting the decoupler function.</param>
        /// <returns></returns>
        internal static bool IsFunctionOutputAvailable(int functionOutputNumber)
        {
            if (functionOutputNumber < 1 || functionOutputNumber > 8) return false;

            switch (functionOutputNumber)
            {
                case 1:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA1 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA1 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 2:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA2 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA2 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 3:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA3 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA3 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 4:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA4 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA4 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 5:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA5 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA5 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 6:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA6 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA6 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 7:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA7 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA7 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
                case 8:
                    if ((DecoderConfiguration.ZIMO.LightEffectOutputFA8 == ZIMO.ZIMOEffects.NoEffect) || (DecoderConfiguration.ZIMO.LightEffectOutputFA8 == ZIMO.ZIMOEffects.Decoupler)) return true;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Shows an error message if the user tries to enable more than 2 decoupler function outputs.
        /// </summary>
        private async void ShowInfoMessage()
        {
            await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertTooManyDecouplerOutputs, AppResources.OK);
        }

        /// <summary>
        /// Returns a list with all available decoupler function states.
        /// </summary>
        /// <returns>A list with all available function states.</returns>
        internal static List<string> GetAvailableFunctionStates()
        {
            List<string> Effect = new List<string>();
            Effect.Add(AppResources.ZIMODecouplerEffectDisabled);
            Effect.Add(AppResources.ZIMODecouplerEffectEnabled);
            return Effect;
        }

        /// <summary>
        /// Returns the number of active decoupler function outputs. The maximum number of active decoupler function outputs is 2.
        /// </summary>
        /// <returns></returns>
        private int ActiveDecouplerFunctionOutputs()
        {
            int i = 0;
            if (ZIMOUncouplerSettingOutput1 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput2 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput3 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput4 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput5 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput6 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput7 == AppResources.ZIMODecouplerEffectEnabled) i++;
            if (ZIMOUncouplerSettingOutput8 == AppResources.ZIMODecouplerEffectEnabled) i++;
            return i;
        }

        #endregion

    }
}
