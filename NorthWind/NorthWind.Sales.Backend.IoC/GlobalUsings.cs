global using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
global using NorthWind.Sales.Backend.UseCases;
global using NorthWind.Sales.Backend.Repositories;
global using NorthWind.Sales.Backend.Presenters;
global using Microsoft.AspNetCore.Builder;

global using Microsoft.Extensions.DependencyInjection;
global using NorthWind.Validation.Entities.Interfaces;
global using NorthWind.ValidationService.FluentValidation;
global using NorthWind.Sales.Validators.Entities;
global using NorthWind.Events.Entities.Interfaces;
global using NorthWind.Sales.Backend.SmtpGateways.Options;
global using NorthWind.DomainLogs.Entities.Interfaces;
global using NorthWind.Transactions.Entities.Interfaces;
global using NorthWind.UserServices;
global using NorthWind.Membership.Backend.AspNetIdentity.Options;
