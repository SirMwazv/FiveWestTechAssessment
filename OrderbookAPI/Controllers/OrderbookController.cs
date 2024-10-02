using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderbookAPI.Models;
using OrderbookAPI.Services;
using System.Globalization;

namespace OrderBookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderbookController : ControllerBase
    {
        private readonly InMemoryOrderBook _orderBook;
        private readonly IHttpClientFactory _httpClientFactory;
        public OrderbookController(InMemoryOrderBook orderBook, IHttpClientFactory httpClientFactory)
        {
            _orderBook = orderBook;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get Best Ask
        /// </summary>
        /// <returns>decimal</returns>
        // Endpoint to get the best ask price
        [HttpGet("bestask")]
        public IActionResult GetBestAsk()
        {
            var bestAsk = _orderBook.GetBestAsk();
            if (bestAsk == null) return NotFound("No asks available.");
            return Ok(new { BestAsk = bestAsk });
        }

        /// <summary>
        /// Get Best Bid
        /// </summary>
        /// <returns>decimal</returns>
        // Endpoint to get the best bid price
        [HttpGet("bestbid")]
        public IActionResult GetBestBid()
        {
            var bestBid = _orderBook.GetBestBid();
            if (bestBid == null) return NotFound("No bids available.");
            return Ok(new { BestBid = bestBid });
        }
        
        /// <summary>
        /// Calculate Crypto Fund
        /// </summary>
        /// <param name="request">request</param>
        /// <returns>CryptoFundRequest</returns>
        [HttpPost("crypto-fund")]
        public async Task<IActionResult> CalculateCryptoFund(
            [FromBody] CryptoFundRequest request)
        {
            // Step 1: Fetch missing prices from Binance if not provided
            foreach (var asset in request.assets)
            {
                if (!asset.price.HasValue)
                {
                    asset.price = await GetBinancePrice(asset.Symbol);
                }
            }

            // Step 2: Calculate the allocations
            var allocations = CalculateCryptoFundAllocation(
                request.total_capital,
                request.asset_cap,
                request.assets
            );

            return Ok(allocations);
        }
        
        /// <summary>
        /// Get Binance Price
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>decimal</returns>
        private async Task<decimal> GetBinancePrice(string symbol)
        {
            string pairSymbol = symbol + "USDT";
            string url = $"https://api.binance.com/api/v3/depth?symbol={pairSymbol}&limit=1";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();
            var binanceResponse = await JsonSerializer.DeserializeAsync<BinanceDepthResponse>(contentStream);

            var bestBidPrice = ParseBidPrice(binanceResponse.bids[0][0]);

            decimal usdtToZar = _orderBook.GetBestAsk() ?? 18.0m; //use in memory value, also with fallback in case no value exists
            if (bestBidPrice == 0)
            {
                Console.WriteLine("Bid price could not be determined.");
            }
            return bestBidPrice * usdtToZar;
        }

        /// <summary>
        /// Parse Bid Price
        /// </summary>
        /// <param name="bidPriceString">bidPriceString</param>
        /// <returns>decimal</returns>
        private decimal ParseBidPrice(string bidPriceString)
        {
            // Check if the bid price string is null or empty
            if (string.IsNullOrWhiteSpace(bidPriceString))
            {
                Console.WriteLine("Bid price is missing or empty.");
                return 0; 
            }

            // Try to parse the string into a decimal
            if (decimal.TryParse(bidPriceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bidPrice))
            {
                return bidPrice;
            }
            else
            {
                Console.WriteLine($"Failed to parse bid price: {bidPriceString}");
                return 0; 
            }
        }

        /// <summary>
        /// Calculate Crypto Fund Allocation
        /// </summary>
        /// <param name="totalCapital">totalCapital</param>
        /// <param name="assetCap">assetCap</param>
        /// <param name="assets">assets</param>
        /// <returns>List of AssetAllocation</returns>
        [HttpPost("calculate-crypto-fund-allocation")]
        public List<AssetAllocation> CalculateCryptoFundAllocation(decimal totalCapital, decimal assetCap, List<CryptoAsset> assets)
        {
            // Step 1: Calculate initial weightings based on market cap
            decimal totalMarketCap = assets.Sum(a => a.market_cap);
            List<AssetAllocation> allocations = new List<AssetAllocation>();

            // Step 2: Apply asset cap and redistribute excess
            decimal remainingCapital = totalCapital;
            decimal remainingPercentage = 1m; // Start with 100% of the fund available

            foreach (var asset in assets)
            {
                decimal initialPercentage = asset.market_cap / totalMarketCap; // Initial percentage allocation based on market cap
                decimal cappedPercentage = Math.Min(initialPercentage, assetCap); // Apply the cap

                decimal allocationAmount = cappedPercentage * totalCapital;
                allocations.Add(new AssetAllocation
                {
                    Symbol = asset.Symbol,
                    // Ensure Price is handled using invariant culture to avoid culture-specific issues I ran into
                    Price = asset.price.HasValue ? decimal.Parse(asset.price.Value.ToString(), CultureInfo.InvariantCulture) : 0,
                    Percentage = cappedPercentage * 100,  // Store as percentage (0-100)
                    Amount = allocationAmount
                });


                // Subtract allocated amount from the remaining capital and percentage
                remainingCapital -= allocationAmount;
                remainingPercentage -= cappedPercentage;
            }

            // Step 3: Redistribute remaining capital to assets that haven't reached the cap
            var uncappedAssets = allocations.Where(a => a.Percentage < assetCap * 100).ToList();

            if (remainingCapital > 0 && uncappedAssets.Count > 0)
            {
                foreach (var allocation in uncappedAssets)
                {
                    decimal additionalPercentage = (allocation.Percentage / uncappedAssets.Sum(a => a.Percentage)) * remainingPercentage;
                    allocation.Percentage += additionalPercentage * 100;
                    allocation.Amount += additionalPercentage * totalCapital;
                }
            }
            return allocations;
        }
        
    }
}