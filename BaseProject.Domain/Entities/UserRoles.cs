using Microsoft.AspNetCore.Identity;

namespace BaseProject.Domain.Entities
{
    public class UserRoles : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual RoleIdentity Role { get; set; }
    }
}
