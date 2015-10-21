using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Trade.WPFPriceTestDriver
{
    class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// This converter is used to change the background color of the datagrid blocks based on the previous values of the
    /// price held by the the block(either bid price ,or ask price)
    /// Two instances of this class would be created in the XAML for tracking bid and ask prices respectively
    /// </summary>
    public class PriceValuetoColorConverter : IMultiValueConverter
    {
        double[] previousBidorAskPrices = new double[10];
        public PriceValuetoColorConverter()
        {
            for (int i = 0; i < 10; i++)
            {
                previousBidorAskPrices[i] = 0.0;
            }
        }
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] != DependencyProperty.UnsetValue && values[0] != DependencyProperty.UnsetValue)
            {
                int instrumentId = (int)values[0];
                double newPrice = (double)values[1];
                if (newPrice < previousBidorAskPrices[instrumentId])
                {
                    previousBidorAskPrices[instrumentId] = newPrice;
                    return Brushes.Red;
                }
                if (newPrice > previousBidorAskPrices[instrumentId])
                {
                    previousBidorAskPrices[instrumentId] = newPrice;
                    return Brushes.Blue;
                }
                return Brushes.LightGray;
            }
            return Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return targetTypes.ToArray();
        }
    }
}
