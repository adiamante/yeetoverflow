using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.Controls;
using YeetOverFlow.Data.Wpf.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace YeetOverFlow.Data.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for YeetTableControl.xaml
    /// </summary>
    public partial class YeetTableControl : YeetControlBase
    {
        #region Public Properties
        #region Table
        private static readonly DependencyProperty TableProperty =
        DependencyProperty.Register("Table", typeof(YeetTableViewModel), typeof(YeetTableControl));

        public YeetTableViewModel Table
        {
            get { return (YeetTableViewModel)GetValue(TableProperty); }
            set { SetValue(TableProperty, value); }
        }
        #endregion Table
        #region Columns
        public static DependencyProperty ColumnsProperty =
        DependencyProperty.RegisterAttached("Columns",
                                            typeof(YeetColumnCollectionViewModel),
                                            typeof(YeetTableControl),
                                            new UIPropertyMetadata(null, BindableColumnsPropertyChanged));

        private static void BindableColumnsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            YeetTableControl fdgDataGrid = source as YeetTableControl;
            DataGrid dataGrid = fdgDataGrid.DataGrid;

            if (dataGrid != null)
            {
                dataGrid.Dispatcher.Invoke(new Action(() =>
                {
                    if (e.NewValue != null)
                    {
                        var ccvm = (YeetColumnCollectionViewModel)e.NewValue;
                        ICollectionView columns = CollectionViewSource.GetDefaultView(ccvm.Children);
                        SortDescription sortDescription = new SortDescription("Sequence", ListSortDirection.Ascending);
                        if (!columns.SortDescriptions.Contains(sortDescription))
                        {
                            columns.SortDescriptions.Add(sortDescription);
                        }

                        dataGrid.Columns.Clear();
                        foreach (YeetColumnViewModel col in columns)
                        {
                            dataGrid.Columns.Add(CreateDataGridColumn(col));
                        }

                        ccvm.CollectionChanged += fdgDataGrid.Columns_CollectionChanged;
                    }
                }));
            }
        }

        private static DataGridTextColumn CreateDataGridColumn(YeetColumnViewModel col)
        {
            DataGridTextColumn dgc = new DataGridTextColumn();
            dgc.Header = col.Key;
            dgc.Binding = new Binding($"[{col.Key}].Value");
            Binding binding = new Binding("IsVisible");
            binding.Source = col;
            binding.Converter = new BooleanToVisibilityConverter();
            BindingOperations.SetBinding(dgc, DataGridTextColumn.VisibilityProperty, binding);
            return dgc;
        }

        private void Columns_CollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            //var columns = (YeetColumnCollectionViewModel)s;
            DataGrid dataGrid = DataGrid;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    dataGrid.Columns.RemoveAt(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (YeetColumnViewModel col in e.NewItems)
                    {
                        dataGrid.Columns.Insert(e.NewStartingIndex, CreateDataGridColumn(col));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    foreach (YeetColumnViewModel col in e.NewItems)
                    {
                        dataGrid.Columns.RemoveAt(e.OldStartingIndex);
                        dataGrid.Columns.Insert(e.NewStartingIndex, CreateDataGridColumn(col));
                    }
                    break;
            }
        }

        public YeetColumnCollectionViewModel Columns
        {
            get { return (YeetColumnCollectionViewModel)GetValue(ColumnsProperty); }
            set
            {
                SetValue(ColumnsProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion Columns
        #region SelectedTotal
        public static DependencyProperty SelectedTotalProperty =
                DependencyProperty.Register(
                    "SelectedTotal",
                    typeof(Decimal),
                    typeof(YeetTableControl));

        public Decimal SelectedTotal
        {
            get { return (Decimal)GetValue(SelectedTotalProperty); }
            set
            {
                SetValue(SelectedTotalProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SelectedTotal
        #region SelectedCount
        public static DependencyProperty SelectedCountProperty =
                DependencyProperty.Register(
                    "SelectedCount",
                    typeof(Int32),
                    typeof(YeetTableControl));

        public Int32 SelectedCount
        {
            get { return (Int32)GetValue(SelectedCountProperty); }
            set
            {
                SetValue(SelectedCountProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SelectedCount
        #endregion Public Properties

        #region Initialization
        public YeetTableControl()
        {
            InitializeComponent();

            BindingOperations.SetBinding(this, YeetTableControl.ColumnsProperty, new Binding("Table.Columns") { RelativeSource = RelativeSource.Self });
        }
        #endregion Initialization

        #region Events
        private void DataGrid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            var movedCol = Table.Columns[e.Column.Header.ToString()];
            Table.Columns.MoveChild(e.Column.DisplayIndex, movedCol);
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count > 0)
            {
                Decimal selectedTotal = 0.0m;
                foreach (DataGridCellInfo cellInfo in DataGrid.SelectedCells)
                {
                    DataGridColumn dgCol = cellInfo.Column;
                    YeetRowViewModel row = (YeetRowViewModel)cellInfo.Item;
                    YeetCellViewModel cell = (YeetCellViewModel)row[dgCol.Header.ToString()];
                    if (cell != null && Decimal.TryParse(cell.GetValue().ToString(), out Decimal val))
                    {
                        selectedTotal += val;
                    }
                }

                this.SelectedTotal = selectedTotal;
                this.SelectedCount = DataGrid.SelectedCells.Count;
            }
        }

        private void ColumnHeader_RenameClick(object sender, RoutedEventArgs e)
        {
            Button btnRename = (Button)sender;
            ContextMenu contextMenu = DependencyObjectHelper.TryFindParent<ContextMenu>(btnRename);
            contextMenu.IsOpen = false;
        }

        private void ColumnHeader_TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            TextBox txtText = (TextBox)sender;
            txtText.SelectAll();
            txtText.Focus();
        }

        private void ColumnHeader_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox txtText = (TextBox)sender;
                MenuItem menuItem = (MenuItem)DependencyObjectHelper.TryFindParent<MenuItem>(txtText);
                Button btn = menuItem.FindLogicalChild<Button>();
                btn.Command.Execute(btn.CommandParameter);
                btn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void ColumnHeader_SelectColumn(object sender, RoutedEventArgs e)
        {
            var colName = ((FrameworkElement)sender).Tag.ToString();
            var colTarget = DataGrid.Columns.FirstOrDefault(dgc => dgc.Header.ToString() == colName);
            DataGrid.SelectedCells.Clear();

            DataGrid.SelectedCellsChanged -= DataGrid_SelectedCellsChanged;
            foreach (var row in Table.Rows.Children)
            {
                DataGrid.SelectedCells.Add(new DataGridCellInfo(row, colTarget));
            }
            DataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;
            DataGrid_SelectedCellsChanged(null, null);
        }
        #endregion Events
    }
}
