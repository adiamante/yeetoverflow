using YeetOverFlow.Wpf.ViewModels;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Controls;
using System.Windows;

namespace YeetOverFlow.Data.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class YeetDataWindow : YeetWindow
    {
        #region Data
        private static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(YeetDataLibraryViewModel), typeof(YeetDataWindow));

        public YeetDataLibraryViewModel Data
        {
            get
            {
                var settings = (YeetDataLibraryViewModel)GetValue(DataProperty);
                return settings;
            }
            set { SetValue(DataProperty, value); }
        }
        #endregion Data

        public YeetDataWindow(YeetDataLibraryViewModel lib, YeetWindowViewModel vm) : base(vm)
        {
            Data = lib;
            InitializeComponent();
        }
    }
}
