using System;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowViewModel : YeetObservableKeyedList<YeetCellViewModel>
    {
        public YeetRowViewModel() : this(Guid.NewGuid())
        {

        }

        public YeetRowViewModel(Guid guid) : base(guid)
        {

        }
    }
}
