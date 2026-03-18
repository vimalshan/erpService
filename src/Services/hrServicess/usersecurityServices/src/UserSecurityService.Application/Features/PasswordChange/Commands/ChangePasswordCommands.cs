using FluentValidation;
using MediatR;
using UserSecurityService.Application.Common;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Domain.Entities;
using UserSecurityService.Domain.Interfaces;

namespace UserSecurityService.Application.Features.PasswordChange.Commands;

public record ChangePasswordCommand(
    string UserId,
    decimal EmpSysId,
    string CurrentPassword,
    string NewPassword,
    decimal ChangedBy
) : IRequest;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8)
            .NotEqual(x => x.CurrentPassword).WithMessage("New password must differ from the current password.");
    }
}

public sealed class ChangePasswordCommandHandler(
    IUserProfileRepository profileRepository,
    IEmpPasswordChangeRepository pwdRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(ChangePasswordCommand cmd, CancellationToken ct)
    {
        var profile = await profileRepository.GetByIdAsync(cmd.UserId, ct)
            ?? throw new KeyNotFoundException($"User '{cmd.UserId}' not found.");

        if (!passwordHasher.Verify(cmd.CurrentPassword, profile.EmUsrPass))
            throw new UnauthorizedAccessException("Current password is incorrect.");

        var hashed = passwordHasher.Hash(cmd.NewPassword);
        profile.ChangePassword(hashed);

        var logId = (decimal)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var log = EmpPasswordChange.Create(logId, cmd.EmpSysId, cmd.ChangedBy);

        profileRepository.Update(profile);
        await pwdRepository.AddAsync(log, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
