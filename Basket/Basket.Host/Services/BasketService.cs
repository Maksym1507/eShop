using Basket.Host.Models.Responses;
using Basket.Host.Services.Abstractions;

namespace Basket.Host.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICacheService _cacheService;

        public BasketService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task TestAdd(string userId, string data)
        {
           await _cacheService.AddOrUpdateAsync(userId, data);
        }

        public async Task<TestGetResponse> TestGet(string userId)
        {
            var result = await _cacheService.GetAsync<string>(userId);
            return new TestGetResponse() { Data = result };
        }
    }
}
