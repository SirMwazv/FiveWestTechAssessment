using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    /// <summary>
    /// AskBid
    /// </summary>
    public class AskBid
    {
        /// <summary>
        /// Gets or sets Price
        /// </summary>
        public string Price { get; set; }
        
        /// <summary>
        /// Gets or sets Orders
        /// </summary>
        public List<Order> Orders { get; set; }
    }
}
