using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.EntityFramework;
using YeetOverFlow.Core.EntityFramework.ServiceExtensions;

namespace YeetOverFlow.Data.EntityFramework.ServiceExtensions
{
    public static class YeetDataServiceExtensions
    {
        public static void InitYeetData(this IServiceProvider sp)
        {
            var ctx = sp.GetRequiredService<YeetDataEfDbContext>();
            ctx.Database.EnsureCreated();

            if (!ctx.Tables.AnyAsync().Result)
            {
                var t = new YeetTable();
                t.Columns.AddChild(new YeetColumn(Guid.NewGuid(), "Col1") { Name = "Col1" });
                t.Columns.AddChild(new YeetColumn(Guid.NewGuid(), "Col2") { Name = "Col2" });

                var r1 = new YeetRow();
                r1["Col1"] = new YeetStringCell(Guid.NewGuid(), "Col1") { Value = "R1Col1" };
                r1["Col2"] = new YeetStringCell(Guid.NewGuid(), "Col2") { Value = "R1Col2" };

                var r2 = new YeetRow();
                r2["Col1"] = new YeetStringCell(Guid.NewGuid(), "Col1") { Value = "R1Col1" };
                r2["Col2"] = new YeetStringCell(Guid.NewGuid(), "Col2") { Value = "R1Col2" };

                t.Rows.AddChild(r1);
                t.Rows.AddChild(r2);

                ctx.Tables.Add(t);
                ctx.SaveChanges();
            }

            var table = ctx.Tables
                .Include(t => t.Columns).ThenInclude(r => r.Children)
                .Include(t => t.Rows).ThenInclude(rc => rc.Children).ThenInclude(r => r.Children)
                .SingleOrDefaultAsync().Result;
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
