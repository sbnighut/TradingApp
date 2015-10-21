using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Trade.PriceService
{
    public class RandomWalkPriceService : IPriceService
    {
        private const double TickSize = 0.01;
        private const int ShouldUpdatePrices = 1;

        private readonly Prices[] _prices;
        private readonly Random _randGenerator;
        private readonly Timer _timer;

        public RandomWalkPriceService()
        {
            _randGenerator = new Random();
            _timer = new Timer(50);
            _timer.Elapsed += OnTimerHandler;
            _prices = new Prices[10];
        }

        public void Start()
        {
            lock (_prices)
            {
                if (_timer.Enabled)
                    throw new InvalidOperationException("Already started!");

                DoStart();
            }
        }

        private void DoStart()
        {
            for (int i = 0; i < 10; ++i)
            {
                var prices = new Prices();
                double mid = _randGenerator.NextDouble() * 100;

                prices.BidPx = Math.Max(0, mid - TickSize);
                prices.AskPx = Math.Max(0, mid + TickSize);
                prices.BidQty = (uint)_randGenerator.Next(1, 10) * 10;
                prices.AskQty = (uint)_randGenerator.Next(1, 10) * 10;
                prices.Volume = 0;

                _prices[i] = prices;
            }

            DispatchPrices();
            _timer.Start();
        }
        private void DispatchPrices()
        {
            if (NewPricesArrived == null)
                return;

            for (int i = 0; i < 10; ++i)
                NewPricesArrived(this, (uint)i, _prices[i]);
        }


        private void OnTimerHandler(object source, ElapsedEventArgs e)
        {
            UpdatePrices();
        }

        private void UpdatePrices()
        {
            for (int i = 0; i < 10; ++i)
            {
                if (_randGenerator.Next(0, 2) != ShouldUpdatePrices)
                    continue;

                Prices prices = _prices[i];
                bool walkUp = Convert.ToBoolean(_randGenerator.Next(0, 2));

                double tickToUse = walkUp ? TickSize : -1 * TickSize;

                prices.BidPx += tickToUse;
                prices.AskPx += tickToUse;

                prices.BidQty = (uint)_randGenerator.Next(1, 10) * 10;
                prices.AskQty = (uint)_randGenerator.Next(1, 10) * 10;
                prices.Volume += (uint)_randGenerator.Next(1, 10) * 10;

                if (NewPricesArrived != null)
                    NewPricesArrived(this, (uint)i, prices);
            }
        }

        public void Stop()
        {
            lock (_prices)
            {
                if (!IsStarted)
                    throw new InvalidOperationException("Not started!");

                _timer.Stop();
            }
        }
        
        public bool IsStarted
        {
            get { return _timer.Enabled; }
        }

        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public event PriceUpdateDelegate NewPricesArrived;
    }
}
