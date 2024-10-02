using Microsoft.AspNetCore.Mvc;
using OrderbookAPI.Services;
using OrderbookAPI.Models;
using OrderBookAPI.Services;

namespace OrderbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly InMemoryOrderBook _orderBook;

        /// <summary>
        /// Price Controller
        /// </summary>
        /// <param name="orderBook">orderBook</param>
        public PriceController(InMemoryOrderBook orderBook)
        {
            _orderBook = orderBook;
        }
        
        /// <summary>
        /// GetPrice
        /// </summary>
        /// <param name="usdtQuantity">usdtQuantity</param>
        /// <returns>decimal</returns>
        [HttpGet]
        public IActionResult GetPrice([FromQuery] decimal usdtQuantity)
        {
        
            return Ok(new { price_in_zar = usdtQuantity  * _orderBook.GetBestAsk()});
        }
    }
}