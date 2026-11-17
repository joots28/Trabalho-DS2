using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SistemaSeguro.Api.Data;
using SistemaSeguro.Api.Middlewares;
using SistemaSeguro.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Serviços e Injeção de Dependências
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProjetoDS2 - API Sistema Seguro",
        Version = "v1",
        Description = "API RESTful profissional para gerenciamento de clientes, automóveis, seguros, acidentes e pagamentos de indenizações.",
        Contact = new OpenApiContact
        {
            Name = "Equipe de Engenharia ProjetoDS2",
            Email = "suporte@seguradora.local"
        }
    });
});

// 2. CORS para integração flexível
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. Registro dos Repositories e Factory de Banco
builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IAutomovelRepository, AutomovelRepository>();
builder.Services.AddScoped<ISeguroRepository, SeguroRepository>();
builder.Services.AddScoped<IAcidenteRepository, AcidenteRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

var app = builder.Build();

// 4. Pipeline de Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjetoDS2 API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:5000)
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Necessário para testes de integração com WebApplicationFactory
public partial class Program { }
