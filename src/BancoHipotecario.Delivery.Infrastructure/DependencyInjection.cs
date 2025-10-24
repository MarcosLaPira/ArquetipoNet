
using BancoHipotecario.Delivery.Application.Abstractions.Ports;
using BancoHipotecario.Delivery.Infrastructure.Data;
using BancoHipotecario.Delivery.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

          

            //CLIENTES
            services.AddScoped<IClientesReadRepository, ClientesReadRepository>();

           

            return services;
        }
    }
}
