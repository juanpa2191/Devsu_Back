using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Infrastructure.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Genero).IsRequired();
            entity.Property(e => e.Edad).IsRequired();
            entity.Property(e => e.Direccion).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(e => e.ClienteId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Contrasena).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Estado).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            // Value Object: Identificacion
            entity.OwnsOne(e => e.Identificacion, identificacion =>
            {
                identificacion.Property(i => i.Numero).HasColumnName("NumeroIdentificacion").IsRequired().HasMaxLength(13);
                identificacion.Property(i => i.Tipo).HasColumnName("TipoIdentificacion").IsRequired().HasMaxLength(20);
                identificacion.WithOwner().HasForeignKey("Id");
            });

            // Índices únicos
            entity.HasIndex(e => e.ClienteId).IsUnique();
            // El índice para NumeroIdentificacion se crea automáticamente por la constraint UNIQUE

            // Relación con Cuentas
            entity.HasMany(e => e.Cuentas)
                  .WithOne(c => c.Cliente)
                  .HasForeignKey(c => c.ClienteId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Cuenta
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumeroCuenta).IsRequired().HasMaxLength(20);
            entity.Property(e => e.TipoCuenta).IsRequired();
            entity.Property(e => e.Estado).IsRequired();
            entity.Property(e => e.ClienteId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            // Value Object: Dinero para SaldoInicial
            entity.OwnsOne(e => e.SaldoInicial, saldo =>
            {
                saldo.Property(s => s.Monto).HasColumnName("SaldoInicial").IsRequired().HasColumnType("decimal(18,2)");
                saldo.Property(s => s.Moneda).HasColumnName("MonedaInicial").IsRequired().HasMaxLength(3).HasDefaultValue("USD");
            });

            // Value Object: Dinero para SaldoActual
            entity.OwnsOne(e => e.SaldoActual, saldo =>
            {
                saldo.Property(s => s.Monto).HasColumnName("SaldoActual").IsRequired().HasColumnType("decimal(18,2)");
                saldo.Property(s => s.Moneda).HasColumnName("MonedaActual").IsRequired().HasMaxLength(3).HasDefaultValue("USD");
            });

            // Índice único
            entity.HasIndex(e => e.NumeroCuenta).IsUnique();

            // Relación con Cliente
            entity.HasOne(e => e.Cliente)
                  .WithMany(c => c.Cuentas)
                  .HasForeignKey(e => e.ClienteId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Movimientos
            entity.HasMany(e => e.Movimientos)
                  .WithOne(m => m.Cuenta)
                  .HasForeignKey(m => m.CuentaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de Movimiento
        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.TipoMovimiento).IsRequired();
            entity.Property(e => e.CuentaId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            // Value Object: Dinero para Valor
            entity.OwnsOne(e => e.Valor, valor =>
            {
                valor.Property(v => v.Monto).HasColumnName("Valor").IsRequired().HasColumnType("decimal(18,2)");
                valor.Property(v => v.Moneda).HasColumnName("MonedaValor").IsRequired().HasMaxLength(3).HasDefaultValue("USD");
            });

            // Value Object: Dinero para Saldo
            entity.OwnsOne(e => e.Saldo, saldo =>
            {
                saldo.Property(s => s.Monto).HasColumnName("Saldo").IsRequired().HasColumnType("decimal(18,2)");
                saldo.Property(s => s.Moneda).HasColumnName("MonedaSaldo").IsRequired().HasMaxLength(3).HasDefaultValue("USD");
            });

            // Relación con Cuenta
            entity.HasOne(e => e.Cuenta)
                  .WithMany(c => c.Movimientos)
                  .HasForeignKey(e => e.CuentaId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.Fecha);
            entity.HasIndex(e => e.CuentaId);
            entity.HasIndex(e => e.TipoMovimiento);
        });

        // Configurar soft delete
        modelBuilder.Entity<Cliente>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Cuenta>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Movimiento>().HasQueryFilter(e => !e.IsDeleted);
    }
}
