using YeetOverFlow.Wpf.ViewModels;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Controls;

namespace YeetOverFlow.Data.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class YeetDataWindow : YeetWindow
    {
        public YeetDataWindow(YeetDataLibraryViewModel lib, YeetWindowViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
