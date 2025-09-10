using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Authorization
{
    public class PermissionRecord : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public virtual ICollection<PermissionAction> Actions { get; set; } = [];
    }
}
