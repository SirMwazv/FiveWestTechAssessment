using Microsoft.AspNetCore.Mvc;
using OrderbookAPI.Services;

namespace OrderbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly OrderbookService _orderbookService;

        public PriceController(OrderbookService orderbookService)
        {
            _orderbookService = orderbookService;
        }

        [HttpGet]
        public IActionResult GetPrice([FromQuery] decimal usdtQuantity)
        {
            var zarPrice = _orderbookService.GetZARPriceForUSDT(usdtQuantity);
            return Ok(new { price_in_zar = zarPrice });
        }
    }
}