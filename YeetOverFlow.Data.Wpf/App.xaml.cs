using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Wpf.ServiceCollectionExtensions;
using YeetOverFlow.Data.EntityFramework.ServiceExtensions;

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
            _serviceProvider.InitYeetData();        //Initialize yeet data
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<YeetDataWindow>();
            services.AddYeetWpf();
            services.AddYeetDataEf((opt) =>
            {
                if (!Directory.Exists("db")) Directory.CreateDirectory("db");
                opt.UseSqlite("Data Source=db/yeetdata.db");
            });
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var demoWindow = _serviceProvider.GetService<YeetDataWindow>();
            demoWindow.Show();
        }
    }
}
