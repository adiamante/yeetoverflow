using System;
using System.Collections.Generic;
using System.Linq;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetRowCollection : YeetDataSet //, IYeetKeyedList<YeetRow>
    {
        public YeetRowCollection() : this(Guid.NewGuid())
        {
        }

        public YeetRowCollection(Guid guid) : base(guid, null)
        {
        }

        public new YeetRow this[string key] { get => (YeetRow)_yeetKeyedList[key]; set => _yeetKeyedList[key] = value; }

        public new YeetRow this[int key] => (YeetRow)_yeetKeyedList[key];

        //public new IEnumerable<YeetRow> Children => _yeetKeyedList.Children.Cast<YeetRow>();

        public void AddChild(YeetRow newChild)
        {
            _yeetKeyedList.AddChild(newChild);
        }

        public void InsertChildAt(int targetSequence, YeetRow newChild)
        {
            _yeetKeyedList.InsertChildAt(targetSequence, newChild);
        }

        public void MoveChild(int targetSequence, YeetRow childToMove)
        {
            _yeetKeyedList.MoveChild(targetSequence, childToMove);
        }

        public void RemoveChild(YeetRow childToRemove)
        {
            _yeetKeyedList.RemoveChild(childToRemove);
        }
    }
}
