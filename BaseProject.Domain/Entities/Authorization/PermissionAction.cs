using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Authorization
{
    public class PermissionAction : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;

        public string? PermissionRecordId { get; set; }
        public virtual PermissionRecord PermissionRecord { get; set; }

        public virtual ICollection<RolePermissionAction> RolePermissionActions { get; set; } = [];
        public virtual ICollection<UserPermissionAction> UserPermissionActions { get; set; } = [];
    }
}