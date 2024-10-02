namespace OrderbookAPI.Models;

/// <summary>
/// Binance Depth Response
/// </summary>
public class BinanceDepthResponse
{
    /// <summary>
    /// Gets or sets bids
    /// </summary>
    public List<List<string>> bids { get; set; }
    
    /// <summary>
    /// Gets or sets asks
    /// </summary>
    public List<List<string>> asks { get; set; }
}