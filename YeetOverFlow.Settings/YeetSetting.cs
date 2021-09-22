using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Settings
{
    public class YeetSetting : YeetKeyedItem
    {
        public Enum Icon { get; set; }
        public Enum Icon2 { get; set; }
        public YeetSetting() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSetting(Guid guid, string key) : base(guid, key)
        {
        }

        #region Indexer
        //This is here for transparency
        public virtual YeetSetting this[string key]
        {
            get
            {
                throw new InvalidOperationException("This type does not support indexing");
            }
            set
            {
                throw new InvalidOperationException("This type does not support indexing");
            }
        }
        #endregion Indexer

        public override string Kind => nameof(YeetSetting);
    }

    public class YeetSetting<T> : YeetSetting
    {
        public YeetSetting(Guid guid, string key) : base(guid, key)
        {
        }

        public T Value { get; set; }
    }

    //public static class YeetSettingExtensions
    //{
    //    public static void TryAddChildSetting(this YeetSetting setting, string key, YeetSetting child)
    //    {
    //        if (setting is YeetSettingList lst)
    //        {
    //            if (!lst.ContainsKey(key))
    //            {
    //                lst[key] = child;
    //            }
    //        }
    //        else
    //        {
    //            throw new InvalidOperationException($"Cannot add child to type {setting.GetType()}");
    //        }
    //    }
    //}
}
