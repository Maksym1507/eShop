using MVC.Models.Enums;
using MVC.Models.Requests;
using MVC.Services.Abstractions;
using MVC.ViewModels;

namespace MVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(IOptions<AppSettings> settings, IHttpClientService httpClient, ILogger<CatalogService> logger)
        {
            _settings = settings;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PaginatedCatalogItems> GetCatalogItems(int page, int take, int? brand, int? type)
        {
            await _httpClient.SendAsync<object, object>(
                $"{_settings.Value.BasketUrl}/anonymous",
                HttpMethod.Post,
                null);

            await _httpClient.SendAsync<object, object>(
                $"{_settings.Value.BasketUrl}/defended",
                HttpMethod.Post,
                null);

            var filters = new Dictionary<CatalogTypeFilter, int>();

            if (brand.HasValue)
            {
                filters.Add(CatalogTypeFilter.Brand, brand.Value);
            }

            if (type.HasValue)
            {
                filters.Add(CatalogTypeFilter.Type, type.Value);
            }

            var result = await _httpClient.SendAsync<PaginatedCatalogItems, PaginatedItemsRequest<CatalogTypeFilter>>(
                $"{_settings.Value.CatalogUrl}/items",
                HttpMethod.Post,
                new PaginatedItemsRequest<CatalogTypeFilter>()
                {
                    PageIndex = page,
                    PageSize = take,
                    Filters = filters
                });

            return result;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            var result = await _httpClient.SendAsync<CatalogData<CatalogBrand>, object>(
                $"{_settings.Value.CatalogUrl}/brands",
                HttpMethod.Post,
                null);

            var listOfBrands = result.Data.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Brand
            });

            return listOfBrands;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var result = await _httpClient.SendAsync<CatalogData<CatalogType>, object>(
                $"{_settings.Value.CatalogUrl}/types",
                HttpMethod.Post,
                null);

            var listOfTypes = result.Data.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Type
            });

            return listOfTypes;
        }
    }
}
