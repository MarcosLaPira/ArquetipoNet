using BancoHipotecario.Delivery.Application.Abstractions.Ports;

using BancoHipotecario.Delivery.Domain.Abstractions.BancoHipotecario.Delivery.Domain.Abstractions;
using CleanArchitecture.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro
{
    public sealed  class GetClienteByFiltroQueryHandler
        : IQueryHandler<GetClienteByFiltroQuery, IReadOnlyList<ClienteResponseDto>>
    {
        private readonly IClientesReadRepository _repo;
        public GetClienteByFiltroQueryHandler(IClientesReadRepository repo) => _repo = repo;
       

        public async Task<Result<IReadOnlyList<ClienteResponseDto>>> Handle(GetClienteByFiltroQuery query, CancellationToken cancellationToken)
        {

            var resultado = await _repo.GetClientesByFiltroAsync(query.DNI,query.IncluirAdicionales,query.Entidad,query.CanalInvocador, cancellationToken);

            if (resultado == null ||! resultado.Any())
            {
                return Result.Failure<IReadOnlyList<ClienteResponseDto>>(
                    new Error("Clientes.Empty", "No se encontraron clientes")
                    );
            }

            return Result.Success<IReadOnlyList<ClienteResponseDto>>(resultado);
        }
    }
}
