using System.Windows;
using OpenDDSharp.ShapesDemo.ViewModel;

namespace OpenDDSharp.ShapesDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }
    }
}
