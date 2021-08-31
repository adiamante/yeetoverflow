using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using YeetOverFlow.Core.Application.Commands;
using YeetOverFlow.Core.Application.Events;
using YeetOverFlow.Core.Application.Data.Core;
using YeetOverFlow.Core.Application.Tests;

namespace YeetOverFlow.Core.Application.CommandHandlers.Tests
{
    [TestFixture]
    //[MethodUnderTest]_With/Of_[Scenario]_Should_[ExpectedResult]
    public class UpdateYeetItemCommandHandlerTests : YeetCommandHandlerTestBase
    {
        #region Handle_WithInvalidTargetGuid_ShouldThrowsException
        [Test]
        public void Handle_WithInvalidTargetGuid_ShouldThrowsException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            UpdateYeetItemCommand<YeetItem> updateCommand = new UpdateYeetItemCommand<YeetItem>(invalidGuid, new Dictionary<string, string>());

            UpdateYeetItemCommandHandler<YeetList, YeetItem> handler = new UpdateYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(updateCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetItem with provided 'targetGuid'");
        }
        #endregion Handle_WithInvalidTargetGuid_ShouldThrowsException

        #region Handle_ShouldInvokeDispatchEvent_WithCorrectEvent
        [Test]
        public void Handle_ShouldInvokeDispatchEvent_WithCorrectEvent()
        {
            _itemA.Name = "IA*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                ][IA*][IB][IC]
            //[          L2          ][IE][IF][IG]             
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Update _itemA
            Dictionary<string, string> updates = new Dictionary<string, string>();
            updates.Add(nameof(YeetItem.Name), "IA_NEW*");

            Dictionary<string, string> original = new Dictionary<string, string>();
            original.Add(nameof(YeetItem.Name), "IA*");

            UpdateYeetItemCommand<YeetItem> updateCommand = new UpdateYeetItemCommand<YeetItem>(_itemA.Guid, updates);

            UpdateYeetItemCommandHandler<YeetList, YeetItem> handler = new UpdateYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(updateCommand);

            TestHelper.TestOutput(_root);
            //*****************************************************
            //[                       ROOT                        ]
            //[                L1                ][IA_NEW*][IB][IC]
            //[          L2          ][IE][IF][IG]                 
            //[    L3    ][II][IJ][IK]                             
            //[IX][IY][IZ]                                         
            //*****************************************************

            _mockEventStore.Verify(es => es.DispatchEvent(It.IsAny<YeetItemUpdatedEvent<YeetItem>>()), Times.Once);
            _events.Should().ContainEquivalentOf<YeetItemUpdatedEvent<YeetItem>>(
                new YeetItemUpdatedEvent<YeetItem>(_itemA.Guid, updates, original));
        }
        #endregion Handle_ShouldInvokeDispatchEvent_WithCorrectEvent

        #region Handle_WithNameUpdate_ShouldUpdateName
        [Test]
        public void Handle_WithNameUpdate_ShouldUpdateName()
        {
            _itemA.Name = "IA*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                ][IA*][IB][IC]
            //[          L2          ][IE][IF][IG]             
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Update _itemA
            Dictionary<string, string> updates = new Dictionary<string, string>();
            updates.Add(nameof(YeetItem.Name), "IA_NEW*");

            UpdateYeetItemCommand<YeetItem> updateCommand = new UpdateYeetItemCommand<YeetItem>(_itemA.Guid, updates);

            UpdateYeetItemCommandHandler<YeetList, YeetItem> handler = new UpdateYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(updateCommand);

            TestHelper.TestOutput(_root);
            //*****************************************************
            //[                       ROOT                        ]
            //[                L1                ][IA_NEW*][IB][IC]
            //[          L2          ][IE][IF][IG]                 
            //[    L3    ][II][IJ][IK]                             
            //[IX][IY][IZ]                                         
            //*****************************************************

            _itemA.Name.Should().Be("IA_NEW*");
        }
        #endregion Handle_WithNameUpdate_ShouldUpdateName
    }
}
