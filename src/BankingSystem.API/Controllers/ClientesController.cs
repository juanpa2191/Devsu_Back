using BankingSystem.Application.Commands.Clientes;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Clientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Obtiene todos los clientes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        var query = new GetAllClientesQuery();
        var clientes = await _mediator.Send(query);
        return Ok(clientes);
    }

    /// <summary>
    /// Obtiene un cliente por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var query = new GetClienteByIdQuery { Id = id };
        var cliente = await _mediator.Send(query);
        
        if (cliente == null)
            return NotFound($"Cliente con ID {id} no encontrado");
            
        return Ok(cliente);
    }

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create([FromBody] CreateClienteCommand command)
    {
        try
        {
            var cliente = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un cliente existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDto>> Update(int id, [FromBody] UpdateClienteCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la URL no coincide con el ID del comando");

        try
        {
            var cliente = await _mediator.Send(command);
            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteClienteCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
