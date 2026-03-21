using AutoMapper;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Interfaces;
using CustomerService.Domain.ValueObjects;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found.");

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

        customer.Update(request.Name, request.CompanyName, contact, address);

        await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }
}
