using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetColumnViewModel : YeetDataViewModel
    {
        #region Private Members
        bool _isSelected, _isColumnFilterOpen;
        #endregion Private Members

        #region Public Properties
        public YeetColumnFilterViewModel ColumnFilter { get; set; } = new YeetColumnFilterViewModel();

        #region IsSelected
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value); }
        }
        #endregion IsSelected

        #region IsColumnFilterOpen
        public bool IsColumnFilterOpen
        {
            get { return _isColumnFilterOpen; }
            set { SetValue(ref _isColumnFilterOpen, value); }
        }
        #endregion IsColumnFilterOpen

        #region HasAppliedFilter
        [JsonIgnore]
        public Boolean HasAppliedFilter
        {
            get { return !String.IsNullOrEmpty(ColumnFilter.Filter) || ColumnFilter.Values.Count > 0; }
        }
        #endregion HasAppliedFilter
        #endregion Public Properties

        #region Initialization
        YeetColumnViewModel() : this(Guid.NewGuid(), null)
        {
            
        }

        private void ColumnFilter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(YeetColumnFilterViewModel.Filter):
                case nameof(YeetColumnFilterViewModel.FilterMode):
                case nameof(YeetColumnFilterViewModel.Values):
                    OnPropertyChanged(nameof(YeetColumnViewModel.HasAppliedFilter));
                    break;
            }
        }

        public YeetColumnViewModel(Guid guid, string key) : base(guid, key)
        {
            ColumnFilter.PropertyChanged += ColumnFilter_PropertyChanged;
        }
        #endregion Initialization

        #region Methods
        public void Rename(string newName)
        {
            Name = newName;
            SetKey(newName);
        }
        #endregion Methods
    }

    public class YeetColumnValueViewModel : YeetItemViewModelBaseExtended
    {
        #region Private Members
        object _value;
        bool _isChecked;
        int _count;
        #endregion Private Members

        #region Public Properties
        #region Value
        public object Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }
        #endregion Value
        #region IsChecked
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetValue(ref _isChecked, value); }
        }
        #endregion IsChecked
        #region Count
        public int Count
        {
            get { return _count; }
            set { SetValue(ref _count, value); }
        }
        #endregion Count
        #endregion Public Properties
    }

    public class YeetColumnFilterViewModel : YeetItemViewModelBaseExtended
    {
        #region Private Members
        FilterMode _filtermode;
        string _filter = "";
        bool _showAllValues = false;
        List<object> _values = new List<object>();
        #endregion Private Members

        #region Public Properties
        #region FilterMode
        public FilterMode FilterMode
        {
            get { return _filtermode; }
            set { SetValue(ref _filtermode, value); }
        }
        #endregion FilterMode

        #region Filter
        public string Filter
        {
            get { return _filter; }
            set { SetValue(ref _filter, value); }
        }
        #endregion Filter

        #region ShowAllValues
        public bool ShowAllValues
        {
            get { return _showAllValues; }
            set { SetValue(ref _showAllValues, value); }
        }
        #endregion ShowAllValues

        #region Values
        public List<object> Values
        {
            get { return _values; }
            set { SetValue(ref _values, value); }
        }
        #endregion Values
        #endregion Public Properties
    }
}
