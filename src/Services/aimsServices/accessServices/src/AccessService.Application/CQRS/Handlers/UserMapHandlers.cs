namespace AccessService.Application.CQRS.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using AccessService.Application.CQRS.Commands;
using AccessService.Application.DTOs;
using AccessService.Application.CQRS.Queries;
using AccessService.Application.Interfaces;
using AccessService.Domain.Entities;

/// <summary>
/// Implementations of UserMap CQRS handlers
/// </summary>

public class CreateUserMapCommandHandlerImpl : IRequestHandler<CreateUserMapCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserMapCommandHandlerImpl> _logger;

    public CreateUserMapCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<CreateUserMapCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid> Handle(CreateUserMapCommand request, CancellationToken cancellationToken)
    {
        var userMap = new UserMap(request.EmployeeSystemId);
        await _unitOfWork.UserMaps.AddAsync(userMap);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"UserMap created successfully for employee ID: {request.EmployeeSystemId}");

        return Guid.NewGuid();
    }
}

public class ActivateUserMapCommandHandlerImpl : IRequestHandler<ActivateUserMapCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivateUserMapCommandHandlerImpl> _logger;

    public ActivateUserMapCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<ActivateUserMapCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ActivateUserMapCommand request, CancellationToken cancellationToken)
    {
        var userMap = await _unitOfWork.UserMaps.GetByEmployeeSystemIdAsync(request.EmployeeSystemId);
        if (userMap == null)
            throw new KeyNotFoundException($"UserMap not found for employee ID: {request.EmployeeSystemId}");

        userMap.SetEffectiveDate(request.EffectiveDate);
        await _unitOfWork.UserMaps.UpdateAsync(userMap);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"UserMap activated for employee ID: {request.EmployeeSystemId}");
    }
}

public class DeactivateUserMapCommandHandlerImpl : IRequestHandler<DeactivateUserMapCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateUserMapCommandHandlerImpl> _logger;

    public DeactivateUserMapCommandHandlerImpl(IUnitOfWork unitOfWork, ILogger<DeactivateUserMapCommandHandlerImpl> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(DeactivateUserMapCommand request, CancellationToken cancellationToken)
    {
        var userMap = await _unitOfWork.UserMaps.GetByEmployeeSystemIdAsync(request.EmployeeSystemId);
        if (userMap == null)
            throw new KeyNotFoundException($"UserMap not found for employee ID: {request.EmployeeSystemId}");

        userMap.SetClosureDate(request.ClosureDate);
        await _unitOfWork.UserMaps.UpdateAsync(userMap);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"UserMap deactivated for employee ID: {request.EmployeeSystemId}");
    }
}

public class GetUserMapByEmployeeIdQueryHandlerImpl : IRequestHandler<GetUserMapByEmployeeIdQuery, UserMapDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserMapByEmployeeIdQueryHandlerImpl(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<UserMapDto?> Handle(GetUserMapByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var userMap = await _unitOfWork.UserMaps.GetByEmployeeSystemIdAsync(request.EmployeeSystemId);
        if (userMap == null)
            return null;

        return new UserMapDto
        {
            EmployeeSystemId = userMap.EmployeeSystemId,
            EffectiveDate = userMap.EffectiveDate,
            ClosureDate = userMap.ClosureDate,
            ModifiedBy = userMap.ModifiedBy,
            ModifiedOn = userMap.ModifiedOn,
            IsActive = userMap.IsActive()
        };
    }
}

public class GetAllUserMapsQueryHandlerImpl : IRequestHandler<GetAllUserMapsQuery, IEnumerable<UserMapDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUserMapsQueryHandlerImpl(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<IEnumerable<UserMapDto>> Handle(GetAllUserMapsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<UserMap> userMaps;

        if (request.ActiveOnly.HasValue && request.ActiveOnly.Value)
        {
            userMaps = await _unitOfWork.UserMaps.GetActiveUserMapsAsync();
        }
        else
        {
            userMaps = await _unitOfWork.UserMaps.GetAllAsync();
        }

        return userMaps.Select(x => new UserMapDto
        {
            EmployeeSystemId = x.EmployeeSystemId,
            EffectiveDate = x.EffectiveDate,
            ClosureDate = x.ClosureDate,
            ModifiedBy = x.ModifiedBy,
            ModifiedOn = x.ModifiedOn,
            IsActive = x.IsActive()
        });
    }
}
