using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;

namespace Basket.Host.Services.Abstractions
{
    public interface IBasketApiService
    {
        Task<AddItemResponse<int?>> AddCatalogItemAsync(CreateProductRequest request);
    }
}
