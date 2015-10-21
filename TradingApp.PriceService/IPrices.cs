using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trade.PriceService
{
    public interface IPrices
    {
        double BidPx { get; }
        uint BidQty { get; }
        double AskPx { get; }
        uint AskQty { get; }
        uint Volume { get; }
    }
}
