using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingInt : YeetSetting<int>
    {
        public override string Kind => nameof(YeetSettingInt);
        YeetSettingInt() : this(Guid.NewGuid(), null)
        {

        }
        public YeetSettingInt(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
