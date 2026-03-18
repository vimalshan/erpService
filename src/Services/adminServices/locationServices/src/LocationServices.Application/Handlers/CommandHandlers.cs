using LocationServices.Application.Abstractions;
using LocationServices.Application.Commands;
using LocationServices.Application.DTOs;
using LocationServices.Domain.Entities;
using LocationServices.Domain.Repositories;
using LocationServices.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace LocationServices.Application.Handlers;

// ── CREATE HANDLER ───────────────────────────────────────────────────────────
public sealed class CreateLocationAppMapCommandHandler
    : ICommandHandler<CreateLocationAppMapCommand, LocationAppMapDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLocationAppMapCommandHandler> _logger;

    public CreateLocationAppMapCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateLocationAppMapCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LocationAppMapDto>> Handle(
        CreateLocationAppMapCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating LocationAppMap: {LocationId}/{AppName}", request.LocationId, request.AppName);

        if (await _unitOfWork.LocationAppMaps.ExistsAsync(request.LocationId, request.AppName, cancellationToken))
            return Result<LocationAppMapDto>.Failure($"Mapping {request.LocationId}/{request.AppName} already exists.");

        var aggregate = LocationAppMapAggregate.Create(
            LocationId.Create(request.LocationId),
            AppName.Create(request.AppName),
            request.SiteCategoryCode.HasValue ? SiteCategoryCode.Create(request.SiteCategoryCode.Value) : null,
            request.SelfAccess,
            request.DeemedApproval,
            request.CreatedBy);

        await _unitOfWork.LocationAppMaps.AddAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created LocationAppMap: {LocationId}/{AppName}", aggregate.LocationId, aggregate.AppName);
        return Result<LocationAppMapDto>.Success(aggregate.ToDto());
    }
}

// ── UPDATE HANDLER ───────────────────────────────────────────────────────────
public sealed class UpdateLocationAppMapCommandHandler
    : ICommandHandler<UpdateLocationAppMapCommand, LocationAppMapDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLocationAppMapCommandHandler> _logger;

    public UpdateLocationAppMapCommandHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateLocationAppMapCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<LocationAppMapDto>> Handle(
        UpdateLocationAppMapCommand request, CancellationToken cancellationToken)
    {
        var mapping = await _unitOfWork.LocationAppMaps.GetMappingAsync(request.LocationId, request.AppName, cancellationToken);
        if (mapping is null)
            return Result<LocationAppMapDto>.Failure($"Mapping {request.LocationId}/{request.AppName} not found.");

        mapping.Update(
            request.SiteCategoryCode.HasValue ? SiteCategoryCode.Create(request.SiteCategoryCode.Value) : null,
            request.SelfAccess,
            request.DeemedApproval,
            request.IsActive,
            request.ModifiedBy);

        _unitOfWork.LocationAppMaps.Update(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated LocationAppMap: {LocationId}/{AppName}", mapping.LocationId, mapping.AppName);
        return Result<LocationAppMapDto>.Success(mapping.ToDto());
    }
}

// ── DELETE HANDLER ───────────────────────────────────────────────────────────
public sealed class DeleteLocationAppMapCommandHandler : ICommandHandler<DeleteLocationAppMapCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLocationAppMapCommandHandler> _logger;

    public DeleteLocationAppMapCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteLocationAppMapCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteLocationAppMapCommand request, CancellationToken cancellationToken)
    {
        var mapping = await _unitOfWork.LocationAppMaps.GetMappingAsync(request.LocationId, request.AppName, cancellationToken);
        if (mapping is null)
            return Result.Failure($"Mapping {request.LocationId}/{request.AppName} not found.");

        mapping.Deactivate(request.ModifiedBy);
        _unitOfWork.LocationAppMaps.Update(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted LocationAppMap: {LocationId}/{AppName}", mapping.LocationId, mapping.AppName);
        return Result.Success();
    }
}

// ── MAPPING EXTENSION ────────────────────────────────────────────────────────
public static class LocationAppMapMappingExtensions
{
    public static LocationAppMapDto ToDto(this LocationAppMapAggregate a) => new(
        a.LocationId, a.AppName, a.SiteCategoryCode, a.SelfAccess,
        a.DeemedApproval, a.IsActive, a.CreatedAt, a.CreatedBy, a.ModifiedDate, a.ModifiedBy);
}
