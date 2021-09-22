using System;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetDataViewModel : YeetKeyedItemViewModel
    {
        public YeetDataViewModel() : base()
        {

        }

        public YeetDataViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        protected virtual void SetKey(String key, YeetDataViewModel data)
        {
            data._key = key;
            OnPropertyChanged(nameof(Key));
        }
    }
}
