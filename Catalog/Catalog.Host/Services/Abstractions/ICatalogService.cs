using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogService
    {
        Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);

        Task<ItemsResponse<CatalogItemDto>> GetCatalogItemsByBrandAsync(string brand);

        Task<ItemsResponse<CatalogItemDto>> GetCatalogItemsByTypeAsync(string type);

        Task<CatalogItemDto> GetCatalogItemById(int id);
    }
}
