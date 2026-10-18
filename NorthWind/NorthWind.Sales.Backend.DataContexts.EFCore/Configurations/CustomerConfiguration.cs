using NorthWind.Sales.Backend.Repositories.Entities;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Id).HasMaxLength(5).IsFixedLength();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(40);
        builder.Property(c => c.CurrentBalance).HasPrecision(8, 2);
        builder.Property(c => c.Identification).IsRequired(false).HasMaxLength(13);
        builder.Property(c => c.Email).IsRequired(false).HasMaxLength(100);

        // Datos de ejemplo (cédulas con dígito verificador válido)
        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste", CurrentBalance = 0, Identification = "1710034065", Email = "alfki@example.com" },
            new Customer { Id = "ANTON", Name = "Antonio Moreno", CurrentBalance = 100, Identification = "1712345675", Email = "anton@example.com" },
            new Customer { Id = "BOLID", Name = "Bolido Comidas", CurrentBalance = 0, Identification = "0912345675", Email = "bolid@example.com" },
            new Customer { Id = "FAMIA", Name = "Familia Arquibaldo", CurrentBalance = 0, Identification = "1801234566", Email = "famia@example.com" },
            new Customer { Id = "GALED", Name = "Galería del gastrónomo", CurrentBalance = 0, Identification = "0102030400", Email = "galed@example.com" }
        );
    }
}
