namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetWindowViewModel : YeetItemViewModelBase
    {
        YeetSettingLibraryViewModel _settings;
        YeetCommandManagerViewModel _commandManager;
        string _message;
        bool _isBusy;

        public YeetSettingLibraryViewModel Settings
        {
            get { return _settings; }
            set { SetValue(ref _settings, value); }
        }

        public YeetCommandManagerViewModel CommandManager
        {
            get { return _commandManager; }
            set { SetValue(ref _commandManager, value); }
        }

        public string Message
        {
            get { return _message; }
            set { 
                SetValue(ref _message, value);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetValue(ref _isBusy, value); }
        }

        public YeetWindowViewModel(YeetSettingLibraryViewModel settings, YeetCommandManagerViewModel commandManager)
        {
            _settings = settings;
            _commandManager = commandManager;
        }
    }
}
