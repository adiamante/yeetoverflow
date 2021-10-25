using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using YeetOverFlow.Wpf.Commands;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetTableViewModel : YeetDataViewModel
    {
        #region Private Members
        ICommand _applyColumnFilterCommand, _clearColumnFilterCommand, _applyColumnValuesFilterCommand, _filterColumnValuesCommand, _applyAllCheckedColumnValuesCommand, _showAllColumnValuesCommand,
            _renameColumnCommand;
        YeetColumnCollectionViewModel _columns = new YeetColumnCollectionViewModel();
        YeetRowCollectionViewModel _rows = new YeetRowCollectionViewModel();
        Dictionary<string, ObservableCollection<YeetColumnValueViewModel>> _columnValues = new Dictionary<string, ObservableCollection<YeetColumnValueViewModel>>();
        #endregion Private Members

        #region Public Properties

        public YeetColumnCollectionViewModel Columns
        {
            get { return _columns; }
            set { 
                SetValue(ref _columns, value); 
            }
        }
        public YeetRowCollectionViewModel Rows
        {
            get { return _rows; }
            set { SetValue(ref _rows, value); }
        }
        #endregion Public Properties

        #region Commands
        #region ApplyColumnFilterCommand
        [JsonIgnore]
        public ICommand ApplyColumnFilterCommand
        {
            get
            {
                return _applyColumnFilterCommand ?? (_applyColumnFilterCommand =
                    new RelayCommand<string, FilterMode, string>((filter, filterMode, colName) =>
                    {
                        Columns[colName].ColumnFilter.Filter = filter;
                        Columns[colName].ColumnFilter.FilterMode = filterMode;
                        ApplyColumnFilters();
                    }));
            }
        }
        #endregion ApplyColumnFilterCommand

        #region ClearColumnFilterCommand
        [JsonIgnore]
        public ICommand ClearColumnFilterCommand
        {
            get
            {
                return _clearColumnFilterCommand ?? (_clearColumnFilterCommand =
                    new RelayCommand<string>((colName) =>
                    {
                        Columns[colName].ColumnFilter.Filter = "";
                        Columns[colName].ColumnFilter.FilterMode = FilterMode.CONTAINS;
                        Columns[colName].ColumnFilter.Values = new List<object>();
                        ApplyColumnFilters();
                    }));
            }
        }
        #endregion ClearColumnFilterCommand

        #region ApplyColumnValuesFilterCommand
        [JsonIgnore]
        public ICommand ApplyColumnValuesFilterCommand
        {
            get
            {
                return _applyColumnValuesFilterCommand ?? (_applyColumnValuesFilterCommand =
                    new RelayCommand<string>((colName) =>
                    {
                        List<object> checkedValues = new List<object>();
                        foreach (YeetColumnValueViewModel colValue in GetColumnValuesView(colName))
                        {
                            if (colValue.IsChecked)
                            {
                                checkedValues.Add(colValue.Value);
                            }
                        }

                        Columns[colName].ColumnFilter.Values = checkedValues;
                        ApplyColumnFilters();
                    }));
            }
        }
        #endregion ApplyColumnValuesFilterCommand

        #region ApplyAllCheckedColumnValuesCommand
        [JsonIgnore]
        public ICommand ApplyAllCheckedColumnValuesCommand
        {
            get
            {
                return _applyAllCheckedColumnValuesCommand ?? (_applyAllCheckedColumnValuesCommand =
                    new RelayCommand<string, bool>((colName, isChecked) =>
                    {
                        foreach (YeetColumnValueViewModel colValue in GetColumnValuesView(colName))
                        {
                            colValue.IsChecked = !isChecked;
                        }
                    }));
            }
        }
        #endregion ApplyAllCheckedColumnValuesCommand

        #region FilterColumnValuesCommand
        [JsonIgnore]
        public ICommand FilterColumnValuesCommand
        {
            get
            {
                return _filterColumnValuesCommand ?? (_filterColumnValuesCommand =
                    new RelayCommand<string, FilterMode, string>((filter, filterMode, colName) =>
                    {
                        FilterColumnValues(filter, filterMode, colName);
                    }));
            }
        }
        #endregion FilterColumnValuesCommand

        #region ShowAllColumnValuesCommand
        [JsonIgnore]
        public ICommand ShowAllColumnValuesCommand
        {
            get
            {
                return _showAllColumnValuesCommand ?? (_showAllColumnValuesCommand =
                    new RelayCommand<string, bool>((colName, isChecked) =>
                    {
                        var colFilter = Columns[colName].ColumnFilter;
                        Columns[colName].ColumnFilter.ShowAllValues = !isChecked;
                        FilterColumnValues(colFilter.Filter, colFilter.FilterMode, colName);
                    }));
            }
        }
        #endregion ShowAllColumnValuesCommand

        #region RenameColumnCommand
        [JsonIgnore]
        public ICommand RenameColumnCommand
        {
            get
            {
                return _renameColumnCommand ?? (_renameColumnCommand =
                    new RelayCommand<string, string>((colName, newName) =>
                    {
                        RenameColumn(colName, newName);
                    }));
            }
        }
        #endregion RenameColumnCommand
        #endregion Commands

        #region Initialization
        public YeetTableViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetTableViewModel(Guid guid, string key) : base(guid, key)
        {
            Init();
        }

        private void Columns_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Columns));
        }

        private void Rows_CollectionPropertyChanged(object sender, CollectionPropertyChangedEventArgs e)
        {
            OnCollectionPropertyChanged(e, nameof(Rows));
        }

        private void Data_PropertyChangedExtended(object sender, PropertyChangedExtendedEventArgs e)
        {
            OnPropertyChangedExtended(e);
        }

        public void Init()
        {
            Columns.Init();
            Rows.Init();

            Rows.PropertyChangedExtended += Data_PropertyChangedExtended;
            Columns.PropertyChangedExtended += Data_PropertyChangedExtended;
            Rows.CollectionPropertyChanged += Rows_CollectionPropertyChanged;
            Columns.CollectionPropertyChanged += Columns_CollectionPropertyChanged;
        }
        #endregion Initialization

        #region Methods
        public void ApplyColumnFilters()
        {
            var view = CollectionViewSource.GetDefaultView(Rows.Children);
            view.Filter = (r) =>
            {
                var row = (YeetRowViewModel)r;
                foreach (var col in Columns.Children)
                {
                    var cell = row[col.Key];
                    var cellVal = cell.GetValue<string>();
                    var colFilter = col.ColumnFilter;
                    var filter = col.ColumnFilter.Filter;
                    var filterMode = col.ColumnFilter.FilterMode;

                    if (!String.IsNullOrEmpty(filter) && !Evaluate(filter, filterMode, cellVal))
                    {
                        return false;
                    }

                    if (colFilter.Values.Count > 0 && !colFilter.Values.Contains(cell.GetValue()))
                    {
                        return false;
                    }
                }

                return true;
            };

            foreach (var col in Columns.Children)
            {
                var colFilter = col.ColumnFilter;
                var filter = col.ColumnFilter.Filter;
                var filterMode = col.ColumnFilter.FilterMode;
                
                FilterColumnValues(filter, filterMode, col.Key);
            }
        }

        private void FilterColumnValues(string filter, FilterMode filterMode, string colName)
        {
            var view = GetColumnValuesView(colName);
            var colFilter = Columns[colName].ColumnFilter;

            view.Filter = (cv) =>
            {
                var colValueVM = (YeetColumnValueViewModel)cv;
                var strVal = colValueVM.Value.ToString();

                if (!String.IsNullOrEmpty(colFilter.Filter) && !Evaluate(colFilter.Filter, colFilter.FilterMode, strVal))
                {
                    return false;
                }

                if (!colFilter.ShowAllValues && colFilter.Values.Count > 0 && !colFilter.Values.Contains(colValueVM.Value))
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(filter) && !Evaluate(filter, filterMode, strVal))
                {
                    return false;
                }

                return true;
            };

            view.Refresh();
        }

        public ICollectionView GetColumnValuesView(string colName)
        {
            if (!_columnValues.ContainsKey(colName))
            {
                ObservableCollection<YeetColumnValueViewModel> values = new ObservableCollection<YeetColumnValueViewModel>();
                //OPT: Subscribe to row changes here

                foreach (var valGroup in Rows.Children.GroupBy(r => r[colName].GetValue()))
                {
                    values.Add(new YeetColumnValueViewModel() { Value = valGroup.Key, Count = valGroup.Count() });
                }

                _columnValues.Add(colName, values);
            }
            return CollectionViewSource.GetDefaultView(_columnValues[colName]);
        }

        private bool Evaluate(string filter, FilterMode filterMode, string targetValue)
        {
            switch (filterMode)
            {
                case FilterMode.CONTAINS:
                    if (!targetValue.Contains(filter))
                    {
                        return false;
                    }
                    break;
                case FilterMode.EQUALS:
                    if (!targetValue.Equals(filter))
                    {
                        return false;
                    }
                    break;
                case FilterMode.STARTS_WITH:
                    if (!targetValue.StartsWith(filter))
                    {
                        return false;
                    }
                    break;
                case FilterMode.ENDS_WITH:
                    if (!targetValue.EndsWith(filter))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void RenameColumn(string colName, string newName)
        {
            HoldChanges();
            var col = Columns[colName];
            col.Rename(newName);
            Columns.Remove(colName);

            foreach (var row in Rows.Children)
            {
                var cell = row[colName];
                cell.Rename(newName);
                row.Remove(colName);
                row[newName] = cell;
            }

            Columns.InsertChildAt(col.Sequence, col);
            DispatchHeldChanges(nameof(RenameColumn), colName, newName);
        }
        #endregion Methods
    }
}
