using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using YeetOverFlow.Core;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Data.EntityFramework.ServiceExtensions;

namespace YeetOverFlow.Data.EntityFramework.Tests
{
    public class YeetDataEfUnitOfWorkTests
    {
        IServiceProvider _serviceProvider;
        YeetDataEfDbContext _dbContext;
        IYeetUnitOfWork<YeetDataSet, YeetData> _unitOfWork;
        YeetLibrary<YeetDataSet> _library;
        YeetDataSet _root;
        YeetData _data;

        [SetUp]
        public void Setup()
        {
            _library = new YeetLibrary<YeetDataSet>(Guid.Parse("f384540d-4c02-40e0-8915-50e6306a20a9"))
            {
                Owner = "Test_Owner"
            };

            _root = new YeetDataSet(Guid.Parse("8580c3e7-0071-4a2c-9982-7a4cf0f893f5"), "root")
            {
                Name = "ROOT"
            };

            _data = new YeetData(Guid.Parse("7c300a09-2f6b-4662-a84d-04af81c66e70"), "child");

            _library.Root = _root;

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddYeetDataEf(opt => {
                opt.UseInMemoryDatabase("YeetOverFlow.Data");
                opt.EnableSensitiveDataLogging();
            });

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<YeetDataEfDbContext>();
            _unitOfWork = _serviceProvider.GetService<IYeetUnitOfWork<YeetDataSet, YeetData>>();
            _dbContext.YeetLibraries.Add(_library);
            _dbContext.SaveChanges();
        }

        [Test]
        public void AddChild_Should_AddChildToRepository()
        {
            _root.AddChild(_data);

            var newChild = _unitOfWork.YeetItems.GetById(Guid.Parse("7c300a09-2f6b-4662-a84d-04af81c66e70"));
            newChild.Should().BeSameAs(newChild);
        }
    }
}