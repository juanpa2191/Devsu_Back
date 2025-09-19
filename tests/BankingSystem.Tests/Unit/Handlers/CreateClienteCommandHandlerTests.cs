using AutoMapper;
using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Handlers.Clientes;
using BankingSystem.Application.Mappings;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Unit.Handlers;

public class CreateClienteCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly CreateClienteCommandHandler _handler;

    public CreateClienteCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _handler = new CreateClienteCommandHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCliente()
    {
        // Arrange
        var command = new CreateClienteCommand
        {
            ClienteId = "CLI001",
            Nombre = "Juan Pérez",
            Genero = Genero.Masculino,
            Edad = 30,
            NumeroIdentificacion = "1234567890",
            TipoIdentificacion = "Cedula",
            Direccion = "Calle 123 #45-67",
            Telefono = "3001234567",
            Contrasena = "1234"
        };

        _unitOfWorkMock.Setup(x => x.Clientes.ExistsByClienteIdAsync(command.ClienteId))
                      .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Clientes.ExistsByIdentificacionAsync(command.NumeroIdentificacion))
                      .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Clientes.AddAsync(It.IsAny<Cliente>()))
                      .ReturnsAsync((Cliente c) => { c.Id = 1; return c; });
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                      .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ClienteId.Should().Be(command.ClienteId);
        result.Nombre.Should().Be(command.Nombre);
        result.Genero.Should().Be(command.Genero);
        result.Edad.Should().Be(command.Edad);
        result.Identificacion.Should().Be(command.NumeroIdentificacion);
        result.Direccion.Should().Be(command.Direccion);
        result.Telefono.Should().Be(command.Telefono);
        result.Estado.Should().BeTrue();

        _unitOfWorkMock.Verify(x => x.Clientes.ExistsByClienteIdAsync(command.ClienteId), Times.Once);
        _unitOfWorkMock.Verify(x => x.Clientes.ExistsByIdentificacionAsync(command.NumeroIdentificacion), Times.Once);
        _unitOfWorkMock.Verify(x => x.Clientes.AddAsync(It.IsAny<Cliente>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ClienteIdExists_ShouldThrowException()
    {
        // Arrange
        var command = new CreateClienteCommand
        {
            ClienteId = "CLI001",
            Nombre = "Juan Pérez",
            Genero = Genero.Masculino,
            Edad = 30,
            NumeroIdentificacion = "1234567890",
            TipoIdentificacion = "Cedula",
            Direccion = "Calle 123 #45-67",
            Telefono = "3001234567",
            Contrasena = "1234"
        };

        _unitOfWorkMock.Setup(x => x.Clientes.ExistsByClienteIdAsync(command.ClienteId))
                      .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain($"Ya existe un cliente con el ID: {command.ClienteId}");
    }

    [Fact]
    public async Task Handle_IdentificacionExists_ShouldThrowException()
    {
        // Arrange
        var command = new CreateClienteCommand
        {
            ClienteId = "CLI001",
            Nombre = "Juan Pérez",
            Genero = Genero.Masculino,
            Edad = 30,
            NumeroIdentificacion = "1234567890",
            TipoIdentificacion = "Cedula",
            Direccion = "Calle 123 #45-67",
            Telefono = "3001234567",
            Contrasena = "1234"
        };

        _unitOfWorkMock.Setup(x => x.Clientes.ExistsByClienteIdAsync(command.ClienteId))
                      .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Clientes.ExistsByIdentificacionAsync(command.NumeroIdentificacion))
                      .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain($"Ya existe un cliente con la identificación: {command.NumeroIdentificacion}");
    }
}
