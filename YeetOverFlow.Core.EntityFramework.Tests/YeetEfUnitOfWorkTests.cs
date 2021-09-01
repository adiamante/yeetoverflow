using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FluentAssertions;
using YeetOverFlow.Core.Application.Tests;
using YeetOverFlow.Core.Application.Persistence;

namespace YeetOverFlow.Core.EntityFramework.Tests
{
    public class YeetEfUnitOfWorkTests : YeetTestBase
    {
        IServiceProvider _serviceProvider;
        YeetEfDbContext<YeetList, YeetItem> _dbContext;
        IYeetUnitOfWork<YeetList, YeetItem> _unitOfWork;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _serviceProvider = YeetEfHelper.ConfigureServices();
            _dbContext = _serviceProvider.GetService<YeetEfDbContext<YeetList, YeetItem>>();
            _unitOfWork = _serviceProvider.GetService<IYeetUnitOfWork<YeetList, YeetItem>>();
        }

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            YeetEfHelper.InitDb(_dbContext, _library);
            _library = _unitOfWork.YeetLibraries.GetById(Guid.Parse("546f8eb2-b4fd-4bbc-9e0f-522f931f557b"));
        }

        [Test]
        public void Get_WithNoFilter_ShouldReturnCorrectCount()
        {
            var allItems = _unitOfWork.YeetItems.Get();

            allItems.Should().HaveCount(16);
        }

        [Test]
        public void GetById_WithItemGuid_ShouldRerturnCorrectItem()
        {
            var resultItem = _unitOfWork.YeetItems.GetById(Guid.Parse("5cbb0bcb-9c81-4ec3-a260-08d8b4f2b7f6"));

            resultItem.Should().Equals(_itemA);
        }

        [Test]
        public void Insert_WithYeetItem_ShouldAddToYeetItems()
        {
            _unitOfWork.YeetItems.Insert(_newItem);
            _unitOfWork.Save();
            var targetItem = _unitOfWork.YeetItems.GetById(Guid.Parse("5fff743a-911a-40e5-8dc2-0e9693bf6d20"));

            targetItem.Should().Equals(_newItem);
        }

        [Test]
        public void Delete_WithGuid_ShouldRemoveItem()
        {
            _unitOfWork.YeetItems.Delete(Guid.Parse("5cbb0bcb-9c81-4ec3-a260-08d8b4f2b7f6"));
            _unitOfWork.Save();
            var allItems = _unitOfWork.YeetItems.Get();

            allItems.Should().NotContain(_itemA);
        }

        [Test]
        public void Delete_WithItem_ShouldRemoveItem()
        {
            _unitOfWork.YeetItems.Delete(_itemA);
            _unitOfWork.Save();
            var allItems = _unitOfWork.YeetItems.Get();

            allItems.Should().NotContain(_itemA);
        }

        [Test]
        public void Update_WithItemName_ShouldUpdateItemName()
        {
            _itemA.Name = "IA*";
            _unitOfWork.YeetItems.Update(_itemA);
            var resultItem = _unitOfWork.YeetItems.GetById(Guid.Parse("5cbb0bcb-9c81-4ec3-a260-08d8b4f2b7f6"));

            resultItem.Name.Should().Equals("IA*");
        }

        [Test]
        public void GetByID_WithYeetLibraries_ShouldReturnCorrectLibrary()
        {
            var resultLibrary = _unitOfWork.YeetLibraries.GetById(Guid.Parse("546f8eb2-b4fd-4bbc-9e0f-522f931f557b"));
            var actual = TestHelper.TestOutput(resultLibrary.Root);
            var expected = File.ReadAllText($"TestOutput/Text/L1LeftL2LeftL3Left.txt");

            actual.Should().Equals(expected);
        }
    }
}