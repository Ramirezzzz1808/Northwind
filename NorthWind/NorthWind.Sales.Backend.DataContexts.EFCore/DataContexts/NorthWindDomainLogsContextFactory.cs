using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

internal class NorthWindDomainLogsContextFactory
    : IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        var DbOptions = Microsoft.Extensions.Options.Options.Create(
            new NorthWind.Sales.Backend.DataContexts.EFCore.Options.DBOptions
            {
                DomainLogsConnectionString =
                    @"Server=(localdb)\mssqllocaldb;Database=NorthWindLogsDB"
            });

        return new NorthWindDomainLogsContext(DbOptions);
    }
}