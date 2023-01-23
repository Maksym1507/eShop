using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfigurations
{
    public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");

            builder.HasKey(h => h.Id);

            builder.Property(p => p.Id)
                .UseHiLo("catalog_brand_hilo")
                .IsRequired();

            builder.Property(p => p.Brand)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
