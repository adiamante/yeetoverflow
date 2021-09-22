using System;
using YeetOverFlow.Core;

namespace YeetOverFlow.Wpf.ViewModels
{
    public class YeetKeyedItemViewModel : YeetItemViewModelBaseExtended, IKeyedItem
    {
        protected string _key;

        #region Key
        public String Key
        {
            get { return _key; }
        }
        #endregion Key

        public YeetKeyedItemViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetKeyedItemViewModel(Guid guid, string key) : base(guid)
        {
            _key = key;
        }
    }
}
