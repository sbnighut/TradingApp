using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trade.PriceService
{
    public delegate void PriceUpdateDelegate(IPriceService sender, uint instrumentID, IPrices prices);

    public interface IPriceService
    {
        void Start();
        void Stop();
        bool IsStarted { get; }

        event PriceUpdateDelegate NewPricesArrived;
    }
}
