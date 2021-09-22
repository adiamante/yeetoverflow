using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingString : YeetSetting<string>
    {
        public override string Kind => nameof(YeetSettingString);
        YeetSettingString() : this(Guid.NewGuid(), null)
        {

        }
        public YeetSettingString(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
