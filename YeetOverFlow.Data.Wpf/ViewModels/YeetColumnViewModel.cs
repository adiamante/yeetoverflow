using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public abstract class YeetColumnViewModel : YeetDataViewModel
    {
        #region Private Members
        bool _isSelected, _isColumnFilterOpen, _isVisible = true, _isChecked_visible, _ischecked_filter;
        double _total;
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
            set { SetValue(ref _isColumnFilterOpen, value, true, false); }
        }
        #endregion IsColumnFilterOpen

        #region IsVisible
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetValue(ref _isVisible, value); }
        }
        #endregion IsVisible

        #region IsChecked_Visible
        public bool IsChecked_Visible
        {
            get { return _isChecked_visible; }
            set { SetValue(ref _isChecked_visible, value, true, false); }
        }
        #endregion IsChecked_Visible

        #region IsChecked_Filter
        public bool IsChecked_Filter
        {
            get { return _ischecked_filter; }
            set { SetValue(ref _ischecked_filter, value, true, false); }
        }
        #endregion IsChecked_Filter

        #region HasAppliedFilter
        [JsonIgnore]
        public Boolean HasAppliedFilter
        {
            get { return !String.IsNullOrEmpty(ColumnFilter.Filter) || ColumnFilter.Values.Count > 0; }
        }
        #endregion HasAppliedFilter

        #region Total
        public double Total
        {
            get { return _total; }
            set { SetValue(ref _total, value, true, false); }
        }
        #endregion Total

        #region DataType
        public abstract Type DataType { get; }
        #endregion DataType
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

    public abstract class YeetColumnViewModel<T> : YeetColumnViewModel
    {
        public YeetColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        public T Value { get; set; }

        public override Type DataType => typeof(T);
    }

    public class YeetBooleanColumnViewModel : YeetColumnViewModel<bool>
    {
        YeetBooleanColumnViewModel() : this(Guid.Empty, null)
        {

        }
        public YeetBooleanColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetStringColumnViewModel : YeetColumnViewModel<string>
    {
        public YeetStringColumnViewModel() : this(Guid.NewGuid(), null)
        {

        }
        public YeetStringColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetIntColumnViewModel : YeetColumnViewModel<int>
    {
        YeetIntColumnViewModel() : this(Guid.Empty, null)
        {

        }
        public YeetIntColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDoubleColumnViewModel : YeetColumnViewModel<double>
    {
        YeetDoubleColumnViewModel() : this(Guid.Empty, null)
        {

        }
        public YeetDoubleColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDateTimeColumnViewModel : YeetColumnViewModel<DateTime>
    {
        YeetDateTimeColumnViewModel() : this(Guid.Empty, null)
        {

        }
        public YeetDateTimeColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
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
