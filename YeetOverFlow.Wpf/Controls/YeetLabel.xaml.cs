﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace YeetOverFlow.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for YeetLabel.xaml
    /// </summary>
    public partial class YeetLabel : YeetControlBase
    {
        #region Kind
        private static readonly DependencyProperty KindProperty =
        DependencyProperty.Register("Kind", typeof(Enum), typeof(YeetLabel));

        public Enum Kind
        {
            get { return (Enum)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
        #endregion Kind
        #region Text
        public static DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(String),
                typeof(YeetLabel));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion Text
        #region TextMaxWidth
        public static DependencyProperty TextMaxWidthProperty =
            DependencyProperty.Register(
                "TextMaxWidth",
                typeof(Double),
                typeof(YeetLabel),
                new PropertyMetadata(Double.MaxValue));

        public Double TextMaxWidth
        {
            get { return (Double)GetValue(TextMaxWidthProperty); }
            set { SetValue(TextMaxWidthProperty, value); }
        }
        #endregion TextMaxWidth
        #region TextWrapping
        public static DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
                "TextWrapping",
                typeof(TextWrapping),
                typeof(YeetLabel));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        #endregion TextWrapping
        #region TextDock (Lost this to enable Grid.SharedSizeScope)
        //public static DependencyProperty TextDockProperty =
        //    DependencyProperty.Register(
        //        "TextDock",
        //        typeof(Dock),
        //        typeof(YeetLabel),
        //        new PropertyMetadata(Dock.Right));

        //public Dock TextDock
        //{
        //    get { return (Dock)GetValue(TextDockProperty); }
        //    set { SetValue(TextDockProperty, value); }
        //}
        #endregion TextDock
        #region TextMargin
        public static DependencyProperty TextMarginProperty =
            DependencyProperty.Register(
                "TextMargin",
                typeof(Thickness),
                typeof(YeetLabel),
                new PropertyMetadata(new Thickness(3, 0, 0, 0)));

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }
        #endregion TextMargin
        #region IconWidth
        public static DependencyProperty IconWidthProperty =
            DependencyProperty.Register(
                "IconWidth",
                typeof(Double),
                typeof(YeetLabel),
                new PropertyMetadata(20.00));

        public Double IconWidth
        {
            get { return (Double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        #endregion IconWidth
        #region IconHeight
        public static DependencyProperty IconHeightProperty =
            DependencyProperty.Register(
                "IconHeight",
                typeof(Double),
                typeof(YeetLabel),
                new PropertyMetadata(20.00));

        public Double IconHeight
        {
            get { return (Double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        #endregion IconHeight
        #region ShowText
        public static DependencyProperty ShowTextProperty =
            DependencyProperty.Register(
                "ShowText",
                typeof(Boolean),
                typeof(YeetLabel),
                new PropertyMetadata(true));

        public Boolean ShowText
        {
            get { return (Boolean)GetValue(ShowTextProperty) && !String.IsNullOrEmpty(Text); }
            set { SetValue(ShowTextProperty, value); }
        }
        #endregion ShowText
        #region ShowIcon
        public static DependencyProperty ShowIconProperty =
            DependencyProperty.Register(
                "ShowIcon",
                typeof(Boolean),
                typeof(YeetLabel),
                new PropertyMetadata(true));

        public Boolean ShowIcon
        {
            get { return (Boolean)GetValue(ShowIconProperty) && Kind != null; }
            set { SetValue(ShowIconProperty, value); }
        }
        #endregion ShowIcon
        #region ContentMargin
        public static DependencyProperty ContentMarginProperty =
            DependencyProperty.Register(
                "ContentMargin",
                typeof(Thickness),
                typeof(YeetLabel),
                new PropertyMetadata(new Thickness(3, 0, 0, 0)));

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }
        #endregion ContentMargin

        #region Kind2
        private static readonly DependencyProperty Kind2Property =
        DependencyProperty.Register("Kind2", typeof(Enum), typeof(YeetLabel));

        public Enum Kind2
        {
            get { return (Enum)GetValue(Kind2Property); }
            set { SetValue(Kind2Property, value); }
        }
        #endregion Kind2
        #region SecondaryIconWidth
        public static DependencyProperty SecondaryIconWidthProperty =
            DependencyProperty.Register(
                "SecondaryIconWidth",
                typeof(Double),
                typeof(YeetLabel),
                new PropertyMetadata(10.00));

        public Double SecondaryIconWidth
        {
            get { return (Double)GetValue(SecondaryIconWidthProperty); }
            set { SetValue(SecondaryIconWidthProperty, value); }
        }
        #endregion SecondaryIconWidth
        #region SecondaryIconHeight
        public static DependencyProperty SecondaryIconHeightProperty =
            DependencyProperty.Register(
                "SecondaryIconHeight",
                typeof(Double),
                typeof(YeetLabel),
                new PropertyMetadata(10.00));

        public Double SecondaryIconHeight
        {
            get { return (Double)GetValue(SecondaryIconHeightProperty); }
            set { SetValue(SecondaryIconHeightProperty, value); }
        }
        #endregion SecondaryIconHeight
        #region SecondaryIconBackground
        public static DependencyProperty SecondaryIconBackgroundProperty =
            DependencyProperty.Register(
                "SecondaryIconBackground",
                typeof(SolidColorBrush),
                typeof(YeetLabel));

        public SolidColorBrush SecondaryIconBackground
        {
            get { return (SolidColorBrush)GetValue(SecondaryIconBackgroundProperty); }
            set { SetValue(SecondaryIconBackgroundProperty, value); }
        }
        #endregion SecondaryIconBackground
        #region SecondaryIconVerticalAlignment
        public static DependencyProperty SecondaryIconVerticalAlignmentProperty =
            DependencyProperty.Register(
                "SecondaryIconVerticalAlignment",
                typeof(VerticalAlignment),
                typeof(YeetLabel),
                new FrameworkPropertyMetadata(VerticalAlignment.Bottom));
        public VerticalAlignment SecondaryIconVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(SecondaryIconVerticalAlignmentProperty); }
            set
            {
                SetValue(SecondaryIconVerticalAlignmentProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SecondaryIconVerticalAlignment
        #region SecondaryIconHorizontalAlignment
        public static DependencyProperty SecondaryIconHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "SecondaryIconHorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(YeetLabel),
                new FrameworkPropertyMetadata(HorizontalAlignment.Right));

        public HorizontalAlignment SecondaryIconHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(SecondaryIconHorizontalAlignmentProperty); }
            set
            {
                SetValue(SecondaryIconHorizontalAlignmentProperty, value);
                OnPropertyChanged();
            }
        }
        #endregion SecondaryIconHorizontalAlignment

        public YeetLabel()
        {
            InitializeComponent();
        }
    }
}
