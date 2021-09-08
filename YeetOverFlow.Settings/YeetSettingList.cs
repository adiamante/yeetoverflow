using System;
using System.Collections.Generic;
using YeetOverFlow.Core;

namespace YeetOverFlow.Settings
{
    public class YeetSettingList : YeetSetting, IYeetKeyedList<YeetSetting>
    {
        protected YeetKeyedList<YeetSettingList, YeetSetting> _yeetKeyedList;

        YeetSettingList() : this(Guid.Empty, null)
        {

        }
        public YeetSettingList(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetKeyedList<YeetSettingList, YeetSetting>();
        }
        public YeetSetting this[int index] { get => _yeetKeyedList[index]; }
        public override YeetSetting this[string key] { get => ((IYeetKeyedList<YeetSetting>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetSetting>)_yeetKeyedList)[key] = value; }
        public IEnumerable<YeetSetting> Children => ((IYeetListBaseRead<YeetSetting>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetSetting>)_yeetKeyedList).Count;

        public void AddChild(YeetSetting newChild)
        {
            ((IYeetListBaseWrite<YeetSetting>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetSetting>)_yeetKeyedList).ContainsKey(key);
        }

        public void InsertChildAt(int targetSequence, YeetSetting newChild)
        {
            ((IYeetListBaseWrite<YeetSetting>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetSetting childToMove)
        {
            ((IYeetListBaseWrite<YeetSetting>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetSetting>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetSetting childToRemove)
        {
            ((IYeetListBaseWrite<YeetSetting>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetSetting>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }

        public void Init()
        {
            ((IYeetKeyedList<YeetSetting>)_yeetKeyedList).Init();
        }
    }
}
