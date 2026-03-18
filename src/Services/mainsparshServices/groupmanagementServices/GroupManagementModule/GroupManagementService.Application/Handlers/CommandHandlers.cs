using AutoMapper;
using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.DTOs;
using GroupManagementService.Domain.Entities;
using GroupManagementService.Domain.Repositories;
using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Application.Handlers
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public CreateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var existingGroup = await _groupRepository.CodeExistsAsync(request.Code, cancellationToken);
            if (existingGroup)
                throw new InvalidOperationException($"Group with code '{request.Code}' already exists");

            var group = new Group(request.Code, request.Name, request.Description, request.CreatedBy, request.IsAdmin);
            await _groupRepository.AddAsync(group, cancellationToken);

            return _mapper.Map<GroupDto>(group);
        }
    }

    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public UpdateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            group.Update(request.Name, request.Description, request.UpdatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return _mapper.Map<GroupDto>(group);
        }
    }

    public class ActivateGroupCommandHandler : IRequestHandler<ActivateGroupCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public ActivateGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(ActivateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            group.Activate(request.UpdatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return true;
        }
    }

    public class DeactivateGroupCommandHandler : IRequestHandler<DeactivateGroupCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public DeactivateGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(DeactivateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            group.Deactivate(request.UpdatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return true;
        }
    }

    public class AddMenuMapCommandHandler : IRequestHandler<AddMenuMapCommand, GroupMenuMapDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public AddMenuMapCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupMenuMapDto> Handle(AddMenuMapCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            var permissions = new MenuPermissions(
                request.Permissions.CanView,
                request.Permissions.CanCreate,
                request.Permissions.CanEdit,
                request.Permissions.CanDelete,
                request.Permissions.CanApprove);

            var menuMap = new GroupMenuMap(
                group.Id,
                request.MenuCode,
                request.MenuName,
                permissions,
                request.CreatedBy,
                request.MenuSequence);

            group.AddMenuMap(menuMap, request.CreatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return _mapper.Map<GroupMenuMapDto>(menuMap);
        }
    }

    public class RemoveMenuMapCommandHandler : IRequestHandler<RemoveMenuMapCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;

        public RemoveMenuMapCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<bool> Handle(RemoveMenuMapCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            group.RemoveMenuMap(request.MenuCode, request.UpdatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return true;
        }
    }

    public class UpdateMenuPermissionsCommandHandler : IRequestHandler<UpdateMenuPermissionsCommand, GroupMenuMapDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public UpdateMenuPermissionsCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupMenuMapDto> Handle(UpdateMenuPermissionsCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            var permissions = new MenuPermissions(
                request.Permissions.CanView,
                request.Permissions.CanCreate,
                request.Permissions.CanEdit,
                request.Permissions.CanDelete,
                request.Permissions.CanApprove);

            var menuMap = group.MenuMaps.FirstOrDefault(m => m.MenuCode == request.MenuCode);
            if (menuMap == null)
                throw new InvalidOperationException($"Menu {request.MenuCode} not found in group {request.GroupId}");

            group.UpdateMenuPermissions(request.MenuCode, permissions, request.UpdatedBy);
            await _groupRepository.UpdateAsync(group, cancellationToken);

            return _mapper.Map<GroupMenuMapDto>(menuMap);
        }
    }
}
