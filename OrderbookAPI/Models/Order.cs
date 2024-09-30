using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string Quantity { get; set; }
    }
}