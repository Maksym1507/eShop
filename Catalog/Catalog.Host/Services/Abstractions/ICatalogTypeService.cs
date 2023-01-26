using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogTypeService
    {
        Task<ItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync();

        Task<int?> AddAsync(string type);

        Task<bool> UpdateAsync(int id, string type);

        Task<bool> DeleteAsync(int id);
    }
}
