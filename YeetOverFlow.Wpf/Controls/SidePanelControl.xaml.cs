using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using YeetOverFlow.Wpf.Ui;

namespace YeetOverFlow.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for SidePanelControl.xaml
    /// </summary>
    [ContentProperty(nameof(Items))]
    public partial class SidePanelControl : YeetControlBase
    {
        //https://stackoverflow.com/questions/52352519/populating-a-collection-in-xaml
        public IList Items { get; } = new List<TabItem>();

        #region BottomContentTemplate
        public static readonly DependencyProperty BottomContentTemplateProperty =
            DependencyProperty.Register("BottomContentTemplate", typeof(DataTemplate), typeof(SidePanelControl));

        public DataTemplate BottomContentTemplate
        {
            get { return (DataTemplate)GetValue(BottomContentTemplateProperty); }
            set { SetValue(BottomContentTemplateProperty, value); }
        }
        #endregion BottomContentTemplate

        public SidePanelControl()
        {
            InitializeComponent();
        }

        private void ToggleButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var tabControl = DependencyObjectHelper.TryFindParent<TabControl>(toggleButton);

            if (toggleButton.IsChecked.Value && tabControl.SelectedIndex != -1)
            {
                tabControl.SelectedIndex = -1;
                toggleButton.IsChecked = false;
            }
        }
    }
}
