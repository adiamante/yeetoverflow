using System;
using YeetOverFlow.Settings;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetSettingViewModel : YeetKeyedItemViewModel
    {
        public Enum Icon { get; set; }
        public Enum Icon2 { get; set; }

        #region Indexer
        //This is here for transparency
        public virtual YeetSettingViewModel this[string key]
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

        YeetSettingViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        protected virtual void SetKey(String key, YeetSettingViewModel setting)
        {
            setting._key = key;
            OnPropertyChanged(nameof(Key));
        }

        //Would have preferred test to be a generic method but Automapper setup crashes with generic methods in nongeneric classes
        internal virtual object GetValue()
        {
            throw new InvalidOperationException("This type does not support returning a value");
        }
        internal virtual void SetValue(object val)
        {
            throw new InvalidOperationException("This type does not support setting a value");
        }
    }

    public class YeetSettingViewModel<T> : YeetSettingViewModel
    {
        T _value;

        YeetSettingViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        #region Value
        public T Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }

        internal override object GetValue()
        {
            return _value;
        }

        internal override void SetValue(object val)
        {
            Value = (T)val;
        }
        #endregion Value
    }

    public class YeetSettingBooleanViewModel : YeetSettingViewModel<bool>
    {
        public override string Kind => nameof(YeetSettingBoolean);

        public YeetSettingBooleanViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingBooleanViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    public class YeetSettingStringViewModel : YeetSettingViewModel<string>
    {
        public override string Kind => nameof(YeetSettingString);
        public YeetSettingStringViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingStringViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }

    //This only exists because Automapper crashes when a generic method exists in a class that is not generic
    public static class YeetSettingHelper
    {
        public static T GetValue<T>(this YeetSettingViewModel vm)
        {
            return (T)vm.GetValue();
        }

        public static void SetValue<T>(this YeetSettingViewModel vm, T val)
        {
            vm.SetValue(val);
        }

        public static void TryAddChildSetting(this YeetSettingViewModel setting, String key, YeetSettingViewModel child)
        {
            if (setting is YeetSettingListViewModel list)
            {
                if (!list.ContainsKey(key))
                {
                    list[key] = child;
                }
            }
            else
            {
                throw new InvalidOperationException($"Cannot add child to type {setting.GetType()}");
            }
        }
    }
}
