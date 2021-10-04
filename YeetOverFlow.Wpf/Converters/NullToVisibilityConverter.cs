﻿using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace YeetOverFlow.Wpf.Converters
{
    public class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        //Key pattern should be a cartesian product of all available public properties
        static ConcurrentDictionary<String, NullToVisibilityConverter> _converters = new ConcurrentDictionary<string, NullToVisibilityConverter>();
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        public NullToVisibilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = value == null;
            return val ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var anonKey = new { TrueValue = TrueValue, FalseValue = FalseValue };
            String key = anonKey.ToString();
            if (!_converters.ContainsKey(key))
            {
                _converters.TryAdd(key, this);
            }
            return _converters[key];
        }
    }
}
