using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BancoHipotecario.Delivery.Api;
using BancoHipotecario.Delivery.Application;
using BancoHipotecario.Delivery.Application.Abstractions.Behaviors;
using BancoHipotecario.Delivery.Application.Clientes.GetClienteByFiltro;
using BancoHipotecario.Delivery.Infrastructure;
using FluentValidation;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Serilog;




var builder = WebApplication.CreateBuilder(args);



builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

//OPCION SERILOG

Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext() // agrega cosas como TraceId, RequestId, etc.
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter()) // salida en JSON a consola
        .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day)
        .CreateLogger();

builder.Host.UseSerilog();


// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// --- Controllers + Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


// --- MediatR + FluentValidation ---

//  Registramos MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetClienteByFiltroQuery).Assembly));

//  Registramos TODOS los validators que estén en Application
builder.Services.AddValidatorsFromAssembly(typeof(GetClienteByFiltroQueryValidator).Assembly);

//  Registramos el pipeline de validación
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// --- HEALTH CHECKS ---
// Liveness (self) + Readiness (DB). Deja UNO: SQL Server o Postgres.
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live" })
    .AddSqlServer( // <-- SQL Server (si usás SQL Server)
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sql",
        tags: new[] { "ready" });



// --- POLLY (resiliencia) ---
builder.Services.AddSingleton<IPolicyRegistry<string>>(ResiliencePolicies.CreateRegistry());




//

// Configuración de OpenTelemetry para consola (solo local)
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("BancoHipotecario.Delivery.API"))
    .WithTracing(t => t
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()

        .AddConsoleExporter()
    );



var app = builder.Build();



// --- Endpoints de Health ---
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


//registro capa de middleware 
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();


if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();

public partial class Program { }
