using AutoMapper;
using GroupManagementService.Application.DTOs;
using GroupManagementService.Domain.Entities;
using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Group mappings
            CreateMap<Group, GroupDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateGroupRequest, Group>()
                .ConstructUsing(src => new Group(src.Code, src.Name, src.Description, src.CreatedBy, src.IsAdmin));

            CreateMap<UpdateGroupRequest, Group>()
                .ForAllMembers(opts => opts.Ignore());

            // GroupMenuMap mappings
            CreateMap<GroupMenuMap, GroupMenuMapDto>();

            CreateMap<MenuPermissionsDto, MenuPermissions>()
                .ConstructUsing(src => new MenuPermissions(src.CanView, src.CanCreate, src.CanEdit, src.CanDelete, src.CanApprove));

            CreateMap<MenuPermissions, MenuPermissionsDto>();

            CreateMap<AddMenuMapRequest, GroupMenuMap>()
                .ConstructUsing(src => new GroupMenuMap(
                    0, // GroupId will be set separately
                    src.MenuCode,
                    src.MenuName,
                    new MenuPermissions(
                        src.Permissions.CanView,
                        src.Permissions.CanCreate,
                        src.Permissions.CanEdit,
                        src.Permissions.CanDelete,
                        src.Permissions.CanApprove),
                    src.CreatedBy,
                    src.MenuSequence));
        }
    }
}
