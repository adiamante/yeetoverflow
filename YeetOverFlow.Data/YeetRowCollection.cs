using System;
using System.Collections.Generic;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetRowCollection : YeetData, IYeetListBase<YeetRow>
    {
        protected YeetList<YeetRow> _yeetKeyedList;

        public YeetRowCollection() : this(Guid.NewGuid(), null)
        {
        }

        public YeetRowCollection(Guid guid, string key) : base(guid, key)
        {
            _yeetKeyedList = new YeetList<YeetRow>();
        }

        public YeetRow this[int key] => ((IYeetListBase<YeetRow>)_yeetKeyedList)[key];

        public IEnumerable<YeetRow> Children => ((IYeetListBaseRead<YeetRow>)_yeetKeyedList).Children;

        public int Count => ((IYeetListBaseRead<YeetRow>)_yeetKeyedList).Count;

        public void AddChild(YeetRow newChild)
        {
            ((IYeetListBaseWrite<YeetRow>)_yeetKeyedList).AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetRow newChild)
        {
            ((IYeetListBaseWrite<YeetRow>)_yeetKeyedList).InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetRow childToMove)
        {
            ((IYeetListBaseWrite<YeetRow>)_yeetKeyedList).MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetRow childToRemove)
        {
            ((IYeetListBaseWrite<YeetRow>)_yeetKeyedList).RemoveChild(childToRemove);
        }

        public void RemoveChildAt(int targetSequence)
        {
            ((IYeetListBaseWrite<YeetRow>)_yeetKeyedList).RemoveChildAt(targetSequence);
        }
    }
}
