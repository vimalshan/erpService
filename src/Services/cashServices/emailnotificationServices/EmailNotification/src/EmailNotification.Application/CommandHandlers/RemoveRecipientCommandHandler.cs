using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.CommandHandlers;

/// <summary>
/// Handler for RemoveRecipientCommand
/// </summary>
public class RemoveRecipientCommandHandler : IRequestHandler<Commands.RemoveRecipientCommand, Unit>
{
    private readonly IMailAccessRepository _mailAccessRepository;

    /// <summary>
    /// Initializes a new instance of the RemoveRecipientCommandHandler class
    /// </summary>
    /// <param name="mailAccessRepository">Mail access repository</param>
    public RemoveRecipientCommandHandler(IMailAccessRepository mailAccessRepository)
    {
        _mailAccessRepository = mailAccessRepository;
    }

    /// <summary>
    /// Handles the RemoveRecipientCommand
    /// </summary>
    /// <param name="request">The command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit</returns>
    /// <exception cref="InvalidOperationException">Thrown when mail access is not found</exception>
    public async Task<Unit> Handle(Commands.RemoveRecipientCommand request, CancellationToken cancellationToken)
    {
        var mailAccess = await _mailAccessRepository.GetByIdAsync(request.MailAccessId, cancellationToken);
        if (mailAccess == null)
            throw new InvalidOperationException($"Mail access with ID {request.MailAccessId} not found");

        // Delete from repository
        await _mailAccessRepository.DeleteAsync(request.MailAccessId, cancellationToken);

        return Unit.Value;
    }
}
