using System.Windows;
using System.Windows.Input;
using YeetOverFlow.Wpf.Commands;
using YeetOverFlow.Wpf.Ui;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Wpf.Demo.ViewModels
{
    public class SearchTextBoxViewModel : YeetItemViewModelBase
    {
        ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand =
                    new RelayCommand<string, FilterMode>((str, fm) =>
                    {
                        MessageBox.Show(fm + ": " + str);
                    }));
            }
        }
    }
}
