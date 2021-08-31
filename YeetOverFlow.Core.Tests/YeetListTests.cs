using System;
using NUnit.Framework;
using FluentAssertions;
using YeetOverFlow.Core;

namespace YeetHub.Core.Tests
{
    //https://enterprisecraftsmanship.com/posts/you-naming-tests-wrong/
    //[MethodUnderTest]_With/Of_[Scenario]_Should_[ExpectedResult]
    [TestFixture]
    public class YeetListTests
    {
        YeetList _list;
        YeetItem _itemA, _itemB, _itemC, _newItem;

        [SetUp]
        public void Setup()
        {
            _list = new YeetList();

            _itemA = new YeetItem(Guid.Parse("5cbb0bcb-9c81-4ec3-a260-08d8b4f2b7f6"))
            {
                Name = "IA"
            };

            _itemB = new YeetItem(Guid.Parse("701139a8-f20b-4fa6-b52f-178e12362163"))
            {
                Name = "IB"
            };

            _itemC = new YeetItem(Guid.Parse("47e14d9a-51c1-4b7f-9f6d-dd3466c458a5"))
            {
                Name = "IC"
            };

            _newItem = new YeetItem(Guid.Parse("5fff743a-911a-40e5-8dc2-0e9693bf6d20"))
            {
                Name = "I*"
            };

            _list.AddChild(_itemA);
            _list.AddChild(_itemB);
            _list.AddChild(_itemC);
        }

        [Test]
        public void AddChild_WithNull_ThrowsException()
        {
            FluentActions.Invoking(() => _list.AddChild(null))
                .Should().Throw<ArgumentNullException>()
                .Where(e => e.Message == "Value cannot be null. (Parameter 'newChild')");
        }

        [Test]
        public void RemoveChild_WithNull_ThrowsException()
        {
            FluentActions.Invoking(() => _list.RemoveChild(null))
                .Should().Throw<ArgumentNullException>()
                .Where(e => e.Message == "Value cannot be null. (Parameter 'childToRemove')");
        }

        [Test]
        public void AddChild_WithDuplicateItemReference_ThrowsException()
        {
            YeetItem newSong = new YeetItem() { Name = "I*" };
            _list.AddChild(newSong);

            FluentActions.Invoking(() => _list.AddChild(newSong))
                .Should().Throw<InvalidOperationException>()
                .Where(e => e.Message == "Cannot add 'newChild' because it already exists in _children.");
        }

        [Test]
        public void AddChild_WithDuplicateGuid_ThrowsException()
        {
            YeetItem newSong1 = new YeetItem(Guid.Parse("26198ac5-405b-40ea-8b37-6a4138827916")) { Name = "I1 *" };
            YeetItem newSong2 = new YeetItem(Guid.Parse("26198ac5-405b-40ea-8b37-6a4138827916")) { Name = "I2*" };
            _list.AddChild(newSong1);

            FluentActions.Invoking(() => _list.AddChild(newSong2))
                .Should().Throw<InvalidOperationException>()
                .Where(e => e.Message == "Cannot add 'newChild' because it already exists in _children.");
        }

        [Test]
        public void InsertChildAt_WithZeroIndex_ShouldAddToBeginning()
        {
            _list.InsertChildAt(0, _newItem);

            _list.Children.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
        }

        [Test]
        public void InsertChildAt_WithUnderBoundIndex_ShouldAddToBeginning()
        {
            _list.InsertChildAt(-1, _newItem);

            _list.Children.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(0);
            _itemA.Sequence.Should().Be(1);
            _itemB.Sequence.Should().Be(2);
            _itemC.Sequence.Should().Be(3);
        }

        [Test]
        public void InsertChildAt_WithMiddleIndex_ShouldAddToMiddle()
        {
            _list.InsertChildAt(2, _newItem);

            _list.Children.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(2);
            _itemA.Sequence.Should().Be(0);
            _itemB.Sequence.Should().Be(1);
            _itemC.Sequence.Should().Be(3);
        }

        [Test]
        public void InsertChildAt_WithEndIndex_ShouldAddToEnd()
        {
            _list.InsertChildAt(3, _newItem);

            _list.Children.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(3);
            _itemA.Sequence.Should().Be(0);
            _itemB.Sequence.Should().Be(1);
            _itemC.Sequence.Should().Be(2);
        }

        [Test]
        public void InsertChildAt_WithOverBoundIndex_ShouldAddToEnd()
        {
            _list.InsertChildAt(int.MaxValue, _newItem);

            _list.Children.Should().Contain(_newItem);
            _newItem.Sequence.Should().Be(3);
            _itemA.Sequence.Should().Be(0);
            _itemB.Sequence.Should().Be(1);
            _itemC.Sequence.Should().Be(2);
        }
    }
}