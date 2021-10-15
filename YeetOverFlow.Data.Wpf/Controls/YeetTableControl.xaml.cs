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

namespace YeetOverFlow.Data.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for YeetTableControl.xaml
    /// </summary>
    public partial class YeetTableControl : YeetControlBase
    {
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
                    #region columnCollectionChanged
                    //Action<object, NotifyCollectionChangedEventArgs> columnCollectionChanged = (s, ne) =>
                    //{

                    //};
                    #endregion columnCollectionChanged

                    if (e.NewValue != null)
                    {
                        var columns = (YeetColumnCollectionViewModel)e.NewValue;
                        //ICollectionView columns = UIHelper.GetCollectionView((IEnumerable)e.NewValue);
                        //SortDescription sortDescription = new SortDescription("Value.ColSeq", ListSortDirection.Ascending);
                        //if (!columns.SortDescriptions.Contains(sortDescription))
                        //{
                        //    columns.SortDescriptions.Add(sortDescription);
                        //}
                        //columns.Filter = (itm) =>
                        //{
                        //    KeyValuePair<String, SwagDataColumn> kvp = (KeyValuePair<String, SwagDataColumn>)itm;
                        //    return kvp.Value.IsVisible;
                        //};

                        dataGrid.Columns.Clear();
                        foreach (YeetColumnViewModel col in columns.Children)
                        {
                            //dataGrid.Columns.Add(sdcKvp.Value.DataGridColumn());
                            DataGridTextColumn dgc = new DataGridTextColumn();
                            dgc.Header = col.Key;
                            dgc.Binding = new Binding($"[{col.Key}].Value");
                            dataGrid.Columns.Add(dgc);
                        }

                        columns.CollectionChanged += fdgDataGrid.Columns_CollectionChanged;
                    }

                    //if (e.OldValue is IEnumerable oldCol)
                    //{
                    //    ICollectionView columns = UIHelper.GetCollectionView(oldCol);
                    //    columns.CollectionChanged -= fdgDataGrid.Columns_CollectionChanged;
                    //}
                }));
            }
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
                    foreach (YeetColumnViewModel colVm in e.NewItems)
                    {
                        DataGridTextColumn dgc = new DataGridTextColumn();
                        dgc.Header = colVm.Key;
                        dgc.Binding = new Binding($"[{colVm.Key}].Value");
                        dataGrid.Columns.Add(dgc);
                    }
                    break;
            }
            //ICollectionView cols = (ICollectionView)s;
            //DataGrid dataGrid = DataGrid;   //This is kind of dirty but gotta get it working yo
            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Reset:
            //    case NotifyCollectionChangedAction.Replace:
            //        dataGrid.Columns.Clear();
            //        foreach (KeyValuePair<String, SwagDataColumn> kvp in cols)
            //        {
            //            dataGrid.Columns.Add(kvp.Value.DataGridColumn());
            //        }
            //        break;
            //    case NotifyCollectionChangedAction.Add:
            //        foreach (KeyValuePair<String, SwagDataColumn> kvp in e.NewItems)
            //        {
            //            kvp.Value.Init();
            //            dataGrid.Columns.Add(kvp.Value.DataGridColumn());
            //        }
            //        break;
            //    case NotifyCollectionChangedAction.Move:
            //        foreach (KeyValuePair<String, SwagDataColumn> kvp in e.NewItems)
            //        {
            //            dataGrid.Columns.RemoveAt(e.OldStartingIndex);
            //            dataGrid.Columns.Add(kvp.Value.DataGridColumn());
            //        }
            //        //dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);
            //        break;
            //    case NotifyCollectionChangedAction.Remove:
            //        foreach (KeyValuePair<String, SwagDataColumn> kvp in e.OldItems)
            //        {
            //            dataGrid.Columns.RemoveAt(e.OldStartingIndex);
            //        }
            //        break;
            //}
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
        public YeetTableControl()
        {
            InitializeComponent();

            BindingOperations.SetBinding(this, YeetTableControl.ColumnsProperty, new Binding("Table.Columns") { RelativeSource = RelativeSource.Self });
        }

        private void DataGrid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            var movedCol = Table.Columns[e.Column.Header.ToString()];
            Table.Columns.MoveChild(e.Column.DisplayIndex, movedCol);
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //if (DataGrid.SelectedCells.Count > 0)
            //{
            //    Decimal selectedTotal = 0.0m;
            //    foreach (DataGridCellInfo cellInfo in DataGrid.SelectedCells)
            //    {
            //        DataGridColumn dgCol = cellInfo.Column;
            //        DataRowView drv = (DataRowView)cellInfo.Item;

            //        if (Decimal.TryParse(drv[dgCol.Header.ToString()].ToString(), out Decimal val))
            //        {
            //            selectedTotal += val;
            //        }
            //    }

            //    this.SelectedTotal = selectedTotal;
            //    this.SelectedCount = DataGrid.SelectedCells.Count;
            //}
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
    }
}
