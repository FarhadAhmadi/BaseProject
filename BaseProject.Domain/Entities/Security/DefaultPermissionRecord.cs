namespace BaseProject.Domain.Entities.Security
{
    /// <summary>
    /// Represents the default set of permissions for a specific user role.
    /// Useful for seeding initial permissions or defining role templates.
    /// </summary>
    public class DefaultPermissionRecord
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultPermissionRecord"/>.
        /// Automatically initializes the <see cref="PermissionRecords"/> collection.
        /// </summary>
        public DefaultPermissionRecord()
        {
            PermissionRecords = new List<PermissionRecord>();
        }

        /// <summary>
        /// Gets or sets the system name of the user role.
        /// Used to associate default permissions with a specific role.
        /// Example: "Admin", "Moderator", "User".
        /// </summary>
        public string UserRoleSystemName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of default permissions for the role.
        /// Defines which actions are allowed by default.
        /// </summary>
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}
