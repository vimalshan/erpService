using FluentValidation;
using MediatR;
using UserManagement.Application.Common.Exceptions;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Features.WebsiteContact.Commands.UpdateWebsiteContact;

public record UpdateWebsiteContactCommand(
    long ContactId,
    string? SecondaryEmail,
    string? Phone,
    string? Mobile,
    string? Website,
    string? SocialMedia,
    bool NewsletterOptIn,
    long UpdatedBy) : IRequest<WebsiteContactDto>;

public class UpdateWebsiteContactCommandValidator : AbstractValidator<UpdateWebsiteContactCommand>
{
    public UpdateWebsiteContactCommandValidator()
    {
        RuleFor(x => x.ContactId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
        RuleFor(x => x.SecondaryEmail).EmailAddress().MaximumLength(255).When(x => !string.IsNullOrEmpty(x.SecondaryEmail));
    }
}

public class UpdateWebsiteContactCommandHandler(
    IWebsiteContactRepository repository,
    MediatR.IPublisher publisher)
    : IRequestHandler<UpdateWebsiteContactCommand, WebsiteContactDto>
{
    public async Task<WebsiteContactDto> Handle(UpdateWebsiteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await repository.GetByIdAsync(request.ContactId, cancellationToken)
            ?? throw new NotFoundException(nameof(WebsiteContactEmail), request.ContactId);

        contact.Update(
            request.SecondaryEmail, request.Phone, request.Mobile,
            request.Website, request.SocialMedia, request.NewsletterOptIn, request.UpdatedBy);

        var updated = await repository.UpdateAsync(contact, cancellationToken);

        foreach (var domainEvent in updated.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        updated.ClearDomainEvents();

        return new WebsiteContactDto(
            updated.ContactId, updated.UserSysId, updated.PrimaryEmail, updated.SecondaryEmail,
            updated.Phone, updated.Mobile, updated.Website, updated.SocialMedia,
            updated.NewsletterOptIn.ToString(), updated.ContactStatus.ToString(),
            updated.CreatedBy, updated.CreatedOn, updated.UpdatedBy, updated.UpdatedOn);
    }
}
