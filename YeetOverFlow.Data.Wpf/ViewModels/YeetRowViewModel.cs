using System;
using System.Collections.Generic;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowViewModel : YeetDataSetViewModel
    {
        public YeetRowViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetRowViewModel(Guid guid) : base(guid, guid.ToString())
        {
            //_yeetKeyedList.CollectionChanged += YeetRowViewModel_CollectionChanged;
        }

        //private void YeetRowViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (YeetCellViewModel data in e.NewItems)
        //        {
        //            data.PropertyChangedExtended += Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (YeetCellViewModel data in e.OldItems)
        //        {
        //            data.PropertyChangedExtended -= Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
        //        }
        //    }

        //    OnCollectionPropertyChanged(e, nameof(Children));
        //}

        //public void Init()
        //{
        //    _yeetKeyedList.Init();
        //}

        //private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        //{
        //    OnPropertyChangedExtended(e);
        //}

        //private void Data_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        //{
        //    OnCollectionPropertyChanged(e, nameof(Children));
        //}
    }
}
