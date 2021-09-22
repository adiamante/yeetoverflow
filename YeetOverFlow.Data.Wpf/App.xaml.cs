using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Wpf.ServiceExtensions;
using YeetOverFlow.Data.EntityFramework.ServiceExtensions;
using YeetOverFlow.Data.Wpf.ServiceExtensions;

namespace YeetOverFlow.Data.Wpf
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
            _serviceProvider.InitYeetDataWpf();        //Initialize yeet data
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<YeetDataWindow>();
            services.AddYeetDataWpf();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var demoWindow = _serviceProvider.GetService<YeetDataWindow>();
            demoWindow.Show();
        }
    }
}
