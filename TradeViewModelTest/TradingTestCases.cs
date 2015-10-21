using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trade.WPFPriceTestDriver;
using System.Windows.Media;
using System.Globalization;
using Trade.PriceService;
using System.Windows.Controls;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TradeViewModelTest
{
    /// <summary>
    /// This test case checks whether the colors are changing according to requirements specification or not
    /// Blue if the nprice is increased, Red if decreased or else Neutral(LightGray)
    /// </summary>
    [TestClass]
    public class PriceValuetoColorConversionTest
    {
        object[] values = new object[2];
        TradeViewModel avm;
        PriceValuetoColorConverter converter;

        [TestInitialize]
        public void Initialize()
        {
            avm = new TradeViewModel();
            converter = new PriceValuetoColorConverter();
        }

        [TestMethod]
        public void ColorChangeTest()
        {
            double[] bidPrices = new double[10];
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                bidPrices[i] = rand.NextDouble() * 100;
            }
            for (int i = 0; i < 20; i++)
            {
                bidPrices[i % 10] = bidPrices[i % 10] + .01;
                values[0] = i % 10;
                values[1] = bidPrices[i % 10];
                Assert.AreEqual(Brushes.Blue, converter.Convert(values, typeof(Brush), null, CultureInfo.CurrentCulture));
            }

            for (int i = 0; i < 20; i++)
            {
                bidPrices[i % 10] = bidPrices[i % 10] - .01;
                values[0] = i % 10;
                values[1] = bidPrices[i % 10];
                Assert.AreEqual(Brushes.Red, converter.Convert(values, typeof(Brush), null, CultureInfo.CurrentCulture));
            }
        }
    }
    
    /// <summary>
    /// This test case basically verifies whether the value in the ViewModel and the UI are the same or not
    /// </summary>
    [TestClass]
    public class PriceValueUIUpdateTest
    {
        TradeViewModel avm;
        MainWindow testView;

        [TestInitialize]
        public void Initialize()
        {
            avm = new TradeViewModel();
            testView = new MainWindow();
            testView.DataContext = avm;
            avm.StartTradingCommand.Execute();
            Thread.Sleep(1200);
            avm.StopTradingCommand.Execute();
            var gridElement = testView.FindName("tradeGrid");
            ObservableCollection<InstrumentPrices> itemSource = (ObservableCollection<InstrumentPrices>)((DataGrid)gridElement).ItemsSource;
            Debug.WriteLine("Value from ViewModel:{0},\n Value from View: {1}", avm.TradeItemsCollection[0].AskPxAfterFrequencyInterval, itemSource[0].AskPxAfterFrequencyInterval);
            Console.WriteLine("Value from ViewModel:{0},\n Value from View: {1}", avm.TradeItemsCollection[0].AskPxAfterFrequencyInterval, itemSource[0].AskPxAfterFrequencyInterval);
            Assert.AreEqual(avm.TradeItemsCollection[0].AskPxAfterFrequencyInterval,itemSource[0].AskPxAfterFrequencyInterval);
        }

        [TestMethod]
        public void PropertyUIBindingTest()
        {
            
        }

        public object ObservableCollection { get; set; }
    }
}
