using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YeetOverFlow.Core.Interface;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Persistence;
using YeetOverFlow.Core.Application.Tests;

namespace YeetOverFlow.Core.Application.CommandHandlers.Tests
{
    public class YeetCommandHandlerTestBase : YeetTestBase
    {
        #region Shared variables
        protected Mock<IYeetUnitOfWork<YeetList, YeetItem>> _mockUnitOfWork;
        protected Mock<IYeetEventStore<YeetEvent<YeetItem>, YeetItem>> _mockEventStore;
        #endregion Shared variables

        #region Setup
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _events = new List<IYeetEvent<YeetItem>>();

            Mock<IRepository<YeetLibrary<YeetList>>> mockYeetLibraryRepository = TestHelper.GetMockYeetRepository<IRepository<YeetLibrary<YeetList>>, YeetLibrary<YeetList>>(_libraries);
            Mock<IRepository<YeetList>> mockYeetListRepositoy = TestHelper.GetMockYeetRepository<IRepository<YeetList>, YeetList>(_lists, _items);
            Mock<IRepository<YeetItem>> mockYeetItemRepository = TestHelper.GetMockYeetRepository<IRepository<YeetItem>, YeetItem>(_items);

            _mockUnitOfWork = new Mock<IYeetUnitOfWork<YeetList, YeetItem>>();
            _mockUnitOfWork.Setup(uow => uow.YeetLibraries).Returns(mockYeetLibraryRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.YeetLists).Returns(mockYeetListRepositoy.Object);
            _mockUnitOfWork.Setup(uow => uow.YeetItems).Returns(mockYeetItemRepository.Object);

            _mockEventStore = new Mock<IYeetEventStore<YeetEvent<YeetItem>, YeetItem>>();
            _mockEventStore.Setup(es => es.DispatchEvent(It.IsAny<YeetEvent<YeetItem>>()))
                .Callback<IYeetEvent<YeetItem>>(se => _events.Add(se));
        }
        #endregion Setup
    }
}