using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetCell : YeetItem, IKeyedItem
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetCell()
        {

        }

        public YeetCell(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }
}
