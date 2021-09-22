using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using AutoMapper;
using MahApps.Metro.IconPacks;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Commands;
using YeetOverFlow.Settings;
using YeetOverFlow.Wpf.Commands;
using YeetOverFlow.Wpf.Mappers;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetSettingLibraryViewModel : YeetLibrary<YeetSettingListViewModel>, INotifyPropertyChanged
    {
        System.Windows.Input.ICommand _saveCommand;
        ICommandDispatcher _commandDispatcher;
        YeetCommandManagerViewModel _commandManager;
        ConcurrentDictionary<Guid, YeetSettingViewModel> _guidToYeetSetting = new ConcurrentDictionary<Guid, YeetSettingViewModel>();
        IMapper _mapper;
        bool _isOpen;

        public YeetSettingLibraryViewModel(ICommandDispatcher commandDispatcher, IMapperFactory mapperFactory, YeetCommandManagerViewModel commandManager)
        {
            _commandDispatcher = commandDispatcher;
            _mapper = mapperFactory.GetMapper("Settings");
            _commandManager = commandManager;
        }

        #region Indexer
        public YeetSettingViewModel this[Guid guid] { get => _guidToYeetSetting[guid]; }
        public virtual YeetSettingViewModel this[string key] { get => Root[key]; }
        #endregion Indexer

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            backingField = value;
            OnPropertyChanged(propertyname);
        }
        #endregion INotifyPropertyChanged

        #region IsOpen
        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetValue(ref _isOpen, value); }
        }
        #endregion IsOpen

        public override YeetSettingListViewModel Root 
        { 
            get => _root; 
            set {
                _root = value;
                _root.PropertyChangedExtended += _root_PropertyChangedExtended;
                _root.CollectionPropertyChanged += _root_CollectionPropertyChanged;
            } 
        }

        private void _root_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            if (e.PropertyName != "IsExpanded")
            {
                var updates = new Dictionary<string, string>() { { e.PropertyName, e.NewValue?.ToString() } };
                var cmd = new UpdateYeetItemCommand<YeetSetting>(((YeetSettingViewModel)e.Object).Guid, updates) { DeferCommit = true };
                _commandDispatcher.Dispatch<UpdateYeetItemCommand<YeetSetting>, Result>(cmd);
            }
        }

        private void _root_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetSettingViewModel setting in e.NewItems)
                {
                    _guidToYeetSetting.TryAdd(setting.Guid, setting);
                    var cmd = new AddYeetItemCommand<YeetSetting>(((YeetSettingListViewModel)e.Object).Guid, _mapper.Map<YeetSetting>(setting), Int32.MaxValue) { DeferCommit = true };
                    _commandDispatcher.Dispatch<AddYeetItemCommand<YeetSetting>, Result>(cmd);
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetSettingViewModel setting in e.OldItems)
                {
                    _guidToYeetSetting.TryRemove(setting.Guid, out YeetSettingViewModel outSetting);
                    var cmd = new RemoveYeetItemCommand<YeetSetting>(((YeetSettingListViewModel)e.Object).Guid, setting.Guid) { DeferCommit = true };
                    _commandDispatcher.Dispatch<RemoveYeetItemCommand<YeetSetting>, Result>(cmd);
                }
            }
        }

        public System.Windows.Input.ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand =
                    new RelayCommand(() =>
                    {
                        var cmd = new SaveCommand<YeetSetting>();
                        _commandDispatcher.Dispatch<SaveCommand<YeetSetting>, Result>(cmd);
                        IsOpen = false;
                        _commandManager.IsOpen = false;
                    }));
            }
        }

        public void Init()
        {
            Resolve(Root);
        }

        private void Resolve(YeetSettingViewModel setting)
        {
            _guidToYeetSetting.TryAdd(setting.Guid, setting);

            if (setting is YeetSettingListViewModel settingList)
            {
                foreach (var child in settingList.Children)
                {
                    Resolve(child);
                }
            }
        }

        public void Reset()
        {
            Root = new YeetSettingListViewModel(Guid.NewGuid(), "root");
            _root.PropertyChangedExtended -= _root_PropertyChangedExtended;
            _root.CollectionPropertyChanged -= _root_CollectionPropertyChanged;
            Root["Window"] = new YeetSettingListViewModel() { Icon = PackIconMaterialKind.CogOutline };
            Root["Window"]["Theme"] = new YeetSettingListViewModel() { Icon = PackIconMaterialKind.PaletteOutline };
            Root["Window"]["Theme"]["Base"] = new YeetSettingStringOptionViewModel() { Value = "Light", Icon = PackIconMaterialKind.PaletteSwatchOutline, Options = new[] { "Light", "Dark" } };
            Root["Window"]["Theme"]["Accent"] = new YeetSettingStringOptionViewModel() { Value = "Blue", Icon = PackIconMaterialKind.Brush, Options = new[] { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" } };
            _root.CollectionPropertyChanged += _root_CollectionPropertyChanged;
            _root.PropertyChangedExtended += _root_PropertyChangedExtended;
        }

        public YeetLibrary<YeetSettingList> ExportLibrary()
        {
            var library = new YeetLibrary<YeetSettingList>();
            library.Root = _mapper.Map<YeetSettingList>(Root);
            return library;
        }

        public void ImportLibrary(YeetLibrary<YeetSettingList> library)
        {
            Root = _mapper.Map<YeetSettingListViewModel>(library.Root);
            Root.Init();
        }
    }
}
