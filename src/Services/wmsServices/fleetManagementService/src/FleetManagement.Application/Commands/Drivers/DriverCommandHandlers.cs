using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Commands.Drivers;

public class CreateDriverHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateDriverCommand, DriverDto>
{
    public async Task<DriverDto> Handle(CreateDriverCommand request, CancellationToken ct)
    {
        var driver = new Driver
        {
            Code = request.Code,
            EmployeeId = request.EmployeeId,
            FullName = request.FullName,
            LicenseNumber = request.LicenseNumber,
            LicenseExpiry = request.LicenseExpiry,
            Phone = request.Phone,
            Email = request.Email
        };
        await uow.Drivers.AddAsync(driver, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<DriverDto>(driver);
    }
}

public class UpdateDriverHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateDriverCommand, DriverDto>
{
    public async Task<DriverDto> Handle(UpdateDriverCommand request, CancellationToken ct)
    {
        var driver = await uow.Drivers.GetByIdAsync(request.DriverId, ct)
            ?? throw new KeyNotFoundException($"Driver {request.DriverId} not found.");
        driver.FullName = request.FullName;
        driver.LicenseNumber = request.LicenseNumber;
        driver.LicenseExpiry = request.LicenseExpiry;
        driver.Phone = request.Phone;
        driver.Email = request.Email;
        driver.ModifiedDate = DateTime.UtcNow;
        await uow.Drivers.UpdateAsync(driver, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<DriverDto>(driver);
    }
}

public class DeleteDriverHandler(IUnitOfWork uow) : IRequestHandler<DeleteDriverCommand, bool>
{
    public async Task<bool> Handle(DeleteDriverCommand request, CancellationToken ct)
    {
        var driver = await uow.Drivers.GetByIdAsync(request.DriverId, ct);
        if (driver is null) return false;
        await uow.Drivers.DeleteAsync(driver, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
