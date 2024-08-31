using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.IO;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.FileAndFolderManagement;
using Microsoft.Extensions.Logging;
using Z2XProgrammer.Helper;
using CommunityToolkit.Mvvm.Messaging;
using Z2XProgrammer.Messages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Z2XProgrammer.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            _logger = Logger.Init(nameof(App));
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            AppActivationArguments appActivationArguments = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

            if (appActivationArguments != null)
            {
            
                if (appActivationArguments.Kind is ExtendedActivationKind.File)
                {

                    if (appActivationArguments.Data is IFileActivatedEventArgs fileArgs)
                    {
                        IStorageItem file = fileArgs.Files[0];

                        _logger.LogInformation("Opening z2x file from command line:" + file.Path);
                        Stream fs = File.OpenRead(file.Path);

                        Z2XReaderWriter.ReadFile(fs);

                        DecoderConfiguration.IsValid = true;

                        WeakReferenceMessenger.Default.Send(new DecoderConfigurationUpdateMessage(true));
                        WeakReferenceMessenger.Default.Send(new DecoderSpecificationUpdatedMessage(true));
                    }
                }
            }
        }
    }

   

}
