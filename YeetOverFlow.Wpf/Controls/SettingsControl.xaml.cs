using System;
using System.Windows;
using System.Windows.Input;
using YeetOverFlow.Wpf.ViewModels;

namespace YeetOverFlow.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : YeetControlBase
    {
        #region Settings
        private static readonly DependencyProperty SettingsProperty =
        DependencyProperty.Register("Settings", typeof(YeetSettingLibraryViewModel), typeof(SettingsControl));

        public YeetSettingLibraryViewModel Settings
        {
            get { return (YeetSettingLibraryViewModel)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }
        #endregion Settings
        #region Save
        public static readonly RoutedEvent SaveEvent =
            EventManager.RegisterRoutedEvent(
            "Save",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SettingsControl));

        public event RoutedEventHandler Save
        {
            add { AddHandler(SaveEvent, value); }
            remove { RemoveHandler(SaveEvent, value); }
        }
        #endregion Save
        #region ShowSaveButton
        public static DependencyProperty ShowSaveButtonProperty =
            DependencyProperty.Register(
                "ShowSaveButton",
                typeof(Boolean),
                typeof(SettingsControl),
                new PropertyMetadata(false));

        public Boolean ShowSaveButton
        {
            get { return (Boolean)GetValue(ShowSaveButtonProperty); }
            set
            {
                SetValue(ShowSaveButtonProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion ShowSaveButton
        #region SaveButtonVerticalAlignment
        public static DependencyProperty SaveButtonVerticalAlignmentProperty =
            DependencyProperty.Register(
                "SaveButtonVerticalAlignment",
                typeof(VerticalAlignment),
                typeof(SettingsControl));
        public VerticalAlignment SaveButtonVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(SaveButtonVerticalAlignmentProperty); }
            set
            {
                SetValue(SaveButtonVerticalAlignmentProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SaveButtonVerticalAlignment
        #region SaveButtonHorizontalAlignment
        public static DependencyProperty SaveButtonHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "SaveButtonHorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(SettingsControl));

        public HorizontalAlignment SaveButtonHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(SaveButtonHorizontalAlignmentProperty); }
            set
            {
                SetValue(SaveButtonHorizontalAlignmentProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SaveButtonHorizontalAlignment

        public SettingsControl()
        {
            InitializeComponent();
        }
    }
}
