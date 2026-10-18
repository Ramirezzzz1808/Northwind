using NorthWind.Transactions.Entities.Interfaces;
using System.Transactions;

namespace NorthWind.Transactions.Entities.Services;

internal class DomainTransaction : IDomainTransaction, IDisposable
{
    TransactionScope TransactionScope;

    public void BeginTransaction()
    {
        // Las transacciones distribuidas (MSDTC) solo existen en Windows;
        // en Linux incluso asignar esta propiedad lanza
        // PlatformNotSupportedException. El caso de uso mantiene una sola
        // conexión dentro de la transacción, por lo que no se necesitan.
        if (OperatingSystem.IsWindows())
        {
            TransactionManager.ImplicitDistributedTransactions = true;
        }

        TransactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public void CommitTransaction()
    {
        TransactionScope.Complete();
        Dispose();
    }

    public void RollbackTransaction()
    {
        Dispose();
    }

    public void Dispose()
    {
        TransactionScope?.Dispose();
    }
}
