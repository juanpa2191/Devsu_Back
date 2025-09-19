using BankingSystem.Application.Commands.Movimientos;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Movimientos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimientosController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Obtiene todos los movimientos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientoDto>>> GetAll()
    {
        var query = new GetAllMovimientosQuery();
        var movimientos = await _mediator.Send(query);
        return Ok(movimientos);
    }

    /// <summary>
    /// Obtiene un movimiento por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientoDto>> GetById(int id)
    {
        var query = new GetMovimientoByIdQuery { Id = id };
        var movimiento = await _mediator.Send(query);
        
        if (movimiento == null)
            return NotFound($"Movimiento con ID {id} no encontrado");
            
        return Ok(movimiento);
    }

    /// <summary>
    /// Crea un nuevo movimiento
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MovimientoDto>> Create([FromBody] CreateMovimientoCommand command)
    {
        try
        {
            var movimiento = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = movimiento.Id }, movimiento);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un movimiento
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteMovimientoCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene movimientos por rango de fechas
    /// </summary>
    [HttpGet("reporte")]
    public async Task<ActionResult<IEnumerable<ReporteMovimientoDto>>> GetReporte(
        [FromQuery] DateTime fechaInicio, 
        [FromQuery] DateTime fechaFin,
        [FromQuery] int? clienteId = null)
    {
        try
        {
            var query = new GetMovimientosByFechaQuery 
            { 
                FechaInicio = fechaInicio, 
                FechaFin = fechaFin,
                ClienteId = clienteId
            };
            var reporte = await _mediator.Send(query);
            return Ok(reporte);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
