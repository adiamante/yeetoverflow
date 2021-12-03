using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetDataSetViewModel : YeetDataViewModel, IYeetKeyedList<YeetDataViewModel>
    {
        protected YeetObservableKeyedList<YeetDataViewModel> _yeetKeyedList;
        bool _selecting = false;
        private YeetDataViewModel _selectedData;

        public YeetDataViewModel SelectedData
        {
            get { return _selectedData; }
            set 
            {
                _selecting = true;
                if (_selectedData != null)
                {
                    _selectedData.IsSelected = false;
                }
                SetValue(ref _selectedData, value);
                if (_selectedData != null)
                {
                    _selectedData.IsSelected = true;
                }
                _selecting = false;
            }
        }

        public YeetDataSetViewModel() : this(Guid.NewGuid(), null)
        {
        }

        public YeetDataSetViewModel(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetObservableKeyedList<YeetDataViewModel>(guid);
            _yeetKeyedList.CollectionChanged += _yeetKeyedList_CollectionChanged;
            _yeetKeyedList.SetInvalidChildCallback((key, child) =>
            {
                if (string.IsNullOrEmpty(child.Key))
                {
                    SetKey(key, child);
                }
            });
        }

        private void _yeetKeyedList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetDataViewModel data in e.NewItems)
                {
                    data.PropertyChanged += Data_PropertyChanged;
                    data.PropertyChangedExtended += Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetDataViewModel data in e.OldItems)
                {
                    data.PropertyChanged -= Data_PropertyChanged;
                    data.PropertyChangedExtended -= Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
                }
            }

            OnCollectionPropertyChanged(e, nameof(Children));
        }

        private void Data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_selecting && e.PropertyName == nameof(YeetDataViewModel.IsSelected))
            {
                var selectedData = (YeetDataViewModel)sender;
                if (selectedData.IsSelected)
                {
                    SelectedData = selectedData;
                }
            }
        }

        private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(e);
        }

        private void Data_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Children));
        }

        public YeetDataViewModel this[string key] { get => ((IYeetKeyedList<YeetDataViewModel>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetDataViewModel>)_yeetKeyedList)[key] = value; }

        public YeetDataViewModel this[int key] => ((IYeetListBase<YeetDataViewModel>)_yeetKeyedList)[key];

        public IEnumerable<YeetDataViewModel> Children => ((IYeetListBaseRead<YeetDataViewModel>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetDataViewModel>)_yeetKeyedList).Count;

        public void AddChild(YeetDataViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetDataViewModel>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetDataViewModel>)_yeetKeyedList).ContainsKey(key);
        }

        public virtual void Init()
        {
            _yeetKeyedList.Init();

            foreach (var child in _yeetKeyedList.Children)
            {
                switch (child)
                {
                    case YeetDataSetViewModel childDataSet:
                        childDataSet.Init();
                        break;
                    case YeetTableViewModel childTable:
                        childTable.Init();
                        break;
                }
            }
        }

        public void InsertChildAt(int targetSequence, YeetDataViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetDataViewModel>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetDataViewModel childToMove)
        {
            ((IYeetListBaseWrite<YeetDataViewModel>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetDataViewModel>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetDataViewModel childToRemove)
        {
            ((IYeetListBaseWrite<YeetDataViewModel>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetDataViewModel>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
