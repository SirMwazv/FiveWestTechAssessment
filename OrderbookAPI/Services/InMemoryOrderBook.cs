using System;
using System.Collections.Generic;
using System.Globalization;
using OrderbookAPI.Models;

namespace OrderbookAPI.Services;

public class InMemoryOrderBook
{
    private SortedDictionary<decimal, List<Order>> _asks = new SortedDictionary<decimal, List<Order>>();

    // Sorted dictionary to maintain the orderbook for bids (sorted by highest price)
    private SortedDictionary<decimal, List<Order>> _bids =
        new SortedDictionary<decimal, List<Order>>(Comparer<decimal>.Create((x, y) =>
            y.CompareTo(x))); // Descending order


    /// <summary>
    /// Add updates to in memory order book
    /// </summary>
    /// <param name="orderBookUpdate">orderBookUpdate</param>
    public void UpdateOrderBook(OrderBookUpdate orderBookUpdate)
    {
        
        // Process Asks
        foreach (var ask in orderBookUpdate.data.Asks)
        {
            if (decimal.TryParse(ask.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
            {
                UpdatePriceLevel(_asks, price, ask.Orders);
            }
        }

        // Process Bids
        foreach (var bid in orderBookUpdate.data.Bids)
        {
            decimal price = decimal.Parse(bid.Price);
            UpdatePriceLevel(_bids, price, bid.Orders);
        }
    }

   /// <summary>
   ///  Update a price level in the orderbook (for both bids and asks)
   /// </summary>
   /// <param name="orderbook">orderbook</param>
   /// <param name="price">price</param>
   /// <param name="orders">orders</param>
    private void UpdatePriceLevel(SortedDictionary<decimal, List<Order>> orderbook, decimal price, List<Order> orders)
    {
        // Check if the orders list is empty or if all orders have a quantity of 0
        if (orders.Count == 0 || orders.TrueForAll(o => IsQuantityZero(o.quantity)))
        {
            orderbook.Remove(price);
        }
        else
        {
            // If the price level already exists, update it; otherwise, add a new entry
            if (!orderbook.ContainsKey(price))
            {
                orderbook[price] = new List<Order>();
            }

            // Update the list of orders at this price level
            orderbook[price] = orders;
        }
    }
   
   /// <summary>
   /// IsQuantityZero
   /// </summary>
   /// <param name="quantityString">quantityString</param>
   /// <returns>boolean</returns>
   private bool IsQuantityZero(string quantityString)
    {
        // Ensure the quantity string is not null or empty
        if (string.IsNullOrEmpty(quantityString))
        {
            Console.WriteLine("Quantity string is null or empty, treating as zero.");
            return true;
        }

        // Try to parse the quantity, treating invalid or unparsable values as zero
        if (decimal.TryParse(quantityString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity))
        {
            return quantity == 0;
        }
        else
        {
            Console.WriteLine($"Failed to parse quantity: {quantityString}, treating as zero.");
            return true; // Treat failed parsing as zero quantity
        }
    }

   /// <summary>
   /// GetBestAsk
   /// </summary>
   /// <returns>decimal</returns>
    public decimal? GetBestAsk()
    {
        if (_asks.Count == 0) return null;
        return _asks.Keys.Min(); // First key in SortedDictionary (smallest price)
    }

    /// <summary>
    /// GetBestBid
    /// </summary>
    /// <returns>decimal</returns>
    public decimal? GetBestBid()
    {
        if (_bids.Count == 0) return null;
        return _bids.Keys.Max(); // First key in SortedDictionary (largest price)
    }
}