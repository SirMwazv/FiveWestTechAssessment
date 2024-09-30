using System;
using System.Collections.Generic;
using OrderbookAPI.Models;

namespace OrderbookAPI.Services;

public class InMemoryOrderBook
{
    // Sorted dictionary to maintain the orderbook for asks (sorted by lowest price)
    private SortedDictionary<decimal, List<Order>> _asks = new SortedDictionary<decimal, List<Order>>();

    // Sorted dictionary to maintain the orderbook for bids (sorted by highest price)
    private SortedDictionary<decimal, List<Order>> _bids =
        new SortedDictionary<decimal, List<Order>>(Comparer<decimal>.Create((x, y) =>
            y.CompareTo(x))); // Descending order

    // Process a new orderbook update (insert into memory)
    public void UpdateOrderBook(OrderBookUpdate orderBookUpdate)
    {
        //Console.WriteLine("Updating in-memory orderbook...");

        // Process Asks
        foreach (var ask in orderBookUpdate.Data.Asks)
        {
            decimal price = decimal.Parse(ask.Price);
            UpdatePriceLevel(_asks, price, ask.Orders);
        }

        // Process Bids
        foreach (var bid in orderBookUpdate.Data.Bids)
        {
            decimal price = decimal.Parse(bid.Price);
            UpdatePriceLevel(_bids, price, bid.Orders);
        }

        //Console.WriteLine("Orderbook updated.");
    }

    // Update a price level in the orderbook (for both bids and asks)
    private void UpdatePriceLevel(SortedDictionary<decimal, List<Order>> orderbook, decimal price,
        List<Order> orders)
    {
        if (orders.Count == 0 || orders.TrueForAll(o => decimal.Parse(o.Quantity) == 0))
        {
            // If all orders at this price level have quantity 0, remove the price level
            orderbook.Remove(price);
        }
        else
        {
            // If the price level already exists, update it, otherwise add a new entry
            if (!orderbook.ContainsKey(price))
            {
                orderbook[price] = new List<Order>();
            }

            // Replace existing orders at this price level
            orderbook[price] = orders;
        }
    }

    // Retrieve the best ask (lowest price)
    public decimal? GetBestAsk()
    {
        if (_asks.Count == 0) return null;
        return _asks.Keys.Min(); // First key in SortedDictionary (smallest price)
    }

    // Retrieve the best bid (highest price)
    public decimal? GetBestBid()
    {
        if (_bids.Count == 0) return null;
        return _bids.Keys.Max(); // First key in SortedDictionary (largest price)
    }
}