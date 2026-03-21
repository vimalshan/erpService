using CustomerService.Application.DTOs;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed record UpdateCustomerCommand(
    int CustomerId,
    string Name,
    string? CompanyName,
    string? ContactPerson,
    string? ContactTitle,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode
) : IRequest<CustomerDto>;
