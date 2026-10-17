using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Id).HasMaxLength(5).IsFixedLength();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(40);
        builder.Property(c => c.CurrentBalance).HasPrecision(8, 2);

        // Datos de ejemplo
        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste", CurrentBalance = 0 },
            new Customer { Id = "ANTON", Name = "Antonio Moreno", CurrentBalance = 100 },
            new Customer { Id = "BOLID", Name = "Bolido Comidas", CurrentBalance = 0 },
            new Customer { Id = "FAMIA", Name = "Familia Arquibaldo", CurrentBalance = 0 },
            new Customer { Id = "GALED", Name = "Galería del gastrónomo", CurrentBalance = 0 }
        );
    }
}