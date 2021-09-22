using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingStringOption : YeetSettingOption<String>
    {
        YeetSettingStringOption() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingStringOption(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
