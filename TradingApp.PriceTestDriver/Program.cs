using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.PriceService;

namespace Trade.PriceTestDriver
{
    static class Program
    {
        static void PriceUpdateHandler(IPriceService sender, uint instrumentID, IPrices prices)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine("{0}: BidPx: {1:#.##}, AskPx: {2:#.##}, BidQty: {3}, AskQty: {4}, Vol: {5}",
                instrumentID, prices.BidPx, prices.AskPx, prices.BidQty, prices.AskQty, prices.Volume);
        }

        static void Main(string[] args)
        {
            var priceService = new RandomWalkPriceService();
            priceService.NewPricesArrived += PriceUpdateHandler;
            priceService.Start();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.D)
                {
                    Console.WriteLine("Done");
                    break;
                }
            }

            priceService.Stop();
        }
    }
}
