using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace YeetOverFlow.Wpf.Converters
{
    public class RelayMultiConverter : MarkupExtension, IMultiValueConverter
    {
        static RelayMultiConverter _converter = new RelayMultiConverter();
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as object[];
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter;
        }
    }
}
