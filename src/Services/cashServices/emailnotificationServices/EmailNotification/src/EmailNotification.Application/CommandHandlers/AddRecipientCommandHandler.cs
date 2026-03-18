using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.CommandHandlers;

/// <summary>
/// Handler for AddRecipientCommand
/// </summary>
public class AddRecipientCommandHandler : IRequestHandler<Commands.AddRecipientCommand, long>
{
    private readonly IEmailTypeRepository _emailTypeRepository;
    private readonly IMailAccessRepository _mailAccessRepository;

    /// <summary>
    /// Initializes a new instance of the AddRecipientCommandHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    /// <param name="mailAccessRepository">Mail access repository</param>
    public AddRecipientCommandHandler(
        IEmailTypeRepository emailTypeRepository,
        IMailAccessRepository mailAccessRepository)
    {
        _emailTypeRepository = emailTypeRepository;
        _mailAccessRepository = mailAccessRepository;
    }

    /// <summary>
    /// Handles the AddRecipientCommand
    /// </summary>
    /// <param name="request">The command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created mail access ID</returns>
    /// <exception cref="InvalidOperationException">Thrown when email type is not found</exception>
    public async Task<long> Handle(Commands.AddRecipientCommand request, CancellationToken cancellationToken)
    {
        // Validate email type exists
        var emailType = await _emailTypeRepository.GetByIdAsync(request.EmailTypeId, cancellationToken);
        if (emailType == null)
            throw new InvalidOperationException($"Email type with ID {request.EmailTypeId} not found");

        // Create email address value object
        var emailAddress = new Domain.ValueObjects.EmailAddress(request.EmailAddress);

        // Create mail access entity
        var mailAccess = new Domain.Entities.MailAccess(
            request.EmailTypeId,
            emailAddress,
            request.CreatedBy,
            request.OrgId,
            request.BusinessId,
            request.EmployeeSysId,
            request.RecipientName);

        // Add to aggregate
        emailType.AddRecipient(mailAccess);

        // Add domain event
        emailType.AddDomainEvent(
            new Domain.Events.RecipientAddedEvent(
                request.EmailTypeId,
                request.EmailAddress,
                request.OrgId,
                request.BusinessId));

        // Save mail access
        await _mailAccessRepository.AddAsync(mailAccess, cancellationToken);

        // Update email type
        await _emailTypeRepository.UpdateAsync(emailType, cancellationToken);

        return mailAccess.Id;
    }
}
