using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogService
    {
        Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);

        Task<ItemsResponse<CatalogItemDto>?> GetCatalogItemsByBrandAsync(string brand);

        Task<ItemsResponse<CatalogItemDto>?> GetCatalogItemsByTypeAsync(string type);

        Task<CatalogItemDto?> GetCatalogItemByIdAsync(int id);
    }
}
