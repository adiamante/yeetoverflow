using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingOption<T> : YeetSetting<T>
    {
        YeetSettingOption() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingOption(Guid guid, string key) : base(guid, key)
        {
        }

        public T[] Options { get; set; }
    }
}
