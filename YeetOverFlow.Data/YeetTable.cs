using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetColumnCollection : YeetKeyedList<YeetColumn>
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetColumnCollection()
        {

        }

        public YeetColumnCollection(Guid guid, string key) : base(guid)
        {
            _key = key;
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
    }
}
