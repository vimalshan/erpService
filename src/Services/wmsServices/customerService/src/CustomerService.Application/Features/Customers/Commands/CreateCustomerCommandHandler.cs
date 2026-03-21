using AutoMapper;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Interfaces;
using CustomerService.Domain.ValueObjects;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Customers.ExistsAsync(request.Code, cancellationToken))
            throw new InvalidOperationException($"Customer with code '{request.Code}' already exists.");

        var contact = new ContactInfo(
            request.ContactPerson ?? string.Empty,
            request.ContactTitle ?? string.Empty,
            request.Email ?? string.Empty,
            request.Phone ?? string.Empty);

        var address = new Address(
            request.Address ?? string.Empty,
            request.City ?? string.Empty,
            request.State ?? string.Empty,
            request.Country ?? string.Empty,
            request.PostalCode ?? string.Empty);

        var customer = Domain.Entities.Customer.Create(
            request.Code, request.Name, request.CompanyName, contact, address);

        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }
}
