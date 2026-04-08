using FluentValidation;
using MediatR;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CloseNorm;

public sealed record CloseNormCommand(long NormNo) : IRequest;

public sealed class CloseNormCommandValidator : AbstractValidator<CloseNormCommand>
{
    public CloseNormCommandValidator()
    {
        RuleFor(x => x.NormNo).GreaterThan(0);
    }
}

public sealed class CloseNormCommandHandler(INormsRepository repository)
    : IRequestHandler<CloseNormCommand>
{
    public async Task Handle(CloseNormCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.NormNo, cancellationToken)
            ?? throw new KeyNotFoundException($"Norm {request.NormNo} not found.");

        entity.Close();
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
