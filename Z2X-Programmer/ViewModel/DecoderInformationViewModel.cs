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
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
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
        bool manufacturerZIMO;

        [ObservableProperty]
        bool zIMO_SUBVERSIONNR_CV65;

        [ObservableProperty]
        bool zIMO_DECODERTYPE_CV250;


        [ObservableProperty]
        bool zIMO_DECODERID_CV25X;

        [ObservableProperty]
        bool zIMO_BOOTLOADER_VERSION_24X;

        // ZIMO_SOUNDPROJECTVERSIONINFO_CV25X
        [ObservableProperty]
        bool zIMO_SOUNDPROJECTVERSIONINFO_CV25X;

        // ZIMO_SOUNDPROJECTMANUFACTURER_CV105X
        [ObservableProperty]
        bool zIMO_SOUNDPROJECTMANUFACTURER_CV105X;

        // Doehler & Haass: Decoder type (DOEHLERANDHAAS_DECODERTYPE_CV261)
        [ObservableProperty]
        bool dOEHLERANDHAAS_DECODERTYPE_CV261 = false;

        // Doehler & Haass: Decoder firmware version (DOEHLERANDHAAS_FIRMWAREVERSION_CV262x)
        [ObservableProperty]
        bool dOEHLERANDHAAS_FIRMWAREVERSION_CV262x = false;

        // PIKO SmartDecoder 4.1 (PIKOSMARTDECODER_DECODERID_CV26X)
        [ObservableProperty]
        bool pIKOSMARTDECODER_DECODERID_CV26X = false;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        #region Doehler & Haass

        // Doehler & Haass: Decoder type (DOEHLERANDHAAS_DECODERTYPE_CV261)
        [ObservableProperty]
        internal string doehlerAndHaasDecoderType = string.Empty;

        [ObservableProperty]
        string cV261Configuration = Subline.Create([261]);

        // Doehler & Haass: Decoder firmware version (DOEHLERANDHAAS_FIRMWAREVERSION_CV262x)
        [ObservableProperty]
        internal string doehlerAndHaasFirmwareVersion = string.Empty;

        [ObservableProperty]
        string haasFirmwareVersionConfiguration = Subline.Create([262, 264]);

        [ObservableProperty]
        string cV262To264Configuration = Subline.Create([262, 264]);


        #endregion

        // RCN225: Manufacturer in CV8
        [ObservableProperty]
        internal string manufacturer = string.Empty;

        [ObservableProperty]
        internal string manufacturerID = string.Empty;

        [ObservableProperty]
        string cV8Configuration = Subline.Create([8]);

        // RCN225: Software version in CV7
        [ObservableProperty]
        internal string version = string.Empty;

        [ObservableProperty]
        string cV7Configuration = Subline.Create([7]);


        //  ZIMO: Software version (ZIMO_SUBVERSIONNR_CV65)
        [ObservableProperty]
        internal string zimoSWVersion = string.Empty;
        [ObservableProperty]

        string cV65and7Configuration = Subline.Create([7, 65]);

        //  ZIMO: Decoder type (ZIMO_DECODERTYPE_CV250)
        [ObservableProperty]
        internal string zimoDecoderType = string.Empty;

        [ObservableProperty]
        string cV250Configuration = Subline.Create([250]);

        // ZIMO: Decoder ID (ZIMO_DECODERID_CV25X)
        [ObservableProperty]
        internal string zimoDecoderID = string.Empty;

        [ObservableProperty]
        string cVDecoderIDConfiguration = Subline.Create([250, 251, 252, 253]);

        // ZIMO: Bootloader version (ZIMO_BOOTLOADER_VERSION_24X)
        [ObservableProperty]
        internal string zimoBootloaderVersion = string.Empty;

        [ObservableProperty]
        string cVBootloaderVersionConfiguration = Subline.Create([248, 249]);

        [ObservableProperty]
        internal bool zimoBootloaderIsFailSafe = false;

        // ZIMO: Sound project manufacturer (ZIMO_SOUNDPROJECTMANUFACTURER_CV105X)
        [ObservableProperty]
        internal string zimoSoundProjectManufacturer = string.Empty;

        [ObservableProperty]
        string cV105XConfiguraion = Subline.Create([105, 106]);

        // ZIMO: Sound project number (ZIMO_SOUNDPROJECTVERSIONINFO_CV25X) 
        [ObservableProperty]
        internal string zimoSoundProjectNumber = string.Empty;

        [ObservableProperty]
        string cV25XConfiguration = Subline.Create([254, 255, 256, 257]);

        [ObservableProperty]
        string userDefinedDecoderManual = String.Empty;
        partial void OnUserDefinedDecoderManualChanged(string value)
        {
            if (value == null) return;
            if (value != DecoderConfiguration.UserDefinedDecoderManual) WeakReferenceMessenger.Default.Send(new SomethingChangedMessage(true));
            DecoderConfiguration.UserDefinedDecoderManual = value;
        }

        [ObservableProperty]
        internal string userDefindedNotes = string.Empty;
        partial void OnUserDefindedNotesChanged(string value)
        {
            if (value == null) return;
            if (value != DecoderConfiguration.UserDefindedNotes) WeakReferenceMessenger.Default.Send(new SomethingChangedMessage(true));
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
                if (value != DecoderConfiguration.UserDefindedDecoderDescription) WeakReferenceMessenger.Default.Send(new SomethingChangedMessage(true));
                DecoderConfiguration.UserDefindedDecoderDescription = value;
                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
            }
        }

        //  PIKO SmartDecoder V4.1: Decoder ID (PIKOSMARTDECODER_DECODERID_CV26X)
        [ObservableProperty]
        internal string pikoDecoderID = string.Empty;

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
            ZIMO_SOUNDPROJECTVERSIONINFO_CV25X = DecoderSpecification.ZIMO_SOUNDPROJECTVERSIONINFO_CV25X;
            ZIMO_SOUNDPROJECTMANUFACTURER_CV105X = DecoderSpecification.ZIMO_SOUNDPROJECTMANUFACTURER_CV105X;

            // PIKO SmartDecoder 4.1 (PIKOSMARTDECODER_DECODERID_CV26X)        
            PIKOSMARTDECODER_DECODERID_CV26X = DecoderSpecification.PIKOSMARTDECODER_DECODERID_CV26X;
        }

        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        public void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;

            // User defined settings
            UserDefindedDecoderDescription = DecoderConfiguration.UserDefindedDecoderDescription;
            UserDefindedNotes = DecoderConfiguration.UserDefindedNotes;
            if (DecoderConfiguration.UserDefindedImage != null)
            {
                LocomotiveImageSource = Base64StringToImage.ConvertBase64String2ImageSource(DecoderConfiguration.UserDefindedImage);
            }
            else
            {
                LocomotiveImageSource = ImageSource.FromFile("ic_fluent_image_add_24_regular.png");
            }
            UserDefinedDecoderManual = DecoderConfiguration.UserDefinedDecoderManual;

            // RCN225

            // RCN225: Manufacturer in CV8
            Manufacturer = DecoderConfiguration.RCN225.Manufacturer;
            ManufacturerID = "ID = " + DecoderConfiguration.RCN225.ManufacturerID.ToString();
            CV8Configuration = Subline.Create([8]);

            // RCN225: Software version in CV7
            Version = DecoderConfiguration.RCN225.Version;
            CV7Configuration = Subline.Create([7]);

            // ZIMO

            //  ZIMO: Decoder type (ZIMO_DECODERTYPE_CV250)
            string DecoderName = DeqSpecReader.GetDecoderName(DecoderSpecification.DeqSpecName, DecoderConfiguration.ZIMO.DecoderType, ApplicationFolders.DecSpecsFolderPath);
            if (DecoderName != "")
            {
                ZimoDecoderType = DecoderConfiguration.ZIMO.DecoderType + " = " + DecoderName;
            }
            else
            {
                ZimoDecoderType = DecoderConfiguration.ZIMO.DecoderType.ToString();
            }
            CV250Configuration = Subline.Create([250]);

            //  ZIMO: Software version (ZIMO_SUBVERSIONNR_CV65)
            ZimoSWVersion = DecoderConfiguration.ZIMO.SoftwareVersion;
            CV65and7Configuration = Subline.Create([7, 65]);

            // ZIMO: Decoder ID (ZIMO_DECODERID_CV25X)
            ZimoDecoderID = DecoderConfiguration.ZIMO.DecoderID;
            CVDecoderIDConfiguration = Subline.Create([250, 251, 252, 253]);

            // ZIMO: Bootloader version (ZIMO_BOOTLOADER_VERSION_24X)
            ZimoBootloaderVersion = DecoderConfiguration.ZIMO.BootloaderVersion.ToString() + "." + DecoderConfiguration.ZIMO.BootloaderSubVersion.ToString();
            CVBootloaderVersionConfiguration = Subline.Create([248, 249]);

            // ZIMO: Sound project version information (ZIMO_SOUNDPROJECTVERSIONINFO_CV25X) 
            ZimoSoundProjectNumber = DecoderConfiguration.ZIMO.SoundProjectNumber.ToString();
            CV25XConfiguration = Subline.Create([254, 255, 256, 257]);

            // ZIMO: Sound project manufacturer (ZIMO_SOUNDPROJECTMANUFACTURER_CV105X)
            ZimoSoundProjectManufacturer = DecoderConfiguration.ZIMO.SoundProjectManufacturer;
            CV105XConfiguraion = Subline.Create([105, 106]);


            ZimoBootloaderIsFailSafe = ZIMO.IsBootloaderVersionFailSafe(DecoderConfiguration.ZIMO.BootloaderVersion, DecoderConfiguration.ZIMO.BootloaderSubVersion);

            #region Doehler & Haass

            //  Doehler & Haass: Decoder type (DOEHLERANDHAAS_DECODERTYPE_CV261)
            DecoderName = DeqSpecReader.GetDecoderName(DecoderSpecification.DeqSpecName, DecoderConfiguration.DoehlerHaas.DecoderType, ApplicationFolders.DecSpecsFolderPath);
            if (DecoderName != "")
            {
                DoehlerAndHaasDecoderType = DecoderName;
            }
            else
            {
                DoehlerAndHaasDecoderType = DecoderConfiguration.DoehlerHaas.DecoderType.ToString();
            }
            CV261Configuration = Subline.Create([261]);

            //  Doehler & Haass: Decoder firmware version (DOEHLERANDHAAS_FIRMWAREVERSION_CV262x)
            DoehlerAndHaasFirmwareVersion = DecoderConfiguration.DoehlerHaas.FirmwareVersion;
            HaasFirmwareVersionConfiguration = Subline.Create([262, 264]);

            #endregion


            // PIKO SmartDecoder 4.1: Decoder ID (PIKOSMARTDECODER_DECODERID_CV26X)
            PikoDecoderID = DecoderConfiguration.PikoSmartDecoderV41.DecoderID;

        }


        #endregion

        #region REGION: COMMANDS

        /// <summary>
        /// Copies the given string clip clipBoardText to the clipboard.
        /// </summary>
        /// <param name="clipBoardText">Text to be copied to the clipboard.</param>
        [RelayCommand]
         async Task CopyTextClipboard (string clipBoardText)
        {
            try
            {
                await Clipboard.Default.SetTextAsync(clipBoardText);
            }
            catch (Exception ex)
            { 
                await MessageBox.Show(AppResources.AlertInformation, ex.Message, AppResources.OK);
            }   
        }

        /// <summary>
        /// Copies the decoder information to the clipboard.
        /// </summary>            
        [RelayCommand]
        async Task CopyClipboard()
        {
            try
            {
                string clipboardText = AppResources.FrameDecoderClipboardManufacturerCV8 + " " + Manufacturer + " (" + DecoderConfiguration.ConfigurationVariables[8].Value.ToString() + ")\n";

                // Depending on the manufacturer, we create different character strings. 
                switch (DecoderConfiguration.ConfigurationVariables[8].Value)
                {
                    case NMRA.ManufacturerID_DoehlerAndHaass:   //  Doehler & Haass
                    case NMRA.ManufacturerID_Trix:              //  Minitrix
                    case NMRA.ManufacturerID_PIKO:              //  PIKO
                                clipboardText += AppResources.FrameDecoderClipboardVersionCV7 +  " " + Version + "\n";        
                                if(DOEHLERANDHAAS_DECODERTYPE_CV261 == true) clipboardText += AppResources.FrameDecoderClipboardDecoderType + " " + DoehlerAndHaasDecoderType + "\n";
                                if(DOEHLERANDHAAS_FIRMWAREVERSION_CV262x == true) clipboardText += AppResources.FrameDecoderClipboardFirmwareVersion  + " " + DoehlerAndHaasFirmwareVersion + "\n";
                                if(PIKOSMARTDECODER_DECODERID_CV26X == true) clipboardText += AppResources.FrameDecoderClipboardDecoderID +  " " + PikoDecoderID + "\n";  
                                break;

                    case NMRA.ManufacturerID_Zimo:   //  ZIMO
                                clipboardText += AppResources.FrameDecoderClipboardDecoderType + " " + ZimoDecoderType + "\n";
                                clipboardText += AppResources.FrameDecoderClipboardFirmwareVersion  + " " + ZimoSWVersion + "\n";
                                clipboardText += AppResources.FrameDecoderClipboardDecoderID +  " " + ZimoDecoderID + "\n";  
                                clipboardText += AppResources.FrameDecoderClipboardBootloaderVersion + " " + ZimoBootloaderVersion + "\n";
                                break;

                    default:    //  All other manufacturers
                                clipboardText += AppResources.FrameDecoderClipboardVersionCV7 +  " " + Version + "\n";  
                                break;  
                }

                await Clipboard.Default.SetTextAsync(clipboardText);

                //  Inform the user that the information has been copied to the clipboard.
                await MessageBox.Show(AppResources.AlertInformation, AppResources.AlertDecoderInfoCopySuccessFull, AppResources.OK);

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertInformation, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Opens the user-defined decoder manual.
        /// </summary>
        [RelayCommand]
        async Task OpenDecoderManual()
        {
          try
            {
                if (string.IsNullOrEmpty(UserDefinedDecoderManual)) return;
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(UserDefinedDecoderManual)
                });
            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }
        }

        /// <summary>
        /// Opens a file picker to select a PDF document as user-defined decoder manual.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SelectDecoderManual()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a PDF document",
                    FileTypes = FilePickerFileType.Pdf
                });

                if (result == null) return;

                UserDefinedDecoderManual = result.FullPath;

                WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                WeakReferenceMessenger.Default.Send(new SomethingChangedMessage(true));

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
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

                WeakReferenceMessenger.Default.Send(new SomethingChangedMessage(true));

            }
            catch (Exception ex)
            {
                await MessageBox.Show(AppResources.AlertError, ex.Message, AppResources.OK);
            }

        }

        #endregion

    }
}
