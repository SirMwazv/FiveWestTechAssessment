namespace OrderbookAPI.Models;

public class CryptoAsset
{
    public string Symbol { get; set; }
    public decimal market_cap { get; set; }
    public decimal? price { get; set; }
}