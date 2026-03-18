using FluentValidation;
using MediatR;
using UserSecurityService.Application.Common;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Domain.Entities;
using UserSecurityService.Domain.Interfaces;

namespace UserSecurityService.Application.Features.UserProfile.Commands;

// ---------- Create ----------
public record CreateUserProfileCommand(
    string UserId,
    decimal EmpNum,
    string UnitCode,
    string NickName,
    string UserType,
    string EmailFlag,
    DateTime EffectiveDate,
    string PlainPassword,
    string RegStatus,
    string? EmpName = null,
    string? OfficeEmail = null,
    string? PersonalEmail = null
) : IRequest<UserProfileDto>;

public sealed class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
{
    public CreateUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.EmpNum).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.NickName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.UserType).NotEmpty().MaximumLength(1);
        RuleFor(x => x.EmailFlag).NotEmpty().MaximumLength(1);
        RuleFor(x => x.PlainPassword).NotEmpty().MinimumLength(8);
        RuleFor(x => x.RegStatus).NotEmpty().MaximumLength(1);
        RuleFor(x => x.OfficeEmail).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.OfficeEmail));
        RuleFor(x => x.PersonalEmail).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail));
    }
}

public sealed class CreateUserProfileCommandHandler(
    IUserProfileRepository repository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : IRequestHandler<CreateUserProfileCommand, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(CreateUserProfileCommand cmd, CancellationToken ct)
    {
        var hashed = passwordHasher.Hash(cmd.PlainPassword);
        var profile = UserProfilePfs.Create(
            cmd.UserId, cmd.EmpNum, cmd.UnitCode, cmd.NickName,
            cmd.UserType, cmd.EmailFlag, cmd.EffectiveDate, hashed,
            cmd.RegStatus, cmd.EmpName);

        await repository.AddAsync(profile, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new UserProfileDto(
            profile.EmUsrId, profile.EmEmpNum, profile.EmUntCod, profile.EmNickNam,
            profile.EmUsrTyp, profile.EmEmlFlg, profile.EmOEmlId, profile.EmPEmlId,
            profile.EmEffDat, profile.EmClsDat, profile.EmEmpNam,
            profile.EmFrsNam, profile.EmMidNam, profile.EmLstNam,
            profile.EmEmpDsg, profile.EmDivNam, profile.EmPhtPth, profile.EmRegStatus);
    }
}

// ---------- Update ----------
public record UpdateUserProfileCommand(
    string UserId,
    string NickName,
    string? EmpName,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? OfficeEmail,
    string? PersonalEmail,
    string? Designation
) : IRequest;

public sealed class UpdateUserProfileCommandHandler(
    IUserProfileRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserProfileCommand>
{
    public async Task Handle(UpdateUserProfileCommand cmd, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(cmd.UserId, ct)
            ?? throw new KeyNotFoundException($"User '{cmd.UserId}' not found.");

        profile.UpdateProfile(cmd.NickName, cmd.EmpName, cmd.FirstName,
            cmd.MiddleName, cmd.LastName, cmd.OfficeEmail, cmd.PersonalEmail, cmd.Designation);

        repository.Update(profile);
        await unitOfWork.SaveChangesAsync(ct);
    }
}

// ---------- Deactivate ----------
public record DeactivateUserCommand(string UserId) : IRequest;

public sealed class DeactivateUserCommandHandler(
    IUserProfileRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeactivateUserCommand>
{
    public async Task Handle(DeactivateUserCommand cmd, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(cmd.UserId, ct)
            ?? throw new KeyNotFoundException($"User '{cmd.UserId}' not found.");

        profile.Deactivate();
        repository.Update(profile);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
