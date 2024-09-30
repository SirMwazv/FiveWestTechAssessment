using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class AskBid
    {
        public string Price { get; set; }
        public List<Order> Orders { get; set; }
    }
}
