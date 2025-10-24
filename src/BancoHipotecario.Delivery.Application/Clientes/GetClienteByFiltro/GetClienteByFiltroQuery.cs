using BancoHipotecario.Delivery.Domain.Abstractions.BancoHipotecario.Delivery.Domain.Abstractions;
using CleanArchitecture.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro
{
    public sealed record GetClienteByFiltroQuery(

         string DNI,
         bool? IncluirAdicionales,
         int? Entidad,
         string CanalInvocador

    ) : IQuery<IReadOnlyList<ClienteResponseDto>>;
      
    
}
