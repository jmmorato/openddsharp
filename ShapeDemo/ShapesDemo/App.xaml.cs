using System.Windows;
using OpenDDSharp.ShapesDemo.Model;
using OpenDDSharp.ShapesDemo.ViewModel;

namespace OpenDDSharp.ShapesDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>    
    public partial class App : Application
    {
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Ace.Init();

            InteroperatibilityProvider provider = InteroperatibilityProvider.OpenDDS;
            if (e.Args.Length > 0 && e.Args[0].StartsWith("-Vendor="))
            {
                string strProvider = e.Args[0].Replace("-Vendor=", string.Empty);
                switch(strProvider.ToLower())
                {
                    case "rti":
                        provider = InteroperatibilityProvider.Rti;
                        break;
                    case "opensplice":
                        provider = InteroperatibilityProvider.OpenSplice;
                        break;
                    default:
                        provider = InteroperatibilityProvider.OpenDDS;
                        break;
                }
            }

            ViewModelLocator.Provider = provider;
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();

            Ace.Fini();
        }
    }
}
