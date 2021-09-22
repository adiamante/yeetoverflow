using System;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetTableViewModel : YeetDataViewModel
    {
        public YeetColumnCollectionViewModel Columns { get; set; } = new YeetColumnCollectionViewModel();
        public YeetRowCollectionViewModel Rows { get; set; } = new YeetRowCollectionViewModel();

        public YeetTableViewModel() : this(Guid.NewGuid(), null)
        {

        }

        public YeetTableViewModel(Guid guid, string key) : base(guid, key)
        {
        }

        public void Init()
        {
            Columns.Init();
            Rows.Init();
        }
    }
}
