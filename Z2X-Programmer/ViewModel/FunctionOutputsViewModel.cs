﻿/*

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
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    public partial class FunctionOutputsViewModel: ObservableObject
    {

       internal ObservableCollection<FunctionOutputType>? _functionOutputs= new ObservableCollection<FunctionOutputType>();

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

        [ObservableProperty]
        bool zIMO_SUSIPORT1CONFIG_CV201;

        #endregion 

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        internal ObservableCollection<FunctionOutputType>? functionOutputs= new ObservableCollection<FunctionOutputType>();

        [ObservableProperty]
        internal FunctionOutputType selectedFunctionOutput = new FunctionOutputType();

        //  ZIMO: ZIMO SUSI Configuration in CV201 (ZIMO_SUSIPORT1CONFIG_CV201)
        [ObservableProperty]
        internal ObservableCollection<string> availableSUSIPinModes;

        [ObservableProperty]
        internal string selectedSUSIInterface1PinMode = "";
        partial void OnSelectedSUSIInterface1PinModeChanged(string value)
        {
            if ((value == "") || (value == null)) return;
            DecoderConfiguration.ZIMO.SUSIInterface1PinMode = ZIMOEnumConverter.GetSUSIInterface1PinMode(value);
            CV201Configuration = Subline.Create(new List<uint>{201});
        }

        [ObservableProperty]
        string cV201Configuration = Subline.Create(new List<uint>{201});


        #endregion

        #region REGION: CONSTRUCTOR
        public FunctionOutputsViewModel()
        {
            AvailableSUSIPinModes = new ObservableCollection<String>(ZIMOEnumConverter.GetAvailableSUSIPinModes());

            _functionOutputs = FunctionOutputs;

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

        /// <summary>
        /// This event handler reacts to the DataStoreUpdatedMessage message. It will fetch
        /// the current data from the data store and update the local properties in this view model.
        /// </summary>
        private void OnGetDataFromDataStore()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            SelectedSUSIInterface1PinMode = ZIMOEnumConverter.GetSUSIInterface1PinModeDescription(DecoderConfiguration.ZIMO.SUSIInterface1PinMode);
            FunctionOutputs = new ObservableCollection<FunctionOutputType>(DecoderConfiguration.UserDefinedFunctionOutputNames);
        }

        /// <summary>
        /// This event handler reacts to the DecoderSpecificationUpdatedMessage message. It will fetch
        /// the supported features of the currently selected decode specification and update the local properties
        /// in this view model.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            ZIMO_SUSIPORT1CONFIG_CV201 = DecoderSpecification.ZIMO_SUSIPORT1CONFIG_CV201;
        }


        #endregion

        #region REGION: COMMANDS
        [RelayCommand]
        async Task EditFunctionOutputName()
        {
            try
            {
                //  Check if a function key has been selected
                if(SelectedFunctionOutput == null)
                {
                    await MessageBox.Show(AppResources.AlertError, AppResources.FrameFunctionKeysZIMONoMappingSelected, AppResources.OK);
                    return;
                }

                if ((Application.Current != null) && (Application.Current.Windows[0].Page != null))
                {
                    string Result = await Application.Current.Windows[0].Page!.DisplayPromptAsync( AppResources.FrameFunctionOutputsGetNamingTitle + " " + SelectedFunctionOutput.ID, AppResources.FrameFunctionOutputsGetNamingDescription, AppResources.OK, AppResources.PopupButtonCancel,null,-1,null, SelectedFunctionOutput.Description);
                    if (Result != null) SelectedFunctionOutput.Description = Result;

                    WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));

                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }


        #endregion
    }
}
