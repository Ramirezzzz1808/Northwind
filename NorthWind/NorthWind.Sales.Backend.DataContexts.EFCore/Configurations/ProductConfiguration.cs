using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(40);
        builder.Property(p => p.UnitPrice).HasPrecision(8, 2);

        // Concurrencia optimista: SQL Server regenera el valor en cada UPDATE.
        builder.Property(p => p.RowVersion).IsRowVersion();

        // Datos de ejemplo
        builder.HasData(
            new Product { Id = 1, Name = "Chai", UnitPrice = 35, UnitsInStock = 20 },
            new Product { Id = 2, Name = "Chang", UnitPrice = 35, UnitsInStock = 0 },
            new Product { Id = 3, Name = "Aniseed Syrup", UnitPrice = 65, UnitsInStock = 30 },
            new Product { Id = 4, Name = "Chef Anton's Cajun", UnitPrice = 75, UnitsInStock = 50 },
            new Product { Id = 5, Name = "Chef Anton's Gumbo", UnitPrice = 45, UnitsInStock = 15 }
        );
    }
}
