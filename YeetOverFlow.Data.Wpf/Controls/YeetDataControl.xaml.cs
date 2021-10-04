using System;
using System.Windows;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Controls;

namespace YeetOverFlow.Data.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for YeetDataSetControl.xaml
    /// </summary>
    public partial class YeetDataControl : YeetControlBase
    {
        #region Data
        private static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(YeetDataLibraryViewModel), typeof(YeetDataControl));

        public YeetDataLibraryViewModel Data
        {
            get { return (YeetDataLibraryViewModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        #endregion Data

        public YeetDataControl()
        {
            InitializeComponent();
        }
    }
}
