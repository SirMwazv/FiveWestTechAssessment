using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class OrderBookUpdate
    {
        public string type { get; set; }
        public string currencyPairSymbol { get; set; }
        public OrderBookData data { get; set; }
    }    
}