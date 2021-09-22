using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Queries;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Data.Queries;
using YeetOverFlow.Data.EntityFramework;
using YeetOverFlow.Data.EntityFramework.ServiceExtensions;
using YeetOverFlow.Data.Wpf.ViewModels;
using YeetOverFlow.Wpf.Mappers;

namespace YeetOverFlow.Data.Wpf.ServiceExtensions
{
    public static class YeetDataWpfServiceProviderExtensions
    {
        public static void InitYeetDataWpf(this IServiceProvider sp)
        {
            sp.InitYeetData();

            var mapperFactory = sp.GetRequiredService<IMapperFactory>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<YeetData, YeetDataViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetDataViewModel, YeetData>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetDataSet, YeetDataSetViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetTable, YeetTableViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetColumnCollection, YeetColumnCollectionViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetColumn, YeetColumnViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetRowCollection, YeetRowCollectionViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetRow, YeetRowViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetCell, YeetCellViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetCellViewModel, YeetCell>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetBooleanCell, YeetBooleanCellViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetStringCell, YeetStringCellViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetIntCell, YeetIntCellViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetDoubleCell, YeetDoubleCellViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetDateTimeCell, YeetDateTimeCellViewModel>()
                    .ReverseMap();
            });

            mapperFactory.AddMapper("Data", mapperConfig.CreateMapper());

            var ctx = sp.GetRequiredService<YeetDataEfDbContext>();
            var qryDispatcher = sp.GetRequiredService<IQueryDispatcher>();
            var qry = new GetYeetLibrariesQuery();
            var result = qryDispatcher.Dispatch<GetYeetLibrariesQuery, Result<IEnumerable<YeetLibrary<YeetDataSet>>>>(qry);
            var vmLib = sp.GetRequiredService<YeetDataLibraryViewModel>();

            if (result.Value.Any()) //load library
            {
                vmLib.ImportLibrary(result.Value.First());
            }
            else //init library
            {
                var root = new YeetDataSetViewModel(Guid.NewGuid(), "Root");

                //test data to play with
                var t = new YeetTableViewModel(Guid.NewGuid(), "Tbl1");
                t.Columns.AddChild(new YeetColumnViewModel(Guid.NewGuid(), "Col1") { Name = "Col1" });
                t.Columns.AddChild(new YeetColumnViewModel(Guid.NewGuid(), "Col2") { Name = "Col2" });

                var r1 = new YeetRowViewModel();
                r1["Col1"] = new YeetStringCellViewModel(Guid.NewGuid(), "Col1") { Value = "R1Col1" };
                r1["Col2"] = new YeetStringCellViewModel(Guid.NewGuid(), "Col2") { Value = "R1Col2" };

                var r2 = new YeetRowViewModel();
                r2["Col1"] = new YeetStringCellViewModel(Guid.NewGuid(), "Col1") { Value = "R1Col1" };
                r2["Col2"] = new YeetStringCellViewModel(Guid.NewGuid(), "Col2") { Value = "R1Col2" };

                t.Rows.AddChild(r1);
                t.Rows.AddChild(r2);

                root.AddChild(t);
                vmLib.Root = root;

                var lib = vmLib.ExportLibrary();
                ctx.YeetLibraries.Add(lib);
                ctx.SaveChanges();
            }


            //var table = ctx.Tables
            //    .Include(t => t.Columns).ThenInclude(r => r.Children)
            //    .Include(t => t.Rows).ThenInclude(rc => rc.Children).ThenInclude(r => r.Children)
            //    .SingleOrDefaultAsync().Result;

        }
    }
}
