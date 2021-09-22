using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Data.EntityFramework.ServiceExtensions;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.ServiceExtensions;

namespace YeetOverFlow.Data.Wpf.ServiceExtensions
{
    public static class YeetDataWpfServiceCollectionExtensions
    {
        public static void AddYeetDataWpf(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
        {
            services.AddYeetWpf();

            if (setup == null)
            {
                services.AddYeetDataEf((opt) =>
                {
                    if (!Directory.Exists("db")) Directory.CreateDirectory("db");
                    opt.UseSqlite("Data Source=db/yeetdata.db");
                });
            }

            services.AddSingleton<YeetDataLibraryViewModel>();
        }
    }
}
