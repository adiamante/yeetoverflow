using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowCollectionViewModel : YeetDataViewModel, IYeetListBase<YeetRowViewModel>
    {
        protected YeetObservableList<YeetRowViewModel> _yeetKeyedList;

        public IEnumerable<YeetRowViewModel> Children => ((IYeetListBaseRead<YeetRowViewModel>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetRowViewModel>)_yeetKeyedList).Count;

        public YeetRowViewModel this[int key] => ((IYeetListBase<YeetRowViewModel>)_yeetKeyedList)[key];

        public YeetRowViewModel this[string key] { get => ((IYeetKeyedList<YeetRowViewModel>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetRowViewModel>)_yeetKeyedList)[key] = value; }

        public YeetRowCollectionViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetRowCollectionViewModel(Guid guid) : base(guid, null)
        {
            _yeetKeyedList = new YeetObservableList<YeetRowViewModel>();
            _yeetKeyedList.CollectionChanged += YeetRowCollectionViewModel_CollectionChanged;
        }

        private void YeetRowCollectionViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetRowViewModel data in e.NewItems)
                {
                    data.PropertyChangedExtended += Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetRowViewModel data in e.OldItems)
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

        public void Init()
        {
            _yeetKeyedList.Init();

            foreach (var child in _yeetKeyedList.Children)
            {
                child.Init();
            }
        }

        public void AddChild(YeetRowViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetRowViewModel>)_yeetKeyedList).AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetRowViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetRowViewModel>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetRowViewModel childToMove)
        {
            ((IYeetListBaseWrite<YeetRowViewModel>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetRowViewModel childToRemove)
        {
            ((IYeetListBaseWrite<YeetRowViewModel>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetRowViewModel>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
