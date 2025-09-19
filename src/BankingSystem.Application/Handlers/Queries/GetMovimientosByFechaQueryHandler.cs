using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Queries.Movimientos;
using BankingSystem.Domain.Interfaces;
using MediatR;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.Handlers.Queries;

public class GetMovimientosByFechaQueryHandler : IRequestHandler<GetMovimientosByFechaQuery, IEnumerable<ReporteMovimientoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMovimientosByFechaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ReporteMovimientoDto>> Handle(GetMovimientosByFechaQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.Movimientos.GetByFechaAsync(request.FechaInicio, request.FechaFin);
        
        if (request.ClienteId.HasValue)
        {
            movimientos = movimientos.Where(m => m.Cuenta.ClienteId == request.ClienteId.Value);
        }

        var reportes = new List<ReporteMovimientoDto>();
        
        foreach (var movimiento in movimientos)
        {
            var reporte = new ReporteMovimientoDto
            {
                Fecha = movimiento.Fecha,
                Cliente = movimiento.Cuenta.Cliente.Nombre,
                NumeroCuenta = movimiento.Cuenta.NumeroCuenta,
                Tipo = movimiento.Cuenta.TipoCuenta.ToString(),
                SaldoInicial = movimiento.Cuenta.SaldoInicial.Monto,
                Estado = movimiento.Cuenta.Estado,
                Movimiento = movimiento.TipoMovimiento.EsCredito() ? movimiento.Valor.Monto : -movimiento.Valor.Monto,
                SaldoDisponible = movimiento.Saldo.Monto
            };
            reportes.Add(reporte);
        }

        return reportes.OrderByDescending(r => r.Fecha);
    }
}
