using System;
using System.Collections.Specialized;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetColumnCollectionViewModel : YeetDataSetViewModel
    {
        public new YeetColumnViewModel this[string key] { get => (YeetColumnViewModel)_yeetKeyedList[key]; set => _yeetKeyedList[key] = value; }

        public new YeetColumnViewModel this[int key] => (YeetColumnViewModel)_yeetKeyedList[key];

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)_yeetKeyedList).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)_yeetKeyedList).CollectionChanged -= value;
            }
        }

        public YeetColumnCollectionViewModel() : this(Guid.NewGuid())
        {
            
        }

        public YeetColumnCollectionViewModel(Guid guid) : base(guid, null)
        {
            //_yeetKeyedList.CollectionChanged += YeetColumnCollectionViewModel_CollectionChanged;
        }

        //private void YeetColumnCollectionViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (YeetColumnViewModel data in e.NewItems)
        //        {
        //            data.PropertyChangedExtended += Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (YeetColumnViewModel data in e.OldItems)
        //        {
        //            data.PropertyChangedExtended -= Data_PropertyChangedExtended;
        //            data.CollectionPropertyChanged -= Data_CollectionPropertyChanged;
        //        }
        //    }

        //    OnCollectionPropertyChanged(e, nameof(Children));
        //}

        private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(e);
        }

        private void Data_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Children));
        }
    }
}
