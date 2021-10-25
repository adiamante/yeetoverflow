using System;
using System.Collections.Generic;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumnCollection : YeetData, IYeetKeyedList<YeetColumn>
    {
        protected YeetKeyedList<YeetColumn> _yeetKeyedList;
        public YeetColumnCollection() : this(Guid.NewGuid())
        {
        }

        public YeetColumnCollection(Guid guid) : base(guid, null)
        {
            _yeetKeyedList = new YeetKeyedList<YeetColumn>();
        }

        public YeetColumn this[string key] { get => ((IYeetKeyedList<YeetColumn>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetColumn>)_yeetKeyedList)[key] = value; }

        public YeetColumn this[int key] => ((IYeetListBase<YeetColumn>)_yeetKeyedList)[key];

        public IEnumerable<YeetColumn> Children => ((IYeetListBaseRead<YeetColumn>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetColumn>)_yeetKeyedList).Count;

        public void AddChild(YeetColumn newChild)
        {
            ((IYeetListBaseWrite<YeetColumn>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetColumn>)_yeetKeyedList).ContainsKey(key);
        }

        public void Init()
        {
            ((IYeetKeyedList<YeetColumn>)_yeetKeyedList).Init();
        }

        public void InsertChildAt(int targetSequence, YeetColumn newChild)
        {
            ((IYeetListBaseWrite<YeetColumn>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetColumn childToMove)
        {
            ((IYeetListBaseWrite<YeetColumn>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetColumn>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetColumn childToRemove)
        {
            ((IYeetListBaseWrite<YeetColumn>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetColumn>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
