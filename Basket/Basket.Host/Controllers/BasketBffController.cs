using Basket.Host.Models.Requests;
using Basket.Host.Services.Abstractions;
using Infrastructure.Identity;
using Infrastructure.RateLimit.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Basket.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketBffController : ControllerBase
    {
        private readonly ILogger<BasketBffController> _logger;
        private readonly IBasketService _basketService;
        private readonly ICacheService _cacheService;

        public BasketBffController(ILogger<BasketBffController> logger, IBasketService basketService, ICacheService cacheService)
        {
            _logger = logger;
            _basketService = basketService;
            _cacheService = cacheService;
        }

        [HttpPost]
        [AllowAnonymous]
        [TypeFilter(typeof(RateLimitAttribute), Arguments = new object[] { 10, 1 })]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Anonymous()
        {
            _logger.LogInformation("This this basket anonymous method");
            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(RateLimitAttribute), Arguments = new object[] { 10, 1 })]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Defended()
        {
            _logger.LogInformation($"User Id {User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value}");
            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(RateLimitAttribute), Arguments = new object[] { 10, 1 })]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> TestAdd(TestAddRequest request)
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            await _basketService.TestAdd(basketId!, request.Data);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> TestGet()
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var response = await _basketService.TestGet(basketId!);
            return Ok(response);
        }
    }
}