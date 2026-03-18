namespace CommunityService.Application.Commands;

using MediatR;
using DTOs;

public record CreateCommunityCommand(CreateCommunityDto Dto) : IRequest<CommunityDto>;

public record UpdateCommunityCommand(UpdateCommunityDto Dto) : IRequest<CommunityDto>;

public record AddCommunityMemberCommand(AddMemberDto Dto) : IRequest<CommunityMemberDto>;

public record RemoveCommunityMemberCommand(RemoveMemberDto Dto) : IRequest<bool>;

public record ChangeMemberRoleCommand(ChangeMemberRoleDto Dto) : IRequest<CommunityMemberDto>;

public record ArchiveCommunityCommand(long CommunityId) : IRequest<bool>;
