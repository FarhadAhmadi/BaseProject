using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseProject.Domain.Entities
{
    [NotMapped]
    public class RoleIdentity : IdentityRole<string>
    {
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
