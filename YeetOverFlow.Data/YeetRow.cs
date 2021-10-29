using System;
using System.Collections.Generic;
using System.Linq;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetRow : YeetDataSet //, IYeetKeyedList<YeetCell>
    {
        public YeetRow() : this(Guid.NewGuid())
        {
        }

        public YeetRow(Guid guid) : base(guid, guid.ToString())
        {
        }

        public new YeetCell this[string key] { get => (YeetCell)_yeetKeyedList[key]; set => _yeetKeyedList[key] = value; }

        public new YeetCell this[int key] => (YeetCell)_yeetKeyedList[key];

        //public new IEnumerable<YeetCell> Children => _yeetKeyedList.Children.Cast<YeetCell>();

        public void AddChild(YeetCell newChild)
        {
            _yeetKeyedList.AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetCell newChild)
        {
            _yeetKeyedList.InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetCell childToMove)
        {
            _yeetKeyedList.MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetCell childToRemove)
        {
            _yeetKeyedList.RemoveChild(childToRemove);
        }
    }
}
