using Basket.Host.Configurations;
using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;
using Basket.Host.Services.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace Basket.Host.Services
{
    public class BasketApiService : IBasketApiService
    {
        private readonly IInternalHttpClientService _httpClient;
        private readonly ILogger<BasketApiService> _logger;
        private readonly BasketConfig _config;

        public BasketApiService(
            IInternalHttpClientService httpClient,
            ILogger<BasketApiService> logger,
            IOptions<BasketConfig> config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config.Value;
        }

        public async Task<AddItemResponse<int?>> AddCatalogItemAsync(CreateProductRequest request)
        {
            var result = await _httpClient.SendAsync<AddItemResponse<int?>, CreateProductRequest>(
                $"{_config.CatalogItemUrl}/add",
                HttpMethod.Post,
                request);

            _logger.LogInformation($"{result}");

            return result;
        }
    }
}
