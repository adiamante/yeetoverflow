using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.EntityFramework;
using YeetOverFlow.Core.EntityFramework.ServiceExtensions;

namespace YeetOverFlow.Settings.EntityFramework.ServiceExtensions
{
    public static class YeetSettingsServiceCollectionExtensions
    {
        public static void AddYeetSettingsEf(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
        {
            services.AddYeetOverFlow<YeetSettingList, YeetSetting>();

            if (setup != null)
            {
                services.AddDbContext<YeetSettingsEfDbContext>(setup);
                services.AddTransient<YeetSettingsEfUnitOfWork>();
                services.AddTransient<YeetEfDbContext<YeetSettingList, YeetSetting>>(sp => sp.GetService<YeetSettingsEfDbContext>());
                services.AddTransient<IYeetUnitOfWork<YeetSettingList, YeetSetting>>(sp => sp.GetService<YeetSettingsEfUnitOfWork>());
            }
        }
    }
}
