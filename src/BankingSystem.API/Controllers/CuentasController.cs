using BankingSystem.Application.Commands.Cuentas;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Cuentas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CuentasController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Obtiene todas las cuentas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CuentaDto>>> GetAll()
    {
        var query = new GetAllCuentasQuery();
        var cuentas = await _mediator.Send(query);
        return Ok(cuentas);
    }

    /// <summary>
    /// Obtiene una cuenta por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CuentaDto>> GetById(int id)
    {
        var query = new GetCuentaByIdQuery { Id = id };
        var cuenta = await _mediator.Send(query);
        
        if (cuenta == null)
            return NotFound($"Cuenta con ID {id} no encontrada");
            
        return Ok(cuenta);
    }

    /// <summary>
    /// Crea una nueva cuenta
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CuentaDto>> Create([FromBody] CreateCuentaCommand command)
    {
        try
        {
            var cuenta = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = cuenta.Id }, cuenta);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una cuenta existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CuentaDto>> Update(int id, [FromBody] UpdateCuentaCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la URL no coincide con el ID del comando");

        try
        {
            var cuenta = await _mediator.Send(command);
            return Ok(cuenta);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una cuenta
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteCuentaCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
