using System;
using System.Collections.Generic;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetDataSet : YeetData, IYeetKeyedList<YeetData>
    {
        protected YeetKeyedList<YeetDataSet, YeetData> _yeetKeyedList;

        public YeetDataSet() : this(Guid.NewGuid(), null)
        {
        }

        public YeetDataSet(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetKeyedList<YeetDataSet, YeetData>(guid);
        }

        public YeetData this[string key] { get => ((IYeetKeyedList<YeetData>)_yeetKeyedList)[key]; set => ((IYeetKeyedList<YeetData>)_yeetKeyedList)[key] = value; }

        public YeetData this[int key] => ((IYeetListBase<YeetData>)_yeetKeyedList)[key];

        public IEnumerable<YeetData> Children => ((IYeetListBaseRead<YeetData>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetData>)_yeetKeyedList).Count;

        public void AddChild(YeetData newChild)
        {
            ((IYeetListBaseWrite<YeetData>)_yeetKeyedList).AddChild(newChild);
        }

        public bool ContainsKey(string key)
        {
            return ((IYeetKeyedList<YeetData>)_yeetKeyedList).ContainsKey(key);
        }

        public void Init()
        {
            ((IYeetKeyedList<YeetData>)_yeetKeyedList).Init();
        }

        public void InsertChildAt(int targetSequence, YeetData newChild)
        {
            ((IYeetListBaseWrite<YeetData>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetData childToMove)
        {
            ((IYeetListBaseWrite<YeetData>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void Remove(string keyToRemove)
        {
            ((IYeetKeyedList<YeetData>)_yeetKeyedList).Remove(keyToRemove);
        }

        public void RemoveChild(YeetData childToRemove)
        {
            ((IYeetListBaseWrite<YeetData>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetData>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
