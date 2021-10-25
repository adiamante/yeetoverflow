using System;

namespace YeetOverFlow.Data
{
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
