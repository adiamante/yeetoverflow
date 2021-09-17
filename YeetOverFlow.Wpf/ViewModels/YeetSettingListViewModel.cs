using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetSettingListViewModel : YeetSettingViewModel, IYeetKeyedList<YeetSettingViewModel>
    {
        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value); }
        }

        protected YeetObservableKeyedList<YeetSettingListViewModel, YeetSettingViewModel> _yeetKeyedList;

        public YeetSettingListViewModel() : this(Guid.NewGuid(), null)
        {
        }

        public YeetSettingListViewModel(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetObservableKeyedList<YeetSettingListViewModel, YeetSettingViewModel>();
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
                foreach (YeetSettingViewModel setting in e.NewItems)
                {
                    setting.PropertyChangedExtended += Setting_PropertyChangedExtended;
                    setting.CollectionPropertyChanged += Setting_CollectionPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (YeetSettingViewModel setting in e.OldItems)
                {
                    setting.PropertyChangedExtended -= Setting_PropertyChangedExtended;
                    setting.CollectionPropertyChanged -= Setting_CollectionPropertyChanged;
                }
            }

            OnCollectionPropertyChanged(e, nameof(Children));
        }

        private void Setting_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(e);
        }

        private void Setting_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Children));
        }

        public override YeetSettingViewModel this[string key] { get => ((IYeetKeyedList<YeetSettingViewModel>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetSettingViewModel>)_yeetKeyedList)[key] = value; }

        public IEnumerable<YeetSettingViewModel> Children => ((IYeetListBaseRead<YeetSettingViewModel>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetSettingViewModel>)_yeetKeyedList).Count;

        public YeetSettingViewModel this[int key] => ((IYeetListBase<YeetSettingViewModel>)_yeetKeyedList)[key];

        public void AddChild(YeetSettingViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetSettingViewModel>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetSettingViewModel>)_yeetKeyedList).ContainsKey(key);
        }

        public void Init()
        {
            _yeetKeyedList.Init();

            foreach (var child in _yeetKeyedList.Children)
            {
                if (child is YeetSettingListViewModel childList)
                {
                    childList.Init();
                }
            }
        }

        public void InsertChildAt(int targetSequence, YeetSettingViewModel newChild)
        {
            ((IYeetListBaseWrite<YeetSettingViewModel>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetSettingViewModel childToMove)
        {
            ((IYeetListBaseWrite<YeetSettingViewModel>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetSettingViewModel>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetSettingViewModel childToRemove)
        {
            ((IYeetListBaseWrite<YeetSettingViewModel>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetSettingViewModel>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
