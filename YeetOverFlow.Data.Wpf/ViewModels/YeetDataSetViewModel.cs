using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetDataSetViewModel : YeetDataViewModel, IYeetKeyedList<YeetDataViewModel>
    {
        protected YeetObservableKeyedList<YeetDataViewModel> _yeetKeyedList;

        public YeetDataSetViewModel() : this(Guid.NewGuid(), null)
        {
        }

        public YeetDataSetViewModel(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetObservableKeyedList<YeetDataViewModel>();
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
                    data.PropertyChangedExtended += Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetDataViewModel data in e.OldItems)
                {
                    data.PropertyChangedExtended -= Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
                }
            }

            OnCollectionPropertyChanged(e, nameof(Children));
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

        public void Init()
        {
            _yeetKeyedList.Init();

            foreach (var child in _yeetKeyedList.Children)
            {
                switch (child)
                {
                    case YeetDataSetViewModel childdataSet:
                        childdataSet.Init();
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
