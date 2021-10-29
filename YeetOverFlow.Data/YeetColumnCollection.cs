using System;
using System.Collections.Generic;
using System.Linq;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumnCollection : YeetDataSet //, IYeetKeyedList<YeetColumn>
    {
        public YeetColumnCollection() : this(Guid.NewGuid())
        {
        }

        public YeetColumnCollection(Guid guid) : base(guid, null)
        {
        }

        public new YeetColumn this[string key] { get => (YeetColumn)_yeetKeyedList[key]; set => _yeetKeyedList[key] = value; }

        public new YeetColumn this[int key] => (YeetColumn)_yeetKeyedList[key];

        //public new IEnumerable<YeetColumn> Children => _yeetKeyedList.Children.Cast<YeetColumn>();

        public void AddChild(YeetColumn newChild)
        {
            _yeetKeyedList.AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetColumn newChild)
        {
            _yeetKeyedList.InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetColumn childToMove)
        {
            _yeetKeyedList.MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetColumn childToRemove)
        {
            _yeetKeyedList.RemoveChild(childToRemove);
        }
    }
}
