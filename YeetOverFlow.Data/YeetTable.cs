using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetTable : YeetData
    {
        private YeetKeyedList<YeetColumn> _columns;
        private YeetList<YeetRow> _rows;

        public YeetTable()
        {
            _columns = new YeetKeyedList<YeetColumn>();
            _rows = new YeetList<YeetRow>();
        }
    }
}
