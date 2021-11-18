using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace YeetOverFlow.Wpf.Converters
{
    //https://stackoverflow.com/questions/1652341/wpf-trigger-based-on-object-type
    public class IsEnumTypeConverter : MarkupExtension, IValueConverter
    {
        static IsEnumTypeConverter _converter = new IsEnumTypeConverter();
        public object Convert(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            return value?.GetType().IsEnum ?? Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter;
        }
    }
}
