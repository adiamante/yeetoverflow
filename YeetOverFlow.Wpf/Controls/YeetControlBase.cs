using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ControlzEx.Theming;

namespace YeetOverFlow.Wpf.Controls
{
    public class YeetControlBase : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            backingField = value;
            OnPropertyChanged(propertyname);
        }
        #endregion INotifyPropertyChanged

        #region Private Properties
        Theme _theme = ThemeManager.Current.GetTheme("Light.Blue");
        #endregion Private Properties

        #region Initialization
        public YeetControlBase()
        {
            this.Loaded += YeetControlBase_Loaded;
        }

        private void YeetControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (YeetWindow)Application.Current.MainWindow;
            #region Prevents Designer Error
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            #endregion Prevents Designer Error

            window.Settings["Window"]["Theme"]["Base"].PropertyChanged += YeetWindowThemPropertyChanged;
            window.Settings["Window"]["Theme"]["Accent"].PropertyChanged += YeetWindowThemPropertyChanged;
            ApplyTheme();
        }
        #endregion Initialization

        #region Events
        private void YeetWindowThemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplyTheme();
        }
        #endregion Events

        #region Methods
        private void ApplyTheme()
        {
            var window = (YeetWindow)Application.Current.MainWindow;
            var currentWindowTheme = window.Theme;

            if (_theme.Name != currentWindowTheme.Name)
            {
                ThemeManager.Current.ChangeTheme(this, this.Resources, currentWindowTheme);
                _theme = currentWindowTheme;
            }
        }
        #endregion Methods
    }
}
