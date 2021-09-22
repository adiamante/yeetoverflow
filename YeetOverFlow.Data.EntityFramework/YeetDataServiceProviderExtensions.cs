using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.EntityFramework;
using YeetOverFlow.Core.EntityFramework.ServiceExtensions;

namespace YeetOverFlow.Data.EntityFramework.ServiceExtensions
{
    public static class YeetDataServiceProviderExtensions
    {
        public static void InitYeetData(this IServiceProvider sp)
        {
            var ctx = sp.GetRequiredService<YeetDataEfDbContext>();
            ctx.Database.EnsureCreated();
        }

        public static void AddYeetDataEf(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
        {
            services.AddYeetOverFlow<YeetDataSet, YeetData>();

            if (setup != null)
            {
                services.AddDbContext<YeetDataEfDbContext>(setup);
                services.AddTransient<YeetDataEfUnitOfWork>();
                services.AddTransient<YeetEfDbContext<YeetDataSet, YeetData>>(sp => sp.GetService<YeetDataEfDbContext>());
                services.AddTransient<IYeetUnitOfWork<YeetDataSet, YeetData>>(sp => sp.GetService<YeetDataEfUnitOfWork>());
            }
        }
    }
}
