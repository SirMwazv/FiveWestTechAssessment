using System;
using System.Collections.Generic;

namespace OrderbookAPI.Models
{
    public class OrderBookData
    {
        public long LastChange { get; set; }
        public List<AskBid> Asks { get; set; }
        public List<AskBid> Bids { get; set; }
        public long SequenceNumber { get; set; }
        public long Checksum { get; set; }
    }
}