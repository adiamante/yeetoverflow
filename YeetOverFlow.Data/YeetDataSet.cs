using System;

namespace YeetOverFlow.Data
{
    public class YeetDataSet : YeetData
    {
        public YeetDataSet() : base()
        {

        }

        public YeetDataSet(Guid guid, string key) : base(guid, key)
        {
            _key = key;
        }
    }
}
