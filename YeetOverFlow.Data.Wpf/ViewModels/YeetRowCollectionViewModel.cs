using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Data.Wpf.ViewModels
{
    public class YeetRowCollectionViewModel : YeetObservableList<YeetRowViewModel>
    {
        public override void Init()
        {
            base.Init();

            foreach (var child in _children)
            {
                child.Init();
            }
        }
    }
}
