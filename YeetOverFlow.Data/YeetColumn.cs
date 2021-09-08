using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumn : YeetItem, IKeyedItem
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetColumn()
        {

        }

        public YeetColumn(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }
}
