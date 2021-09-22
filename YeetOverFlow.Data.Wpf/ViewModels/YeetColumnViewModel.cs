using System;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetColumnViewModel : YeetKeyedItemViewModel
    {
        YeetColumnViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetColumnViewModel(Guid guid, string key) : base(guid, key)
        {
        }
    }
}
