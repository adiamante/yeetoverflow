using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Logging;
using YeetOverFlow.Settings.EntityFramework.ServiceExtensions;
using YeetOverFlow.Wpf.ViewModels;
using YeetOverFlow.Wpf.Mappers;

namespace YeetOverFlow.Wpf.ServiceExtensions
{
    public static class YeetWpfServiceCollectionsExtensions
    {
        public static void AddYeetWpf(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
        {
            services.AddYeetLogger();

            if (setup == null)
            {
                setup = (opt) => {
                    if (!Directory.Exists("db")) Directory.CreateDirectory("db");
                    opt.UseSqlite("Data Source=db/settings.db");
                    //opt.EnableSensitiveDataLogging();
                };
            }

            services.AddYeetSettingsEf(setup);
            services.AddSingleton<IMapperFactory, MapperFactory>();
            services.AddSingleton<YeetWindowViewModel>();
            services.AddSingleton<YeetCommandManagerViewModel>();
            services.AddSingleton<YeetSettingLibraryViewModel>();
            //services.Replace<IYeetEventStore<YeetEvent<YeetSetting>, YeetSetting>, YeetWpfEventStore>(ServiceLifetime.Transient);
        }
    }
}
