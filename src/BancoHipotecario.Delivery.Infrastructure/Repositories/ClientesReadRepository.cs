
using BancoHipotecario.Delivery.Application.Abstractions.Ports;
using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using BancoHipotecario.Delivery.Infrastructure.Data;
using BancoHipotecario.Delivery.Infrastructure.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Infrastructure.Repositories
{
    public class ClientesReadRepository : IClientesReadRepository
    {
        private readonly ISqlConnectionFactory _factory;
        private readonly IAsyncPolicy _retryPolicy;

        public ClientesReadRepository(ISqlConnectionFactory factory, IPolicyRegistry<string> registry)
        { 
            _factory = factory;
            _retryPolicy = registry.Get<IAsyncPolicy>("SqlRetryPolicy");

        }

        public async Task<IReadOnlyList<ClienteResponseDto>> GetClientesByFiltroAsync(
            string dni, 
            bool? incluirAdicionales, 
            int? entidad, 
            string canalInvocador, 
            CancellationToken cancellationToken
        )
        {
            try {

                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    using var conn = _factory.CreateConnection();

                    var result = await conn.QueryAsync<ClienteResponseDto>
                        (
                            new CommandDefinition("dbo.ObtenerPiezasAPIByFiltro",
                            new {   NumeroDocumento = dni, 
                                    IncluirAdicionales = incluirAdicionales, 
                                    Entidad = entidad,
                                    CanalInvocador = canalInvocador 
                                },
                            commandType: CommandType.StoredProcedure,
                            cancellationToken: cancellationToken
                        )
                    );

                    return result.ToList();
                });

            }
            catch (SqlException ex) // error de SQL
            {
                throw new InfraSqlException("Error SQL al acceder a la base de datos", ex);
            }
            catch (TimeoutException ex) // error de timeout explícito
            {
                throw new InfraTimeoutException("La consulta a la base de datos superó el tiempo de espera", ex);
            }
            catch (Exception ex) // fallback para cualquier otro error
            {
                throw new InfraDataAccessException("Error accediendo a datos", ex);
            }

        }
    }
}
