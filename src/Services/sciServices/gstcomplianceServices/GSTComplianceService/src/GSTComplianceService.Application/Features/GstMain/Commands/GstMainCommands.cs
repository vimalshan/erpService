using FluentValidation;
using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using MediatR;

namespace GSTComplianceService.Application.Features.GstMain.Commands;

// ── Register GST ──────────────────────────────────────────────────
public record RegisterGstCommand(
    string PanNo,
    string? Type,
    string? Email,
    string? Mobile,
    long RegisteredBy) : IRequest<long>;

public class RegisterGstCommandValidator : AbstractValidator<RegisterGstCommand>
{
    public RegisterGstCommandValidator()
    {
        RuleFor(x => x.PanNo)
            .NotEmpty().WithMessage("PAN number is required.")
            .Matches(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$").WithMessage("Invalid PAN number format.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email address.");

        RuleFor(x => x.RegisteredBy)
            .GreaterThan(0).WithMessage("RegisteredBy must be a valid user ID.");
    }
}

public class RegisterGstCommandHandler : IRequestHandler<RegisterGstCommand, long>
{
    private readonly IGstMainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterGstCommandHandler(IGstMainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(RegisterGstCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByPanNoAsync(request.PanNo, cancellationToken))
            throw new Domain.Exceptions.DuplicatePanException(request.PanNo);

        var entity = Domain.Entities.GstMain.Create(
            request.PanNo, request.Type, request.Email, request.Mobile, request.RegisteredBy);

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.GstId;
    }
}

// ── Update GST Vendor Info ─────────────────────────────────────────
public record UpdateGstVendorCommand(
    long GstId,
    string? VendorName,
    string? AddLine1,
    string? AddLine2,
    string? City,
    string? State,
    string? Pincode) : IRequest<Unit>;

public class UpdateGstVendorCommandHandler : IRequestHandler<UpdateGstVendorCommand, Unit>
{
    private readonly IGstMainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGstVendorCommandHandler(IGstMainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateGstVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(GstMain), request.GstId);

        entity.UpdateVendorInfo(request.VendorName, request.AddLine1, request.AddLine2,
            request.City, request.State, request.Pincode);

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

// ── Activate / Deactivate ──────────────────────────────────────────
public record ActivateGstCommand(long GstId) : IRequest<Unit>;

public class ActivateGstCommandHandler : IRequestHandler<ActivateGstCommand, Unit>
{
    private readonly IGstMainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateGstCommandHandler(IGstMainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ActivateGstCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(GstMain), request.GstId);
        entity.Activate();
        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public record DeactivateGstCommand(long GstId) : IRequest<Unit>;

public class DeactivateGstCommandHandler : IRequestHandler<DeactivateGstCommand, Unit>
{
    private readonly IGstMainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateGstCommandHandler(IGstMainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeactivateGstCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(GstMain), request.GstId);
        entity.Deactivate();
        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

// ── Delete GST ────────────────────────────────────────────────────
public record DeleteGstCommand(long GstId) : IRequest<Unit>;

public class DeleteGstCommandHandler : IRequestHandler<DeleteGstCommand, Unit>
{
    private readonly IGstMainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGstCommandHandler(IGstMainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteGstCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GstId, cancellationToken)
            ?? throw new Common.Exceptions.NotFoundException(nameof(GstMain), request.GstId);
        await _repository.DeleteAsync(entity.GstId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
