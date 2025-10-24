using BancoHipotecario.Delivery.Application.Abstractions.Ports;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure.Data
{

    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
    public sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _cs;
        public SqlConnectionFactory(IConfiguration cfg)
            => _cs = cfg.GetConnectionString("DefaultConnection")
                  ?? throw new InvalidOperationException("Missing connection string");

        public IDbConnection CreateConnection() => new SqlConnection(_cs);
    }

   
}
