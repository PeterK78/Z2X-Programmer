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
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer.ViewModel
{
    /// <summary>
    /// The view model for the DecoderInformationPage
    /// </summary>
    public partial class DecoderInformationViewModel : ObservableObject
    {

        #region REGION: DECODER FEATURES
        [ObservableProperty]
        bool manufacturerZIMO;

        [ObservableProperty]
        bool zIMO_SUBVERSIONNR_CV65;

        [ObservableProperty]
        bool zIMO_DECODERTYPE_CV250;

        [ObservableProperty]
        bool dOEHLERANDHAAS_DECODERTYPE_CV261;

        [ObservableProperty]
        bool dOEHLERANDHAAS_FIRMWAREVERSION_CV262x;

        
        [ObservableProperty]
        bool zIMO_DECODERID_CV25X;

        [ObservableProperty]
        bool zIMO_BOOTLOADER_VERSION_24X;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool dataStoreDataValid;

        //  RCN225 properties
        [ObservableProperty]
        internal string manufacturer = string.Empty;

        [ObservableProperty]
        internal string manufacturerID = string.Empty;

        [ObservableProperty]
        internal string version = string.Empty;

        //  Doehler & Haass properties
        [ObservableProperty]
        internal string doehlerAndHaasDecoderType = string.Empty;

        [ObservableProperty]
        internal string doehlerAndHaasFirmwareVersion = string.Empty;

        //  ZIMO properties
        [ObservableProperty]
        internal string zimoSWVersion = string.Empty;

        [ObservableProperty]
        internal string zimoDecoderType = string.Empty;

        [ObservableProperty]
        internal string zimoDecoderID = string.Empty;

        [ObservableProperty]
        internal string zimoBootloaderVersion = string.Empty;

        [ObservableProperty]
        internal bool zimoBootloaderIsFailSafe = false;

        [ObservableProperty]
        internal string userDefindedNotes = string.Empty;
        partial void OnUserDefindedNotesChanged(string value)
        {
            if (value == null) return;
            DecoderConfiguration.UserDefindedNotes = value;
        }

        [ObservableProperty]
        internal ImageSource locomotiveImageSource = string.Empty;

        [ObservableProperty]
        internal string userDefindedDecoderDescription = string.Empty;
        partial void OnUserDefindedDecoderDescriptionChanged(string value)
        {
            if (DataStoreDataValid == true)
            {
                DecoderConfiguration.UserDefindedDecoderDescription = value;
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
            }
        }

        #endregion

        #region REGION: CONSTRUCTOR
        /// <summary>
        /// ViewModel constructor
        /// </summary>
        public DecoderInformationViewModel()
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
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>         
        public void OnGetDataFromDecoderSpecification()
        {

            //  Doehler & Haass Decoder
            DOEHLERANDHAAS_DECODERTYPE_CV261 = DecoderSpecification.DOEHLERANDHAAS_DECODERTYPE_CV261;
            DOEHLERANDHAAS_FIRMWAREVERSION_CV262x = DecoderSpecification.DOEHLERANDHAAS_FIRMWAREVERSION_CV262x;

            //  ZIMO Decoder
            ZIMO_SUBVERSIONNR_CV65 = DecoderSpecification.ZIMO_SUBVERSIONNR_CV65;
            ZIMO_DECODERTYPE_CV250 = DecoderSpecification.ZIMO_DECODERTYPE_CV250;
            ZIMO_DECODERID_CV25X = DecoderSpecification.ZIMO_DECODERID_CV25X;
            if (DecoderSpecification.ManufacturerID == NMRA.ManufacturerID_Zimo)
            {
                ManufacturerZIMO = true;
            }
            else
            {
                ManufacturerZIMO = false;
            }
            ZIMO_BOOTLOADER_VERSION_24X = DecoderSpecification.ZIMO_BOOTLOADER_VERSION_24X;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            Manufacturer = DecoderConfiguration.RCN225.Manufacturer;
            ManufacturerID = "ID = " + DecoderConfiguration.RCN225.ManufacturerID.ToString();
            Version = DecoderConfiguration.RCN225.Version;

            //  ZIMO specific settings
            string DecoderName = DeqSpecReader.GetDecoderName(DecoderSpecification.DeqSpecName, DecoderConfiguration.ZIMO.DecoderType, ApplicationFolders.DecSpecsFolderPath);
            if (DecoderName != "")
            {
                ZimoDecoderType = DecoderConfiguration.ZIMO.DecoderType + " = "  + DecoderName;
            }
            else
            {
                ZimoDecoderType = DecoderConfiguration.ZIMO.DecoderType.ToString();
            }

            ZimoSWVersion = DecoderConfiguration.ZIMO.SoftwareVersion;
            ZimoDecoderID = DecoderConfiguration.ZIMO.DecoderID;
            UserDefindedDecoderDescription = DecoderConfiguration.UserDefindedDecoderDescription;
            ZimoBootloaderVersion = DecoderConfiguration.ZIMO.BootloaderVersion.ToString() + "." + DecoderConfiguration.ZIMO.BootloaderSubVersion.ToString();
            ZimoBootloaderIsFailSafe = ZIMO.IsBootloaderVersionFailSafe(DecoderConfiguration.ZIMO.BootloaderVersion, DecoderConfiguration.ZIMO.BootloaderSubVersion);


            UserDefindedNotes = DecoderConfiguration.UserDefindedNotes;

            //  Doehler & Haass specific settings
            DoehlerAndHaasDecoderType = DoehlerAndHaass.GetDecoderTypeDesciption(DecoderConfiguration.DoehlerHaas.DecoderType);
            DoehlerAndHaasFirmwareVersion = DecoderConfiguration.DoehlerHaas.FirmwareVersion;

            if (DecoderConfiguration.UserDefindedImage != null)
            {
                LocomotiveImageSource = Base64StringToImage.ConvertBase64String2ImageSource(DecoderConfiguration.UserDefindedImage);
            }
            else
            {
                LocomotiveImageSource = ImageSource.FromFile("ic_fluent_image_add_24_regular.png");
            }
        }

        
        #endregion

        #region REGION: COMMANDS

        [RelayCommand]
        async Task CopyClipboard()
        {
            try
            {
                await Clipboard.Default.SetTextAsync(Manufacturer + " " + ZimoDecoderType + " " + ZimoSWVersion + " " + ZimoDecoderID + " " + ZimoBootloaderVersion);
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }
        }


        [RelayCommand]
        async Task SelectImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null) return;

                var stream = await result.OpenReadAsync();
                MemoryStream memStream = new MemoryStream();
                stream.CopyTo(memStream);
                var ConvertImage = Convert.ToBase64String(memStream.ToArray());
                DecoderConfiguration.UserDefindedImage = ConvertImage;

                LocomotiveImageSource = Base64StringToImage.ConvertBase64String2ImageSource(DecoderConfiguration.UserDefindedImage);

                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
            }
            catch (Exception ex)
            {
                if ((Application.Current != null) && (Application.Current.MainPage != null))
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.AlertError, ex.Message, AppResources.OK);
                }
            }

        }

        #endregion

    }
}
