using GalaSoft.MvvmLight;
using AntiMicBoostGui.Model;
using GalaSoft.MvvmLight.Command;
using System;

namespace AntiMicBoostGui.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private readonly Core Core = new Core();

        private int _desiredVolume;
        public int DesiredVolume
        {
            get
            {
                return _desiredVolume;
            }
            set
            {
                Set(ref _desiredVolume, value);
                Core.DesiredVolume = value / 100f;
                SaveConfig();
            }
        }
       
        private bool _openOnSystemStartup;
        public bool OpenOnSystemStartup
        {
            get
            {
                return _openOnSystemStartup;
            }
            set
            {
                Set(ref _openOnSystemStartup, value);
                
                if (value)
                    Startup.CreateStartupFolderShortcut();
                else
                    Startup.DeleteStartupFolderShortcuts();

                SaveConfig();
            }
        }

        private bool _startMinimized;
        public bool StartMinimized
        {
            get
            {
                return _startMinimized;
            }
            set
            {
                Set(ref _startMinimized, value);
                SaveConfig();
            }
        }

        //private string _notification;
        //public string Notification
        //{
        //    get { return _notification; }
        //    set
        //    {
        //        Set(ref _notification, value);
        //    }
        //}

        public void Notify(string message)
        {
            MessengerInstance.Send(message);
        }

        private RelayCommand _showWindowCommand;
        public RelayCommand ShowWindowCommand
        {
            get
            {
                return _showWindowCommand
                    ?? (_showWindowCommand = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(true);
                    }));
            }
        }

        private RelayCommand _hideWindowCommand;
        public RelayCommand HideWindowCommand
        {
            get
            {
                return _hideWindowCommand
                    ?? (_hideWindowCommand = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(false);
                    }));
            }
        }

        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand
                    ?? (_exitCommand = new RelayCommand(
                    () =>
                    {
                        Environment.Exit(0);
                    }));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.LoadData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    DesiredVolume = item.DesiredVolume;
                    OpenOnSystemStartup = item.OpenOnSystemStartup;
                    StartMinimized = item.StartMinimized;

                    if (OpenOnSystemStartup)
                        Startup.CreateStartupFolderShortcut();
                });

            Core.RecordLevelChangedCallback = Notify;
            Core.Start();
            
        }

        private void SaveConfig()
        {
            _dataService.SaveData(new DataItem
            {
                DesiredVolume = DesiredVolume,
                OpenOnSystemStartup = OpenOnSystemStartup,
                StartMinimized = StartMinimized
            });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}