using FluentAssertions;
using NUnit.Framework;
using System;
using YeetOverFlow.Settings;

namespace YeetOverFlow.Settings.Test
{
    //[MethodUnderTest]_With/Of_[Scenario]_Should_[ExpectedResult]
    public class YeetSettingListTests
    {
        YeetSettingList _list;
        YeetSettingInt _intSetting;
        YeetSettingString _stringSetting;
        YeetSettingBoolean _boolSetting;
        YeetSettingDateTime _dateSetting;

        [SetUp]
        public void Setup()
        {
            _list = new YeetSettingList(Guid.NewGuid(), "List");
            _intSetting = new YeetSettingInt(Guid.NewGuid(), "Int");
            _stringSetting = new YeetSettingString(Guid.NewGuid(), "String");
            _boolSetting = new YeetSettingBoolean(Guid.NewGuid(), "Bool");
            _dateSetting = new YeetSettingDateTime(Guid.NewGuid(), "Date");

            _list[_intSetting.Key] = _intSetting;
            _list[_stringSetting.Key] = _stringSetting;
            _list[_boolSetting.Key] = _boolSetting;
        }

        [Test]
        public void AccessNonExistantKey_ShouldAddChild()
        {
            var setting = _list["DoesNotExist"];
            setting.Should().NotBeNull();
        }

        [Test]
        public void AddChild_ShouldAddChild()
        {
            _list.AddChild(_dateSetting);
            _list.Children.Should().Contain(_dateSetting);
            _list[_dateSetting.Key].Should().BeEquivalentTo(_dateSetting);
            _dateSetting.Sequence.Should().Be(3);
            _intSetting.Sequence.Should().Be(0);
            _stringSetting.Sequence.Should().Be(1);
            _boolSetting.Sequence.Should().Be(2);
        }

        [Test]
        public void AddByKey_ShouldAddChild()
        {
            _list[_dateSetting.Key] = _dateSetting;
            _list.Children.Should().Contain(_dateSetting);
            _list[_dateSetting.Key].Should().BeEquivalentTo(_dateSetting);
            _dateSetting.Sequence.Should().Be(3);
            _intSetting.Sequence.Should().Be(0);
            _stringSetting.Sequence.Should().Be(1);
            _boolSetting.Sequence.Should().Be(2);
        }

        [Test]
        public void RemoveChild_ShouldRemoveChild()
        {
            _list.RemoveChild(_stringSetting);
            _list.Children.Should().NotContain(_stringSetting);
            _list[_stringSetting.Key].Should().NotBe(_stringSetting);
            _intSetting.Sequence.Should().Be(0);
            _boolSetting.Sequence.Should().Be(1);
        }

        [Test]
        public void RemoveChild_WithKey_ShouldRemoveChild()
        {
            _list.Remove(_stringSetting.Key);
            _list.Children.Should().NotContain(_stringSetting);
            _list[_stringSetting.Key].Should().NotBe(_stringSetting);
            _intSetting.Sequence.Should().Be(0);
            _boolSetting.Sequence.Should().Be(1);
        }

        [Test]
        public void InsertChildAt_WithZeroIndex_ShouldAddToBeginning()
        {
            _list.InsertChildAt(0, _dateSetting);

            _list.Children.Should().Contain(_dateSetting);
            _dateSetting.Sequence.Should().Be(0);
            _intSetting.Sequence.Should().Be(1);
            _stringSetting.Sequence.Should().Be(2);
            _boolSetting.Sequence.Should().Be(3);
        }
    }
}