using GroupManagementService.Domain.Events;
using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Domain.Entities
{
    /// <summary>
    /// Group entity representing a user group/role for access control
    /// </summary>
    public class Group : BaseEntity
    {
        private readonly List<DomainEvent> _domainEvents = new();

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public GroupStatus Status { get; private set; }
        public bool IsAdmin { get; private set; }
        public IReadOnlyCollection<GroupMenuMap> MenuMaps => _menuMaps.AsReadOnly();
        private readonly List<GroupMenuMap> _menuMaps = new();

        protected Group() { }

        public Group(string code, string name, string? description, long createdBy, bool isAdmin = false)
            : base(createdBy)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Status = GroupStatus.Active;
            IsAdmin = isAdmin;

            AddDomainEvent(new GroupCreatedEvent(this.Id, code, name));
        }

        public void Update(string name, string? description, long updatedBy)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new GroupUpdatedEvent(this.Id, name, description));
        }

        public void Activate(long updatedBy)
        {
            Status = GroupStatus.Active;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new GroupStatusChangedEvent(this.Id, Status));
        }

        public void Deactivate(long updatedBy)
        {
            Status = GroupStatus.Inactive;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new GroupStatusChangedEvent(this.Id, Status));
        }

        public void AddMenuMap(GroupMenuMap menuMap, long updatedBy)
        {
            if (menuMap == null) throw new ArgumentNullException(nameof(menuMap));

            if (_menuMaps.Any(m => m.MenuCode == menuMap.MenuCode))
                throw new InvalidOperationException($"Menu {menuMap.MenuCode} is already mapped to this group");

            _menuMaps.Add(menuMap);
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new MenuMapAddedEvent(this.Id, menuMap.MenuCode));
        }

        public void RemoveMenuMap(string menuCode, long updatedBy)
        {
            var menuMap = _menuMaps.FirstOrDefault(m => m.MenuCode == menuCode);
            if (menuMap != null)
            {
                _menuMaps.Remove(menuMap);
                UpdatedBy = updatedBy;
                UpdatedOn = DateTime.UtcNow;

                AddDomainEvent(new MenuMapRemovedEvent(this.Id, menuCode));
            }
        }

        public void UpdateMenuPermissions(string menuCode, MenuPermissions permissions, long updatedBy)
        {
            var menuMap = _menuMaps.FirstOrDefault(m => m.MenuCode == menuCode);
            if (menuMap != null)
            {
                menuMap.UpdatePermissions(permissions, updatedBy);
                UpdatedBy = updatedBy;
                UpdatedOn = DateTime.UtcNow;

                AddDomainEvent(new MenuPermissionsUpdatedEvent(this.Id, menuCode, permissions));
            }
        }

        public void AddDomainEvent(DomainEvent @event)
        {
            _domainEvents.Add(@event);
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
