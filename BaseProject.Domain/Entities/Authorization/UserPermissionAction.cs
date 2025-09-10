using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Authorization
{
    public partial class UserPermissionAction : BaseEntity 
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string? PermissionActionId { get; set; }
        public virtual PermissionAction PermissionAction { get; set; }

        public bool IsAllowed { get; set; }
    }
}
