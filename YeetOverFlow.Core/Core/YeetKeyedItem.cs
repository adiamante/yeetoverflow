using System;

namespace YeetOverFlow.Core
{
    public class YeetKeyedItem : YeetItem, IKeyedItem
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetKeyedItem() : this(Guid.NewGuid(), null)
        {

        }

        public YeetKeyedItem(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }
}
