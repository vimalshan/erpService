using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.CommandHandlers;

/// <summary>
/// Handler for UpdateEmailTypeCommand
/// </summary>
public class UpdateEmailTypeCommandHandler : IRequestHandler<Commands.UpdateEmailTypeCommand, Unit>
{
    private readonly IEmailTypeRepository _emailTypeRepository;

    /// <summary>
    /// Initializes a new instance of the UpdateEmailTypeCommandHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    public UpdateEmailTypeCommandHandler(IEmailTypeRepository emailTypeRepository)
    {
        _emailTypeRepository = emailTypeRepository;
    }

    /// <summary>
    /// Handles the UpdateEmailTypeCommand
    /// </summary>
    /// <param name="request">The command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit</returns>
    /// <exception cref="InvalidOperationException">Thrown when email type is not found</exception>
    public async Task<Unit> Handle(Commands.UpdateEmailTypeCommand request, CancellationToken cancellationToken)
    {
        var emailType = await _emailTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (emailType == null)
            throw new InvalidOperationException($"Email type with ID {request.Id} not found");

        // Update aggregate
        emailType.Update(request.EmailName, request.EmailProcName, request.ModifiedBy);

        // Add domain event
        emailType.AddDomainEvent(
            new Domain.Events.EmailTypeUpdatedEvent(
                request.Id,
                request.EmailName,
                request.EmailProcName));

        // Save to repository
        await _emailTypeRepository.UpdateAsync(emailType, cancellationToken);

        return Unit.Value;
    }
}
