using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Authorization
{
    public class RolePermissionAction : BaseEntity
    {
        public string RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        public string? PermissionActionId { get; set; }
        public virtual PermissionAction PermissionAction { get; set; }
    }
}
