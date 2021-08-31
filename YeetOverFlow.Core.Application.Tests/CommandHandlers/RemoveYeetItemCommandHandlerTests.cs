using System;
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
    public class RemoveYeetItemCommandHandlerTests : YeetCommandHandlerTestBase
    {
        #region Handle_WithInvalidTargetListAndTargetItemChild_ShouldThrowException
        [Test]
        public void Handle_WithInvalidTargetListAndTargetItemChild_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            RemoveYeetItemCommand<YeetItem> removeCommand =
                new RemoveYeetItemCommand<YeetItem>(invalidGuid, _itemA.Guid);

            RemoveYeetItemCommandHandler<YeetList, YeetItem> handler = new RemoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(removeCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetList with provided 'targetListGuid'");
        }
        #endregion Handle_WithInvalidTargetListAndTargetItemChild_ShouldThrowException

        #region Handle_WithRootTargetListAndInvalidTargetChild_ShouldThrowException
        [Test]
        public void Handle_WithRootTargetListAndInvalidTargetChild_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            RemoveYeetItemCommand<YeetItem> removeCommand =
                 new RemoveYeetItemCommand<YeetItem>(_root.Guid, invalidGuid);

            RemoveYeetItemCommandHandler<YeetList, YeetItem> handler = new RemoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(removeCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetChild with provided 'targetChildGuid'");
        }
        #endregion Handle_WithRootTargetListAndInvalidTargetChild_ShouldThrowException

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

            //Remove _itemA from _root
            RemoveYeetItemCommand<YeetItem> removeCommand =
                new RemoveYeetItemCommand<YeetItem>(_root.Guid, _itemA.Guid);

            RemoveYeetItemCommandHandler<YeetList, YeetItem> handler = new RemoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(removeCommand);

            TestHelper.TestOutput(_root);
            //********************************************
            //[                   ROOT                   ]
            //[                L1                ][IB][IC]
            //[          L2          ][IE][IF][IG]        
            //[    L3    ][II][IJ][IK]                    
            //[IX][IY][IZ]                                
            //********************************************

            _mockEventStore.Verify(es => es.DispatchEvent(It.IsAny<YeetItemRemovedEvent<YeetItem>>()), Times.Once);
            _events.Should().ContainEquivalentOf<YeetItemRemovedEvent<YeetItem>>(
                new YeetItemRemovedEvent<YeetItem>(_root.Guid, _itemA.Guid, _itemA));
        }
        #endregion Handle_ShouldInvokeDispatchEvent_WithCorrectEvent

        #region Handle_WithRootTargetListAndTargetItemChild_ShouldRemovesItem
        [Test]
        public void Handle_WithRootTargetListAndTargetItemChild_ShouldRemovesItem()
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

            //Remove _itemA from _root
            RemoveYeetItemCommand<YeetItem> removeCommand =
                new RemoveYeetItemCommand<YeetItem>(_root.Guid, _itemA.Guid);

            RemoveYeetItemCommandHandler<YeetList, YeetItem> handler = new RemoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(removeCommand);

            TestHelper.TestOutput(_root);
            //********************************************
            //[                   ROOT                   ]
            //[                L1                ][IB][IC]
            //[          L2          ][IE][IF][IG]        
            //[    L3    ][II][IJ][IK]                    
            //[IX][IY][IZ]                                
            //********************************************

            _root.Children.Should().NotContain(_itemA);
            _items.Should().NotContain(_newItem);
            _list1.Sequence.Should().Be(0);
            _itemB.Sequence.Should().Be(1);
            _itemC.Sequence.Should().Be(2);
        }
        #endregion Handle_WithRootTargetListAndTargetItemChild_ShouldRemovesItem

        #region Handle_WithRootTargetListAndTargetListChild_ShouldRemovesList
        [Test]
        public void Handle_WithRootTargetListAndTargetListChild_ShouldRemovesList()
        {
            _list1.Name = "L1*";
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[               L1*                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Remove _list1 from _root
            RemoveYeetItemCommand<YeetItem> removeCommand =
                new RemoveYeetItemCommand<YeetItem>(_root.Guid, _list1.Guid);

            RemoveYeetItemCommandHandler<YeetList, YeetItem> handler = new RemoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(removeCommand);

            TestHelper.TestOutput(_root);
            //************
            //[   ROOT   ]
            //[IA][IB][IC]
            //************

            _root.Children.Should().NotContain(_list1);
            _items.Should().NotContain(_list1);
            _itemA.Sequence.Should().Be(0);
            _itemB.Sequence.Should().Be(1);
            _itemC.Sequence.Should().Be(2);
        }
        #endregion Handle_WithRootTargetListAndTargetListChild_ShouldRemovesList
    }
}
