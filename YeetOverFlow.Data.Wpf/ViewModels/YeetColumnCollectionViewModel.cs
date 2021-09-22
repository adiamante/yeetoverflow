using System;
using YeetOverFlow.Core;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetColumnCollectionViewModel : YeetObservableKeyedList<YeetColumnViewModel>
    {
        public YeetColumnCollectionViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetColumnCollectionViewModel(Guid guid) : base(guid)
        {
        }
    }
}
