using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Abstractions
{
    public interface ICatalogTypeRepository
    {
        Task<Items<CatalogType>> GetAsync();

        Task<CatalogType?> GetByIdAsync(int id);

        Task<int?> AddAsync(string type);

        Task<bool> UpdateAsync(CatalogType type);

        Task<bool> DeleteAsync(CatalogType brand);
    }
}
