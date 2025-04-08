using Microsoft.Extensions.Logging;
using Z2XProgrammer.Pages;
using Z2XProgrammer.ViewModel;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Z2XProgrammer.Helper;
using System.Globalization;
using Z2XProgrammer.DataModel;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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

            builder.Services.AddSingleton<ZIMOFunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<ZIMOFunctionKeysFunctionOutputsViewModel>();

            builder.Services.AddSingleton<DoehlerAndHaassFunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<DoehlerAndHaassFunctionKeysFunctionOutputsViewModel>();

            builder.Services.AddSingleton<RCN225FunctionKeysFunctionOutputsPage>();
            builder.Services.AddSingleton<RCN225FunctionKeysFunctionOutputsViewModel>();


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
