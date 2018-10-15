using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using GalaSoft.MvvmLight.Threading;

namespace AntiMicBoostGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();

            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
        }
    }
}
