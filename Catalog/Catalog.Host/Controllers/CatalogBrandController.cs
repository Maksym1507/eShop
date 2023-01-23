using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services;
using Catalog.Host.Services.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogBrandController : Controller
    {
        private readonly ICatalogBrandService _catalogBrandService;
        private readonly ILogger<CatalogBrandController> _logger;

        public CatalogBrandController(
            ICatalogBrandService catalogBrandService,
            ILogger<CatalogBrandController> logger)
        {
            _catalogBrandService = catalogBrandService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(CreateUpdateBrandRequest request)
        {
            var result = await _catalogBrandService.AddAsync(request.Brand);
            return Ok(new AddItemResponse<int?>() { Id = result });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(UpdateItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(int id, CreateUpdateBrandRequest request)
        {
            var result = await _catalogBrandService.UpdateAsync(id, request.Brand);
            return Ok(new UpdateItemResponse<bool>() { IsUpdated = result });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(DeleteItemResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _catalogBrandService.DeleteAsync(id);
            return Ok(new DeleteItemResponse<bool>() { IsDeleted = result });
        }
    }
}
