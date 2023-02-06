using Catalog.Host.Configurations;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Abstractions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogBffController : ControllerBase
    {
        private readonly ILogger<CatalogBrandController> _logger;
        private readonly IOptions<CatalogConfig> _config;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly ICatalogTypeService _catalogTypeService;

        public CatalogBffController(
            ILogger<CatalogBrandController> logger,
            IOptions<CatalogConfig> config,
            ICatalogService catalogService,
            ICatalogBrandService catalogBrandService,
            ICatalogTypeService catalogTypeService)
        {
            _logger = logger;
            _config = config;
            _catalogService = catalogService;
            _catalogBrandService = catalogBrandService;
            _catalogTypeService = catalogTypeService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
        {
            var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemsByBrand(ItemsByBrandRequest request)
        {
            var result = await _catalogService.GetCatalogItemsByBrandAsync(request.Brand);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemsByType(ItemsByTypeRequest request)
        {
            var result = await _catalogService.GetCatalogItemsByTypeAsync(request.Type);
            return Ok(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(CatalogItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ItemById(int id)
        {
            var result = await _catalogService.GetCatalogItemByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ItemsResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Brands()
        {
            var result = await _catalogBrandService.GetCatalogBrandsAsync();

            _logger.LogInformation($"User Id {User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value}");

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ItemsResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Types()
        {
            var result = await _catalogTypeService.GetCatalogTypesAsync();
            return Ok(result);
        }
    }
}
