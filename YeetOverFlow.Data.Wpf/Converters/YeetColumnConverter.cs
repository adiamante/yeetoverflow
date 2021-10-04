using System;
using System.Windows.Data;
using System.Windows.Markup;
using YeetOverFlow.Data.Wpf.Controls;

namespace YeetOverFlow.Data.Wpf.Converters
{
    public class YeetColumnConverter : MarkupExtension, IMultiValueConverter
    {
        static YeetColumnConverter _converter = new YeetColumnConverter();
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is YeetTableControl && values[1] is String)
            {
                YeetTableControl ytc = values[0] as YeetTableControl;
                String colName = values[1].ToString();

                return ytc.Table.Columns[colName];
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter;
        }
    }
}
