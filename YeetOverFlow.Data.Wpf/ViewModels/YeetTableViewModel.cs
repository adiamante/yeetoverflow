using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using YeetOverFlow.Data.Wpf.HelperExtensions;
using YeetOverFlow.Reflection;
using YeetOverFlow.Wpf.Commands;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetTableViewModel : YeetDataViewModel
    {
        #region Private Members
        ICommand _applyColumnFilterCommand, _clearColumnFilterCommand, _applyColumnValuesFilterCommand, _filterColumnValuesCommand, _applyAllCheckedColumnValuesCommand, _showAllColumnValuesCommand,
            _renameColumnCommand, _toggleColumnTotals, _toggleDebug, _convertColumnCommand, _moveColumnCommand;
        YeetColumnCollectionViewModel _columns = new YeetColumnCollectionViewModel();
        YeetRowCollectionViewModel _rows = new YeetRowCollectionViewModel();
        Dictionary<string, ObservableCollection<YeetColumnValueViewModel>> _columnValues = new Dictionary<string, ObservableCollection<YeetColumnValueViewModel>>();
        bool _showColumnTotals = false, _showDebug = false;
        IMapper _columnMapper;
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
        public bool ShowColumnTotals
        {
            get { return _showColumnTotals; }
            set { SetValue(ref _showColumnTotals, value, true, false); }
        }
        public bool ShowDebug
        {
            get { return _showDebug; }
            set { SetValue(ref _showDebug, value, true, false); }
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

        #region ToggleColumnTotals
        [JsonIgnore]
        public ICommand ToggleColumnTotals
        {
            get
            {
                return _toggleColumnTotals ?? (_toggleColumnTotals =
                    new RelayCommand(() =>
                    {
                        ShowColumnTotals = !ShowColumnTotals;
                    }));
            }
        }
        #endregion ToggleColumnTotals

        #region ToggleDebug
        [JsonIgnore]
        public ICommand ToggleDebug
        {
            get
            {
                return _toggleDebug ?? (_toggleDebug =
                    new RelayCommand(() =>
                    {
                        ShowDebug = !ShowDebug;
                    }));
            }
        }
        #endregion ToggleDebug

        #region ConvertColumnCommand
        [JsonIgnore]
        public ICommand ConvertColumnCommand
        {
            get
            {
                return _convertColumnCommand ?? (_convertColumnCommand =
                    new RelayCommand<string, string, string>((colName, type, defaultValue) =>
                    {
                        ConvertColumn(colName, type, defaultValue);
                    }));
            }
        }
        #endregion ConvertColumnCommand

        #region MoveColumnCommand
        [JsonIgnore]
        public ICommand MoveColumnCommand
        {
            get
            {
                return _moveColumnCommand ?? (_moveColumnCommand =
                    new RelayCommand<string, string>((colName, diff) =>
                    {
                        MoveColumn(colName, Convert.ToInt32(diff));
                    }));
            }
        }
        #endregion MoveColumnCommand
        #endregion Commands

        #region Initialization
        public YeetTableViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetTableViewModel(Guid guid, string key) : base(guid, key)
        {
            var columnMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<YeetColumnViewModel, YeetIntColumnViewModel>();
                cfg.CreateMap<YeetColumnViewModel, YeetDoubleColumnViewModel>();
                cfg.CreateMap<YeetColumnViewModel, YeetDateTimeColumnViewModel>();
                cfg.CreateMap<YeetColumnViewModel, YeetStringColumnViewModel>();
            });
            _columnMapper = columnMapperConfig.CreateMapper();
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

            RefreshColumnTotals();
        }
        #endregion Initialization

        #region Methods
        public void ApplyColumnFilters()
        {
            var view = CollectionViewSource.GetDefaultView(Rows.Children);
            view.Filter = (r) =>
            {
                var row = (YeetRowViewModel)r;
                foreach (YeetColumnViewModel col in Columns.Children)
                {
                    var cell = (YeetCellViewModel)row[col.Key];
                    var cellVal = cell.GetValue().ToString();
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

            foreach (YeetColumnViewModel col in Columns.Children)
            {
                var colFilter = col.ColumnFilter;
                var filter = col.ColumnFilter.Filter;
                var filterMode = col.ColumnFilter.FilterMode;
                
                FilterColumnValues(filter, filterMode, col.Key);
            }

            RefreshColumnTotals();
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

                foreach (var valGroup in Rows.Children.GroupBy(r => ((YeetCellViewModel)(((YeetRowViewModel)r)[colName])).GetValue()))
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
            using (var scope = new YeetItemViewModelBaseExtended.ChangeScope(this, nameof(RenameColumn), colName, newName))
            {
                var col = Columns[colName];
                col.Rename(newName);
                Columns.Remove(colName);

                foreach (YeetRowViewModel row in Rows.Children)
                {
                    var cell = (YeetCellViewModel)row[colName];
                    cell.Rename(newName);
                    row.Remove(colName);
                    row[newName] = cell;
                }

                Columns.InsertChildAt(col.Sequence, col);
            }
        }

        private void ConvertColumn(string colName, string targetType, string defaultString)
        {
            var originalColumn = Columns[colName];
            using (var scope = new YeetItemViewModelBaseExtended.ChangeScope(this, nameof(ConvertColumn), originalColumn.DataType.Name, targetType))
            {
                Columns.Remove(colName);

                Func<string, String, object, object> convert = (type, input, defaultOutput) =>
                {
                    switch (type)
                    {
                        case "Int32":
                            if (Int32.TryParse(input, out Int32 outInt32))
                            {
                                return outInt32;
                            }
                            break;
                        case "Double":
                            if (Double.TryParse(input, out Double outDouble))
                            {
                                return outDouble;
                            }
                            break;
                        case "DateTime":
                            if (DateTimeOffset.TryParse(input, out DateTimeOffset outDateTime))
                            {
                                return outDateTime;
                            }
                            break;
                        case "TimeSpan":
                            if (TimeSpan.TryParse(input, out TimeSpan outTimeSpan))
                            {
                                return outTimeSpan;
                            }
                            break;
                        case "String":
                        default:
                            return input.ToString();
                    }
                    return defaultOutput;
                };

                Object defaultValue = null;
                if (targetType == "String")
                {
                    defaultValue = defaultString;
                }
                else
                {
                    defaultValue = convert(targetType, defaultString, null);
                }

                foreach (YeetRowViewModel row in Rows.Children)
                {
                    var originalCell = (YeetCellViewModel)row[colName];
                    row.Remove(colName);

                    YeetCellViewModel newCell = null;
                    switch (targetType)
                    {
                        case "Int32":
                            newCell = new YeetIntCellViewModel(Guid.NewGuid(), originalCell.Key);
                            break;
                        case "Double":
                            newCell = new YeetDoubleCellViewModel(Guid.NewGuid(), originalCell.Key);
                            break;
                        case "DateTime":
                            newCell = new YeetDateTimeCellViewModel(Guid.NewGuid(), originalCell.Key);
                            break;
                        case "String":
                        default:
                            newCell = new YeetStringCellViewModel(Guid.NewGuid(), originalCell.Key);
                            break;
                    }

                    newCell.SetValue(convert(targetType, originalCell.GetValue()?.ToString(), defaultValue));
                    row.AddChild(newCell);
                }

                YeetColumnViewModel newColumn = null;
                switch (targetType)
                {
                    case "Int32":
                        newColumn = _columnMapper.Map<YeetIntColumnViewModel>((YeetColumnViewModel)originalColumn);
                        break;
                    case "Double":
                        newColumn = _columnMapper.Map<YeetDoubleColumnViewModel>((YeetColumnViewModel)originalColumn);
                        break;
                    case "DateTime":
                        newColumn = _columnMapper.Map<YeetDateTimeColumnViewModel>((YeetColumnViewModel)originalColumn);
                        break;
                    //case "TimeSpan":
                    //    newColumn = _columnMapper.Map<YeetTimeSpanColumnViewModel>((YeetColumnViewModel)originalColumn);
                    //    break;
                    case "String":
                    default:
                        newColumn = _columnMapper.Map<YeetStringColumnViewModel>((YeetColumnViewModel)originalColumn);
                        break;
                }

                ReflectionHelper.FieldInfoCollection[newColumn.GetType()]["_guid"].SetValue(newColumn, Guid.NewGuid());
                Columns.InsertChildAt(originalColumn.Sequence, newColumn);
                RefreshColumnTotals(newColumn.Name);
            }
        }

        private void MoveColumn(string colName, int diff)
        {
            var col = Columns[colName];
            var targetSequence = col.Sequence + diff;
            if (targetSequence > Columns.Count - 1)
            {
                targetSequence = Columns.Count - 1;
            }
            if (targetSequence < 0)
            {
                targetSequence = 0;
            }

            using (var scope = new YeetItemViewModelBaseExtended.ChangeScope(this, nameof(MoveColumn), col.Sequence, targetSequence))
            {
                Columns.MoveChild(targetSequence, col);
            }
        }

        private void RefreshColumnTotals()
        {
            foreach (var col in Columns.Children)
            {
                RefreshColumnTotals(col.Name);
            }
        }

        private void RefreshColumnTotals(string colName)
        {
            var rowsView = CollectionViewSource.GetDefaultView(Rows.Children);

            var col = Columns[colName];
            col.Total = 0;
            if (col.DataType.IsNumericType())
            {
                foreach (YeetRowViewModel row in rowsView)
                {
                    var cell = (YeetCellViewModel)row[col.Name];
                    if (double.TryParse(cell.GetValue()?.ToString(), out double val))
                    {
                        col.Total += val;
                    }
                }
            }
        }

        #endregion Methods
    }
}
