using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using AutoMapper;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Commands;
using YeetOverFlow.Wpf.Commands;
using YeetOverFlow.Wpf.Mappers;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetDataLibraryViewModel : YeetLibrary<YeetDataSetViewModel>, INotifyPropertyChanged
    {
        System.Windows.Input.ICommand _saveCommand;
        ICommandDispatcher _commandDispatcher;
        YeetCommandManagerViewModel _commandManager;
        ConcurrentDictionary<Guid, YeetDataViewModel> _guidToYeetData = new ConcurrentDictionary<Guid, YeetDataViewModel>();
        IMapper _mapper;
        bool _isOpen;

        public YeetDataLibraryViewModel(ICommandDispatcher commandDispatcher, IMapperFactory mapperFactory, YeetCommandManagerViewModel commandManager)
        {
            _commandDispatcher = commandDispatcher;
            _mapper = mapperFactory.GetMapper("Data");
            _commandManager = commandManager;
        }

        #region Indexer
        public YeetDataViewModel this[Guid guid] { get => _guidToYeetData[guid]; }
        public virtual YeetDataViewModel this[string key] { get => Root[key]; }
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

        public override YeetDataSetViewModel Root
        {
            get => _root;
            set
            {
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
                var cmd = new UpdateYeetItemCommand<YeetData>(((YeetDataViewModel)e.Object).Guid, updates) { DeferCommit = true };
                _commandDispatcher.Dispatch<UpdateYeetItemCommand<YeetData>, Result>(cmd);
            }
        }

        private void _root_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetDataViewModel data in e.NewItems)
                {
                    _guidToYeetData.TryAdd(data.Guid, data);
                    var cmd = new AddYeetItemCommand<YeetData>(((YeetDataSetViewModel)e.Object).Guid, _mapper.Map<YeetData>(data), Int32.MaxValue) { DeferCommit = true };
                    _commandDispatcher.Dispatch<AddYeetItemCommand<YeetData>, Result>(cmd);
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetDataViewModel data in e.OldItems)
                {
                    _guidToYeetData.TryRemove(data.Guid, out YeetDataViewModel outData);
                    var cmd = new RemoveYeetItemCommand<YeetData>(((YeetDataSetViewModel)e.Object).Guid, data.Guid) { DeferCommit = true };
                    _commandDispatcher.Dispatch<RemoveYeetItemCommand<YeetData>, Result>(cmd);
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
                        var cmd = new SaveCommand<YeetData>();
                        _commandDispatcher.Dispatch<SaveCommand<YeetData>, Result>(cmd);
                        IsOpen = false;
                        _commandManager.IsOpen = false;
                    }));
            }
        }

        public void Init()
        {
            Resolve(Root);
        }

        private void Resolve(YeetDataViewModel data)
        {
            _guidToYeetData.TryAdd(data.Guid, data);

            if (data is YeetDataSetViewModel dataList)
            {
                foreach (var child in dataList.Children)
                {
                    Resolve(child);
                }
            }
        }

        public void Reset()
        {
            Root = new YeetDataSetViewModel(Guid.NewGuid(), "root");
            //_root.PropertyChangedExtended -= _root_PropertyChangedExtended;
            //_root.CollectionPropertyChanged -= _root_CollectionPropertyChanged;
            
            //_root.CollectionPropertyChanged += _root_CollectionPropertyChanged;
            //_root.PropertyChangedExtended += _root_PropertyChangedExtended;
        }

        public YeetLibrary<YeetDataSet> ExportLibrary()
        {
            var library = new YeetLibrary<YeetDataSet>(Guid);
            library.Root = _mapper.Map<YeetDataSet>(Root);
            return library;
        }

        public void ImportLibrary(YeetLibrary<YeetDataSet> library)
        {
            Root = _mapper.Map<YeetDataSetViewModel>(library.Root);
            Root.Init();
        }
    }
}
