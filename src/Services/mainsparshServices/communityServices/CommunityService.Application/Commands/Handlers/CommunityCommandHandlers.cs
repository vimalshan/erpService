namespace CommunityService.Application.Commands.Handlers;

using MediatR;
using AutoMapper;
using DTOs;
using Domain.Entities;

public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, CommunityDto>
{
    private readonly IMapper _mapper;

    public CreateCommunityCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CommunityDto> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var community = Community.Create(
            dto.CommunityCode,
            dto.CommunityName,
            dto.CommunityDescription,
            dto.CommunityType,
            dto.CommunityIcon,
            dto.CommunityBanner,
            dto.PrivacyLevel,
            dto.OwnerId,
            dto.OwnerId
        );

        // TODO: Save to repository
        await Task.CompletedTask;

        return _mapper.Map<CommunityDto>(community);
    }
}

public class UpdateCommunityCommandHandler : IRequestHandler<UpdateCommunityCommand, CommunityDto>
{
    private readonly IMapper _mapper;

    public UpdateCommunityCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CommunityDto> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        // TODO: Load from repository; using a placeholder community until persistence is wired
        var community = Community.Create(
            "PLACEHOLDER",
            dto.CommunityName,
            dto.CommunityDescription,
            "FORUM",
            null,
            null,
            dto.PrivacyLevel,
            1,
            1
        );

        community.Update(
            dto.CommunityName,
            dto.CommunityDescription,
            dto.PrivacyLevel,
            1 // TODO: Get from context
        );

        // TODO: Save to repository
        await Task.CompletedTask;
        return _mapper.Map<CommunityDto>(community);
    }
}

public class AddCommunityMemberCommandHandler : IRequestHandler<AddCommunityMemberCommand, CommunityMemberDto>
{
    private readonly IMapper _mapper;

    public AddCommunityMemberCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CommunityMemberDto> Handle(AddCommunityMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var member = CommunityMember.Create(
            dto.CommunityId,
            dto.UserId,
            dto.MemberRole,
            1 // TODO: Get from context
        );

        // TODO: Save to repository
        await Task.CompletedTask;

        return _mapper.Map<CommunityMemberDto>(member);
    }
}

public class RemoveCommunityMemberCommandHandler : IRequestHandler<RemoveCommunityMemberCommand, bool>
{
    public async Task<bool> Handle(RemoveCommunityMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        // TODO: Load member from repository
        // TODO: Call member.Remove()
        // TODO: Save to repository
        await Task.CompletedTask;
        return true;
    }
}

public class ChangeMemberRoleCommandHandler : IRequestHandler<ChangeMemberRoleCommand, CommunityMemberDto>
{
    private readonly IMapper _mapper;

    public ChangeMemberRoleCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<CommunityMemberDto> Handle(ChangeMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        // TODO: Load member from repository; using a placeholder member until persistence is wired
        var member = CommunityMember.Create(
            dto.CommunityId,
            dto.UserId,
            dto.NewRole,
            1 // TODO: Get from context
        );

        member.ChangeRole(dto.NewRole, 1); // TODO: Get updatedBy from context
        // TODO: Save to repository
        await Task.CompletedTask;
        return _mapper.Map<CommunityMemberDto>(member);
    }
}

public class ArchiveCommunityCommandHandler : IRequestHandler<ArchiveCommunityCommand, bool>
{
    public async Task<bool> Handle(ArchiveCommunityCommand request, CancellationToken cancellationToken)
    {
        // TODO: Load community from repository
        // TODO: Call community.Archive()
        // TODO: Save to repository
        await Task.CompletedTask;
        return true;
    }
}
