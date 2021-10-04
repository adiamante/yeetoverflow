using YeetOverFlow.Wpf.Controls;
using YeetOverFlow.Wpf.Demo.ViewModels;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DemoWindow : YeetWindow
    {
        public DemoViewModel DemoViewModel { get; set; } = new DemoViewModel();

        public DemoWindow(YeetWindowViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
