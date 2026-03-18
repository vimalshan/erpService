using MediatR;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Features.WebsiteContact.Queries;

public record GetWebsiteContactByIdQuery(long ContactId) : IRequest<WebsiteContactDto>;

public class GetWebsiteContactByIdQueryHandler(IWebsiteContactRepository repository)
    : IRequestHandler<GetWebsiteContactByIdQuery, WebsiteContactDto>
{
    public async Task<WebsiteContactDto> Handle(GetWebsiteContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await repository.GetByIdAsync(request.ContactId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.WebsiteContactEmail), request.ContactId);

        return new WebsiteContactDto(
            contact.ContactId, contact.UserSysId, contact.PrimaryEmail, contact.SecondaryEmail,
            contact.Phone, contact.Mobile, contact.Website, contact.SocialMedia,
            contact.NewsletterOptIn.ToString(), contact.ContactStatus.ToString(),
            contact.CreatedBy, contact.CreatedOn, contact.UpdatedBy, contact.UpdatedOn);
    }
}

public record GetContactsByUserSysIdQuery(long UserSysId) : IRequest<IEnumerable<WebsiteContactDto>>;

public class GetContactsByUserSysIdQueryHandler(IWebsiteContactRepository repository)
    : IRequestHandler<GetContactsByUserSysIdQuery, IEnumerable<WebsiteContactDto>>
{
    public async Task<IEnumerable<WebsiteContactDto>> Handle(GetContactsByUserSysIdQuery request, CancellationToken cancellationToken)
    {
        var contacts = await repository.GetByUserSysIdAsync(request.UserSysId, cancellationToken);

        return contacts.Select(c => new WebsiteContactDto(
            c.ContactId, c.UserSysId, c.PrimaryEmail, c.SecondaryEmail,
            c.Phone, c.Mobile, c.Website, c.SocialMedia,
            c.NewsletterOptIn.ToString(), c.ContactStatus.ToString(),
            c.CreatedBy, c.CreatedOn, c.UpdatedBy, c.UpdatedOn));
    }
}
