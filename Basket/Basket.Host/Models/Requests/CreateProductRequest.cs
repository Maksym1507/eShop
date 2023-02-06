namespace Basket.Host.Models.Requests;

public class CreateProductRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public double Weight { get; set; }

    public string PictureFileName { get; set; } = null!;

    public int CatalogTypeId { get; set; }

    public int CatalogBrandId { get; set; }

    public int AvailableStock { get; set; }
}