using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Application.Abstractions.Ports
{
    public interface IClientesReadRepository
    {
        Task<IReadOnlyList<ClienteResponseDto>> GetClientesByFiltroAsync(
            string dni,
            bool? incluirAdicionales,
            int? entidad,
            string canalInvocador,
            CancellationToken cancellationToken
        );
    }
}
