using FluentValidation;
using MediatR;
using TrustService.Application.Common.Interfaces;
using TrustService.Domain.Entities;
using TrustService.Domain.ValueObjects;

namespace TrustService.Application.Features.Trusts.Commands;

// --- Create Trust ---
public record CreateTrustCommand : IRequest<string>
{
    public string TrustCode { get; init; } = string.Empty;
    public string TrustShortName { get; init; } = string.Empty;
    public string TrustType { get; init; } = string.Empty;
    public DateTime TrustStartDate { get; init; }
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string? AddressLine3 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PinCode { get; init; }
    public string? Country { get; init; }
    public string? PhoneNo { get; init; }
    public string? FaxNo { get; init; }
    public string? Email { get; init; }
    public string? RegistrarName { get; init; }
    public string? RegistrarPhone { get; init; }
}

public class CreateTrustCommandValidator : AbstractValidator<CreateTrustCommand>
{
    public CreateTrustCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TrustShortName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.TrustType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TrustStartDate).NotEmpty();
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class CreateTrustCommandHandler : IRequestHandler<CreateTrustCommand, string>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrustCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(CreateTrustCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsAsync(request.TrustCode, cancellationToken))
            throw new InvalidOperationException($"Trust with code '{request.TrustCode}' already exists.");

        var address = Address.Create(request.AddressLine1, request.AddressLine2, request.AddressLine3,
            request.City, request.State, request.PinCode, request.Country);
        var contact = ContactInfo.Create(request.PhoneNo, request.FaxNo, request.Email);

        var trust = TrustMaster.Create(request.TrustCode, request.TrustShortName, request.TrustType,
            request.TrustStartDate, address, contact, request.RegistrarName, request.RegistrarPhone);

        await _repository.AddAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return trust.TrustCode;
    }
}

// --- Update Trust ---
public record UpdateTrustCommand : IRequest
{
    public string TrustCode { get; init; } = string.Empty;
    public string TrustShortName { get; init; } = string.Empty;
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string? AddressLine3 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PinCode { get; init; }
    public string? Country { get; init; }
    public string? PhoneNo { get; init; }
    public string? FaxNo { get; init; }
    public string? Email { get; init; }
    public string? RegistrarName { get; init; }
    public string? RegistrarPhone { get; init; }
}

public class UpdateTrustCommandValidator : AbstractValidator<UpdateTrustCommand>
{
    public UpdateTrustCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TrustShortName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(200);
    }
}

public class UpdateTrustCommandHandler : IRequestHandler<UpdateTrustCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTrustCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTrustCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        var address = Address.Create(request.AddressLine1, request.AddressLine2, request.AddressLine3,
            request.City, request.State, request.PinCode, request.Country);
        var contact = ContactInfo.Create(request.PhoneNo, request.FaxNo, request.Email);

        trust.Update(request.TrustShortName, address, contact, request.RegistrarName, request.RegistrarPhone);

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Close Trust ---
public record CloseTrustCommand(string TrustCode, DateTime ClosureDate) : IRequest;

public class CloseTrustCommandHandler : IRequestHandler<CloseTrustCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseTrustCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CloseTrustCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        trust.Close(request.ClosureDate);

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

// --- Activate Trust ---
public record ActivateTrustCommand(string TrustCode) : IRequest;

public class ActivateTrustCommandHandler : IRequestHandler<ActivateTrustCommand>
{
    private readonly ITrustRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateTrustCommandHandler(ITrustRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActivateTrustCommand request, CancellationToken cancellationToken)
    {
        var trust = await _repository.GetByCodeAsync(request.TrustCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Trust '{request.TrustCode}' not found.");

        trust.Activate();

        await _repository.UpdateAsync(trust, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
