namespace MVC.ViewModels
{
    public class CatalogData<T>
    {
        public IEnumerable<T> Data { get; init; } = null!;
    }
}
