using BankingSystem.API;
using BankingSystem.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace BankingSystem.Tests.Integration.Controllers;

public class MovimientosControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MovimientosControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remover el DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BankingDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Agregar DbContext en memoria para pruebas
                services.AddDbContext<BankingDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"InMemoryDbForMovimientosTesting_{Guid.NewGuid()}");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetMovimientosReporte_ValidDateRange_ShouldReturnOk()
    {
        // Arrange
        var fechaInicio = DateTime.UtcNow.AddDays(-7);
        var fechaFin = DateTime.UtcNow;

        // Act
        var response = await _client.GetAsync($"/api/movimientos/reporte?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
