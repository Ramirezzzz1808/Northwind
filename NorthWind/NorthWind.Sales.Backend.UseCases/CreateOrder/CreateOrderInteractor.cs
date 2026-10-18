using NorthWind.Entities.Guards;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.BusinessObjects.Specifications;

namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

internal class CreateOrderInteractor(
    ICreateOrderOutputPort outputPort,
    ICommandsRepository repository,
    IQueriesRepository queriesRepository,
    IModelValidatorHub<CreateOrderDto> modelValidatorHub,
    IDomainEventHub<SpecialOrderCreatedEvent> domainEventHub,
    IDomainLogger domainLogger,
    IDomainTransaction domainTransaction,
    IUserService userService)
    : ICreateOrderInputPort
{
    public async Task Handle(CreateOrderDto orderDto)
    {
        GuardUser.AgainstUnauthenticated(userService);

        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);

        await domainLogger.LogInformation(new DomainLog(
            CreateOrderMessages.StartingPurchaseOrderCreation,
            userService.UserName));

        // El precio nunca se toma del cliente: se usa el del catálogo.
        CreateOrderDto TrustedOrderDto =
            await ApplyCatalogPrices(orderDto);

        OrderAggregate Order = OrderAggregate.From(TrustedOrderDto);

        try
        {
            domainTransaction.BeginTransaction();

            await repository.CreateOrder(Order);
            await repository.SaveChanges();

            await domainLogger.LogInformation(new DomainLog(
                string.Format(
                    CreateOrderMessages.PurchaseOrderCreatedTemplate,
                    Order.Id),
                userService.UserName));

            await outputPort.Handle(Order);

            if (new SpecialOrderSpecification().IsSatisfiedBy(Order))
            {
                await domainEventHub.Raise(
                    new SpecialOrderCreatedEvent(
                        Order.Id, Order.OrderDetails.Count));
            }

            domainTransaction.CommitTransaction();
        }
        catch
        {
            domainTransaction.RollbackTransaction();

            string Information = string.Format(
                CreateOrderMessages.OrderCreationCancelledTemplate,
                Order.Id);

            await domainLogger.LogInformation(new DomainLog(
                Information, userService.UserName));

            throw;
        }
    }

    async Task<CreateOrderDto> ApplyCatalogPrices(CreateOrderDto orderDto)
    {
        IDictionary<int, decimal> CatalogPrices =
            await queriesRepository.GetProductsUnitPrices(
                orderDto.OrderDetails.Select(d => d.ProductId));

        IEnumerable<CreateOrderDetailDto> Details = orderDto.OrderDetails
            .Select(d => new CreateOrderDetailDto(
                d.ProductId, CatalogPrices[d.ProductId], d.Quantity))
            .ToList();

        return new CreateOrderDto(
            orderDto.CustomerId,
            orderDto.ShipAddress,
            orderDto.ShipCity,
            orderDto.ShipCountry,
            orderDto.ShipPostalCode,
            Details);
    }
}
