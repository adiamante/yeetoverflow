using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumnCollection : YeetKeyedList<YeetColumn>
    {
        public YeetColumnCollection() : this(Guid.NewGuid())
        {

        }

        public YeetColumnCollection(Guid guid) : base(guid)
        {
        }
    }

    public class YeetRowCollection : YeetList<YeetRow>
    {

    }

    public class YeetTable : YeetData
    {

        public YeetColumnCollection Columns { get; set; } = new YeetColumnCollection();
        public YeetRowCollection Rows { get; set; } = new YeetRowCollection();

        public YeetTable()
        {

        }

        public YeetTable(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
