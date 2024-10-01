using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class Order
    {
        public string orderId { get; set; }
        public string quantity { get; set; }
    }
}