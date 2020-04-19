using System;
using System.Globalization;
using System.Windows.Data;

namespace Chart.Legends {
    public class MultiplyConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double v = System.Convert.ToDouble(value);
            double k = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return v * k;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
