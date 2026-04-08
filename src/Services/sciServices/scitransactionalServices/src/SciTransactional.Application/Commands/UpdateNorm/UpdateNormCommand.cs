using FluentValidation;
using MediatR;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.UpdateNorm;

public sealed record UpdateNormCommand(
    long NormId, int? InputCode, int? OutputCode, int? Rate) : IRequest;

public sealed class UpdateNormCommandValidator : AbstractValidator<UpdateNormCommand>
{
    public UpdateNormCommandValidator()
    {
        RuleFor(x => x.NormId).GreaterThan(0);
    }
}

public sealed class UpdateNormCommandHandler(INormsRepository repository)
    : IRequestHandler<UpdateNormCommand>
{
    public async Task Handle(UpdateNormCommand request, CancellationToken cancellationToken)
    {
        var details = await repository.GetDetailsByNormNoAsync(0, cancellationToken);
        var detail = details.FirstOrDefault(d => d.Id == request.NormId)
            ?? throw new KeyNotFoundException($"Norm detail {request.NormId} not found.");

        detail.Update(request.InputCode, request.OutputCode, request.Rate);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
