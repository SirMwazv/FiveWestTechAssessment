using Microsoft.AspNetCore.Mvc;
using OrderbookAPI.Services;
using OrderBookAPI.Services;

namespace OrderBookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderbookController : ControllerBase
    {
        private readonly InMemoryOrderBook _orderBook;

        public OrderbookController(InMemoryOrderBook orderBook)
        {
            _orderBook = orderBook;
        }

        // Endpoint to get the best ask price
        [HttpGet("bestask")]
        public IActionResult GetBestAsk()
        {
            var bestAsk = _orderBook.GetBestAsk();
            if (bestAsk == null) return NotFound("No asks available.");
            return Ok(new { BestAsk = bestAsk });
        }

        // Endpoint to get the best bid price
        [HttpGet("bestbid")]
        public IActionResult GetBestBid()
        {
            var bestBid = _orderBook.GetBestBid();
            if (bestBid == null) return NotFound("No bids available.");
            return Ok(new { BestBid = bestBid });
        }
    }
}