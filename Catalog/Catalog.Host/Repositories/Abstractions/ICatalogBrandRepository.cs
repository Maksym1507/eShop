using Catalog.Host.Data.Entities;
using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Abstractions
{
    public interface ICatalogBrandRepository
    {
        Task<Items<CatalogBrand>> GetAsync();

        Task<CatalogBrand?> GetByIdAsync(int id);

        Task<int?> AddAsync(string brand);

        Task<bool> UpdateAsync(CatalogBrand brand);

        Task<bool> DeleteAsync(CatalogBrand brand);
    }
}
