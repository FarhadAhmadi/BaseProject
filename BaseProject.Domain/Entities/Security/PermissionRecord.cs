using BaseProject.Domain.Entities.Base;

namespace BaseProject.Domain.Entities.Security
{
    /// <summary>
    /// Represents a permission record in the system.
    /// Defines what actions are allowed or restricted and to which user roles.
    /// </summary>
    public partial class PermissionRecord : BaseEntity
    {
        private ICollection<string> _userRoles;
        private ICollection<string> _actions;

        /// <summary>
        /// Gets or sets the friendly display name of the permission.
        /// Example: "Manage Orders" or "View Reports".
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the system name of the permission.
        /// Should be unique and used internally by the system.
        /// Example: "ManageOrders".
        /// </summary>
        public string SystemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the permission.
        /// Useful for grouping permissions by feature or module.
        /// Example: "Orders", "Products", "Administration".
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of user roles associated with this permission.
        /// Each role defines which users can perform actions linked to this permission.
        /// </summary>
        public virtual ICollection<string> UserRoles
        {
            get => _userRoles ??= new List<string>();
            protected set => _userRoles = value;
        }

        /// <summary>
        /// Gets or sets the collection of actions associated with this permission.
        /// Each action defines what operations are allowed under this permission.
        /// </summary>
        public virtual ICollection<string> Actions
        {
            get => _actions ??= new List<string>();
            set => _actions = value;
        }
    }
}
