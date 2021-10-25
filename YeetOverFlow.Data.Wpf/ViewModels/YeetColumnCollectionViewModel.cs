using System;
using System.Collections.Specialized;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetColumnCollectionViewModel : YeetObservableKeyedList<YeetColumnViewModel>
    {
        public YeetColumnCollectionViewModel() : this(Guid.NewGuid())
        {
            
        }

        public YeetColumnCollectionViewModel(Guid guid) : base(guid)
        {
            CollectionChanged += YeetColumnCollectionViewModel_CollectionChanged;
        }

        private void YeetColumnCollectionViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (YeetColumnViewModel data in e.NewItems)
                {
                    data.PropertyChangedExtended += Data_PropertyChangedExtended;
                    data.CollectionPropertyChanged += Data_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetColumnViewModel data in e.OldItems)
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
    }
}
