﻿using System;
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
                cfg.ShouldMapField = fieldInfo => fieldInfo.Name == "_sequence";

                cfg.CreateMap<YeetData, YeetDataViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetDataViewModel, YeetData>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetDataSet, YeetDataSetViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetDataSetViewModel, YeetDataSet>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetTable, YeetTableViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetColumnCollection, YeetColumnCollectionViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetColumn, YeetColumnViewModel>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetColumnViewModel, YeetColumn>()
                    .IncludeAllDerived();

                cfg.CreateMap<YeetBooleanColumn, YeetBooleanColumnViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetStringColumn, YeetStringColumnViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetIntColumn, YeetIntColumnViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetDoubleColumn, YeetDoubleColumnViewModel>()
                    .ReverseMap();

                cfg.CreateMap<YeetDateTimeColumn, YeetDateTimeColumnViewModel>()
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
            var retrievedLib = result.Value.FirstOrDefault();

            if (retrievedLib != null) //load library
            {
                LoadData(retrievedLib.Root, ctx);
                vmLib.ImportLibrary(retrievedLib);
            }
            else //init library
            {
                var root = new YeetDataSetViewModel(Guid.NewGuid(), "Root");

                //test data to play with
                var t = new YeetTableViewModel(Guid.NewGuid(), "Tbl1");
                t.Columns.AddChild(new YeetDoubleColumnViewModel(Guid.NewGuid(), "Col1") { Name = "Col1" });
                t.Columns.AddChild(new YeetDoubleColumnViewModel(Guid.NewGuid(), "Col2") { Name = "Col2" });

                var r1 = new YeetRowViewModel();
                r1["Col1"] = new YeetDoubleCellViewModel(Guid.NewGuid(), "Col1") { Value = 1 };
                r1["Col2"] = new YeetDoubleCellViewModel(Guid.NewGuid(), "Col2") { Value = 2 };

                var r2 = new YeetRowViewModel();
                r2["Col1"] = new YeetDoubleCellViewModel(Guid.NewGuid(), "Col1") { Value = 3 };
                r2["Col2"] = new YeetDoubleCellViewModel(Guid.NewGuid(), "Col2") { Value = 4 };

                t.Rows.AddChild(r1);
                t.Rows.AddChild(r2);

                root.AddChild(t);
                vmLib.Root = root;
                vmLib.Init();

                var lib = vmLib.ExportLibrary();
                ctx.YeetLibraries.Add(lib);
                ctx.SaveChanges();
            }


            //var table = ctx.Tables
            //    .Include(t => t.Columns).ThenInclude(r => r.Children)
            //    .Include(t => t.Rows).ThenInclude(rc => rc.Children).ThenInclude(r => r.Children)
            //    .SingleOrDefaultAsync().Result;

        }

        private static void LoadData(YeetData yeetData, YeetDataEfDbContext ctx)
        {
            switch (yeetData)
            {
                case YeetDataSet ds:
                    foreach (YeetData child in ds.Children)
                    {
                        LoadData(child, ctx);
                    }
                    break;
                case YeetTable tbl:
                    ctx.Entry(tbl.Columns).Collection(cc => cc.Children).Load();
                    ctx.Entry(tbl.Rows).Collection(cc => cc.Children).Load();

                    foreach (YeetRow row in tbl.Rows.Children)
                    {
                        ctx.Entry(row).Collection(cc => cc.Children).Load();
                    }
                    break;
            }
        }
    }
}
