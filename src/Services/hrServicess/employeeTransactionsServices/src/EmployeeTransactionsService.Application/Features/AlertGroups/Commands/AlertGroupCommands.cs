using EmployeeTransactionsService.Application.DTOs;
using EmployeeTransactionsService.Domain.Entities;
using EmployeeTransactionsService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeTransactionsService.Application.Features.AlertGroups.Commands;

public sealed record AlertGroupRecipientInput(
    decimal? EmpSysId,
    string? EmailId,
    decimal OrgId,
    decimal UnitId,
    decimal? CalendarId,
    DateTime EffectiveDate,
    DateTime? CloseDate);

public sealed record CreateAlertGroupCommand(
    string Name,
    string Type,
    decimal CreatedBy,
    IReadOnlyList<AlertGroupRecipientInput> Recipients) : IRequest<decimal>;

public sealed class CreateAlertGroupCommandValidator : AbstractValidator<CreateAlertGroupCommand>
{
    public CreateAlertGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Type).Must(type => type is "R" or "P" or "C");
        RuleForEach(x => x.Recipients).ChildRules(recipient =>
        {
            recipient.RuleFor(x => x.OrgId).GreaterThanOrEqualTo(0);
            recipient.RuleFor(x => x.UnitId).GreaterThanOrEqualTo(0);
        });
    }
}

public sealed class CreateAlertGroupCommandHandler(IAlertGroupRepository alertGroupRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAlertGroupCommand, decimal>
{
    public async Task<decimal> Handle(CreateAlertGroupCommand request, CancellationToken cancellationToken)
    {
        var groupId = await alertGroupRepository.GetNextIdAsync(cancellationToken);
        var group = AlertGroup.Create(groupId, request.Name, request.Type, request.CreatedBy);

        foreach (var recipient in request.Recipients)
        {
            var mapId = await alertGroupRepository.GetNextMapIdAsync(cancellationToken);
            group.AddRecipient(
                mapId,
                recipient.EmpSysId,
                recipient.EmailId,
                recipient.OrgId,
                recipient.UnitId,
                recipient.CalendarId,
                recipient.EffectiveDate,
                recipient.CloseDate,
                request.CreatedBy);
        }

        await alertGroupRepository.AddAsync(group, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return groupId;
    }
}