namespace OrderbookAPI.Models;

/// <summary>
/// Asset Allocation
/// </summary>
public class AssetAllocation
{
    /// <summary>
    /// Gets or sets Symbol
    /// </summary>
    public string Symbol { get; set; }
    
    /// <summary>
    /// Gets or sets Price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets Percentage
    /// </summary>
    public decimal Percentage { get; set; }
    
   /// <summary>
   /// Gets or sets Amount
   /// </summary>
    public decimal Amount { get; set; } 
}