namespace CommunityService.Application.Commands.Validators;

using FluentValidation;
using DTOs;

public class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
{
    public CreateCommunityCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        
        RuleFor(x => x.Dto.CommunityCode)
            .NotEmpty().WithMessage("Community code is required.")
            .MaximumLength(50).WithMessage("Community code cannot exceed 50 characters.")
            .Matches(@"^[A-Z0-9\-]+$").WithMessage("Community code must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.Dto.CommunityName)
            .NotEmpty().WithMessage("Community name is required.")
            .MaximumLength(255).WithMessage("Community name cannot exceed 255 characters.");

        RuleFor(x => x.Dto.CommunityType)
            .NotEmpty().WithMessage("Community type is required.")
            .Must(x => new[] { "FORUM", "INTEREST_GROUP", "TEAM", "DEPARTMENT" }.Contains(x.ToUpper()))
            .WithMessage("Invalid community type.");

        RuleFor(x => x.Dto.PrivacyLevel)
            .NotEmpty().WithMessage("Privacy level is required.")
            .Must(x => new[] { "PUBLIC", "PRIVATE", "RESTRICTED" }.Contains(x.ToUpper()))
            .WithMessage("Invalid privacy level.");

        RuleFor(x => x.Dto.OwnerId)
            .GreaterThan(0).WithMessage("Owner ID must be greater than 0.");
    }
}

public class UpdateCommunityCommandValidator : AbstractValidator<UpdateCommunityCommand>
{
    public UpdateCommunityCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        
        RuleFor(x => x.Dto.CommunityId)
            .GreaterThan(0).WithMessage("Community ID must be greater than 0.");

        RuleFor(x => x.Dto.CommunityName)
            .NotEmpty().WithMessage("Community name is required.")
            .MaximumLength(255).WithMessage("Community name cannot exceed 255 characters.");

        RuleFor(x => x.Dto.PrivacyLevel)
            .NotEmpty().WithMessage("Privacy level is required.")
            .Must(x => new[] { "PUBLIC", "PRIVATE", "RESTRICTED" }.Contains(x.ToUpper()))
            .WithMessage("Invalid privacy level.");
    }
}

public class AddCommunityMemberCommandValidator : AbstractValidator<AddCommunityMemberCommand>
{
    public AddCommunityMemberCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        
        RuleFor(x => x.Dto.CommunityId)
            .GreaterThan(0).WithMessage("Community ID must be greater than 0.");

        RuleFor(x => x.Dto.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Dto.MemberRole)
            .NotEmpty().WithMessage("Member role is required.")
            .Must(x => new[] { "ADMIN", "MODERATOR", "MEMBER", "GUEST" }.Contains(x.ToUpper()))
            .WithMessage("Invalid member role.");
    }
}

public class RemoveCommunityMemberCommandValidator : AbstractValidator<RemoveCommunityMemberCommand>
{
    public RemoveCommunityMemberCommandValidator()
    {
        RuleFor(x => x.Dto.CommunityId)
            .GreaterThan(0).WithMessage("Community ID must be greater than 0.");

        RuleFor(x => x.Dto.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class ChangeMemberRoleCommandValidator : AbstractValidator<ChangeMemberRoleCommand>
{
    public ChangeMemberRoleCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull();
        
        RuleFor(x => x.Dto.CommunityId)
            .GreaterThan(0).WithMessage("Community ID must be greater than 0.");

        RuleFor(x => x.Dto.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Dto.NewRole)
            .NotEmpty().WithMessage("New role is required.")
            .Must(x => new[] { "ADMIN", "MODERATOR", "MEMBER", "GUEST" }.Contains(x.ToUpper()))
            .WithMessage("Invalid member role.");
    }
}
