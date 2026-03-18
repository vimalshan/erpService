using AutoMapper;
using MediatR;
using GroupManagementService.Application.Queries;
using GroupManagementService.Application.DTOs;
using GroupManagementService.Domain.Repositories;

namespace GroupManagementService.Application.Handlers
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupByIdQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with id {request.GroupId} not found");

            return _mapper.Map<GroupDto>(group);
        }
    }

    public class GetGroupByCodeQueryHandler : IRequestHandler<GetGroupByCodeQuery, GroupDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupByCodeQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GroupDto> Handle(GetGroupByCodeQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByCodeAsync(request.GroupCode, cancellationToken);
            if (group == null)
                throw new InvalidOperationException($"Group with code '{request.GroupCode}' not found");

            return _mapper.Map<GroupDto>(group);
        }
    }

    public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetAllGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<GroupDto>>(groups);
        }
    }

    public class GetGroupsByStatusQueryHandler : IRequestHandler<GetGroupsByStatusQuery, IEnumerable<GroupDto>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupsByStatusQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsByStatusQuery request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<Domain.ValueObjects.GroupStatus>(request.Status, out var status))
                throw new ArgumentException($"Invalid status: {request.Status}");

            var groups = await _groupRepository.GetByStatusAsync(status, cancellationToken);
            return _mapper.Map<IEnumerable<GroupDto>>(groups);
        }
    }

    public class SearchGroupsQueryHandler : IRequestHandler<SearchGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public SearchGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<GroupDto>> Handle(SearchGroupsQuery request, CancellationToken cancellationToken)
        {
            var allGroups = await _groupRepository.GetAllAsync(cancellationToken);

            var results = allGroups
                .Where(g => string.IsNullOrEmpty(request.SearchTerm) || 
                           g.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           g.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .Where(g => string.IsNullOrEmpty(request.Status) || g.Status.ToString() == request.Status)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return _mapper.Map<IEnumerable<GroupDto>>(results);
        }
    }

    public class GetAdminGroupsQueryHandler : IRequestHandler<GetAdminGroupsQuery, IEnumerable<GroupDto>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetAdminGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetAdminGroupsQuery request, CancellationToken cancellationToken)
        {
            var allGroups = await _groupRepository.GetAllAsync(cancellationToken);
            var adminGroups = allGroups.Where(g => g.IsAdmin);

            return _mapper.Map<IEnumerable<GroupDto>>(adminGroups);
        }
    }
}
