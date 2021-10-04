﻿using Newtonsoft.Json;
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
        ICommand _applyColumnFilterCommand, _clearColumnFilterCommand, _applyColumnValuesFilterCommand, _filterColumnValuesCommand, _applyAllCheckedColumnValuesCommand, _showAllColumnValuesCommand;
        Dictionary<string, ObservableCollection<YeetColumnValueViewModel>> _columnValues = new Dictionary<string, ObservableCollection<YeetColumnValueViewModel>>();
        #endregion Private Members

        #region Public Properties
        public YeetColumnCollectionViewModel Columns { get; set; } = new YeetColumnCollectionViewModel();
        public YeetRowCollectionViewModel Rows { get; set; } = new YeetRowCollectionViewModel();
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
        #endregion Commands

        #region Initialization
        public YeetTableViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetTableViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        public void Init()
        {
            Columns.Init();
            Rows.Init();
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
        #endregion Methods
    }
}
