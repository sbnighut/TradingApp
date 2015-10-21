using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trade.PriceService
{
    public sealed class Prices : IPrices
    {
        public double BidPx { get; set; }

        public uint BidQty { get; set; }

        public double AskPx { get; set; }

        public uint AskQty { get; set; }

        public uint Volume { get; set; }
    }
}