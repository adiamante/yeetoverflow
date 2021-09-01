using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Queries;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Queries;
using YeetOverFlow.Settings;
using YeetOverFlow.Settings.EntityFramework;
using YeetOverFlow.Wpf.ViewModels;
using YeetOverFlow.Logging;

namespace YeetOverFlow.Wpf.ServiceCollectionExtensions
{
    public static class YeetWpfServiceProviderExtensions
    {
        public static void InitYeetWpf(this IServiceProvider sp)
        {
            var window = sp.GetRequiredService<YeetWindowViewModel>();
            var actionProvider = sp.GetRequiredService<YeetSinkActionProvider>();
            actionProvider.AddAction(evnt => window.Message = evnt.Message);

            var lib = window.Settings;
            var logger = sp.GetRequiredService<ILogger<YeetSettingLibraryViewModel>>();


            var ctx = sp.GetRequiredService<YeetSettingsEfDbContext>();
            var qryDispatcher = sp.GetRequiredService<IQueryDispatcher>();
            var qry = new GetYeetLibrariesQuery();

            ctx.Database.EnsureCreated();

            logger.LogInformation("Loading settings.");
            var result = qryDispatcher.Dispatch<GetYeetLibrariesQuery, Result<IEnumerable<YeetLibrary<YeetSettingList>>>>(qry);
            if (result.Value.Any())     //load
            {
                logger.LogInformation("Found existing settings.");
                lib.ImportLibrary(result.Value.First());
            }
            else //initialize and save
            {
                lib.Reset();
                var export = lib.ExportLibrary();
                ctx.YeetLibraries.Add(export);
                ctx.SaveChanges();
                logger.LogInformation("Initialized settings.");
            }

            lib.Init();
        }
    }
}
