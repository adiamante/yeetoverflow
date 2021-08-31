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
    //[MethodUnderTest]_Should_[Scenario]_With_[ExpectedResult]
    public class AddYeetItemCommandHandlerTests : YeetCommandHandlerTestBase
    {
        #region Handle_WithInvalidTargetListGuid_ShouldThrowException
        [Test]
        public void Handle_WithInvalidTargetListGuid_ShouldThrowException()
        {
            Guid invalidGuid = Guid.Parse("c36bf1cd-4d44-4ec2-a2be-14cec07ccc13");
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(invalidGuid, _newItem, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);

            Result result = handler.Handle(addCommand);
            result.IsFailure.Should().BeTrue();
            result.Exception.Should().BeOfType<InvalidOperationException>();
            result.Error.Should().Be("Could not find targetList with provided 'targetListGuid'");
        }
        #endregion Handle_WithInvalidTargetListGuid_ShouldThrowException

        #region Handle_ShouldInvokeDispatchEvent_WithCorrectEvent
        [Test]
        public void Handle_ShouldInvokeDispatchEvent_WithCorrectEvent()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _root
            AddYeetItemCommand<YeetItem> addCommand = 
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[I*][                L1                ][IA][IB][IC]
            //    [          L2          ][IE][IF][IG]            
            //    [    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _mockEventStore.Verify(es => es.DispatchEvent(It.IsAny<YeetItemAddedEvent<YeetItem>>()), Times.Once);
            _events.Should().ContainEquivalentOf<YeetItemAddedEvent<YeetItem>>(
                new YeetItemAddedEvent<YeetItem>(_root.Guid, _newItem, 0));
        }
        #endregion Handle_ShouldInvokeDispatchEvent_WithCorrectEvent

        #region Handle_WithTargetRootAndItemChildAndZeroIndex_ShouldAddToBeginning
        [Test]
        public void Handle_WithTargetRootAndItemChildAndZeroIndex_ShouldAddToBeginning()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _root
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[I*][                L1                ][IA][IB][IC]
            //    [          L2          ][IE][IF][IG]            
            //    [    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _root.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetRootAndItemChildAndZeroIndex_ShouldAddToBeginning

        #region Handle_WithTargetRootAndItemChildAndUnderBoundIndex_ShouldAddToBeginning
        [Test]
        public void Handle_WithTargetRootAndItemChildAndUnderBoundIndex_ShouldAddToBeginning()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _root
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, -1);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[I*][                L1                ][IA][IB][IC]
            //    [          L2          ][IE][IF][IG]            
            //    [    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _root.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetRootAndItemChildAndUnderBoundIndex_ShouldAddToBeginning

        #region Handle_WithTargetRootAndItemChildAndMiddleIndex_ShouldAddToMiddle
        [Test]
        public void Handle_WithTargetRootAndItemChildAndMiddleIndex_ShouldAddToMiddle()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _root
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, 2);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                L1                ][IA][I*][IB][IC]
            //[          L2          ][IE][IF][IG]                
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _root.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(2);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetRootAndItemChildAndMiddleIndex_ShouldAddToMiddle

        #region Handle_WithTargetRootAndItemChildAndEndIndex_ShouldAddToEnd
        [Test]
        public void Handle_WithTargetRootAndItemChildAndEndIndex_ShouldAddToEnd()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _root
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, 4);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);
            
            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                L1                ][IA][IB][IC][I*]
            //[          L2          ][IE][IF][IG]                
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _root.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
        }
        #endregion Handle_WithTargetRootAndItemChildAndEndIndex_ShouldAddToEnd

        #region Handle_WithTargetRootAndItemChildAndOverBoundIndex_ShouldAddToEnd
        [Test]
        public void Handle_WithTargetRootAndItemChildAndOverBoundIndex_ShouldAddToEnd()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _root to _newItem
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newItem, Int32.MaxValue);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                L1                ][IA][IB][IC][I*]
            //[          L2          ][IE][IF][IG]                
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _root.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
        }
        #endregion Handle_WithTargetRootAndItemChildAndOverBoundIndex_ShouldAddToEnd

        #region Handle_WithTargetRootAndListChildAndZeroIndex_ShouldAddToBeginning
        [Test]
        public void Handle_WithTargetRootAndListChildAndZeroIndex_ShouldAddToBeginning()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _root
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newList, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[L*][                L1                ][IA][IB][IC]
            //[I*][          L2          ][IE][IF][IG]            
            //    [    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _root.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(0);
            _list1.Sequence.Should().Be(1);
            _itemA.Sequence.Should().Be(2);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetRootAndListChildAndZeroIndex_ShouldAddToBeginning

        #region Handle_WithTargetRootAndListChildAndMidleIndex_ShouldAddToMiddle
        [Test]
        public void Handle_WithTargetRootAndListChildAndMidleIndex_ShouldAddToMiddle()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _root
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newList, 2);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                L1                ][IA][L*][IB][IC]
            //[          L2          ][IE][IF][IG]    [I*]        
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _root.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(2);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(3);
            _itemC.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetRootAndListChildAndMidleIndex_ShouldAddToMiddle

        #region Handle_WithTargetRootAndListChildAndEndIndex_ShouldAddToEnd
        [Test]
        public void Handle_WithTargetRootAndListChildAndEndIndex_ShouldAddToEnd()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _root
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_root.Guid, _newList, 4);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                L1                ][IA][IB][IC][L*]
            //[          L2          ][IE][IF][IG]            [I*]
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _root.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(4);
            _list1.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
        }
        #endregion Handle_WithTargetRootAndListChildAndEndIndex_ShouldAddToEnd

        #region Handle_WithTargetListAndItemChildAndZeroIndex_ShouldAddToBeginning
        [Test]
        public void Handle_WithTargetListAndItemChildAndZeroIndex_ShouldAddToBeginning()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _list1
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newItem, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[I*][          L2          ][IE][IF][IG]            
            //    [    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _list1.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(0);
            _list2.Sequence.Should().Be(1);
            _itemE.Sequence.Should().Be(2);
            _itemF.Sequence.Should().Be(3);
            _itemG.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetListAndItemChildAndZeroIndex_ShouldAddToBeginning

        #region Handle_WithTargetListAndItemChildAndMiddleIndex_ShouldAddToMiddle
        [Test]
        public void Handle_WithTargetListAndItemChildAndMiddleIndex_ShouldAddToMiddle()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _list1
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newItem, 2);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[          L2          ][IE][I*][IF][IG]            
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _list1.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(2);
            _list2.Sequence.Should().Be(0);
            _itemE.Sequence.Should().Be(1);
            _itemF.Sequence.Should().Be(3);
            _itemG.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetListAndItemChildAndMiddleIndex_ShouldAddToMiddle

        #region Handle_WithTargetListAndItemChildAndEndIndex_ShouldAddToEnd
        [Test]
        public void Handle_WithTargetListAndItemChildAndEndIndex_ShouldAddToEnd()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newItem to _list1
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newItem, 4);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[          L2          ][IE][IF][IG][I*]            
            //[    L3    ][II][IJ][IK]                            
            //[IX][IY][IZ]                                        
            //****************************************************

            _list1.Children.Should().Contain(_newItem);
            _items.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(4);
            _list2.Sequence.Should().Be(0);
            _itemE.Sequence.Should().Be(1);
            _itemF.Sequence.Should().Be(2);
            _itemG.Sequence.Should().Be(3);
        }
        #endregion Handle_WithTargetListAndItemChildAndEndIndex_ShouldAddToEnd

        #region Handle_WithTargetListAndListChildAndZeroIndex_ShouldAddToBeginning
        [Test]
        public void Handle_WithTargetListAndListChildAndZeroIndex_ShouldAddToBeginning()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _list1
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newList, 0);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[L*][          L2          ][IE][IF][IG]            
            //[I*][    L3    ][II][IJ][IK]                        
            //    [IX][IY][IZ]                                    
            //****************************************************

            _list1.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(0);
            _list2.Sequence.Should().Be(1);
            _itemE.Sequence.Should().Be(2);
            _itemF.Sequence.Should().Be(3);
            _itemG.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetListAndListChildAndZeroIndex_ShouldAddToBeginning

        #region Handle_WithTargetListAndListChildAndMiddleIndex_ShouldAddToMiddle
        [Test]
        public void Handle_WithTargetListAndListChildAndMiddleIndex_ShouldAddToMiddle()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _list1
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newList, 2);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[          L2          ][IE][L*][IF][IG]            
            //[    L3    ][II][IJ][IK]    [I*]                    
            //    [IX][IY][IZ]                                    
            //****************************************************

            _list1.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(2);
            _list2.Sequence.Should().Be(0);
            _itemE.Sequence.Should().Be(1);
            _itemF.Sequence.Should().Be(3);
            _itemG.Sequence.Should().Be(4);
        }
        #endregion Handle_WithTargetListAndListChildAndMiddleIndex_ShouldAddToMiddle

        #region Handle_WithTargetListAndListChildAndEndIndex_ShouldAddToEnd
        [Test]
        public void Handle_WithTargetListAndListChildAndEndIndex_ShouldAddToEnd()
        {
            TestHelper.TestOutput(_root);
            //************************************************
            //[                     ROOT                     ]
            //[                L1                ][IA][IB][IC]
            //[          L2          ][IE][IF][IG]            
            //[    L3    ][II][IJ][IK]                        
            //[IX][IY][IZ]                                    
            //************************************************

            //Add _newList to _list1
            _newList.AddChild(_newItem);
            AddYeetItemCommand<YeetItem> addCommand =
                new AddYeetItemCommand<YeetItem>(_list1.Guid, _newList, 4);

            AddYeetItemCommandHandler<YeetList, YeetItem> handler = new AddYeetItemCommandHandler<YeetList, YeetItem>(_mockUnitOfWork.Object, _mockEventStore.Object);
            handler.Handle(addCommand);

            TestHelper.TestOutput(_root);
            //****************************************************
            //[                       ROOT                       ]
            //[                  L1                  ][IA][IB][IC]
            //[          L2          ][IE][IF][IG][L*]            
            //[    L3    ][II][IJ][IK]            [I*]            
            //    [IX][IY][IZ]                                    
            //****************************************************

            _list1.Children.Should().Contain(_newList);
            _items.Should().Contain(_newList);
            _items.Should().Contain(_newItem);
            _newList.Sequence.Should().Be(4);
            _list2.Sequence.Should().Be(0);
            _itemE.Sequence.Should().Be(1);
            _itemF.Sequence.Should().Be(2);
            _itemG.Sequence.Should().Be(3);
        }
        #endregion Handle_WithTargetListAndListChildAndEndIndex_ShouldAddToEnd
    }
}