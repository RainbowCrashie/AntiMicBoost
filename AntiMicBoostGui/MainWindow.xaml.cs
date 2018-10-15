using System;
using System.Threading.Tasks;
using System.Windows;
using AntiMicBoostGui.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace AntiMicBoostGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            Messenger.Default.Register(this, (Action<string>)Notify);
            Messenger.Default.Register(this, (Action<bool>)ShowHideWindow);

            InitializeComponent();

            Closing += (s, e) => ViewModelLocator.Cleanup();

            MinimzizeLazily();
        }

        private async Task MinimzizeLazily()
        {
            await Task.Delay(100);
            if (((MainViewModel)DataContext).StartMinimized)
                ((MainViewModel)DataContext).HideWindowCommand.Execute(this);
        }

        private void Notify(string message)
        {
            TaskbarIcon.ShowBalloonTip("AntiMicBoost", message, TaskbarIcon.Icon);
        }

        private void ShowHideWindow(bool show)
        {
            if (show)
            {
                Visibility = Visibility.Visible;
                Activate();
                Focus();
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Cloasing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            ((MainViewModel)DataContext).HideWindowCommand.Execute(this);
        }
    }
}