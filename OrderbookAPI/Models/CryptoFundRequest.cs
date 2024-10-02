namespace OrderbookAPI.Models;

/// <summary>
/// CryptoFundRequest
/// </summary>
public class CryptoFundRequest
{
    /// <summary>
    /// Gets or sets asset_cap
    /// </summary>
    public decimal asset_cap { get; set; }
    
    /// <summary>
    /// Gets or sets total_capital
    /// </summary>
    public decimal total_capital { get; set; } 
    

    /// <summary>
    /// Gets or sets assets
    /// </summary>
    public List<CryptoAsset> assets { get; set; }
}