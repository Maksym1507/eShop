using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Abstractions
{
    public interface ICatalogItemRepository
    {
        Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize);

        Task<CatalogItem?> GetByIdAsync(int id);

        Task<Items<CatalogItem>> GetByBrandAsync(string brand);

        Task<Items<CatalogItem>> GetByTypeAsync(string type);

        Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);

        Task<bool> UpdateAsync(CatalogItem item);

        Task<bool> DeleteAsync(CatalogItem item);
    }
}
