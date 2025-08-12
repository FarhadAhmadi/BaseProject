using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Domain.Constants
{
    public static class HealthCheck
    {
        public const string InfrastructureCheck = "InfrastructureCheck";
        public const string ExternalServiceCheck = "ExternalServiceCheck";
        public const string DBHealthCheck = "DBHealthCheck";
    }
}
