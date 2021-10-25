using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Queries;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.Application.Data.Queries;
using YeetOverFlow.Core.Application.Data.Commands;
using YeetOverFlow.Core.Application.QueryHandlers;
using YeetOverFlow.Core.Application.CommandHandlers;

namespace YeetOverFlow.Core.EntityFramework.ServiceExtensions
{
    public static class YeetOverFlowServiceCollectionExtensions
    {
        public static void AddYeetOverFlow(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
        {
            services.AddYeetOverFlow<YeetList, YeetItem>(setup);
        }

        public static void AddYeetOverFlow<TParent, TChild>(this IServiceCollection services, Action<DbContextOptionsBuilder> setup = null)
            where TParent : YeetItem, IYeetListBase<TChild>
            where TChild : YeetItem
        {
            services.AddTransient<IRepository<YeetLibrary<TParent>>, EfRepository<YeetEfDbContext<TParent, TChild>, YeetLibrary<TParent>>>();
            services.AddTransient<IRepository<TChild>, EfRepository<YeetEfDbContext<TParent, TChild>, TChild>>();
            services.AddTransient<IRepository<TParent>, EfRepository<YeetEfDbContext<TParent, TChild>, TParent>>();
            services.AddTransient<IRepository<YeetEvent<TChild>>, EfRepository<YeetEfDbContext<TParent, TChild>, YeetEvent<TChild>>>();
            services.AddTransient<IYeetEventStore<YeetEvent<TChild>, TChild>, YeetEventStore<TChild>>();

            if (setup != null)
            {
                services.AddDbContext<YeetEfDbContext<TParent, TChild>>(setup);
                services.AddTransient<IYeetUnitOfWork<TParent, TChild>, YeetEfUnitOfWork<TParent, TChild>>();
            }

            services.AddYeetOverFlowQueryHandlers<TParent, TChild>();
            services.AddYeetOverFlowCommandHandlers<TParent, TChild>();
        }

        private static void AddYeetOverFlowQueryHandlers<TParent, TChild>(this IServiceCollection services)
            where TParent : YeetItem, IYeetListBase<TChild>
            where TChild : YeetItem
        {
            services.AddTransient<IQueryHandler<GetYeetLibrariesQuery, Result<IEnumerable<YeetLibrary<TParent>>>>, GetYeetLibrariesQueryHandler<TParent, TChild>>();
            services.AddTransient<IQueryHandler<GetYeetLibraryByGuidQuery, Result<YeetLibrary<TParent>>>, GetYeetLibraryByGuidQueryHandler<TParent, TChild>>();

            services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        }

        private static void AddYeetOverFlowCommandHandlers<TParent, TChild>(this IServiceCollection services)
            where TParent : YeetItem, IYeetListBase<TChild>
            where TChild : YeetItem
        {
            services.AddTransient<ICommandHandler<AddYeetItemCommand<TChild>, Result>, AddYeetItemCommandHandler<TParent, TChild>>();
            services.AddTransient<ICommandHandler<MoveYeetItemCommand<TChild>, Result>, MoveYeetItemCommandHandler<TParent, TChild>>();
            services.AddTransient<ICommandHandler<RemoveYeetItemCommand<TChild>, Result>, RemoveYeetItemCommandHandler<TParent, TChild>>();
            services.AddTransient<ICommandHandler<UpdateYeetItemCommand<TChild>, Result>, UpdateYeetItemCommandHandler<TParent, TChild>>();
            services.AddTransient<ICommandHandler<SaveCommand<TChild>, Result>, SaveCommandHandler<TParent, TChild>>();

            services.AddTransient<ICommandDispatcher, CommandDispatcher>();
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
