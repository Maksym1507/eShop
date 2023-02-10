using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;
using Basket.Host.Services.Abstractions;

namespace Basket.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("basket.basketapi")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class BasketApiContoller : Controller
    {
        private readonly ILogger<BasketApiContoller> _logger;
        private readonly IBasketApiService _basketApiService;

        public BasketApiContoller(
        ILogger<BasketApiContoller> logger,
        IBasketApiService basketApiService)
        {
            _logger = logger;
            _basketApiService = basketApiService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(CreateProductRequest request)
        {
            var result = await _basketApiService.AddCatalogItemAsync(request);
            return Ok(result);
        }
    }
}
