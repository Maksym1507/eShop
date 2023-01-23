using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;
using Catalog.Host.Repositories.Abstractions;
using Catalog.Host.Services.Abstractions;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
    {
        private readonly ICatalogBrandRepository _catalogBrandRepository;
        private readonly IMapper _mapper;

        public CatalogBrandService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogBrandRepository catalogBrandRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
            _mapper = mapper;
        }

        public async Task<ItemsResponse<CatalogBrandDto>> GetCatalogBrands()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _catalogBrandRepository.GetAsync();

                if (result.Data.Count() == 0)
                {
                    throw new Exception($"Brands not found");
                }

                return new ItemsResponse<CatalogBrandDto>()
                {
                    Data = result.Data.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList()
                };
            });
        }

        public async Task<int?> AddAsync(string brand)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _catalogBrandRepository.AddAsync(brand);
            });
        }

        public async Task<bool> UpdateAsync(int id, string brand)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var brandToUpdate = await _catalogBrandRepository.GetByIdAsync(id);

                if (brandToUpdate == null)
                {
                    return false;
                }

                brandToUpdate.Brand = brand;

                return await _catalogBrandRepository.UpdateAsync(brandToUpdate);
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var brandToDelete = await _catalogBrandRepository.GetByIdAsync(id);

                if (brandToDelete == null)
                {
                    return false;
                }

                return await _catalogBrandRepository.DeleteAsync(brandToDelete);
            });
        }
    }
}
