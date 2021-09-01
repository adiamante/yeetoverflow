using System;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetSettingStringOptionViewModel : YeetSettingOptionViewModel<String>
    {
        public YeetSettingStringOptionViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetSettingStringOptionViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
