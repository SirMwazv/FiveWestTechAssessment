namespace OrderbookAPI.Models
{
   /// <summary>
   /// OrderBookData
   /// </summary>
    public class OrderBookData
    {
        /// <summary>
        /// Gets or sets LastChange
        /// </summary>
        public long LastChange { get; set; }
        
        /// <summary>
        /// Gets or sets Asks
        /// </summary>
        public List<AskBid> Asks { get; set; }
        
        /// <summary>
        /// Gets or sets Bids
        /// </summary>
        public List<AskBid> Bids { get; set; }
        
        /// <summary>
        /// Gets or sets SequenceNumber
        /// </summary>
        public long SequenceNumber { get; set; }
        
        /// <summary>
        /// Gets or sets Checksum
        /// </summary>
        public long Checksum { get; set; }
    }
}