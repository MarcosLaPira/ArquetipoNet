namespace BancoHipotecario.Delivery.Api
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log entrada
            _logger.LogInformation(" Request {method} {url}",
                context.Request?.Method,
                context.Request?.Path.Value);

            await _next(context);

            // Log salida
            _logger.LogInformation(" Response {statusCode}",
                context.Response?.StatusCode);
        }
    }

}
