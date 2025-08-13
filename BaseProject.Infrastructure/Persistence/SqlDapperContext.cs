using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BaseProject.Infrastructure.Persistence
{
    public class SqlDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }

}
