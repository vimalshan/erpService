using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.CommandHandlers;

/// <summary>
/// Handler for CreateEmailTypeCommand
/// </summary>
public class CreateEmailTypeCommandHandler : IRequestHandler<Commands.CreateEmailTypeCommand, long>
{
    private readonly IEmailTypeRepository _emailTypeRepository;

    /// <summary>
    /// Initializes a new instance of the CreateEmailTypeCommandHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    public CreateEmailTypeCommandHandler(IEmailTypeRepository emailTypeRepository)
    {
        _emailTypeRepository = emailTypeRepository;
    }

    /// <summary>
    /// Handles the CreateEmailTypeCommand
    /// </summary>
    /// <param name="request">The command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created email type ID</returns>
    public async Task<long> Handle(Commands.CreateEmailTypeCommand request, CancellationToken cancellationToken)
    {
        // Parse email type
        var emailType = request.EmailType == "D" 
            ? Domain.ValueObjects.EmailTypeEnum.Daily 
            : Domain.ValueObjects.EmailTypeEnum.Event;

        // Create aggregate
        var emailTypeAggregate = new Domain.Aggregates.EmailTypeAggregate(
            request.EmailName,
            emailType,
            request.EmailProcName,
            request.CreatedBy);

        // Add domain event
        emailTypeAggregate.AddDomainEvent(
            new Domain.Events.EmailTypeCreatedEvent(
                emailTypeAggregate.Id,
                request.EmailName,
                emailType,
                request.EmailProcName));

        // Save to repository
        await _emailTypeRepository.AddAsync(emailTypeAggregate, cancellationToken);

        return emailTypeAggregate.Id;
    }
}
