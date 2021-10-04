using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace YeetOverFlow.Wpf.Converters
{
    public enum BoolToVisibilityMode
    {
        NORMAL,
        INVERSE
    }
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        //Key pattern should be a cartesian product of all available public properties
        static ConcurrentDictionary<String, BoolToVisibilityConverter> _converters = new ConcurrentDictionary<string, BoolToVisibilityConverter>();
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;
        public BoolToVisibilityMode Mode { get; set; } = BoolToVisibilityMode.NORMAL;

        public BoolToVisibilityConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = System.Convert.ToBoolean(value);
            if (Mode == BoolToVisibilityMode.INVERSE) val = !val;
            return val ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = TrueValue.Equals(value) ? true : false;
            if (Mode == BoolToVisibilityMode.INVERSE) val = !val;
            return val;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var anonKey = new { TrueValue = TrueValue, FalseValue = FalseValue, Mode = Mode };
            String key = anonKey.ToString();
            if (!_converters.ContainsKey(key))
            {
                _converters.TryAdd(key, this);
            }
            return _converters[key];
        }
    }
}
