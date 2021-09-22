using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetData : YeetKeyedItem
    {
        public YeetData() : this(Guid.NewGuid(), null)
        {

        }

        public YeetData(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
