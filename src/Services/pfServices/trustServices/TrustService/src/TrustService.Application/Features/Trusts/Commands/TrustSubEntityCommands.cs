using FluentValidation;
using MediatR;
using TrustService.Application.Common.Interfaces;

namespace TrustService.Application.Features.Trusts.Commands;

// --- Add Fund Type ---
public record AddTrustFundTypeCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public string FundType { get; init; } = string.Empty;
    public string FundName { get; init; } = string.Empty;
    public string FundPrefix { get; init; } = string.Empty;
}

public class AddTrustFundTypeCommandValidator : AbstractValidator<AddTrustFundTypeCommand>
{
    public AddTrustFundTypeCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.FundType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.FundName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.FundPrefix).NotEmpty().MaximumLength(65);
    }
}

public class AddTrustFundTypeCommandHandler : IRequestHandler<AddTrustFundTypeCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTrustFundTypeCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddTrustFundTypeCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        trust.AddFundType(request.FundType, request.FundName, request.FundPrefix);

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Add Trust Role ---
public record AddTrustRoleCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public int RoleId { get; init; }
    public string RoleCode { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public long UserNo { get; init; }
}

public class AddTrustRoleCommandValidator : AbstractValidator<AddTrustRoleCommand>
{
    public AddTrustRoleCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.RoleCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(25);
    }
}

public class AddTrustRoleCommandHandler : IRequestHandler<AddTrustRoleCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTrustRoleCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddTrustRoleCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        trust.AddRole(request.RoleId, request.RoleCode, request.UserId, request.UserNo);

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Add Trust Unit ---
public record AddTrustUnitCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public string UnitCode { get; init; } = string.Empty;
    public string UnitName { get; init; } = string.Empty;
    public string UnitType { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public long? UnitHeadSysId { get; init; }
}

public class AddTrustUnitCommandValidator : AbstractValidator<AddTrustUnitCommand>
{
    public AddTrustUnitCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.UnitName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UnitType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.State).NotEmpty().MaximumLength(50);
    }
}

public class AddTrustUnitCommandHandler : IRequestHandler<AddTrustUnitCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTrustUnitCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddTrustUnitCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        trust.AddUnit(request.UnitCode, request.UnitName, request.UnitType,
            request.AddressLine1, request.AddressLine2, request.City, request.State, request.UnitHeadSysId);

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Add Trust Approver ---
public record AddTrustApproverCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public long ApproverSysId { get; init; }
    public int ApproverLevel { get; init; }
    public string ApproverType { get; init; } = string.Empty;
}

public class AddTrustApproverCommandHandler : IRequestHandler<AddTrustApproverCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTrustApproverCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddTrustApproverCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        var approver = TrustService.Domain.Entities.TrustApprover.Create(
            request.TrustCode, request.ApproverSysId, request.ApproverLevel,
            request.ApproverType, DateTime.UtcNow);

        trust.Approvers.Add(approver);
        trust.AddDomainEvent(new TrustService.Domain.Events.TrustApproverAddedEvent(
            request.TrustCode, request.ApproverSysId, request.ApproverLevel));

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Add Trust Configuration ---
public record AddTrustConfigurationCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public string ConfigName { get; init; } = string.Empty;
    public string ConfigValue { get; init; } = string.Empty;
    public string ConfigCategory { get; init; } = string.Empty;
}

public class AddTrustConfigurationCommandHandler : IRequestHandler<AddTrustConfigurationCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTrustConfigurationCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddTrustConfigurationCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        var config = TrustService.Domain.Entities.TrustConfiguration.Create(
            request.TrustCode, request.ConfigName, request.ConfigValue,
            request.ConfigCategory, DateTime.UtcNow);

        trust.Configurations.Add(config);
        trust.AddDomainEvent(new TrustService.Domain.Events.TrustConfigurationChangedEvent(
            request.TrustCode, request.ConfigName, request.ConfigValue));

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
