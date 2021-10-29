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
        YeetCommandManagerViewModel _commandManager;
        ConcurrentDictionary<Guid, YeetDataViewModel> _guidToYeetData = new ConcurrentDictionary<Guid, YeetDataViewModel>();
        IMapper _mapper;
        bool _isOpen;

        public YeetDataLibraryViewModel(IMapperFactory mapperFactory, YeetCommandManagerViewModel commandManager)
        {
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
            if (e.PropertyName != "IsColumnFilterOpen")
            {
                _commandManager.Handle<YeetData>(e, _mapper);
            }
        }

        private void _root_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            _commandManager.Handle<YeetData>(e, _mapper);

            if (e.NewItems != null)
            {
                foreach (YeetDataViewModel data in e.NewItems)
                {
                    _guidToYeetData.TryAdd(data.Guid, data);
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetDataViewModel data in e.OldItems)
                {
                    _guidToYeetData.TryRemove(data.Guid, out YeetDataViewModel outData);
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
                        _commandManager.DispatchSave<YeetData>();
                        IsOpen = false;
                    }));
            }
        }

        public void Init()
        {
            Resolve(Root);
            Root.Init();
        }

        private void Resolve(YeetDataViewModel data)
        {
            _guidToYeetData.TryAdd(data.Guid, data);

            switch (data)
            {
                case YeetTableViewModel dataTable:
                    foreach (var row in dataTable.Rows.Children)
                    {
                        Resolve(row);
                    }
                    foreach (var col in dataTable.Rows.Children)
                    {
                        Resolve(col);
                    }
                    break;
                case YeetRowViewModel row:
                    foreach (var cell in row.Children)
                    {
                        Resolve(cell);
                    }
                    break;
                case YeetDataSetViewModel dataSet:
                    foreach (var child in dataSet.Children)
                    {
                        Resolve(child);
                    }
                    break;
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
            Init();
        }
    }
}
