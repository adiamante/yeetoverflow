﻿using System;

namespace YeetOverFlow.Settings
{
    public class YeetSettingDateTime : YeetSetting<DateTime>
    {
        public override string Kind => nameof(YeetSettingDateTime);
        YeetSettingDateTime() : this(Guid.NewGuid(), null)
        {

        }
        public YeetSettingDateTime(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
