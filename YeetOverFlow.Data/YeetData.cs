using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetData : YeetItem, IKeyedItem
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetData() : base()
        {

        }

        public YeetData(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }
}
