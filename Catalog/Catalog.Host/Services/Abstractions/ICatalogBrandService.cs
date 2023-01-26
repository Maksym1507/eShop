using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogBrandService
    {
        Task<ItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync();

        Task<int?> AddAsync(string brand);

        Task<bool> UpdateAsync(int id, string brand);

        Task<bool> DeleteAsync(int id);
    }
}
