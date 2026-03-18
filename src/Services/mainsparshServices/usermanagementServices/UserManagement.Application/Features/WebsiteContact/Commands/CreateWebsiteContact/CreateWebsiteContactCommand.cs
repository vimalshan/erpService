using FluentValidation;
using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Features.WebsiteContact.Commands.CreateWebsiteContact;

public record CreateWebsiteContactCommand(
    long UserSysId,
    string PrimaryEmail,
    long CreatedBy,
    string? SecondaryEmail = null,
    string? Phone = null,
    string? Mobile = null,
    string? Website = null,
    string? SocialMedia = null,
    bool NewsletterOptIn = true) : IRequest<WebsiteContactDto>;

public class CreateWebsiteContactCommandValidator : AbstractValidator<CreateWebsiteContactCommand>
{
    public CreateWebsiteContactCommandValidator()
    {
        RuleFor(x => x.UserSysId).GreaterThan(0);
        RuleFor(x => x.PrimaryEmail).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.SecondaryEmail).EmailAddress().MaximumLength(255).When(x => !string.IsNullOrEmpty(x.SecondaryEmail));
        RuleFor(x => x.Phone).MaximumLength(20).When(x => x.Phone is not null);
        RuleFor(x => x.Mobile).MaximumLength(20).When(x => x.Mobile is not null);
        RuleFor(x => x.Website).MaximumLength(255).When(x => x.Website is not null);
    }
}

public class CreateWebsiteContactCommandHandler(
    IWebsiteContactRepository repository,
    MediatR.IPublisher publisher)
    : IRequestHandler<CreateWebsiteContactCommand, WebsiteContactDto>
{
    public async Task<WebsiteContactDto> Handle(CreateWebsiteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = WebsiteContactEmail.Create(
            request.UserSysId, request.PrimaryEmail, request.CreatedBy,
            request.SecondaryEmail, request.Phone, request.Mobile,
            request.Website, request.SocialMedia, request.NewsletterOptIn);

        var saved = await repository.AddAsync(contact, cancellationToken);

        foreach (var domainEvent in saved.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        saved.ClearDomainEvents();

        return MapToDto(saved);
    }

    private static WebsiteContactDto MapToDto(WebsiteContactEmail c) => new(
        c.ContactId, c.UserSysId, c.PrimaryEmail, c.SecondaryEmail,
        c.Phone, c.Mobile, c.Website, c.SocialMedia,
        c.NewsletterOptIn.ToString(), c.ContactStatus.ToString(),
        c.CreatedBy, c.CreatedOn, c.UpdatedBy, c.UpdatedOn);
}
