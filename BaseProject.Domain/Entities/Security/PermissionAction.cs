using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Security
{
    /// <summary>
    /// Represents a specific permission action that has been denied for a user role.
    /// Used for fine-grained permission control in the system.
    /// </summary>
    public partial class PermissionAction : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique system identifier for this permission.
        /// Example: "ManageOrders" or "EditProduct".
        /// </summary>
        public string SystemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the user role associated with this permission denial.
        /// Links to a role in your role management system.
        /// </summary>
        public string UserRoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the specific action name for which access is denied.
        /// Example: "Create", "Delete", "Update".
        /// </summary>
        public string Action { get; set; } = string.Empty;
    }
}