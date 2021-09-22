using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
