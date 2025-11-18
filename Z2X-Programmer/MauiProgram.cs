using Microsoft.Extensions.Logging;
using Z2XProgrammer.Pages;
using Z2XProgrammer.ViewModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Z2XProgrammer.Helper;
using System.Globalization;
using Z2XProgrammer.DataModel;
using Syncfusion.Maui.Toolkit.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Z2XProgrammer.DataStore;
using System.Xml.Serialization;
using Z2XProgrammer.Model;
using Z2XProgrammer.FileAndFolderManagement;
using Z2XProgrammer.Resources.Strings;

namespace Z2XProgrammer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {          
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    

                });

            #if WINDOWS
            
            //  This code fragment checks if the main window should be closed. If so
            //  we check if 

            builder.ConfigureLifecycleEvents(lifecycle =>
                lifecycle.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        appWindow.Closing += async (s, e) =>
                        {
                            try
                            {
                                //  Check if we are closing the external controller window.
                                if(appWindow.Title == AppResources.ControllerWindowTitle )  
                                {
                                    e.Cancel = false;
                                    return;
                                }


                                // Check if something has changed - indicated by an * at the end of the window title.
                                if (appWindow.Title.EndsWith("*"))
                                {

                                    //  We stop the closing of the window.
                                    e.Cancel = true;

                                    // To create a .NET MAUI messagebox, we need a reference to the page.
                                    Page currentpage = AppShell.Current.CurrentPage;
                                    if (await Application.Current!.Windows[0]!.Page!.DisplayAlert("Z2X-Programmer", AppResources.AlertSaveChanges, AppResources.YES, AppResources.NO) == true)
                                    {
                                        //  We save the Z2X file before we exit the program.
                                        if ((DecoderConfiguration.Z2XFilePath != "") && (File.Exists(DecoderConfiguration.Z2XFilePath) == true))
                                        {
                                            XmlSerializer x = new XmlSerializer(typeof(Z2XProgrammerFileType));
                                            if (File.Exists(DecoderConfiguration.Z2XFilePath) == true) File.Delete(DecoderConfiguration.Z2XFilePath);
                                            using FileStream outputStream = System.IO.File.OpenWrite(DecoderConfiguration.Z2XFilePath);
                                            using StreamWriter streamWriter = new StreamWriter(outputStream);
                                            x.Serialize(streamWriter, Z2XReaderWriter.CreateZ2XProgrammerFile());
                                            streamWriter.Flush();
                                            streamWriter.Close();
                                        }
                                    }

                                    App.Current!.Quit();
                                }
                            }
                            catch
                            {
                                e.Cancel = false;
                                App.Current!.Quit();
                            }
                        };
                    });
                })
            );
             #endif

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddSingleton<AddressPage>();
            builder.Services.AddSingleton<AddressViewModel>();

            builder.Services.AddSingleton<DecoderInformationPage>();
            builder.Services.AddSingleton<DecoderInformationViewModel>();

            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsPageViewModel>();

            builder.Services.AddSingleton<DriveCharacteristicsPage>();
            builder.Services.AddSingleton<DriveCharacteristicsViewModel>();

            builder.Services.AddSingleton<MotorCharacteristicsPage>();
            builder.Services.AddSingleton<MotorCharacteristicsViewModel>();

            builder.Services.AddSingleton<FunctionKeysPage>();
            builder.Services.AddSingleton<FunctionKeysViewModel>();

            builder.Services.AddSingleton<RailComPage>();
            builder.Services.AddSingleton<ProtocolViewModel>();

            builder.Services.AddSingleton<SecurityPage>();
            builder.Services.AddSingleton<SecurityViewModel>();

            builder.Services.AddSingleton<LightPage>();
            builder.Services.AddSingleton<LightViewModel>();

            builder.Services.AddSingleton<ExpertPage>();
            builder.Services.AddSingleton<ExpertViewModel>();

            builder.Services.AddSingleton<InfoPage>();
            builder.Services.AddSingleton<InfoPageViewModel>();

            builder.Services.AddSingleton<SoundPage>();
            builder.Services.AddSingleton<SoundViewModel>();

            builder.Services.AddSingleton<MaintenancePage>();
            builder.Services.AddSingleton<MaintenanceViewModel>();

            builder.Services.AddSingleton<SettingsSearchPage>();
            builder.Services.AddSingleton<SettingsSearchViewModel>();

            builder.Services.AddSingleton<FunctionOutputsPage>();
            builder.Services.AddSingleton<FunctionOutputsViewModel>();

            builder.Services.AddSingleton<FunctionKeysSecondaryAddressPage>();
            builder.Services.AddSingleton<FunctionKeysSecondaryAddressViewModel>();

            builder.Services.AddSingleton<ControllerPage>();
            builder.Services.AddSingleton<ControllerViewModel>();

            builder.Services.AddSingleton<ControllerPageEx>();
            builder.Services.AddSingleton<ControllerViewModelEx>();

            builder.Services.AddSingleton<ZIMOFunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<ZIMOFunctionKeysFunctionOutputsViewModel>();

            builder.Services.AddSingleton<DoehlerAndHaassFunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<DoehlerAndHaassFunctionKeysFunctionOutputsViewModel>();

            builder.Services.AddSingleton<RCN225FunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<RCN225FunctionKeysFunctionOutputsViewModel>();

            builder.Services.AddSingleton<FunctionConfigurationPage>();
            builder.Services.AddSingleton<FunctionConfigurationViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Initialize the logging service.
            Logger.Init(nameof(MauiProgram));
            Logger.LogInformation("Starting InitialSetup.DoFirstSetup() ...");

            // Initialize the UndoRedo managing service.
            UndoRedoManager.Init();

            InitialSetup.DoFirstSetup();

            Logger.LogInformation(" ... waiting for InitialSetup.DoFirstSetup() to complete ...");
            Logger.LogInformation("... InitialSetup.DoFirstSetup() has completed.");


            Routing.RegisterRoute("ZIMOFunctionKeysFunctionOutputsPage", typeof(ZIMOFunctionKeysFunctionOutputsPage));
            Routing.RegisterRoute("RCN225FunctionKeysFunctionOutputsPage", typeof(RCN225FunctionKeysFunctionOutputsPage));
            Routing.RegisterRoute("DoehlerAndHaassFunctionKeysFunctionOutputsPage", typeof(DoehlerAndHaassFunctionKeysFunctionOutputsPage));

            return builder.Build();
        }
    }
}
