using YeetOverFlow.Wpf.Controls;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class YeetDataWindow : YeetWindow
    {
        public YeetDataWindow(YeetWindowViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
