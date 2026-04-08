using FluentValidation;
using MediatR;
using SciTransactional.Domain.Entities;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Commands.CreateNorm;

public sealed record CreateNormCommand(
    DateTime EffectiveDate,
    List<CreateNormDetailItem>? Details) : IRequest<long>;

public sealed record CreateNormDetailItem(
    long NormId, int? InputCode, int? OutputCode, int? Rate);

public sealed class CreateNormCommandValidator : AbstractValidator<CreateNormCommand>
{
    public CreateNormCommandValidator()
    {
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

public sealed class CreateNormCommandHandler(INormsRepository repository)
    : IRequestHandler<CreateNormCommand, long>
{
    public async Task<long> Handle(CreateNormCommand request, CancellationToken cancellationToken)
    {
        var entity = NormsMainEntity.Create(request.EffectiveDate);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        if (request.Details is { Count: > 0 })
        {
            foreach (var detail in request.Details)
            {
                var detailEntity = NormsMasterEntity.Create(
                    detail.NormId, detail.InputCode, detail.OutputCode,
                    detail.Rate, entity.Id);
                entity.AddDetail(detailEntity);
                await repository.AddDetailAsync(detailEntity, cancellationToken);
            }
            await repository.SaveChangesAsync(cancellationToken);
        }

        return entity.Id;
    }
}
