using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowCollectionViewModel : YeetDataSetViewModel
    {
        public YeetRowCollectionViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetRowCollectionViewModel(Guid guid) : base(guid, null)
        {
            //_yeetKeyedList.CollectionChanged += YeetRowCollectionViewModel_CollectionChanged;
        }

        //private void YeetRowCollectionViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (YeetRowViewModel data in e.NewItems)
        //        {
        //            data.PropertyChangedExtended += Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (YeetRowViewModel data in e.OldItems)
        //        {
        //            data.PropertyChangedExtended -= Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
        //        }
        //    }

        //    OnCollectionPropertyChanged(e, nameof(Children));
        //}

        //private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        //{
        //    OnPropertyChangedExtended(e);
        //}

        //private void Data_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        //{
        //    OnCollectionPropertyChanged(e, nameof(Children));
        //}

        //public void Init()
        //{
        //    _yeetKeyedList.Init();

        //    foreach (var child in _yeetKeyedList.Children)
        //    {
        //        child.Init();
        //    }
        //}

        public void AddChild(YeetRowViewModel newChild)
        {
            _yeetKeyedList.AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetRowViewModel newChild)
        {
            _yeetKeyedList.InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetRowViewModel childToMove)
        {
            _yeetKeyedList.MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetRowViewModel childToRemove)
        {
            _yeetKeyedList.RemoveChild(childToRemove);
        }
    }
}
