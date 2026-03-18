namespace GroupManagementService.Domain.ValueObjects
{
    /// <summary>
    /// Represents menu permissions for a group
    /// </summary>
    public class MenuPermissions
    {
        public bool CanView { get; private set; }
        public bool CanCreate { get; private set; }
        public bool CanEdit { get; private set; }
        public bool CanDelete { get; private set; }
        public bool CanApprove { get; private set; }

        public MenuPermissions() { }

        public MenuPermissions(bool canView, bool canCreate = false, bool canEdit = false, bool canDelete = false, bool canApprove = false)
        {
            CanView = canView;
            CanCreate = canCreate;
            CanEdit = canEdit;
            CanDelete = canDelete;
            CanApprove = canApprove;
        }

        public static MenuPermissions ViewOnly => new(true, false, false, false, false);
        public static MenuPermissions FullAccess => new(true, true, true, true, true);
        public static MenuPermissions CreateEditAccess => new(true, true, true, false, false);

        public bool HasAnyPermission => CanView || CanCreate || CanEdit || CanDelete || CanApprove;

        public override bool Equals(object? obj)
        {
            if (obj is not MenuPermissions other) return false;
            return CanView == other.CanView && CanCreate == other.CanCreate && 
                   CanEdit == other.CanEdit && CanDelete == other.CanDelete && 
                   CanApprove == other.CanApprove;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CanView, CanCreate, CanEdit, CanDelete, CanApprove);
        }
    }
}
