using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Abstractions;
using Catalog.Host.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogItemRepository : ICatalogItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogItemRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogItems
                .Include(i => i.CatalogBrand)
                .Include(i => i.CatalogType)
                .OrderBy(c => c.Title)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
        }

        public async Task<Items<CatalogItem>> GetByBrandAsync(string brand)
        {
            var result = await _dbContext.CatalogItems
                .Include(i => i.CatalogBrand).Where(w => w.CatalogBrand!.Brand == brand)
                .ToListAsync();

            return new Items<CatalogItem>() { Data = result };
        }

        public async Task<Items<CatalogItem>> GetByTypeAsync(string type)
        {
            var result = await _dbContext.CatalogItems
                .Include(i => i.CatalogType).Where(w => w.CatalogType!.Type == type)
                .ToListAsync();

            return new Items<CatalogItem>() { Data = result };
        }

        public async Task<CatalogItem?> GetByIdAsync(int id)
        {
            return await _dbContext.CatalogItems.Include(i => i.CatalogBrand).Include(i => i.CatalogType).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<int?> AddAsync(string title, string description, decimal price, double weight, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
        {
            var item = await _dbContext.AddAsync(new CatalogItem
            {
                CatalogBrandId = catalogBrandId,
                CatalogTypeId = catalogTypeId,
                Description = description,
                Title = title,
                PictureFileName = pictureFileName,
                Price = price,
                Weight = weight,
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<bool> UpdateAsync(CatalogItem item)
        {
            _dbContext.CatalogItems.Update(item);

            var quantityCatalogItemsUpdated = await _dbContext.SaveChangesAsync();

            if (quantityCatalogItemsUpdated > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(CatalogItem item)
        {
            _dbContext.CatalogItems.Remove(item);

            var quantityCatalogItemsRemoved = await _dbContext.SaveChangesAsync();

            if (quantityCatalogItemsRemoved > 0)
            {
                return true;
            }

            return false;
        }
    }
}
