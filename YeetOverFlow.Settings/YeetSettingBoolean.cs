using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingBoolean : YeetSetting<bool>
    {
        public override string Kind => nameof(YeetSettingBoolean);
        YeetSettingBoolean() : this(Guid.NewGuid(), null)
        {

        }
        public YeetSettingBoolean(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
