using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Domain.Entities
{
    /// <summary>
    /// Represents a menu mapping for a group with associated permissions
    /// </summary>
    public class GroupMenuMap : BaseEntity
    {
        public long GroupId { get; private set; }
        public string MenuCode { get; private set; }
        public string MenuName { get; private set; }
        public MenuPermissions Permissions { get; private set; }
        public int? MenuSequence { get; private set; }

        protected GroupMenuMap() { }

        public GroupMenuMap(long groupId, string menuCode, string menuName, MenuPermissions permissions, 
                          long createdBy, int? menuSequence = null)
            : base(createdBy)
        {
            GroupId = groupId;
            MenuCode = menuCode ?? throw new ArgumentNullException(nameof(menuCode));
            MenuName = menuName ?? throw new ArgumentNullException(nameof(menuName));
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            MenuSequence = menuSequence;
        }

        public void UpdatePermissions(MenuPermissions permissions, long updatedBy)
        {
            Permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }

        public void UpdateSequence(int? sequence, long updatedBy)
        {
            MenuSequence = sequence;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
