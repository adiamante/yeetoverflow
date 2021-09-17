using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Logging;
using YeetOverFlow.Settings;
using YeetOverFlow.Settings.EntityFramework.ServiceExtensions;
using YeetOverFlow.Wpf.Events;
using YeetOverFlow.Wpf.ViewModels;
using System.IO;

namespace YeetOverFlow.Wpf.ServiceCollectionExtensions
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

            services.AddSingleton<IMapper>(sp => {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<YeetSetting, YeetSettingViewModel>()
                        .IncludeAllDerived();

                    cfg.CreateMap<YeetSettingViewModel, YeetSetting>()
                        .IncludeAllDerived();

                    cfg.CreateMap<YeetSettingList, YeetSettingListViewModel>()
                        .ReverseMap();

                    cfg.CreateMap<YeetSettingBoolean, YeetSettingBooleanViewModel>()
                        .ReverseMap();

                    cfg.CreateMap<YeetSettingString, YeetSettingStringViewModel>()
                        .ReverseMap();

                    cfg.CreateMap<YeetSettingStringOption, YeetSettingStringOptionViewModel>()
                        .ReverseMap();
                });

                return config.CreateMapper();
            });

            services.AddSingleton<YeetWindowViewModel>();
            services.AddSingleton<YeetCommandManagerViewModel>();
            services.AddSingleton<YeetSettingLibraryViewModel>();
            services.Replace<IYeetEventStore<YeetEvent<YeetSetting>, YeetSetting>, YeetOverFlowWpfEventStore>(ServiceLifetime.Transient);
        }

        //https://stackoverflow.com/questions/43590769/replace-service-registration-in-asp-net-core-built-in-di-container
        public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}
