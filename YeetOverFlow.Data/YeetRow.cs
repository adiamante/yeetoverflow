using System;
using System.Collections.Generic;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetRow : YeetData, IYeetKeyedList<YeetCell>
    {
        protected YeetKeyedList<YeetCell> _yeetKeyedList;

        public YeetRow() : this(Guid.NewGuid())
        {
        }

        public YeetRow(Guid guid) : base(guid, null)
        {
            _yeetKeyedList = new YeetKeyedList<YeetCell>();
        }

        public YeetCell this[string key] { get => ((IYeetKeyedList<YeetCell>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetCell>)_yeetKeyedList)[key] = value; }

        public YeetCell this[int key] => ((IYeetListBase<YeetCell>)_yeetKeyedList)[key];

        public IEnumerable<YeetCell> Children => ((IYeetListBaseRead<YeetCell>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetCell>)_yeetKeyedList).Count;

        public void AddChild(YeetCell newChild)
        {
            ((IYeetListBaseWrite<YeetCell>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetCell>)_yeetKeyedList).ContainsKey(key);
        }

        public void Init()
        {
            ((IYeetKeyedList<YeetCell>)_yeetKeyedList).Init();
        }

        public void InsertChildAt(int targetSequence, YeetCell newChild)
        {
            ((IYeetListBaseWrite<YeetCell>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetCell childToMove)
        {
            ((IYeetListBaseWrite<YeetCell>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetCell>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetCell childToRemove)
        {
            ((IYeetListBaseWrite<YeetCell>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetCell>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
