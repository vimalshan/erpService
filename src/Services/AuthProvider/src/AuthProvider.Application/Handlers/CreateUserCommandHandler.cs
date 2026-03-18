using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using AuthProvider.Application.Interfaces;
using AuthProvider.Domain.Entities;
using AuthProvider.Domain.Interfaces;
using AuthProvider.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Application.Handlers;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessagePublisher _publisher;
    private readonly IPasswordHasher _hasher;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork uow,
        IMessagePublisher publisher,
        IPasswordHasher hasher,
        ILogger<CreateUserCommandHandler> logger)
    {
        _uow = uow;
        _publisher = publisher;
        _hasher = hasher;
        _logger = logger;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Creating user {Username}", request.Username);

        if (await _uow.Users.ExistsAsync(request.Email, ct))
            throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

        var email = Email.Create(request.Email);
        var passwordHash = Password.FromHash(_hasher.Hash(request.Password));
        var user = User.Create(request.Username, email, passwordHash, request.FirstName, request.LastName);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        // Publish domain event via message queue (RabbitMQ)
        foreach (var domainEvent in user.DomainEvents)
            await _publisher.PublishAsync(domainEvent, ct);

        user.ClearDomainEvents();

        _logger.LogInformation("User {UserId} created successfully", user.Id);

        return MapToDto(user);
    }

    private static UserDto MapToDto(User user) =>
        new(user.Id, user.Username, user.Email.Value,
            user.FirstName, user.LastName,
            user.IsActive, user.IsEmailVerified,
            user.CreatedAt, user.LastLoginAt,
            user.UserRoles.Select(ur => ur.Role?.Name ?? string.Empty));
}
