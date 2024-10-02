namespace OrderbookAPI.Models
{
    /// <summary>
    /// OrderBookUpdate
    /// </summary>
    public class OrderBookUpdate
    {
        /// <summary>
        /// Gets or sets type
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// Gets or sets currencyPairSymbol
        /// </summary>
        public string currencyPairSymbol { get; set; }
        
        /// <summary>
        /// Gets or sets data
        /// </summary>
        public OrderBookData data { get; set; }
    }    
}