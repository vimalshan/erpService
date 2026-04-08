using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateDirectEntry;

public sealed record CreateDirectEntryCommand(
    long? TrackingNumber, string? EnteredUser) : IRequest<long>;

public sealed class CreateDirectEntryCommandValidator : AbstractValidator<CreateDirectEntryCommand>
{
    public CreateDirectEntryCommandValidator()
    {
        RuleFor(x => x.EnteredUser).MaximumLength(50).When(x => x.EnteredUser is not null);
    }
}

public sealed class CreateDirectEntryCommandHandler(IDirectEntryRepository repository)
    : IRequestHandler<CreateDirectEntryCommand, long>
{
    public async Task<long> Handle(CreateDirectEntryCommand request, CancellationToken cancellationToken)
    {
        var entity = VehicleDirectEntryEntity.Create(
            request.TrackingNumber, request.EnteredUser);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
