using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Shared.Constants
{
    public static partial class CacheKey
    {
        #region Permission

        public const string PERMISSIONS_PATTERN_KEY = "permissions.";
        public const string PERMISSIONS_ALLOWED_KEY = "permissions.allowed-{0}-{1}"; // roleId, permission
        public const string PERMISSIONS_ALLOWED_ACTION_KEY = "permissions.allowedaction-{0}-{1}-{2}"; // roleId, permission, action
        public const string PERMISSIONS_USER_OVERRIDE_KEY = "permissions.override-{0}-{1}"; // userId, permission
        public const string PERMISSIONS_USER_OVERRIDE_ACTION_KEY = "permissions.overrideaction-{0}-{1}-{2}"; // userId, permission, action

        #endregion

    }
}
