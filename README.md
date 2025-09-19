# Banking System API

Sistema bancario desarrollado con .NET 8, Clean Architecture, DDD y Entity Framework Core.

## 🏗️ Arquitectura

- **Clean Architecture (Onion Architecture)**
- **Domain-Driven Design (DDD)**
- **CQRS con MediatR**
- **Repository Pattern + Unit of Work**
- **Value Objects y Domain Services**

## 🚀 Tecnologías

- .NET 8
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Serilog
- xUnit (Testing)
- Docker

## 📋 Funcionalidades

### Entidades
- **Cliente**: Gestión de clientes del banco
- **Cuenta**: Cuentas bancarias (Ahorro/Corriente)
- **Movimiento**: Transacciones bancarias

### Reglas de Negocio
- Límite diario de retiro: $1,000
- Validación de saldo disponible
- Control de cupo diario
- Validación de identificación única

## 🛠️ Instalación

### Prerrequisitos
- .NET 8 SDK
- SQL Server
- Docker (opcional)

### Configuración

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd backend_cursor
```

2. **Configurar base de datos**
```bash
# Ejecutar script de base de datos
sqlcmd -S localhost -i BaseDatos.sql
```

3. **Configurar connection string**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BankingSystemDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

4. **Ejecutar migraciones**
```bash
cd src/BankingSystem.API
dotnet ef database update
```

5. **Ejecutar aplicación**
```bash
dotnet run
```

## 🐳 Docker

### Construir imagen
```bash
docker build -t banking-system-api .
```

### Ejecutar contenedor
```bash
docker run -p 5000:80 banking-system-api
```

## 🧪 Testing

### Ejecutar pruebas unitarias
```bash
dotnet test tests/BankingSystem.Tests
```

### Ejecutar pruebas de integración
```bash
dotnet test tests/BankingSystem.Tests --filter "Integration"
```

## 📚 API Endpoints

### Clientes
- `GET /api/clientes` - Listar clientes
- `GET /api/clientes/{id}` - Obtener cliente por ID
- `POST /api/clientes` - Crear cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Cuentas
- `GET /api/cuentas` - Listar cuentas
- `GET /api/cuentas/{id}` - Obtener cuenta por ID
- `POST /api/cuentas` - Crear cuenta
- `PUT /api/cuentas/{id}` - Actualizar cuenta
- `DELETE /api/cuentas/{id}` - Eliminar cuenta

### Movimientos
- `GET /api/movimientos` - Listar movimientos
- `GET /api/movimientos/{id}` - Obtener movimiento por ID
- `POST /api/movimientos` - Crear movimiento
- `DELETE /api/movimientos/{id}` - Eliminar movimiento
- `GET /api/movimientos/reporte` - Reporte de movimientos

## 📖 Documentación

La documentación de la API está disponible en Swagger UI:
- Desarrollo: `http://localhost:5000`
- Producción: `https://your-domain.com`

## 🔧 Configuración de Producción

### Variables de Entorno
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection="Server=prod-server;Database=BankingSystemDb;..."
```

### Logs
Los logs se almacenan en:
- Archivo: `logs/banking-system-{date}.txt`
- Retención: 30 días
- Nivel: Information (Producción)

## 📝 Licencia

Este proyecto está bajo la Licencia MIT.

## 👥 Contribuidores

- Desarrollador Principal

## 📞 Soporte

Para soporte técnico, contactar al equipo de desarrollo.