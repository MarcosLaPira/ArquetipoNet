using BancoHipotecario.Delivery.Api;
using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace BancoHipotecario.Delivery.IntegrationTests
{
    public class ClientesControllerIntegrationTests
         : IClassFixture<WebApplicationFactory<Program>> // Program.cs de la api
    {
        private readonly HttpClient _client;

        public ClientesControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
              //se levanta api en memoria
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetClientesByFiltro_ShouldReturnNotFound_WhenNoClientes()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/clientes/filtro?dni=99999999&canalInvocador=TEST&entidad=1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            error.Should().NotBeNull();
            error!["code"].Should().Be("Clientes.Empty");
        }

        [Fact]
        public async Task GetClientesByFiltro_ShouldReturnOk_WhenClientesFound()
        {
            // ⚠️ Este depende de que tengas datos en la DB de prueba
            var response = await _client.GetAsync("/api/v1/clientes/filtro?dni=44892562&canalInvocador=MOBILBANKING&entidad=100");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Deserialize directamente al DTO que devuelve tu API
            var clientes = await response.Content.ReadFromJsonAsync<List<ClienteResponseDto>>();

            clientes.Should().NotBeNull();
            clientes!.Count.Should().BeGreaterThan(0);

            // Verifico que el primero tenga el DNI esperado
            clientes[0].NumeroDocumento.Should().Be("44892562");
        }
    }
}
