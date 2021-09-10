using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public class YeetCell : YeetItem, IKeyedItem
    {
        protected string _key;
        public string Key { get => _key; }

        public YeetCell()
        {

        }

        public YeetCell(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }

    public class YeetCell<T> : YeetCell
    {
        public YeetCell(Guid guid, string key) : base(guid, key)
        {
        }

        public T Value { get; set; }
    }

    public class YeetBooleanCell : YeetCell<bool>
    {
        YeetBooleanCell() : this(Guid.Empty, null)
        {

        }
        public YeetBooleanCell(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetStringCell : YeetCell<string>
    {
        YeetStringCell() : this(Guid.Empty, null)
        {

        }
        public YeetStringCell(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetIntCell : YeetCell<int>
    {
        YeetIntCell() : this(Guid.Empty, null)
        {

        }
        public YeetIntCell(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDoubleCell : YeetCell<double>
    {
        YeetDoubleCell() : this(Guid.Empty, null)
        {

        }
        public YeetDoubleCell(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDateTimeCell : YeetCell<DateTimeOffset>
    {
        YeetDateTimeCell() : this(Guid.Empty, null)
        {

        }
        public YeetDateTimeCell(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
