using Asp.Versioning;
using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoHipotecario.Delivery.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    public class ClientesController : Controller
    {
        private readonly ISender _sender;
        public ClientesController(ISender sender) => _sender = sender;

        [MapToApiVersion("1.0")]
        [HttpGet("filtro")]       
        public async Task<IActionResult> GetClientesByFiltro(
                [FromQuery] string dni,
                [FromQuery] bool? incluirAdicionales,
                [FromQuery] int? entidad,
                [FromQuery] string canalInvocador,
                CancellationToken ct
        )
        {
            var query = new GetClienteByFiltroQuery(dni, incluirAdicionales, entidad, canalInvocador);
            var result = await _sender.Send(query, ct);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

    }
}
