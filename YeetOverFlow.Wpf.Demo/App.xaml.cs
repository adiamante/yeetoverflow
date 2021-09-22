using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Wpf.ServiceExtensions;

namespace YeetOverFlow.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            //https://executecommands.com/dependency-injection-in-wpf-net-core-csharp/
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.InitYeetWpf();         //Initialize wpf app
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<DemoWindow>();
            services.AddYeetWpf();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var demoWindow = _serviceProvider.GetService<DemoWindow>();
            demoWindow.Show();
        }
    }
}
