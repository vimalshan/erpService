using EmailNotification.Application.Dtos;
using EmailNotification.Application.Queries;
using EmailNotification.Application.Commands;
using MediatR;

namespace EmailNotification.API.GraphQL;

// ── GraphQL Query Type ──────────────────────────────────────────────────────

public class EmailNotificationQuery
{
    public async Task<IEnumerable<EmailTypeDto>> GetEmailTypes(IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllEmailTypesQuery(), ct);

    public async Task<EmailTypeDto?> GetEmailType(long id, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetEmailTypeByIdQuery(id), ct);

    public async Task<IEnumerable<EmailTypeDto>> GetEmailTypesByType(string emailType, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetEmailTypesByTypeQuery(emailType), ct);

    public async Task<IEnumerable<MailAccessDto>> GetRecipients(
        long emailTypeId, long orgId, long? businessId, IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetRecipientsByOrgAndBusinessQuery(emailTypeId, orgId, businessId), ct);
}

// ── GraphQL Mutation Type ───────────────────────────────────────────────────

public class EmailNotificationMutation
{
    public async Task<long> CreateEmailType(
        [Service] IMediator mediator,
        CreateEmailTypeInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateEmailTypeCommand
        {
            EmailName = input.EmailName,
            EmailType = input.EmailType,
            EmailProcName = input.EmailProcName,
            CreatedBy = input.CreatedBy
        }, ct);

    public async Task<bool> UpdateEmailType(
        [Service] IMediator mediator,
        UpdateEmailTypeInput input,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateEmailTypeCommand
        {
            Id = input.Id,
            EmailName = input.EmailName,
            EmailProcName = input.EmailProcName,
            ModifiedBy = input.ModifiedBy
        }, ct);
        return true;
    }

    public async Task<long> AddRecipient(
        [Service] IMediator mediator,
        AddRecipientInput input,
        CancellationToken ct)
        => await mediator.Send(new AddRecipientCommand
        {
            EmailTypeId = input.EmailTypeId,
            EmailAddress = input.EmailAddress,
            OrgId = input.OrgId,
            BusinessId = input.BusinessId,
            EmployeeSysId = input.EmployeeSysId,
            RecipientName = input.RecipientName,
            CreatedBy = input.CreatedBy
        }, ct);

    public async Task<bool> RemoveRecipient(
        [Service] IMediator mediator,
        long mailAccessId, long modifiedBy,
        CancellationToken ct)
    {
        await mediator.Send(new RemoveRecipientCommand
        {
            MailAccessId = mailAccessId,
            ModifiedBy = modifiedBy
        }, ct);
        return true;
    }
}

// ── GraphQL Input Types ─────────────────────────────────────────────────────

public record CreateEmailTypeInput(
    string EmailName,
    string EmailType,
    string EmailProcName,
    long CreatedBy);

public record UpdateEmailTypeInput(
    long Id,
    string EmailName,
    string EmailProcName,
    long ModifiedBy);

public record AddRecipientInput(
    long EmailTypeId,
    string EmailAddress,
    long? OrgId,
    long? BusinessId,
    long? EmployeeSysId,
    string? RecipientName,
    long CreatedBy);
