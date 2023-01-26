using System.Net;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogBffController : Controller
    {
        private readonly ILogger<CatalogBrandController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly ICatalogTypeService _catalogTypeService;

        public CatalogBffController(
            ILogger<CatalogBrandController> logger,
            ICatalogService catalogService,
            ICatalogBrandService catalogBrandService,
            ICatalogTypeService catalogTypeService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _catalogBrandService = catalogBrandService;
            _catalogTypeService = catalogTypeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items(PaginatedItemsRequest request)
        {
            var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
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
        [ProducesResponseType(typeof(ItemsResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Brands()
        {
            var result = await _catalogBrandService.GetCatalogBrandsAsync();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Types()
        {
            var result = await _catalogTypeService.GetCatalogTypesAsync();
            return Ok(result);
        }
    }
}
