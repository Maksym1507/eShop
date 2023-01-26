using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<Items<CatalogBrand>> GetAsync()
        {
            var result = await _dbContext.CatalogBrands
                .ToListAsync();

            return new Items<CatalogBrand>() { Data = result };
        }

        public async Task<CatalogBrand?> GetByIdAsync(int id)
        {
            return await _dbContext.CatalogBrands.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<int?> AddAsync(string brand)
        {
            var item = await _dbContext.AddAsync(new CatalogBrand
            {
                Brand = brand
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool> UpdateAsync(CatalogBrand brand)
        {
            _dbContext.CatalogBrands.Update(brand);

            var quantityCatalogBrandsUpdated = await _dbContext.SaveChangesAsync();

            if (quantityCatalogBrandsUpdated > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(CatalogBrand brand)
        {
            _dbContext.CatalogBrands.Remove(brand);

            var quantityCatalogBrandsRemoved = await _dbContext.SaveChangesAsync();

            if (quantityCatalogBrandsRemoved > 0)
            {
                return true;
            }

            return false;
        }
    }
}
