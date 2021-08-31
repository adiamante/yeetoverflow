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
    public class MoveYeetItemCommandHandlerTests : YeetCommandHandlerTestBase
    {
        #region Handle_WithInvalidOriginalListGuid_ShouldThrowException
        [Test]
        public void Handle_WithInvalidOriginalListGuid_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(invalidGuid, _root.Guid, _itemE.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetList with provided 'originalListGuid'");
        }
        #endregion Handle_WithInvalidOriginalListGuid_ShouldThrowException

        #region Handle_WithInvalidTargetListGuid_ShouldThrowException
        [Test]
        public void Handle_WithInvalidTargetListGuid_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, invalidGuid, _itemE.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetList with provided 'targetListGuid'");
        }
        #endregion Handle_WithInvalidTargetListGuid_ShouldThrowException

        #region Handle_WithInvalidTargetChildGuid_ShouldThrowException
        [Test]
        public void Handle_WithInvalidTargetChildGuid_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, invalidGuid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetChild with provided 'targetChildGuid'");
        }
        #endregion Handle_WithInvalidTargetChildGuid_ShouldThrowException

        #region Handle_ShouldCallDispatchEvent_WithCorrectEvent
        [Test]
        public void Handle_ShouldCallDispatchEvent_WithCorrectEvent()
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

            //Add _itemA to _list1
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_root.Guid, _list1.Guid, _itemA.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                  L1                   ][IB][IC]
            //[IA*][          L2          ][IE][IF][IG]        
            //     [    L3    ][II][IJ][IK]                    
            //     [IX][IY][IZ]                                
            //*************************************************

            _mockEventStore.Verify(es => es.DispatchEvent(It.IsAny<YeetItemMovedEvent<YeetItem>>()), Times.Once);
            _events.Should().ContainEquivalentOf<YeetItemMovedEvent<YeetItem>>(
                new YeetItemMovedEvent<YeetItem>(_root.Guid, _list1.Guid, _itemA.Guid, 0));
        }
        #endregion Handle_ShouldCallDispatchEvent_WithCorrectEvent

        #region Handle_WithRootOriginalListAndListTargetAndTargetToOwnHeirarchy_ShouldThrowException
        [Test]
        public void Handle_WithRootOriginalListAndListTargetAndTargetToOwnHeirarchy_ShouldThrowException()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _list1 to _list2 which is in its own heirarchy
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_root.Guid, _list2.Guid, _list1.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Cannot move a list within own heirarchy.");
        }
        #endregion Handle_WithRootOriginalListAndListTargetAndTargetToOwnHeirarchy_ShouldThrowException

        #region Handle_WithEmptyOriginalListAndListTargetAndInvalidTargetParent_ShouldThrowException
        [Test]
        public void Handle_WithEmptyOriginalListAndListTargetAndInvalidTargetParent_ShouldThrowException()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _itemA within _list1 but _itemA does not already exist in _list1
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(Guid.Empty, _list1.Guid, _itemA.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("When exluding originalListGuid, targetList should already contain targetChild.");
        }
        #endregion Handle_WithEmptyOriginalListAndListTargetAndInvalidTargetParent_ShouldThrowException

        #region Handle_WithOriginalListAndListTargetAndInvalidOriginalParent_ShouldThrowException
        [Test]
        public void Handle_WithOriginalListAndListTargetAndInvalidOriginalParent_ShouldThrowException()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _itemA to _list1 but _itemA does not already exist in _list2
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list2.Guid, _list1.Guid, _itemA.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(moveCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Original list does not already contain targetChild.");
        }
        #endregion Handle_WithOriginalListAndListTargetAndInvalidOriginalParent_ShouldThrowException

        #region Handle_WithOriginalListAndRootTargetAndItemChildAndZeroIndex_ShouldMoveToBeginning
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndItemChildAndZeroIndex_ShouldMoveToBeginning()
        {
            _itemE.Name = "IE*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                 ][IA][IB][IC]
            //[          L2          ][IE*][IF][IG]            
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Move _itemE from _list1 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, _itemE.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[IE*][              L1              ][IA][IB][IC]
            //     [          L2          ][IF][IG]            
            //     [    L3    ][II][IJ][IK]                    
            //     [IX][IY][IZ]                                
            //*************************************************

            _list1.Children.Should().NotContain(_itemE);
            _root.Children.Should().ContainSingle(item => item == _itemE);
            _items.Should().ContainSingle(item => item == _itemE);
            _itemE.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
            _list2.Sequence.Should().Be(0);
            _itemF.Sequence.Should().Be(1);
            _itemG.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndItemChildAndZeroIndex_ShouldMoveToBeginning

        #region Handle_WithOriginalListAndRootTargetAndItemChildAndUnderBoundIndex_ShouldMoveToBeginning
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndItemChildAndUnderBoundIndex_ShouldMoveToBeginning()
        {
            _itemE.Name = "IE*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                 ][IA][IB][IC]
            //[          L2          ][IE*][IF][IG]            
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Move _itemE from _list1 to _root with under bound index
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, _itemE.Guid, -1);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[IE*][              L1              ][IA][IB][IC]
            //     [          L2          ][IF][IG]            
            //     [    L3    ][II][IJ][IK]                    
            //     [IX][IY][IZ]                                
            //*************************************************

            _list1.Children.Should().NotContain(_itemE);
            _root.Children.Should().ContainSingle(item => item == _itemE);
            _items.Should().ContainSingle(item => item == _itemE);
            _itemE.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
            _list2.Sequence.Should().Be(0);
            _itemF.Sequence.Should().Be(1);
            _itemG.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndItemChildAndUnderBoundIndex_ShouldMoveToBeginning

        #region Handle_WithOriginalListAndRootTargetAndItemChildAndMiddleIndex_ShouldMoveToMiddle
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndItemChildAndMiddleIndex_ShouldMoveToMiddle()
        {
            _itemE.Name = "IE*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                 ][IA][IB][IC]
            //[          L2          ][IE*][IF][IG]            
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Move _itemE from _list1 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, _itemE.Guid, 2);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[              L1              ][IA][IE*][IB][IC]
            //[          L2          ][IF][IG]                 
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            _list1.Children.Should().NotContain(_itemE);
            _root.Children.Should().ContainSingle(item => item == _itemE);
            _items.Should().ContainSingle(item => item == _itemE);
            _itemE.Sequence.Should().Be(2);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
            _list2.Sequence.Should().Be(0);
            _itemF.Sequence.Should().Be(1);
            _itemG.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndItemChildAndMiddleIndex_ShouldMoveToMiddle

        #region Handle_WithOriginalListAndRootTargetAndItemChildAndEndIndex_ShouldMoveToEnd
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndItemChildAndEndIndex_ShouldMoveToEnd()
        {
            _itemE.Name = "IE*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                 ][IA][IB][IC]
            //[          L2          ][IE*][IF][IG]            
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Move _itemE from _list1 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, _itemE.Guid, 4);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[              L1              ][IA][IB][IC][IE*]
            //[          L2          ][IF][IG]                 
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            _list1.Children.Should().NotContain(_itemE);
            _root.Children.Should().ContainSingle(item => item == _itemE);
            _items.Should().ContainSingle(item => item == _itemE);
            _itemE.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
            _list2.Sequence.Should().Be(0);
            _itemF.Sequence.Should().Be(1);
            _itemG.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndItemChildAndEndIndex_ShouldMoveToEnd

        #region Handle_WithOriginalListAndRootTargetAndItemChildAndOverBoundIndex_ShouldMoveToEnd
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndItemChildAndOverBoundIndex_ShouldMoveToEnd()
        {
            _itemE.Name = "IE*";
            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[                L1                 ][IA][IB][IC]
            //[          L2          ][IE*][IF][IG]            
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            //Move _itemE from _list1 to _root with over bound index
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list1.Guid, _root.Guid, _itemE.Guid, Int32.MaxValue);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //*************************************************
            //[                     ROOT                      ]
            //[              L1              ][IA][IB][IC][IE*]
            //[          L2          ][IF][IG]                 
            //[    L3    ][II][IJ][IK]                         
            //[IX][IY][IZ]                                     
            //*************************************************

            _list1.Children.Should().NotContain(_itemE);
            _root.Children.Should().ContainSingle(item => item == _itemE);
            _items.Should().ContainSingle(item => item == _itemE);
            _itemE.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
            _list2.Sequence.Should().Be(0);
            _itemF.Sequence.Should().Be(1);
            _itemG.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndItemChildAndOverBoundIndex_ShouldMoveToEnd

        #region Handle_WithOriginalListAndRootTargetAndListChildAndZeroIndex_ShouldMoveToBeginning
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndListChildAndZeroIndex_ShouldMoveToBeginning()
        {
            _list3.Name = "L3*";
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[   L3*    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _list3 from _list2 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list2.Guid, _root.Guid, _list3.Guid, 0);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[   L3*    ][          L1          ][IA][IB][IC]
            //[IX][IY][IZ][    L2    ][IE][IF][IG]            
            //            [II][IJ][IK]                        
            //************************************************

            _list2.Children.Should().NotContain(_list3);
            _root.Children.Should().ContainSingle(item => item == _list3);
            _items.Should().ContainSingle(item => item == _list3);
            _list3.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
            _itemI.Sequence.Should().Be(0);
            _itemJ.Sequence.Should().Be(1);
            _itemK.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndListChildAndZeroIndex_ShouldMoveToBeginning

        #region Handle_WithOriginalListAndRootTargetAndListChildAndMiddleIndex_ShouldMoveToMiddle
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndListChildAndMiddleIndex_ShouldMoveToMiddle()
        {
            _list3.Name = "L3*";
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[   L3*    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _list3 from _list2 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list2.Guid, _root.Guid, _list3.Guid, 2);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[          L1          ][IA][   L3*    ][IB][IC]
            //[    L2    ][IE][IF][IG]    [IX][IY][IZ]        
            //[II][IJ][IK]                                    
            //************************************************

            _list2.Children.Should().NotContain(_list3);
            _root.Children.Should().ContainSingle(item => item == _list3);
            _items.Should().ContainSingle(item => item == _list3);
            _list3.Sequence.Should().Be(2);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
            _itemI.Sequence.Should().Be(0);
            _itemJ.Sequence.Should().Be(1);
            _itemK.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndListChildAndMiddleIndex_ShouldMoveToMiddle

        #region Handle_WithOriginalListAndRootTargetAndListChildAndEndIndex_ShouldMoveToEnd
        [Test]
        public void Handle_WithOriginalListAndRootTargetAndListChildAndEndIndex_ShouldMoveToEnd()
        {
            _list3.Name = "L3*";
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[   L3*    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Move _list3 from _list2 to _root
            MoveYeetItemCommand<YeetItem> moveCommand =
                new MoveYeetItemCommand<YeetItem>(_list2.Guid, _root.Guid, _list3.Guid, 4);

            MoveYeetItemCommandHandler<YeetList, YeetItem> handler = new MoveYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(moveCommand);

            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[          L1          ][IA][IB][IC][   L3*    ]
            //[    L2    ][IE][IF][IG]            [IX][IY][IZ]
            //[II][IJ][IK]                                    
            //************************************************

            _list2.Children.Should().NotContain(_list3);
            _root.Children.Should().ContainSingle(item => item == _list3);
            _items.Should().ContainSingle(item => item == _list3);
            _list3.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
            _itemI.Sequence.Should().Be(0);
            _itemJ.Sequence.Should().Be(1);
            _itemK.Sequence.Should().Be(2);
        }
        #endregion Handle_WithOriginalListAndRootTargetAndListChildAndEndIndex_ShouldMoveToEnd
    }
}
