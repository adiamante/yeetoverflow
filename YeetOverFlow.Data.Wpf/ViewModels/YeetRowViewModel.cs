using System;
using System.Collections.Generic;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowViewModel : YeetDataViewModel, IYeetKeyedList<YeetCellViewModel>
    {
        protected YeetObservableKeyedList<YeetCellViewModel> _yeetKeyedList;

        public IEnumerable<YeetCellViewModel> Children => ((IYeetListBaseRead<YeetCellViewModel>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetCellViewModel>)_yeetKeyedList).Count;

        public YeetCellViewModel this[int key] => ((IYeetListBase<YeetCellViewModel>)_yeetKeyedList)[key];

        public YeetCellViewModel this[string key] { get => ((IYeetKeyedList<YeetCellViewModel>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetCellViewModel>)_yeetKeyedList)[key] = value; }

        public YeetRowViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetRowViewModel(Guid guid) : base(guid, null)
        {
            _yeetKeyedList = new YeetObservableKeyedList<YeetCellViewModel>();
            _yeetKeyedList.CollectionChanged += YeetRowViewModel_CollectionChanged;
        }

        private void YeetRowViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetCellViewModel data in e.NewItems)
                {
                    data.PropertyChangedExtended += Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetCellViewModel data in e.OldItems)
                {
                    data.PropertyChangedExtended -= Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
                }
            }

            OnCollectionPropertyChanged(e, nameof(Children));
        }

        public void Init()
        {
            _yeetKeyedList.Init();
        }

        private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(e);
        }

        private void Data_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Children));
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetCellViewModel>)_yeetKeyedList).ContainsKey(key);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetCellViewModel>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void AddChild(YeetCellViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetCellViewModel>)_yeetKeyedList).AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetCellViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetCellViewModel>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetCellViewModel childToMove)
        {
            ((IYeetListBaseWrite<YeetCellViewModel>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetCellViewModel childToRemove)
        {
            ((IYeetListBaseWrite<YeetCellViewModel>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetCellViewModel>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
