using BancoHipotecario.Delivery.Infrastructure.Exceptions;
using BancoHipotecario.Delivery.Infrastructure.Exceptions;
using FluentValidation;


namespace BancoHipotecario.Delivery.Api
{

    //middleware para manejar excepciones globales en el controller
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
               
                // Dejo seguir la pipeline
                await _next(context);
              
               
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                await context.Response.WriteAsJsonAsync(new { Errors = errors });
            }
            catch (InfraSqlException ex)
            {
                _logger.LogError(ex, "Timeout accediendo a BD");
                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                await context.Response.WriteAsJsonAsync(new { error = "La base de datos no respondió a tiempo." });
            }

            catch (InfraTimeoutException ex)
            {
                _logger.LogError(ex, "Error SQL");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Error en la base de datos." });
            }
            catch (InfraDataAccessException ex)
            {
                _logger.LogError(ex, "Error de infraestructura");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Error accediendo a datos." });
            }
            

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "Ocurrió un error inesperado." });
            }
        }
    }

}
