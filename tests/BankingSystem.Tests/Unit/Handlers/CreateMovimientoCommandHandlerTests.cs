using AutoMapper;
using BankingSystem.Application.Commands.Movimientos;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Handlers.Movimientos;
using BankingSystem.Application.Mappings;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Services;
using BankingSystem.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Unit.Handlers;

public class CreateMovimientoCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransaccionService> _transaccionServiceMock;
    private readonly IMapper _mapper;
    private readonly CreateMovimientoCommandHandler _handler;

    public CreateMovimientoCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transaccionServiceMock = new Mock<ITransaccionService>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _handler = new CreateMovimientoCommandHandler(
            _unitOfWorkMock.Object, 
            _transaccionServiceMock.Object, 
            _mapper);
    }

    [Fact]
    public async Task Handle_ValidCreditoCommand_ShouldCreateMovimiento()
    {
        // Arrange
        var command = new CreateMovimientoCommand
        {
            TipoMovimiento = TipoMovimiento.Credito,
            Valor = 1000,
            CuentaId = 1
        };

        var cuenta = new Cuenta(
            "1234567890",
            TipoCuenta.Ahorro,
            new Dinero(500),
            1
        );
        cuenta.Id = 1;

        var movimiento = new Movimiento(
            TipoMovimiento.Credito,
            new Dinero(1000),
            new Dinero(1500),
            1
        );

        _unitOfWorkMock.Setup(x => x.Cuentas.GetByIdAsync(command.CuentaId))
                      .ReturnsAsync(cuenta);
        _transaccionServiceMock.Setup(x => x.RealizarMovimientoAsync(
            It.IsAny<Cuenta>(), 
            command.TipoMovimiento, 
            It.IsAny<Dinero>()))
                      .ReturnsAsync(movimiento);
        _unitOfWorkMock.Setup(x => x.Movimientos.AddAsync(It.IsAny<Movimiento>()))
                      .ReturnsAsync((Movimiento m) => { m.Id = 1; return m; });
        _unitOfWorkMock.Setup(x => x.Cuentas.UpdateAsync(It.IsAny<Cuenta>()))
                      .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                      .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TipoMovimiento.Should().Be(command.TipoMovimiento);
        result.Valor.Should().Be(command.Valor);
        result.CuentaId.Should().Be(command.CuentaId);

        _unitOfWorkMock.Verify(x => x.Cuentas.GetByIdAsync(command.CuentaId), Times.Once);
        _transaccionServiceMock.Verify(x => x.RealizarMovimientoAsync(
            It.IsAny<Cuenta>(), 
            command.TipoMovimiento, 
            It.IsAny<Dinero>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Movimientos.AddAsync(It.IsAny<Movimiento>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Cuentas.UpdateAsync(It.IsAny<Cuenta>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CuentaNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new CreateMovimientoCommand
        {
            TipoMovimiento = TipoMovimiento.Credito,
            Valor = 1000,
            CuentaId = 999
        };

        _unitOfWorkMock.Setup(x => x.Cuentas.GetByIdAsync(command.CuentaId))
                      .ReturnsAsync((Cuenta?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BankingSystem.Domain.Exceptions.CuentaNoEncontradaException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain($"Cuenta con ID {command.CuentaId} no encontrada");
    }
}
