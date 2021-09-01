using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Core.EntityFramework.ServiceExtensions;

namespace YeetOverFlow.Core.EntityFramework.Tests
{
    public static class YeetEfHelper
    {
        public static IServiceProvider ConfigureServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            //Not using dockerClient at the moment but it is here for future reference
            //serviceCollection.AddDockerClientServices();
            serviceCollection.AddYeetOverFlow(opt => {
                opt.UseInMemoryDatabase("YeetOverFlow");
                opt.EnableSensitiveDataLogging();
            });
            return serviceCollection.BuildServiceProvider();
        }

        public static void InitDb(YeetEfDbContext<YeetList, YeetItem> dbContext, params YeetLibrary<YeetList>[] seedLibraries)
        {
            if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                dbContext.Database.Migrate();
            }

            if (dbContext.YeetLibraries.Any())
            {
                //wipe out database
                List<YeetLibrary<YeetList>> libs = dbContext.YeetLibraries.ToList();
                for (int i = 0; i < libs.Count; i++)
                {
                    YeetLibrary<YeetList> lib = libs[i];
                    dbContext.Entry(lib).Reference(lib => lib.Root).Load();
                    YeetList lst = lib.Root;

                    CascadeDelete(dbContext, lst);
                    dbContext.YeetLibraries.Remove(lib);
                }

                List<YeetItem> items = dbContext.YeetItems.ToList();
                dbContext.YeetItems.RemoveRange(items);
            }

            foreach (YeetLibrary<YeetList> lib in seedLibraries)
            {
                dbContext.YeetLibraries.Add(lib);
            }
            dbContext.SaveChanges();
        }

        private static void CascadeDelete(YeetEfDbContext<YeetList, YeetItem> dbContext, YeetItem item)
        {
            if (item is YeetList list)
            {
                dbContext.Entry(list).Collection(nameof(list.Children)).Load();
                foreach (YeetItem child in list.Children)
                {
                    if (child is YeetList subList)
                    {
                        CascadeDelete(dbContext, subList);
                    }
                    else
                    {
                        dbContext.YeetItems.Remove(child);
                    }
                }
            }

            dbContext.YeetItems.Remove(item);
        }
    }
}
