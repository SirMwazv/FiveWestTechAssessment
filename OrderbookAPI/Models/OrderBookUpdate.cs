using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class OrderBookUpdate
    {
        public string Type { get; set; }
        public string CurrencyPairSymbol { get; set; }
        public OrderBookData Data { get; set; }
    }    
}