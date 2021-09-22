using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumn : YeetKeyedItem
    {
        public YeetColumn() : this(Guid.NewGuid(), null)
        {

        }

        public YeetColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
