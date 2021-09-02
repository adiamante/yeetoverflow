using YeetOverFlow.Wpf.Controls;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DemoWindow : YeetWindow
    {
        public DemoWindow(YeetWindowViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
