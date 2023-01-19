namespace Catalog.Host.Data.Entities
{
    public class CatalogEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? UrlImg { get; set; }

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public string Consist { get; set; } = null!;
    }
}
