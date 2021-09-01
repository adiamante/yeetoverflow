using System;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Markup;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Wpf.Controls
{
    public class YeetWindow : MetroWindow, INotifyPropertyChanged
    {
        #region Properties
        #region ViewModel
        private static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(YeetWindowViewModel), typeof(YeetWindow));

        public YeetWindowViewModel ViewModel
        {
            get
            {
                var settings = (YeetWindowViewModel)GetValue(ViewModelProperty);
                return settings;
            }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion ViewModel
        #region Settings
        public YeetSettingLibraryViewModel Settings { get => ViewModel.Settings; }
        #endregion Settings
        #region CommandManager
        public YeetCommandManagerViewModel CommandManager { get => ViewModel.CommandManager; }
        #endregion CommandManager
        #region Theme
        public Theme Theme
        {
            get
            {
                String myBase = Settings["Window"]["Theme"]["Base"].GetValue<String>();
                String myAccent = Settings["Window"]["Theme"]["Accent"].GetValue<String>();
              
                Theme theme = ThemeManager.Current.GetTheme($"{myBase}.{myAccent}");
                return theme;
            }
        }
        #endregion Theme
        #endregion Properties

        #region Initialization
        static YeetWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YeetWindow), new FrameworkPropertyMetadata(typeof(YeetWindow)));
        }

        public YeetWindow(YeetWindowViewModel vm) : this()
        {
            ViewModel = vm;
        }
        #endregion Initialization

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

        #region Initialization
        public YeetWindow()
        {
            Loaded += YeetWindow_Loaded;

            ResourceDictionary rdYeetWindow = new ResourceDictionary();
            rdYeetWindow.Source = new Uri("/YeetOverFlow.Wpf;component/Controls/YeetWindow.xaml", UriKind.RelativeOrAbsolute);
            this.Resources.MergedDictionaries.Add(rdYeetWindow);
        }

        private void YeetWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings != null)
            {
                Settings["Window"]["Theme"]["Base"].PropertyChanged += WindowSettingCollection_ThemePropertyChanged;
                Settings["Window"]["Theme"]["Accent"].PropertyChanged += WindowSettingCollection_ThemePropertyChanged;
                WindowSettingCollection_ThemePropertyChanged(this, new PropertyChangedEventArgs("Value"));

                InputBindings.Add(new KeyBinding() { Modifiers = ModifierKeys.Control, Key = Key.Z, Command = CommandManager.UndoCommand });
                InputBindings.Add(new KeyBinding() { Modifiers = ModifierKeys.Control, Key = Key.Y, Command = CommandManager.RedoCommand });
            }
        }
        #endregion Initialization

        #region Events
        private void WindowSettingCollection_ThemePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                Theme theme = Theme;

                #region Add Custom Brushes
                //Available Resources https://mahapps.com/docs/themes/thememanager
                String strGradientXaml =
                    @"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                            xmlns:options=""http://schemas.microsoft.com/winfx/2006/xaml/presentation/options""
                            EndPoint =""0.5,1"" StartPoint=""0.5,0""
                            options:Freeze=""True"">
                        <GradientStop Color=""{DynamicResource MahApps.Colors.Accent}"" Offset=""0""/>
                        <GradientStop Color=""{DynamicResource MahApps.Colors.Accent2}"" Offset=""0.01""/>
                        <GradientStop Color=""{DynamicResource MahApps.Colors.Accent3}"" Offset=""0.2""/>
                        <GradientStop Color=""{DynamicResource MahApps.Colors.ThemeBackground}"" Offset=""0.21""/>
                    </LinearGradientBrush>";
                XmlReader xmlReader = XmlReader.Create(new StringReader(strGradientXaml));
                LinearGradientBrush linearGradientBrush = (LinearGradientBrush)XamlReader.Load(xmlReader);

                //theme.Resources["MahApps.Brushes.ThemeBackground"] = linearGradientBrush;
                theme.Resources["MahApps.Brushes.Control.Background"] = linearGradientBrush;
                //theme.Resources["MahApps.Brushes.Window.Background"] = linearGradientBrush;
                theme.Resources["MahApps.Brushes.Menu.Background"] = linearGradientBrush;
                theme.Resources["MahApps.Brushes.ContextMenu.Background"] = linearGradientBrush;
                theme.Resources["MahApps.Brushes.SubMenu.Background"] = linearGradientBrush;
                theme.Resources["MahApps.Brushes.MenuItem.Background"] = linearGradientBrush;
                #endregion Add Custom Brushes

                ThemeManager.Current.ChangeTheme(this, this.Resources, theme);
            }
        }
        #endregion Events
    }
}
