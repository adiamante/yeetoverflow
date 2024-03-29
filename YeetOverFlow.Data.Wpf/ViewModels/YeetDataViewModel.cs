﻿using System;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetDataViewModel : YeetKeyedItemViewModel
    {
        bool _isVisible = true, _isSelected;

        #region IsVisible
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetValue(ref _isVisible, value, true, false); }
        }
        #endregion IsVisible

        #region IsSelected
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value, true, false); }
        }
        #endregion IsSelected

        public YeetDataViewModel() : base()
        {
            
        }

        public YeetDataViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        #region Methods
        public void Rename(string newName)
        {
            Name = newName;
            SetKey(newName);
        }

        protected virtual void SetKey(String key, YeetDataViewModel data)
        {
            data._key = key;
            data.OnPropertyChanged(nameof(Key));
        }
        #endregion Methods
    }
}
