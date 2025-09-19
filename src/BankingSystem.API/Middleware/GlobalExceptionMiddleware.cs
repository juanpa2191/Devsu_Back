using BankingSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace BankingSystem.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = exception.Message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
            method = context.Request.Method
        };

        switch (exception)
        {
            case SaldoInsuficienteException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case CupoDiarioExcedidoException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case ClienteNoEncontradoException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case CuentaNoEncontradaException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case ArgumentNullException:
            case ArgumentException:
            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new
                {
                    message = "Ha ocurrido un error interno del servidor",
                    timestamp = DateTime.UtcNow,
                    path = context.Request.Path,
                    method = context.Request.Method
                };
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
