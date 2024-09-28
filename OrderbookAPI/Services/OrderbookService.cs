using System;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Collections.Generic;

namespace OrderbookAPI.Services
{
    public class OrderbookService
    {
        private Dictionary<decimal, decimal> orderbookBids = new Dictionary<decimal, decimal>(); // Bid: price -> quantity
        private Dictionary<decimal, decimal> orderbookAsks = new Dictionary<decimal, decimal>(); // Ask: price -> quantity

        public OrderbookService()
        {
            ConnectToVALR();
        }

        public void ConnectToVALR()
        {
            using (var ws = new WebSocket("wss://api.valr.com/v1/ws/trade"))
            {
                ws.OnMessage += (sender, e) =>
                {
                    var message = e.Data;
                    ProcessOrderbookUpdate(message);
                };

                ws.Connect();
                ws.Send("{\"type\":\"SUBSCRIBE\",\"channels\":[\"FULL_ORDERBOOK_UPDATE\"]}");
            }
        }

        private void ProcessOrderbookUpdate(string message)
        {
            // Deserialize the message and update the orderbookBids and orderbookAsks
            Console.WriteLine($"Received orderbook update: {message}");

            // Implement the parsing of the message and update the in-memory orderbook dictionaries.
            // Assume message contains JSON object with the structure:
            // { "bids": [{ "price": "x", "quantity": "y" }], "asks": [{ "price": "a", "quantity": "b" }] }
        }

        public decimal GetZARPriceForUSDT(decimal usdtQuantity)
        {
            decimal totalZAR = 0;
            decimal remainingUSDT = usdtQuantity;

            // Match the best asks in the orderbook
            foreach (var ask in orderbookAsks)
            {
                decimal price = ask.Key;
                decimal quantity = ask.Value;

                if (remainingUSDT <= quantity)
                {
                    totalZAR += remainingUSDT * price;
                    break;
                }
                else
                {
                    totalZAR += quantity * price;
                    remainingUSDT -= quantity;
                }
            }

            return totalZAR;
        }
    }
}
