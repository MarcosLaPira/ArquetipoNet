using BancoHipotecario.Delivery.Application.Abstractions.Ports;
using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using BancoHipotecario.Delivery.Domain.Abstractions;
using BancoHipotecario.Delivery.Infrastructure.Data;
using BancoHipotecario.Delivery.Infrastructure.Exceptions;
using BancoHipotecario.Delivery.Infrastructure.Repositories;
using Moq;
using Polly;
using Polly.Registry;
namespace BancoHipotecario.Delivery.Clientes.UnitTests
{
    public class GetClienteByFiltroQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoClientesFound()
        {
            // Arrange
            var repoMock = new Mock<IClientesReadRepository>();

            repoMock
                .Setup(r => r.GetClientesByFiltroAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ClienteResponseDto>());

            var handler = new GetClienteByFiltroQueryHandler(repoMock.Object);

            var query = new GetClienteByFiltroQuery("99999999", null, null, "TEST");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Clientes.Empty", result.Error.Code);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenClientesFound()
        {
            // Arrange
            var repoMock = new Mock<IClientesReadRepository>();
            repoMock
                .Setup(r => r.GetClientesByFiltroAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ClienteResponseDto> { new ClienteResponseDto {NumeroDocumento = "44892562" } });

            var handler = new GetClienteByFiltroQueryHandler(repoMock.Object);

            var query = new GetClienteByFiltroQuery("44892562", null, 100, "MOBILBANKING");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            Console.WriteLine(result);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("44892562", result.Value[0].NumeroDocumento);
        }

        [Fact]
        public async Task GetClientesByFiltroAsync_ShouldAlwaysThrowInfraDataAccessException_OnAnyException()
        {
            // Arrange
            var fakeFactory = new Mock<ISqlConnectionFactory>();
            fakeFactory.Setup(f => f.CreateConnection())
                       .Throws(new NullReferenceException("falló factory"));

            var registry = new PolicyRegistry();
            registry.Add("SqlRetryPolicy", Policy.NoOpAsync());

            var repo = new ClientesReadRepository(fakeFactory.Object, registry);

            // Act
            InfraDataAccessException? capturedException = null;
            try
            {
                await repo.GetClientesByFiltroAsync("dni", null, null, "TEST", CancellationToken.None);
            }
            catch (InfraDataAccessException ex)
            {
                capturedException = ex;
            }

            // Assert
            Assert.NotNull(capturedException); // validamos que efectivamente se lanzó
            Assert.Equal("Error accediendo a datos", capturedException.Message);         
            Assert.IsType<InfraDataAccessException>(capturedException);
        }


    }
}
