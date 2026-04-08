using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateAutoMail;

public sealed record CreateAutoMailStatusCommand(
    string MailType, DateTime MailDate, string MailStatus, string? MailRemarks) : IRequest<int>;

public sealed class CreateAutoMailStatusCommandValidator : AbstractValidator<CreateAutoMailStatusCommand>
{
    public CreateAutoMailStatusCommandValidator()
    {
        RuleFor(x => x.MailType).NotEmpty().MaximumLength(25);
        RuleFor(x => x.MailStatus).NotEmpty().MaximumLength(1);
    }
}

public sealed class CreateAutoMailStatusCommandHandler(IAutoMailRepository repository)
    : IRequestHandler<CreateAutoMailStatusCommand, int>
{
    public async Task<int> Handle(CreateAutoMailStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = AutoMailStatusEntity.Create(
            request.MailType, request.MailDate, request.MailStatus, request.MailRemarks);

        await repository.AddStatusAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
