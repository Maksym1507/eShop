using Catalog.Host.Models.Requests;

namespace Catalog.Host.Services.Abstractions
{
    public interface ICatalogItemService
    {
        Task<int?> AddAsync(string title, string description, decimal price,  double weight, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);

        Task<bool> UpdateAsync(int id, string title, string description, decimal price, double weight, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);

        Task<bool> DeleteAsync(int id);
    }
}
