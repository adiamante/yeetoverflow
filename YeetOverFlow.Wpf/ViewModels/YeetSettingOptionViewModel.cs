using System;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetSettingOptionViewModel<T> : YeetSettingViewModel<T>
    {
        YeetSettingOptionViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingOptionViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        public T[] Options { get; set; }
    }
}
