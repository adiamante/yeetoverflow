﻿using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetKeyedItemViewModel : YeetItemViewModelBaseExtended, IKeyedItem
    {
        protected string _key;

        #region Key
        public String Key
        {
            get { return _key; }
        }
        #endregion Key

        public YeetKeyedItemViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetKeyedItemViewModel(Guid guid, string key) : base(guid)
        {
            _key = key;
        }

        protected virtual void SetKey(string key)
        {
            var args = new PropertyChangedExtendedEventArgs(nameof(_key), this, _key, key);
            _key = key;
            OnPropertyChangedExtended(args);
        }
    }
}
