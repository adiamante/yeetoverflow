using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Data
{
    public abstract class YeetColumn : YeetData
    {
        public YeetColumn() : this(Guid.NewGuid(), null)
        {

        }

        public YeetColumn(Guid guid, string key) : base(guid, key)
        {
        }

        public abstract Type DataType { get; }
    }

    public abstract class YeetColumn<T> : YeetColumn
    {
        public YeetColumn(Guid guid, string key) : base(guid, key)
        {
        }

        public T Value { get; set; }

        public override Type DataType => typeof(T);
    }

    public class YeetBooleanColumn : YeetColumn<bool>
    {
        YeetBooleanColumn() : this(Guid.Empty, null)
        {

        }
        public YeetBooleanColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetStringColumn : YeetColumn<string>
    {
        YeetStringColumn() : this(Guid.Empty, null)
        {

        }
        public YeetStringColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetIntColumn : YeetColumn<int>
    {
        YeetIntColumn() : this(Guid.Empty, null)
        {

        }
        public YeetIntColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDoubleColumn : YeetColumn<double>
    {
        YeetDoubleColumn() : this(Guid.Empty, null)
        {

        }
        public YeetDoubleColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetDateTimeColumn : YeetColumn<DateTime>
    {
        YeetDateTimeColumn() : this(Guid.Empty, null)
        {

        }
        public YeetDateTimeColumn(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
