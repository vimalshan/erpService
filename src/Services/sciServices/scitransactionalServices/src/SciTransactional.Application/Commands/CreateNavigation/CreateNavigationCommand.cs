using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateNavigation;

public sealed record CreateNavigationCommand(
    long RequestNum, string UserId, long UserNum,
    string? RandomNum, string SciId, string? StatusFlag) : IRequest<long>;

public sealed class CreateNavigationCommandValidator : AbstractValidator<CreateNavigationCommand>
{
    public CreateNavigationCommandValidator()
    {
        RuleFor(x => x.RequestNum).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.SciId).NotEmpty().MaximumLength(1);
    }
}

public sealed class CreateNavigationCommandHandler(INavigationRepository repository)
    : IRequestHandler<CreateNavigationCommand, long>
{
    public async Task<long> Handle(CreateNavigationCommand request, CancellationToken cancellationToken)
    {
        var entity = SparshNavigationEntity.Create(
            request.RequestNum, request.UserId, request.UserNum,
            request.RandomNum, request.SciId, request.StatusFlag);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
