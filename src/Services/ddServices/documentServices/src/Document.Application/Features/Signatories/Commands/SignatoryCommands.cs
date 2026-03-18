using FluentValidation;
using MediatR;
using Document.Application.Common.Interfaces;
using Document.Application.DTOs;
using Document.Domain.Entities;

namespace Document.Application.Features.Signatories.Commands;

public record CreateSignatoryCommand(
    decimal SignatoryNumber,
    string Name,
    string Designation,
    decimal? EmployeeSysId = null,
    string? ImageFileName = null) : IRequest<SignatoryDto>;

public class CreateSignatoryCommandValidator : AbstractValidator<CreateSignatoryCommand>
{
    public CreateSignatoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SignatoryNumber).GreaterThan(0);
    }
}

public class CreateSignatoryCommandHandler : IRequestHandler<CreateSignatoryCommand, SignatoryDto>
{
    private readonly IApplicationDbContext _ctx;

    public CreateSignatoryCommandHandler(IApplicationDbContext ctx) => _ctx = ctx;

    public async Task<SignatoryDto> Handle(CreateSignatoryCommand request, CancellationToken cancellationToken)
    {
        var signatory = Signatory.Create(
            request.SignatoryNumber,
            request.Name,
            request.Designation,
            request.EmployeeSysId,
            request.ImageFileName);

        await _ctx.Signatories.AddAsync(signatory, cancellationToken);
        await _ctx.SaveChangesAsync(cancellationToken);

        return new SignatoryDto(
            signatory.SignatoryNumber,
            signatory.Name,
            signatory.Designation,
            signatory.LiveFlag,
            signatory.EmployeeSysId,
            signatory.ImageFileName);
    }
}

public record UpdateSignatoryCommand(decimal SignatoryNumber, string Name, string Designation, string? ImageFileName) : IRequest<bool>;

public class UpdateSignatoryCommandHandler : IRequestHandler<UpdateSignatoryCommand, bool>
{
    private readonly IApplicationDbContext _ctx;

    public UpdateSignatoryCommandHandler(IApplicationDbContext ctx) => _ctx = ctx;

    public async Task<bool> Handle(UpdateSignatoryCommand request, CancellationToken cancellationToken)
    {
        var signatory = await _ctx.Signatories.FindAsync([request.SignatoryNumber], cancellationToken);
        if (signatory == null) return false;
        signatory.Update(request.Name, request.Designation, request.ImageFileName);
        await _ctx.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public record DeleteSignatoryCommand(decimal SignatoryNumber) : IRequest<bool>;

public class DeleteSignatoryCommandHandler : IRequestHandler<DeleteSignatoryCommand, bool>
{
    private readonly IApplicationDbContext _ctx;

    public DeleteSignatoryCommandHandler(IApplicationDbContext ctx) => _ctx = ctx;

    public async Task<bool> Handle(DeleteSignatoryCommand request, CancellationToken cancellationToken)
    {
        var signatory = await _ctx.Signatories.FindAsync([request.SignatoryNumber], cancellationToken);
        if (signatory == null) return false;
        signatory.Deactivate();
        await _ctx.SaveChangesAsync(cancellationToken);
        return true;
    }
}
