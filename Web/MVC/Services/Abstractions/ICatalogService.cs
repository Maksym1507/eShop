using MVC.ViewModels;

namespace MVC.Services.Abstractions
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type);

        Task<IEnumerable<SelectListItem>> GetBrands();

        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
