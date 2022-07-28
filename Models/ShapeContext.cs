using Microsoft.EntityFrameworkCore;
using Shape.Common;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Shape.Models
{
    public class ShapeContext : DbContext
    {
        readonly IConfiguration? _config;

        public ShapeContext() { }

        public ShapeContext(DbContextOptions<ShapeContext> options, IConfiguration configuration) : base(options) 
        {
            _config = configuration;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(Constants.CONNECTION_STRING_NAME));
        }

    }
}
