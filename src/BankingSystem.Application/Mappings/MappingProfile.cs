using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion != null ? src.Identificacion.Numero : string.Empty));

        CreateMap<Cuenta, CuentaDto>()
            .ForMember(dest => dest.SaldoInicial, opt => opt.MapFrom(src => src.SaldoInicial != null ? src.SaldoInicial.Monto : 0))
            .ForMember(dest => dest.SaldoActual, opt => opt.MapFrom(src => src.SaldoActual != null ? src.SaldoActual.Monto : 0))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : string.Empty));

        CreateMap<Movimiento, MovimientoDto>()
            .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Valor != null ? src.Valor.Monto : 0))
            .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.Saldo != null ? src.Saldo.Monto : 0))
            .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.Cuenta != null ? src.Cuenta.NumeroCuenta : string.Empty))
            .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cuenta != null && src.Cuenta.Cliente != null ? src.Cuenta.Cliente.Nombre : string.Empty))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.ObtenerDescripcion()));
    }
}
