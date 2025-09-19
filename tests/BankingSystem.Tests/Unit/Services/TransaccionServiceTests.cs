using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Services;
using BankingSystem.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Unit.Services;

public class TransaccionServiceTests
{
    private readonly Mock<IMovimientoRepository> _movimientoRepositoryMock;
    private readonly TransaccionService _service;

    public TransaccionServiceTests()
    {
        _movimientoRepositoryMock = new Mock<IMovimientoRepository>();
        _service = new TransaccionService(_movimientoRepositoryMock.Object);
    }

    [Fact]
    public async Task RealizarMovimientoAsync_ValidCredito_ShouldCreateMovimiento()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(1000),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(500);
        var tipoMovimiento = TipoMovimiento.Credito;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(0);

        // Act
        var result = await _service.RealizarMovimientoAsync(cuenta, tipoMovimiento, monto);

        // Assert
        result.Should().NotBeNull();
        result.TipoMovimiento.Should().Be(tipoMovimiento);
        result.Valor.Should().Be(monto);
        result.Saldo.Monto.Should().Be(1500); // 1000 + 500
        result.CuentaId.Should().Be(cuenta.Id);
    }

    [Fact]
    public async Task RealizarMovimientoAsync_ValidDebito_ShouldCreateMovimiento()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(1000),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(300);
        var tipoMovimiento = TipoMovimiento.Debito;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(0);

        // Act
        var result = await _service.RealizarMovimientoAsync(cuenta, tipoMovimiento, monto);

        // Assert
        result.Should().NotBeNull();
        result.TipoMovimiento.Should().Be(tipoMovimiento);
        result.Valor.Should().Be(monto);
        result.Saldo.Monto.Should().Be(700); // 1000 - 300
        result.CuentaId.Should().Be(cuenta.Id);
    }

    [Fact]
    public async Task RealizarMovimientoAsync_SaldoInsuficiente_ShouldThrowException()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(100),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(150);
        var tipoMovimiento = TipoMovimiento.Debito;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SaldoInsuficienteException>(
            () => _service.RealizarMovimientoAsync(cuenta, tipoMovimiento, monto));

        exception.Message.Should().Be("Saldo no disponible");
    }

    [Fact]
    public async Task RealizarMovimientoAsync_CupoDiarioExcedido_ShouldThrowException()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(2000),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(500);
        var tipoMovimiento = TipoMovimiento.Debito;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(600); // Ya se retiraron $600, el límite es $1000

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CupoDiarioExcedidoException>(
            () => _service.RealizarMovimientoAsync(cuenta, tipoMovimiento, monto));

        exception.Message.Should().Be("Cupo diario Excedido");
    }

    [Fact]
    public async Task ValidarLimiteDiarioAsync_WithinLimit_ShouldReturnTrue()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(1000),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(300);
        var fecha = DateTime.UtcNow;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(cuenta.Id, fecha))
                                .ReturnsAsync(500); // Ya se retiraron $500

        // Act
        var result = await _service.ValidarLimiteDiarioAsync(cuenta, monto, fecha);

        // Assert
        result.Should().BeTrue(); // 500 + 300 = 800, que es menor a 1000
    }

    [Fact]
    public async Task ValidarLimiteDiarioAsync_ExceedsLimit_ShouldReturnFalse()
    {
        // Arrange
        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(1000),
            1
        );
        cuenta.Id = 1;

        var monto = new Dinero(600);
        var fecha = DateTime.UtcNow;

        _movimientoRepositoryMock.Setup(x => x.GetTotalRetirosDelDiaAsync(cuenta.Id, fecha))
                                .ReturnsAsync(500); // Ya se retiraron $500

        // Act
        var result = await _service.ValidarLimiteDiarioAsync(cuenta, monto, fecha);

        // Assert
        result.Should().BeFalse(); // 500 + 600 = 1100, que es mayor a 1000
    }
}
